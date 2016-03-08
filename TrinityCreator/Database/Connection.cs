using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TrinityCreator.Database
{
    class Connection
    {
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

            try
            {
                MySqlConnection conn = new MySqlConnection(conn_string.ToString());
                conn.Open();
                conn.Close();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
