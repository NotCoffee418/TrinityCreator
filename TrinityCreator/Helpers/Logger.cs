using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TrinityCreator.Helpers
{
    class Logger
    {
        static Logger()
        {
            // Set log level
            //LogLevel = (Status)Properties.Settings.Default.LogLevel;
            LogLevel = Status.Info;

            string logsDir = GetLogsDirectory();

            // Create log filename with timestamp
            LogFilePath = Path.Combine(logsDir, string.Format("tc-{0}.log", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));

            // Start log writer
            GHelper.SafeThreadStart(() => StartLogWriter());

            // Application exit event
            Application.Current.Exit += Application_Exit;

            // Clean log files older than 6 months
            GHelper.SafeThreadStart(() => CleanLogs());
        }

        public enum Status
        {
            Info = 0,
            Warning = 1,
            Error = 2,
            Fatal = 3
        }
        static string[] statusNames = Enum.GetNames(typeof(Status));
        static MessageBoxImage[] statusIcons = new MessageBoxImage[]
        {
            MessageBoxImage.Information,
            MessageBoxImage.Warning,
            MessageBoxImage.Error,
            MessageBoxImage.Stop,
        };

        static Queue<string> writeQueue = new Queue<string>();
        public static string LogFilePath { get; set; }
        public static Status LogLevel { get; set; }

        public static void Log(string message, Status status = Status.Info, bool showMessageBox = false)
        {
            if (Debugger.IsAttached || status >= LogLevel)
            {
                string entry = $"[{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss.fff")}][{statusNames[(int)status]}] {message}";
                writeQueue.Enqueue(entry);
                Debug.WriteLine(entry);
            }

            // Kill app on fatal errors (this is a bit dodgy but it'll do for now.)
            if (status == Status.Fatal)
            {
                Thread.Sleep(50); // give logwriter time to write
                App.Current.Properties["IsRunning"] = false; // Stop all loops
                MessageBox.Show(message, "Fatal error - closing application", MessageBoxButton.OK, MessageBoxImage.Stop);
                Application.Current.Shutdown();
            }
            else if (showMessageBox || status == Status.Fatal)
                MessageBox.Show(message, statusNames[(int)status], MessageBoxButton.OK, statusIcons[(int)status]);
        }

        private static void StartLogWriter()
        {
            while (App.Current != null && App.Current.Properties["IsRunning"] != null && (bool)App.Current.Properties["IsRunning"])
            {
                if (writeQueue.Count == 0)
                    Thread.Sleep(25);
                else
                {
                    List<string> newLines = new List<string>();
                    while (writeQueue.Count > 0)
                        newLines.Add(writeQueue.Dequeue());
                    File.AppendAllLines(LogFilePath, newLines);
                }
            }
        }


        // Removes log files older than 6 months
        private static void CleanLogs()
        {
            string logsDir = Path.GetDirectoryName(LogFilePath);

            // Ensure it's an TC logfile, not a backup, not some important file that somehow ended up here
            Regex rValidLogFile = new Regex(@"tc-\d\d\d\d-\d\d-\d\d-\d\d-\d\d-\d\d.log");

            // Find approperiate files to delete & delete them
            Directory.EnumerateFiles(logsDir)
                .Where(s => rValidLogFile.IsMatch(Path.GetFileName(s)))
                .Where(s => File.GetCreationTime(s) < DateTime.Now.AddMonths(-6))
                .ToList() // No foreach in IEnumerable, needs list
                .ForEach(s => File.Delete(s));
        }

        private static void Application_Exit(object sender, ExitEventArgs e)
        {
            // Delete empty log files when application closes
            // Also helps to have these empty logfiles as an indication of unclean shutdown
            if (File.Exists(LogFilePath) && File.ReadAllText(LogFilePath) == "")
                File.Delete(LogFilePath);
        }

        public static string GetLogsDirectory()
        {
            // Get working AppData Local directory
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NotCoffee418\\TrinityCreator");
            if (Debugger.IsAttached) // Seperate directory for debugger
                path = Path.Combine(path, "Debug");

            // Enter logs subdirectory & create if it doesn't exist.
            path = Path.Combine(path, "Logs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
