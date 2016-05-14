using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrinityCreator.Database;

namespace TrinityCreator
{
    class ExportQuery
    {
        /// <summary>
        /// Export to sql file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="query"></param>
        public static void ToFile(string filename, string query)
        {
            var sfd = new SaveFileDialog();
            sfd.DefaultExt = ".sql";
            sfd.FileName = filename;
            sfd.Filter = "SQL File (.sql)|*.sql";
            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, query);
                MessageBox.Show("Your creation has been saved to a file.", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// insert into database
        /// </summary>
        /// <param name="query"></param>
        public static void ToDatabase(string query)
        {
            if (Connection.Open())
            {                
                if (Connection.IsAlive)
                {
                    Connection.ExecuteQuery(query);
                    MessageBox.Show("Your creation has been saved to the database.", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            MessageBox.Show("Your creation was not saved to the database.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
