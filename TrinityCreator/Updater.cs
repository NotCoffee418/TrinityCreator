using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace TrinityCreator
{    class Updater
    {
        static Updater()
        {
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
        }

        internal static void Run(bool force = false)
        {
            try
            {
                AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
                AutoUpdater.Start("https://raw.githubusercontent.com/NotCoffee418/TrinityCreator/master/TrinityCreator/Properties/AssemblyInfo.cs", Assembly.GetExecutingAssembly());
                if (force)
                    AutoUpdater.ReportErrors = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Updater Error: " + ex.Message, "Updater", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private static void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            // Get latest version
            string currentVersionLine =
                args.RemoteData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None) // Assembly.cs split by line
                .Where(l => l.StartsWith("[assembly: AssemblyVersion")) // Find the correct line
                .FirstOrDefault();

            // Grab version
            Regex rVersion = new Regex("AssemblyVersion\\(\"(\\S+)\"\\)");
            Version latestVersion = Version.Parse(rVersion.Match(currentVersionLine).Groups[1].Value);

            // Set update info
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                Mandatory = false,
                InstalledVersion = Assembly.GetExecutingAssembly().GetName().Version,
                CurrentVersion = latestVersion,
                ChangelogURL = "https://github.com/NotCoffee418/TrinityCreator/commits/master#branch-select-menu",
                DownloadURL = "https://github.com/NotCoffee418/TrinityCreator/raw/master/TrinityCreator/bin/Publish/TrinityCreator.zip"
            };
        }

    }
}