using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrinityCreator.Database;
using TrinityCreator.Helpers;

namespace TrinityCreator
{
    class SaveQuery
    {
        /// <summary>
        /// Export to sql file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="query"></param>
        public static void ToFile(string filename, string query)
        {
            if (QueryHasIssues(query))
                return;

            try
            {
                // Prepare export dir
                string exportDir = System.IO.Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDoc‌​uments), "TrinityCreator", "Export");
                if (!Directory.Exists(exportDir))
                    Directory.CreateDirectory(exportDir);

                // Save file
                var sfd = new SaveFileDialog();
                sfd.DefaultExt = ".sql";
                sfd.FileName = filename;
                sfd.InitialDirectory = exportDir;
                sfd.Filter = "SQL File (.sql)|*.sql";
                if (sfd.ShowDialog() == true)
                {
                    File.WriteAllText(sfd.FileName, query);
                    Logger.Log("Your creation has been saved to a file.", Logger.Status.Info, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("There was an error exporting the query to a file. See log file for more info.", Logger.Status.Error, true);
                Logger.Log(ex.Message, Logger.Status.Error, false);
            }
        }

        /// <summary>
        /// insert into database
        /// </summary>
        /// <param name="query"></param>
        public static void ToDatabase(string query)
        {
            if (QueryHasIssues(query))
                return;

            try
            {
                // Insert to db
                if (Connection.Open())
                {
                    if (Connection.IsAlive)
                    {
                        Connection.ExecuteQuery(query);
                        Logger.Log("Your creation has been saved to the database.", Logger.Status.Info, true);
                        return;
                    }
                }
                else Logger.Log("There was an error connecting to the database. Your creation was not saved.", Logger.Status.Error, true);
            }
            catch (Exception ex)
            {
                Logger.Log("There was an error inserting the query to the database. See log file for more info.", Logger.Status.Error, true);
                Logger.Log(ex.Message, Logger.Status.Error, false);
            }
        }

        /// <summary>
        /// Displays errors or warnings if the query has issues and returns true if it found any
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static bool QueryHasIssues(string query)
        {
            if (query == "")
            {
                Logger.Log("Query is empty. Not inserting to database.", Logger.Status.Warning, true);
                return true;
            }
            else if (query.Contains("%t"))
            {
                Logger.Log("Your export appears to contain an unhandled wildcard. You may need to update Trinity Creator to the latest version.",
                    Logger.Status.Warning, true);
                return true;
            }
            return false;
        }
    }
}
