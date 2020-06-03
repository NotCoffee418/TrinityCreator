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
using System.IO.Compression;
using TrinityCreator.Profiles;

namespace TrinityCreator.Helpers
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
                Logger.Log("Updater: Running Updater.");
                AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
                if (force)
                {
                    AutoUpdater.ReportErrors = true;
                    AutoUpdater.Mandatory = true;
                }
                AutoUpdater.Start("https://raw.githubusercontent.com/NotCoffee418/TrinityCreator/master/TrinityCreator/Properties/AssemblyInfo.cs", Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Logger.Log("Updater: Error: " + ex.Message, Logger.Status.Error, true);
            }

        }

        private static void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            // Default data in case of problem
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                Mandatory = false,
                InstalledVersion = Assembly.GetExecutingAssembly().GetName().Version,
                CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version,
            };

            Logger.Log($"Updater: Currently running on version {args.UpdateInfo.InstalledVersion}");
            Logger.Log("Updater: AutoUpdaterOnParseUpdateInfoEvent");
            // Get latest version
            String currentVersionLine =
                args.RemoteData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None) // Assembly.cs split by line
                .Where(l => l.StartsWith("[assembly: AssemblyVersion")) // Find the correct line
                .FirstOrDefault();

            // This can happen if the above string doesnt match or github changes their url structure
            // Or public wifi or something along those lines
            if (currentVersionLine == null)
            {
                // Pretend that latest version is current version, warn user
                Logger.Log("Updater: Failed to load version info. You need to update manually.", Logger.Status.Error, true);
                Logger.Log("Updater: Returned text:" + Environment.NewLine + args.RemoteData);                
                return;
            }

            // Grab version
            Logger.Log("Updater: Version line found. Attempting to parse latest version.");
            try
            {
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
                Logger.Log($"Updater: Latest version is {args.UpdateInfo.CurrentVersion}. Installed version is {args.UpdateInfo.InstalledVersion}.");
            }
            catch
            {
                Logger.Log("Updater: Failed to parse version data. You may have to update manually.", Logger.Status.Error, true);
            }            
        }

        public static void UpdateProfiles()
        {
            // Update Profiles from https://github.com/NotCoffee418/TrinityCreatorProfiles
            Logger.Log("Updater: Updating Profiles from GitHub.");

            // Get relevant directories
            string sysProfilesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NotCoffee418", "TrinityCreator", "Profiles");
            string usrProfilesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TrinityCreator", "Profiles");
            string tmpDir = Path.Combine(Path.GetTempPath(), "tc-" + GHelper.RandomString(6));

            // Download repository, update any new files
            try
            {
                // Generate user profile directory if it doesn't exist (this function doesn't affect this folder aside from creating it)
                if (!Directory.Exists(usrProfilesDir))
                    Directory.CreateDirectory(usrProfilesDir);

                // Generate system profile directory if it doesn't exist
                if (!Directory.Exists(sysProfilesDir))
                    Directory.CreateDirectory(sysProfilesDir);

                // Generate tmp directory if it doesn't exist
                if (!Directory.Exists(tmpDir))
                    Directory.CreateDirectory(tmpDir);

                // Download latest version of profiles
                string zipPath = Path.Combine(tmpDir, "latest.zip");
                string sourceUrl = "https://github.com/NotCoffee418/TrinityCreatorProfiles/archive/master.zip";
                Logger.Log("Updater: Downloading updated profiles from '" + sourceUrl);
                using (var client = new WebClient())
                {
                    client.DownloadFile(sourceUrl, zipPath);
                }

                // Extract download & delete zip
                Logger.Log("Updater: Extracting downloaded profiles and cleaning zip. Path: " + tmpDir);
                ZipFile.ExtractToDirectory(zipPath, tmpDir);
                File.Delete(zipPath);

                // Check each file to see if it's updated (using revision)
                Logger.Log("Updater: Installing updated profiles...");
                bool isOutdatedTcForProfiles = false;
                foreach (string tmpFilePath in Directory.GetFiles(Path.Combine(tmpDir, "TrinityCreatorProfiles-master")))
                {
                    string fileName = Path.GetFileName(tmpFilePath);
                    string profileDestPath = Path.Combine(sysProfilesDir, fileName);

                    Logger.Log("Checking for new version on " + fileName);
                    if (!File.Exists(profileDestPath))
                    {
                        Logger.Log(fileName + ": is a new profile. Installing...");
                        File.Copy(tmpFilePath, profileDestPath);
                    }
                    else // profile exists, check if it's a new version
                    {
                        Profile localVer = Profile.LoadFile(profileDestPath, false); // Not error display, versions for older version of app will default to rev 0
                        Profile remoteVer = Profile.LoadFile(tmpFilePath, true); // display error, if the downloaded version is corrupt, we have an issue

                        if (remoteVer.Revision > localVer.Revision) // New version found, install
                        {
                            Logger.Log($"Updater: {fileName} - New version found.");

                            int runningAppBuild = Assembly.GetExecutingAssembly().GetName().Version.Revision;
                            if (remoteVer.TestedBuild > runningAppBuild) // profile may not support this version of Tcreator
                            {
                                // used to show warning, doesn't update
                                isOutdatedTcForProfiles = true;
                                Logger.Log($"Updater: Profile {fileName} was tested on a newer version of Trinity Creator. Not updating or installing local file.",
                                    Logger.Status.Warning, false);
                            }
                            else
                            {
                                Logger.Log($"Updater: Updating {fileName} from revision {localVer.Revision} to {remoteVer.Revision}.");
                                File.Copy(tmpFilePath, profileDestPath, overwrite: true);
                            }
                        }
                        else // Up to date
                            Logger.Log($"{fileName}: Up to date.");
                    }
                }

                // Show warning profiles are newer than build
                if (isOutdatedTcForProfiles)
                    Logger.Log("Profile: Some profiles were not installed or updated as this version of Trinity Creator may not support them." +
                        Environment.NewLine + "Please update Trinity Creator to resolve this issue. (Help > Check for updates)", Logger.Status.Warning, true);
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to update profiles from GitHub: " + ex.Message, Logger.Status.Warning, true);
            }
            finally
            {
                // Delete temp dir
                if (Directory.Exists(tmpDir))
                    Directory.Delete(tmpDir, recursive: true);
            }
        }

    }
}