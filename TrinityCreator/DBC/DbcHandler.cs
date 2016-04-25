using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace TrinityCreator.DBC
{
    class DbcHandler
    {
        public static bool VerifyDbcDir()
        {
            string dbcDir = Properties.Settings.Default.DbcDir;

            if (File.Exists(GetDbcFilePath("AreaTable")))
                return true;
            else // DBC configuration invalid or incomplete
            {
                if (!Directory.Exists(dbcDir)) // try to find wow dir, dbc dir deleted?
                {
                    string[] parts = dbcDir.Split('\\');
                    dbcDir = "";
                    for (int i = 0; i < parts.Length - 1; i++) // all but last 
                        dbcDir += parts[i] + "\\";
                }

                // Check if this is wow dir
                if (File.Exists(dbcDir + @"\Wow.exe"))
                {
                    if (File.Exists(GetDbcFilePath("AreaTable", dbcDir + @"\dbc"))) // Does selected wow dir contain a dbc dir?
                    {
                        Properties.Settings.Default.DbcDir = dbcDir + @"\dbc";
                        Properties.Settings.Default.Save();
                        return true;
                    }
                    else
                    {
                        var r = MessageBox.Show("DBC files have not been extracted. Would you like to extract them now with ad.exe?", "Extract DBC", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (r == MessageBoxResult.Yes)
                            ExtractDbc(dbcDir);
                    }
                }
                else // invalid directory
                {
                    var r = MessageBox.Show("WoW or DBC directory is not set correctly.\r\nWould you like to set it now?", "Invalid DBC Settings", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (r == MessageBoxResult.Yes)
                        new DbcConfigWindow().Show();
                }
                return false;
            }
        }

        private static void ExtractDbc(string wowRoot)
        {
            string adPath = wowRoot + @"\mapextractor.exe"; // try trinity's extractor first
            if (!File.Exists(adPath))
                adPath = wowRoot + @"\ad.exe";
            if (!File.Exists(adPath)) // download ad.exe from cmangos repo
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile("https://github.com/cmangos/mangos-wotlk/blob/master/contrib/extractor_binary/ad.exe?raw=true", adPath);
                }

            // Start process
            var proc = new ProcessStartInfo(adPath);
            proc.WorkingDirectory = wowRoot;
            Process.Start(proc);

            // Info
            MessageBox.Show("You can close the console window now. No need to extract maps.");
        }

        /// <summary>
        /// Returns full file path of a dbc file
        /// </summary>
        /// <param name="name">DBC Name without extension</param>
        /// <param name="path">optional custom path</param>
        /// <returns></returns>
        private static string GetDbcFilePath(string name, string path = "")
        {
            if (path == "")
                path = Properties.Settings.Default.DbcDir;
            return string.Format("{0}\\{1}.dbc", path, name);
        }
    }
}
