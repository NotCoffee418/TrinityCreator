using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Data;
using DBCViewer;
using System.Globalization;
using System.Xml;
using TrinityCreator.UI;
using TrinityCreator.Helpers;

namespace TrinityCreator.DBC
{
    class DbcHandler
    {

        public static bool VerifyDbcDir(bool openConfigWindowOnError = true)
        {
            string dbcDir = Properties.Settings.Default.DbcDir;

            if (File.Exists(GetDbcFilePath("AreaTable")))
            {
                Logger.Log($"DBC: DBC directory {dbcDir} appears to be valid.");
                return true;
            }
            else // DBC configuration invalid or incomplete
            {
                Logger.Log("DBC: DBC directory is invalid, removed or was never configured.", Logger.Status.Warning, false);
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
                    Logger.Log($"DBC: Selected DBC directory is WoW directory. ({dbcDir})");
                    if (File.Exists(GetDbcFilePath("AreaTable", dbcDir + @"\dbc"))) // Does selected wow dir contain a dbc dir?
                    {
                        Properties.Settings.Default.DbcDir = dbcDir + @"\dbc";
                        Properties.Settings.Default.Save();
                        Logger.Log($"DBC: Changed DBC directory to ({Properties.Settings.Default.DbcDir}). Is valid DBC directory.");
                        return true;
                    }
                    else
                    {
                        Logger.Log("DBC: No DBC directory found in this WoW directory. Request automatic extraction.");
                        var r = MessageBox.Show("DBC files have not been extracted. Would you like to extract them now with ad.exe?", "Extract DBC", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (r == MessageBoxResult.Yes)
                            ExtractDbc(dbcDir);
                        return false;
                    }
                }
                
                // invalid directory
                else if (openConfigWindowOnError)
                {
                    Logger.Log($"DBC: Known DBC directory ({dbcDir}) is invalid. Prompting for setup.");
                    var r = MessageBox.Show("WoW or DBC directory is not set correctly.\r\nWould you like to set it now?", "Invalid DBC Settings", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (r == MessageBoxResult.Yes)
                        new DbcConfigWindow().Show();
                }
                else Logger.Log("The selected DBC directory is not valid.", Logger.Status.Warning, true);
                return false;
            }
        }

        private static void ExtractDbc(string wowRoot)
        {
            Logger.Log("DBC: Extracting DBC through ExtractDbc(). Looking for TrinityCore's mapextractor.exe...");
            Logger.Log("DBC: If the application crashes during extraction, user should delete ad.exe and mapextractor.exe from wow client directory.");
            string adPath = wowRoot + @"\mapextractor.exe"; // try trinity's extractor first
            if (!File.Exists(adPath))
            {
                adPath = wowRoot + @"\ad.exe";
                Logger.Log("DBC: mapextractor.exe wasn't found. Trying ad.exe...");
            }
            if (!File.Exists(adPath)) // download ad.exe from cmangos repo
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        Logger.Log("DBC: Neither were found. Downloading ad.exe from https://github.com/cmangos/mangos-wotlk/.");
                        wc.DownloadFile("https://github.com/cmangos/mangos-wotlk/blob/b011fe90f20ee35ca8ef3d5a1ffa92ad0259b22c/contrib/extractor_binary/ad.exe?raw=true", adPath);
                    }
                }
                catch
                {
                    Logger.Log("DBC: Failed to download DBC extractor. Please ensure you have an active internet connection or extract DBC files manually.", 
                        Logger.Status.Error, showMessageBox:true);
                    return;
                }

            try
            {
                // Start process
                Logger.Log("DBC: Trying to run DBC extractor: " + Path.GetFileName(adPath));
                var proc = new ProcessStartInfo(adPath);
                proc.WorkingDirectory = wowRoot;
                Process.Start(proc);

                // Info
                Logger.Log("You can close the console window now. No need to extract maps, unless you need them for your emulator.", Logger.Status.Info, true);
            }
            catch
            {
                Logger.Log("DBC: There was an error extracting DBC files. Please remove ad.exe/mapextractor.exe from your WoW directory and try again or extract your DBC files manually.",
                    Logger.Status.Error, true);
            }
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

        /// <summary>
        /// Get DataTable from DBC file, can be modified to support other versions of WoW
        /// </summary>
        /// <param name="DbcName">DBC file name without ext</param>
        /// <returns></returns>
        public static DataTable LoadDbc(string dbcName, string[] returnColumns, int build = 12340)
        {
            if (!VerifyDbcDir())
            {
                DataTable error = new DataTable();
                foreach (string col in returnColumns)
                    error.Columns.Add(col);
                error.Rows.Add("Failed to load DBC file.");
                Logger.Log($"DBC: Error loading DBC file {dbcName}. Likely not configured correctly.", Logger.Status.Warning);
                return error;
            }

            string file = GetDbcFilePath(dbcName);
            var m_dbreader = DBReaderFactory.GetReader(file);
            Logger.Log($"DBC: Attempting to load DBC file: {file}");

            // Filter definitions
            XmlDocument m_definitions = new XmlDocument();
            m_definitions.LoadXml(Properties.Resources.dbclayout);

            // Get correct definition
            XmlElement m_definition = (XmlElement)m_definitions.SelectSingleNode("/DBFilesClient/" + dbcName + "[@build='" + build + "']");
            if (m_definition == null) // try with build 0
                m_definition = (XmlElement)m_definitions.SelectSingleNode("/DBFilesClient/" + dbcName + "[@build='0']");
            if (m_definition == null) // return error
            {
                DataTable error = new DataTable();
                error.Columns.Add("Error");
                error.Rows.Add("No definitions for this table on this build.");
                return error;
            }

            var m_fields = m_definition.GetElementsByTagName("field");
            string[] types = new string[m_fields.Count];
            for (int j = 0; j < m_fields.Count; ++j)
                types[j] = m_fields[j].Attributes["type"].Value;
            var m_dataTable = new DataTable(dbcName);
            m_dataTable.Locale = CultureInfo.InvariantCulture;

            // Create columns
            foreach (XmlElement field in m_fields)
            {
                var colName = field.Attributes["name"].Value;

                switch (field.Attributes["type"].Value)
                {
                    case "long":
                        m_dataTable.Columns.Add(colName, typeof(long));
                        break;
                    case "ulong":
                        m_dataTable.Columns.Add(colName, typeof(ulong));
                        break;
                    case "int":
                        m_dataTable.Columns.Add(colName, typeof(int));
                        break;
                    case "uint":
                        m_dataTable.Columns.Add(colName, typeof(uint));
                        break;
                    case "short":
                        m_dataTable.Columns.Add(colName, typeof(short));
                        break;
                    case "ushort":
                        m_dataTable.Columns.Add(colName, typeof(ushort));
                        break;
                    case "sbyte":
                        m_dataTable.Columns.Add(colName, typeof(sbyte));
                        break;
                    case "byte":
                        m_dataTable.Columns.Add(colName, typeof(byte));
                        break;
                    case "float":
                        m_dataTable.Columns.Add(colName, typeof(float));
                        break;
                    case "double":
                        m_dataTable.Columns.Add(colName, typeof(double));
                        break;
                    case "string":
                        m_dataTable.Columns.Add(colName, typeof(string));
                        break;
                    default:
                        throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Unknown field type {0}!", field.Attributes["type"].Value));
                }
            }

            // Create indexes
            XmlNodeList indexes = m_definition.GetElementsByTagName("index");
            var columns = new DataColumn[indexes.Count];
            var idx = 0;
            foreach (XmlElement index in indexes)
                columns[idx++] = m_dataTable.Columns[index["primary"].InnerText];
            m_dataTable.PrimaryKey = columns;
            
            try
            {
                // Create rows
                foreach (var row in m_dbreader.Rows) // Add rows
                {
                    DataRow dataRow = m_dataTable.NewRow();

                    using (BinaryReader br = row)
                    {
                        for (int j = 0; j < m_fields.Count; ++j)    // Add cells
                        {
                            switch (types[j])
                            {
                                case "long":
                                    dataRow[j] = br.ReadInt64();
                                    break;
                                case "ulong":
                                    dataRow[j] = br.ReadUInt64();
                                    break;
                                case "int":
                                    dataRow[j] = br.ReadInt32();
                                    break;
                                case "uint":
                                    dataRow[j] = br.ReadUInt32();
                                    break;
                                case "short":
                                    dataRow[j] = br.ReadInt16();
                                    break;
                                case "ushort":
                                    dataRow[j] = br.ReadUInt16();
                                    break;
                                case "sbyte":
                                    dataRow[j] = br.ReadSByte();
                                    break;
                                case "byte":
                                    dataRow[j] = br.ReadByte();
                                    break;
                                case "float":
                                    dataRow[j] = br.ReadSingle();
                                    break;
                                case "double":
                                    dataRow[j] = br.ReadDouble();
                                    break;
                                case "string":
                                    if (m_dbreader is WDBReader)
                                        dataRow[j] = br.ReadStringNull();
                                    else if (m_dbreader is STLReader)
                                    {
                                        int offset = br.ReadInt32();
                                        dataRow[j] = (m_dbreader as STLReader).ReadString(offset);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            dataRow[j] = m_dbreader.StringTable[br.ReadInt32()];
                                        }
                                        catch
                                        {
                                            dataRow[j] = "Invalid string index!";
                                        }
                                    }
                                    break;
                                default:
                                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Unknown field type {0}!", types[j]));
                            }
                        }
                    }

                    m_dataTable.Rows.Add(dataRow);
                }
            }
            catch
            {
                // Lazy catch. See issue #104. Optimally relevant data should be hardcoded in the application with version restrictions
                // or definitions for all DBC files of all wow versions should be added to the current system.
                Logger.Log("DBC: Unexpected data structure or corrupt DBC file. Please ensure that you're using 3.3.5a DBC files.", 
                    Logger.Status.Error, true);
            }

            Logger.Log($"DBC: Successfully prepared datatable for {dbcName}.dbc.");
            return m_dataTable.DefaultView.ToTable(false, returnColumns);
        }
        
    }
}
