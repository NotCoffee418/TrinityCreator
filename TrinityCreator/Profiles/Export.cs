using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TrinityCreator.Profiles
{
    class Export
    {
        // Creation type, used for requests to profile
        internal enum C
        {
            Creature,
            Quest,
            Item,
            Loot,
            Vendor
        }


        /// <summary>
        /// Generate SQL insert based on string[0] (table name), sting[1] column name and dynamic value
        /// </summary>
        /// <param name="data">Dictionary of string[] generated through Profile.gtk() and value</param>
        /// <returns></returns>
        private static string GenerateSql(Dictionary<string[], dynamic> data)
        {
            // Find table names
            List<string> tableNames = new List<string>();
            foreach (var entry in data)
                if (!tableNames.Contains(entry.Key[0]))
                    tableNames.Add(entry.Key[0]);

            // Prepare datatables
            List<DataTable> dtList = new List<DataTable>();

            // Generate Datatables
            foreach (var tableName in tableNames)
            {
                DataTable dt = new DataTable(tableName);
                // Create columns
                foreach (var entry in data.Where(e => e.Key[0] == tableName))
                    dt.Columns.Add(entry.Key[1], entry.Value.GetType());

                // Create row
                DataRow row = dt.NewRow();
                foreach (var entry in data.Where(e => e.Key[0] == tableName))
                    row[entry.Key[1]] = entry.Value;
                dt.Rows.Add(row);

                // Add to result tables
                dtList.Add(dt);
            }

            // Build mysql insert string (manually for now)
            string sql = String.Empty;
            foreach (var dt in dtList)
            {
                string[] colNames = dt.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToArray();

                // Generate query
                sql += "INSERT INTO " + dt.TableName +
                    " (" + string.Join(", ", colNames) + ") VALUES ";

                // Datermine how to write values into the query based on type
                List<string> rowValuesReady = new List<string>();
                foreach (string col in colNames)
                {
                    Type colType = dt.Columns[col].DataType;

                    // Handle string-y types
                    if (colType == typeof(String) || colType == typeof(string))
                    {
                        rowValuesReady.Add("'" + MySqlHelper.EscapeString((string)dt.Rows[0][col]) + "'");
                    }
                    else // handle numeric types
                    {
                        rowValuesReady.Add(dt.Rows[0][col].ToString().Replace(',', '.'));
                    }
                }

                // Generate row in query
                sql += "(" + string.Join(", ", rowValuesReady) + ")" + Environment.NewLine;
            }
            return sql;
        }

        // Loot is DDC based, loop through the entries
        public static string Loot(LootPage loot)
        {
            /*
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            foreach (LootRowControl row in loot.lootRowSp.Children)
            {
                // old
                var kvplist = new Dictionary<string, string>
                {
                    {"Entry", loot.entryTb.Text},
                    {"Item", row.Item.ToString()},
                    {"Chance", row.Chance.ToString()},
                    {"QuestRequired", Convert.ToInt16(row.QuestRequired).ToString()},
                    {"MinCount", row.MinCount.ToString()},
                    {"MaxCount", row.MaxCount.ToString()},
                };
                result.Add(kvplist);


                // new

            }
            return result.ToArray();

            */
            throw new NotImplementedException();
        }
    }
}
