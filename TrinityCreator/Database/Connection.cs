using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;

namespace TrinityCreator.Database
{
    class Connection
    {
        private static bool _isAlive;
        private static MySqlConnection conn;

        public static bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            private set
            {
                _isAlive = value;
            }
        }

        /// <summary>
        /// Input db info to test
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="dbName"></param>
        /// <returns>null on success or exception</returns>
        internal static Exception Test(MySqlConnectionStringBuilder conn_string = null)
        {
            if (conn_string == null)
                conn_string = Properties.Settings.Default.worldDb;
            else if (conn_string == Properties.Settings.Default.worldDb && IsAlive)
                return null;

            try
            {
                MySqlConnection c = new MySqlConnection(conn_string.ToString());
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
                conn = new MySqlConnection(Properties.Settings.Default.worldDb.ToString());
                IsAlive = true;
            }
            catch (Exception ex)
            {
                string msg = string.Format("Opening the database connection resulted in the following error:{0}{1}{0}{0}"+
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

        private static void RequestConfiguration()
        {
            var r = MessageBox.Show("You can only use this feature by configuring and connecting to your world database. Would you like to configure your connection now?",
                "Failed to connect", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (r == MessageBoxResult.OK)
                new DbConfigWindow().Show();
        }


        /// <summary>
        /// Returns true if valid settings were entered at some point.
        /// Does not confirm if settings are valid right now
        /// </summary>
        /// <returns></returns>
        internal static bool IsConfigured()
        {
            if (IsAlive)
                return true;
            else if (Properties.Settings.Default.worldDb == null)
                return true;
            else return false;
        }
    }
}
