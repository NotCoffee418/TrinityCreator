using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using TrinityCreator.Properties;
using System.Windows.Threading;
using System.Threading;

namespace TrinityCreator.Database
{
    internal class Connection
    {
        private static MySqlConnection _conn;

        public static bool IsAlive { get; private set; }

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
            else if (connString == Settings.Default.worldDb && IsAlive)
                return null;

            try
            {
                var c = new MySqlConnection(connString.ToString());
                c.Open();
                c.Close();
                return null;
            }
            catch (Exception ex)
            {
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
            if (IsAlive)
                return true;

            if (!IsConfigured())
            {
                if (requestConfig)
                    RequestConfiguration();
                return false;
            }

            try
            {
                _conn = new MySqlConnection(Settings.Default.worldDb.ToString());
                _conn.Open();
                IsAlive = true;
                return true;
            }
            catch (Exception ex)
            {
                var msg = string.Format("Opening the database connection resulted in the following error:{0}{1}{0}{0}" +
                                        "Would you like to try again?", Environment.NewLine, ex.Message);
                var r = MessageBox.Show(msg, "Failed to connect", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (r == MessageBoxResult.Yes)
                {
                    Open();
                    return true;
                }
                else return false;
            }
        }

        internal static void Close()
        {
            if (!IsAlive)
                return;

            _conn.Close();
            IsAlive = false;
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
            if (IsAlive)
                return true;
            if (Settings.Default.worldDb == "")
                return false;
            return true;
        }

        internal static DataTable ExecuteQuery(string query, bool requestConfig = true)
        {
            Open(requestConfig);
            if (!IsAlive)
                return new DataTable();
            var result = new DataTable();
            var cmd = new MySqlCommand(query, _conn);
            using (var rdr = cmd.ExecuteReader())
            {
                result.Load(rdr);
            }
            return result;
        }

        internal static object ExecuteScalar(string query, bool requestConfig = true)
        {
            Open(requestConfig);
            if (!IsAlive)
                return null;
            var cmd = new MySqlCommand(query, _conn);
            return cmd.ExecuteScalar();
        }
    }
}