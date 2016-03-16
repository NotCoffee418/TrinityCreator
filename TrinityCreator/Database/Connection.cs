using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using TrinityCreator.Properties;

namespace TrinityCreator.Database
{
    internal class Connection
    {
        private static MySqlConnection conn;

        public static bool IsAlive { get; private set; }

        /// <summary>
        ///     Input db info to test
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="dbName"></param>
        /// <returns>null on success or exception</returns>
        internal static Exception Test(MySqlConnectionStringBuilder conn_string = null)
        {
            if (conn_string == null)
                conn_string = Settings.Default.worldDb;
            else if (conn_string == Settings.Default.worldDb && IsAlive)
                return null;

            try
            {
                var c = new MySqlConnection(conn_string.ToString());
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
        ///     Opens or reopens connection
        /// </summary>
        internal static void Open()
        {
            if (IsAlive)
                Close();

            if (!IsConfigured())
            {
                RequestConfiguration();
                return;
            }

            try
            {
                conn = new MySqlConnection(Settings.Default.worldDb.ToString());
                conn.Open();
                IsAlive = true;
            }
            catch (Exception ex)
            {
                var msg = string.Format("Opening the database connection resulted in the following error:{0}{1}{0}{0}" +
                                        "Would you like to try again?", Environment.NewLine, ex.Message);
                var r = MessageBox.Show(msg, "Failed to connect", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (r == MessageBoxResult.Yes)
                    Open();
            }
        }

        internal static void Close()
        {
            if (!IsAlive)
                return;

            conn.Close();
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
            if (Settings.Default.worldDb == null)
                return true;
            return false;
        }

        protected static DataTable ExecuteQuery(string query)
        {
            Open();

            var result = new DataTable();
            var cmd = new MySqlCommand(query, conn);
            using (var rdr = cmd.ExecuteReader())
            {
                result.Load(rdr);
            }
            return result;
        }
    }
}