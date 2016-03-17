using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.WowClient
{
    /// <summary>
    /// Gets data from previously extracted DBC files 3.3.5a
    /// You can use different DBC files by placing them by creating a DBC directory in your
    /// TrinityCore installation folder and placing your DBCUtil converted csvfiles in there.
    /// example file: C:\Program Files\TrinityCreator\DBC\AreaTable.csv
    /// </summary>
    public class DbcReader
    {
        private static DataTable ReadCsvFile(string dbcName)
        {
            DataTable dt = new DataTable();
            string csvText = "";
            try
            {
                if (File.Exists(string.Format(@"DBC\{0}.csv", dbcName)))
                    csvText = File.ReadAllText(string.Format(@"DBC\{0}.csv", dbcName));
                else
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = dbcName + ".csv";

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        csvText = reader.ReadToEnd();
                    }
                }

                if (csvText == "" || !csvText.Contains(","))
                    throw new Exception("Invalid CSV File.");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed t read " + dbcName + ".csv" + Environment.NewLine + ex.Message);
            }

            return dt;
        }

        public static Faction[] GetFactions()
        {
            DataTable factions = ReadCsvFile("Factions.dbc");
            return null;
        }
    }
}
