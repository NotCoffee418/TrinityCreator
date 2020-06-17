using Microsoft.Win32;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TrinityCreator.Database;
using TrinityCreator.Helpers;
using TrinityCreator.Profiles;
using TrinityCreator.Tools.QuestCreator;

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
                    Connection.ExecuteNonQuery(query);
                    Logger.Log("Your creation has been saved to the database.", Logger.Status.Info, true);
                    return;
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




        /// <summary>
        /// Checks if an ID is already in use. Prompts to overwrite if needed.
        /// Deletes original row with given id if user agrees.
        /// </summary>
        /// <param name="type">creation type</param>
        /// <param name="id">creation id</param>
        /// <returns>True if user gave permission to insert</returns>
        public static bool CheckDuplicateHandleOverride(Export.C type, int id)
        {
            // Get primary keys to base update queries on
            var fieldsToCheck = GetRelevantTablesAndIds(type, id);
            if (fieldsToCheck == null)
                return true; // Irrelevant to this type or error

            // Look for duplicates
            bool alreadyExists = false;
            try
            {
                // Query every relevant table to see if ID  already exists
                foreach (var kvp in fieldsToCheck)
                {
                    if (kvp.Key.Contains("%t"))
                        continue; // I'm not gonna even

                    var result = Connection.ExecuteScalar($"SELECT COUNT(*) AS matches FROM {kvp.Key} WHERE {kvp.Value} = {id};");
                    if ((dynamic)result > 0)
                    {
                        alreadyExists = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Database Error: There was an error looking for duplicates. Creation will not export.", Logger.Status.Error, true);
                Logger.Log(ex.Message);
                return false; // Failed, cancel insert
            }

            // Ask if user wants to overwrite
            if (alreadyExists)
            {
                var dr = MessageBox.Show($"WARNING: {type} with ID {id} already exists.{Environment.NewLine}Would you like to replace it?",
                    "Already Exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
                if (dr == MessageBoxResult.Yes)
                {
                    
                    // Delete from database
                    foreach (var kvp in fieldsToCheck)
                    {
                        // Handle quest customs
                        if (type == Export.C.Quest)
                        {
                            // Handle %t tablenames
                            if (kvp.Key.Contains("%t"))
                            {
                                string[] starterEnderTypes = new string[]
                                {
                                    "creature",
                                    "gameobject",
                                };

                                string overrideTableName = "";

                                foreach (string replacement in starterEnderTypes)
                                {
                                    if (kvp.Key.Contains("queststarter"))
                                    {
                                        overrideTableName = kvp.Key.Replace("%t", replacement);
                                        Connection.ExecuteNonQuery($"DELETE FROM {overrideTableName} WHERE {kvp.Value} = {id};");
                                    }
                                    else if (kvp.Key.Contains("questender"))
                                    {
                                        overrideTableName = kvp.Key.Replace("%t", replacement);
                                        Connection.ExecuteNonQuery($"DELETE FROM {overrideTableName} WHERE {kvp.Value} = {id};");
                                    }
                                    else
                                    {
                                        Logger.Log("Profile Error: QuestGiver customs with wildcard only accept tablenames containing 'queststarter' and 'questender'." + Environment.NewLine +
                                            "This is because customs have no other way to identify that the custom is in relation to queststarter/ender." + Environment.NewLine +
                                            "If this causes issues, it's preferable to just use 'creature_questender' instead of '%t_questender' and starter.",
                                            Logger.Status.Error, true);
                                    }
                                }                                
                            }
                        }
                        else if (type == Export.C.Creature)
                        {
                            if (Profile.Active.IsKeyDefined(Export.C.Creature, "SpellCreatureID"))
                            {
                                var d = new ExpKvp("SpellCreatureID", null, Export.C.Creature);
                                Connection.ExecuteNonQuery($"DELETE FROM {d.SqlTableName} WHERE {d.SqlKey} = {id};");
                            }
                            if (Profile.Active.IsKeyDefined(Export.C.Creature, "ResistanceCreatureId"))
                            {
                                var d = new ExpKvp("ResistanceCreatureId", null, Export.C.Creature);
                                Connection.ExecuteNonQuery($"DELETE FROM {d.SqlTableName} WHERE {d.SqlKey} = {id};");
                            }
                            if (Profile.Active.IsKeyDefined(Export.C.Creature, "InhabitCreatureID"))
                            {
                                var d = new ExpKvp("InhabitCreatureID", null, Export.C.Creature);
                                Connection.ExecuteNonQuery($"DELETE FROM {d.SqlTableName} WHERE {d.SqlKey} = {id};");
                            }
                        }

                        // No special, just delete.
                        if (!kvp.Key.Contains("%t"))
                            Connection.ExecuteNonQuery($"DELETE FROM {kvp.Key} WHERE {kvp.Value} = {id};");
                    }                       

                    return true; // Duplicate, deleted & permission to insert
                }
                else return false; // Duplicate found, user does not wish to override
            }
            else return true; // Not duplicate, proceed as normal.
        }

        /// <summary>
        /// Used for UPDATE queries, determines the correct SqlKeys to base the query on
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns>tablename, id column</returns>
        public static Dictionary<string, string> GetRelevantTablesAndIds(Export.C type, int id)
        {
            // Table name, sqlKey
            Dictionary<string, string> fieldsToCheck = new Dictionary<string, string>();
            string idAppKey = string.Empty;

            try
            {
                // Select correct dictionary to search for creation
                Dictionary<string, Dictionary<string, string>> targetDict = null;
                switch (type)
                {
                    case Export.C.Creature:
                        targetDict = Profile.Active.Creature;
                        idAppKey = "Entry";
                        break;
                    case Export.C.Quest:
                        targetDict = Profile.Active.Quest;
                        idAppKey = "EntryId";
                        break;
                    case Export.C.Item:
                        targetDict = Profile.Active.Item;
                        idAppKey = "EntryId";
                        break;

                    // Other tools allow duplicate IDs and this method doesn't apply to them.
                    // Defining tools here that allow duplicate entry IDs will break UPDATE queries for them because spaghetti.
                    default:
                        return null;
                }


                // Generate list of tables and IDs from profile
                // Add primary table
                var primTableData = targetDict
                    .Where(x => x.Value.Keys.Contains(idAppKey))
                    .First();
                fieldsToCheck.Add(primTableData.Key, primTableData.Value[idAppKey]);

                // Add secondary tables
                foreach (var tablesDict in Profile.Active.CustomFields)
                    foreach (var keysDict in tablesDict.Value)
                        if (keysDict.Key.StartsWith(type.ToString()))
                            fieldsToCheck.Add(tablesDict.Key, keysDict.Value);

                // Add special cases
                if (type == Export.C.Creature)
                {

                }

                // Return result
                return fieldsToCheck;
            }
            catch (Exception ex)
            {
                Logger.Log($"Profile: CanWriteHandleOverride failed to determine ID of a table for {type}. Export failed.", Logger.Status.Error, true);
                Logger.Log(ex.Message);
                return null; // Failed, cancel insert
            }
        }

        public static void DeleteCreation(Export.C type, int id)
        {
            Logger.Log($"Requesting to delete {type} {id}");
            var relTabIds = GetRelevantTablesAndIds(type, id);
            foreach (var kvp in relTabIds)
                Connection.ExecuteNonQuery($"DELETE FROM {kvp.Key} WHERE {kvp.Value}={id};");
        }
    }
}
