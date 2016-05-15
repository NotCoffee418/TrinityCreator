using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace TrinityCreatorUpdater
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("- Trinity Creator Updater -");
            var c = new WebClient();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\TrinityCreator.exe";
            if (args.Length > 0)
                path = args[0];

            Console.WriteLine("Determining old build version...");
            int oldBuild = 0;
            if (File.Exists(path))
                oldBuild = FileVersionInfo.GetVersionInfo(path).FilePrivatePart;

            if (oldBuild == 0)
            {
                Reinstall();
                return;
            }
            Console.WriteLine("Old build: " + oldBuild);

            while (Process.GetProcessesByName("TrinityCreator").Length != 0)
            {
                Thread.Sleep(2000);
                Console.WriteLine("Waiting for Trinity Creator to close...");
            }

            // Start Update
            Console.WriteLine("Downloading latest version of Trinity Creator...");
            c.DownloadFile(
                "https://github.com/RStijn/TrinityCreator/blob/master/TrinityCreator/bin/Release/TrinityCreator.exe?raw=true",
                path);

            Console.WriteLine("Downloaded to {0}", path);
            DownloadAdditionalFiles(path, oldBuild);
            Console.WriteLine("Opening Trinity Creator...");
            var currentExe = Assembly.GetExecutingAssembly().Location;
            var proc = new ProcessStartInfo(path);
            proc.Arguments = "\"" + currentExe + "\"";
            Process.Start(proc);

            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        private static void DownloadAdditionalFiles(string exePath, int build)
        {
            Console.WriteLine("Looking for new required files...");
            List<string> newFiles = new List<string>();
            string dir = Path.GetDirectoryName(exePath);

            if (build < 14)
                newFiles.AddRange(new string[] {
                    "CefSharp.BrowserSubprocess.exe",
                    "CefSharp.BrowserSubprocess.Core.dll",
                    "CefSharp.Core.dll",
                    "CefSharp.dll",
                    "d3dcompiler_43.dll",
                    "d3dcompiler_47.dll",
                    "libcef.dll",
                    "libEGL.dll",
                    "libGLESv2.dll",
                    "icudtl.dat",
                    "cef_100_percent.pak",
                    "cef_200_percent.pak",
                    "natives_blob.bin",
                    "snapshot_blob.bin",
                });
            if (build < 15)
                newFiles.Add("CefSharp.WinForms.dll");

            // Download files
            Console.WriteLine("Found " + newFiles.Count + " new files.");
            using (WebClient c = new WebClient())
                for (int i = 0; i < newFiles.Count; i++)
                {
                    Console.WriteLine(string.Format("Downloading {0} of {1}: {2}", i, newFiles.Count, newFiles[i]));
                    c.DownloadFile("https://github.com/RStijn/TrinityCreator/blob/master/TrinityCreator/bin/Release/" + newFiles[i] + "?raw=true",
                        dir + "\\" + newFiles[i]);
                }

            Console.WriteLine("All new files are installed.");
        }

        private static void Reinstall()
        {
            Console.WriteLine("A problem has occurred. Downloading installer...");
            string tempPath = Path.GetTempFileName() + ".msi";
            using (var c = new WebClient())
            c.DownloadFile("https://github.com/RStijn/TrinityCreator/blob/master/TrinityCreatorSetup/bin/Release/TrinityCreatorSetup.msi?raw=true",
            tempPath);
            Process.Start(tempPath);
        }
    }
}