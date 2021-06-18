using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using TrinityCreator.Properties;
using System.Windows.Threading;
using System.Threading;
using TrinityCreator.UI;
using TrinityCreator.Helpers;
using TrinityCreator.Data;
using System.Text.RegularExpressions;

namespace TrinityCreator.Database
{
    internal class Connection
    {
        private static MySqlConnection _conn;

        /// <summary>
        ///     Input db info to test
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="dbName"></param>
        /// <returns>null on success or exception</returns>
        internal static Exception Test(string connString = "")
        {
            if (connString == "")
                connString = Settings.Default.worldDb;
            else if (connString == Settings.Default.worldDb && Open())
                return null;

            try
            {
                var c = new MySqlConnection(connString.ToString());
                c.Open();
                c.Close();
                Logger.Log("MySQL: Connection.Test() successful.");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Log($"MySQL: Connection.Test() failed with error: {ex.Message}");
                return ex;
            }
        }


        /// <summary>
        /// Opens or reopens connection
        /// </summary>
        /// <param name="requestConfig">Asked to enter configuration?</param>
        /// <returns></returns>
        internal static bool Open(bool requestConfig = true)
        {
            if (!IsConfigured())
            {
                Logger.Log("MySQL: Attempting to open connection but database login is not configured yet.");
                if (requestConfig)
                    RequestConfiguration();
                return false;
            }

            try
            {
                if (_conn != null && _conn.State == ConnectionState.Open)
                    return true; // connection is still open
                else
                {
                    Logger.Log("MySQL: Attemption to open MySQL connection...");
                    _conn = new MySqlConnection(Settings.Default.worldDb.ToString() +"charset=utf8;");
                    _conn.Open();
                    Logger.Log("MySQL: Successfully connected.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"MySQL: Failed to connect to MySQL with previously valid credentials. Error: {ex.Message}", Logger.Status.Warning, false);
                if (requestConfig) // important query
                {
                    var msg = string.Format("Opening the database connection resulted in the following error:{0}{1}{0}{0}" +
                                            "Would you like to try again?", Environment.NewLine, ex.Message);
                    var r = MessageBox.Show(msg, "Failed to connect", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (r == MessageBoxResult.Yes)
                    {
                        Logger.Log("MySQL: User prompted to retry...");                        
                        return Open();
                    }
                }
                Logger.Log("MySQL: Connection.Open() User prompted to give up with these credentials.");
                return false;
            }
        }

        internal static void Close()
        {
            if (_conn == null)
                return;

            Logger.Log("MySQL: Connection manually closed.");
            _conn.Close();
            _conn = null;
        }

        internal static void RequestConfiguration()
        {
            var r =
                MessageBox.Show(
                    "You can only use this feature by configuring and connecting to your world database. Would you like to configure your connection now?",
                    "Failed to connect", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (r == MessageBoxResult.OK)
                new DbConfigWindow().Show();
        }


        /// <summary>
        ///     Returns true if valid settings were entered at some point.
        ///     Does not confirm if settings are valid right now
        /// </summary>
        /// <returns></returns>
        internal static bool IsConfigured()
        {
            if (Settings.Default.worldDb == "")
            {
                Logger.Log("MySQL: Connection has not been configured yet.");
                return false;
            }
            else
            {
                Logger.Log("MySQL: Connection was correctly configured at some point.");
                return true;
            }
        }

        internal static (DataTable, bool) ExecuteQuery(string query, bool requestConfig = true)
        {
            Open(requestConfig);
            try
            {
                Logger.Log("MySQL: Attempting to ExecuteQuery: " + query);
                if (_conn.State != ConnectionState.Open)
                {
                    Logger.Log("MySQL: IsAlive was false. Returning empty datatable.");
                    HandleException(new CExceptions.DatabaseOffline());
                    return (new DataTable(), false);
                }                    
                var result = new DataTable();
                var cmd = new MySqlCommand(query, _conn);
                using (var rdr = cmd.ExecuteReader())
                {
                    result.Load(rdr);
                }
                Logger.Log("MySQL: Successfully return data.");
                return (result, true);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return (new DataTable(), false);
            }
        }

        internal static (object, bool) ExecuteScalar(string query, bool requestConfig = true)
        {
            Open(requestConfig);
            try
            {
                Logger.Log("MySQL: Attempting to ExecuteScalar: " + query);
                if (_conn.State != ConnectionState.Open)
                {
                    Logger.Log("MySQL: IsAlive was false. Returning null.");
                    HandleException(new CExceptions.DatabaseOffline());
                    return (null, false);
                }
                var cmd = new MySqlCommand(query, _conn);
                var result = cmd.ExecuteScalar();
                Logger.Log("MySQL: Successfully return data.");
                return (result, true);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return (null, false);
            }
        }

        internal static bool ExecuteNonQuery(string query, bool requestConfig = true)
        {
            Open(requestConfig);
            try
            {
                Logger.Log("MySQL: Attempting to ExecuteNonQuery: " + query);
                if (_conn.State != ConnectionState.Open)
                {
                    Logger.Log("MySQL: IsAlive was false. Returning false.");
                    HandleException(new CExceptions.DatabaseOffline());
                    return false;
                }
                var cmd = new MySqlCommand(query, _conn);
                cmd.ExecuteNonQuery();
                Logger.Log("MySQL: Successfully executed.");
                return true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private static void HandleException(Exception ex)
        {
            // Log always to file
            Logger.Log("Raw DB Error: " + ex.Message, Logger.Status.Info, false);

            // If database offline
            if (ex.GetType() == typeof(CExceptions.DatabaseOffline))
            {
                Logger.Log("Connection to the database has failed. Try reconnecting.", Logger.Status.Error, showMessageBox: true);
                return;
            }

            // Unknown column
            Regex unknownColumnRx = new Regex(@"Unknown column '(\S+)' in 'field list'");
            if (unknownColumnRx.IsMatch(ex.Message))
            {
                var rxResult = unknownColumnRx.Match(ex.Message);
                Logger.Log($"Column not found in database: {rxResult.Groups[1]}" + Environment.NewLine +
                    "This error is usually caused by not using the correct profile for your emulator/repack." + Environment.NewLine +
                    "You can change your profile in Top Menu > Config > Choose Emulator Profile.", Logger.Status.Error, showMessageBox: true);

                return;
            }

            // else
            Logger.Log("Undefined database exception: " + ex.Message, Logger.Status.Error, showMessageBox: true);
        }
    }
}