using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace TrinityCreator
{
    internal class Updater
    {
        public static void CheckLatestVersion()
        {
            var local = GetLocalVersionNumber();
            var latest = string.Empty;
            try
            {
                latest = GetLatestVersionNumber();
            }
            catch
            {
                MessageBox.Show("Could not check the latest version online", "Can't check version", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                latest = local;
            }

            if (local != latest)
            {
                var msg =
                    string.Format(
                        "A newer version of Trinity Creator is available.{0}You are using v{1} while v{2} is available.{0}Click yes to update within seconds.",
                        Environment.NewLine, local, latest);
                var result = MessageBox.Show(msg, "Update available",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                    StartUpdate();
            }
        }

        private static void StartUpdate()
        {
            try
            {
                var updateFile = Path.GetTempFileName() + "-TrinityUpdater.exe";
                var c = new WebClient();
                c.DownloadFile(
                    "https://github.com/Nadromar/TrinityCreator/blob/master/Updater/bin/Release/TrinityCreatorUpdater.exe?raw=true",
                    updateFile);
                var currentExe = Assembly.GetExecutingAssembly().Location;

                var proc = new ProcessStartInfo(updateFile);
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
        ///     Reads AssemblyFileVersion from GitHub
        /// </summary>
        /// <returns></returns>
        public static string GetLatestVersionNumber()
        {
            var version = string.Empty;
            var client = new WebClient();
            var stream =
                client.OpenRead(
                    "https://raw.githubusercontent.com/Nadromar/TrinityCreator/master/TrinityCreator/Properties/AssemblyInfo.cs");
            var reader = new StreamReader(stream);
            var lines = Regex.Split(reader.ReadToEnd(), "\r\n|\r|\n");
            foreach (var line in lines)
            {
                if (line.Contains("AssemblyFileVersion"))
                    return line.Replace("[assembly: AssemblyFileVersion(\"", string.Empty).Replace("\")]", string.Empty);
            }
            throw new Exception();
        }

        public static string GetLocalVersionNumber()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}