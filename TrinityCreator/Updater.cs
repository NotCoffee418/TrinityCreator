using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Windows;
using System.Text.RegularExpressions;
using System.Reflection;

namespace TrinityCreator
{
    class Updater
    {
        public static void CheckLatestVersion()
        {
            string local = GetLocalVersionNumber();
            string latest = String.Empty;
            try
            {
                latest = GetLatestVersionNumber(); 
            }
            catch
            {
                MessageBox.Show("Could not check the latest version online", "Can't check version", MessageBoxButton.OK, MessageBoxImage.Information);
                latest = local;
            }

            if (local != latest)
            {
                MessageBoxResult result = MessageBox.Show("A newer version of Trinity Creator is available. Would you like to quick-install it?", "Update available",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    StartUpdate();
            }
        }

        private static void StartUpdate()
        {
            try
            {
                string updateFile = Path.GetTempFileName() + ".exe";
                WebClient c = new WebClient();
                c.DownloadFile("https://github.com/RStijn/TrinityCreator/blob/master/Updater/bin/Release/TrinityCreatorUpdater.exe?raw=true", updateFile);
                string currentExe = Assembly.GetExecutingAssembly().Location;

                ProcessStartInfo proc = new ProcessStartInfo(updateFile);
                proc.Arguments = "\"" + currentExe + "\"";
                Process.Start(proc);
                Environment.Exit(0);
            }
            catch
            {
                MessageBox.Show("Please update manually", "Update failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Reads AssemblyFileVersion from GitHub
        /// </summary>
        /// <returns></returns>
        public static string GetLatestVersionNumber()
        {
            string version = String.Empty;
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://raw.githubusercontent.com/RStijn/TrinityCreator/master/TrinityCreator/Properties/AssemblyInfo.cs");
            StreamReader reader = new StreamReader(stream);
            string[] lines = Regex.Split(reader.ReadToEnd(), "\r\n|\r|\n");
            foreach (string line in lines)
            {
                if (line.Contains("AssemblyFileVersion"))
                    return line.Replace("[assembly: AssemblyFileVersion(\"", String.Empty).Replace("\")]", String.Empty);
            }
            throw new Exception();
        }

        public static string GetLocalVersionNumber()
        {
            Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}
