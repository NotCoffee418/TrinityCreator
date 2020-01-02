using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace TrinityCreator.Profiles
{
    public class Export
    {
        // Creation type, used for requests to profile
        public enum C
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
            try
            {
                // Find table names
                List<string> tableNames = new List<string>();
                foreach (var entry in data)
                    if (entry.Key != null && !tableNames.Contains(entry.Key[0]))
                        tableNames.Add(entry.Key[0]);

                // Prepare datatables
                List<DataTable> dtList = new List<DataTable>();

                // Generate Datatables
                foreach (var tableName in tableNames)
                {
                    DataTable dt = new DataTable(tableName);
                    // Create columns
                    foreach (var entry in data.Where(e => e.Key != null && e.Key[0] == tableName))
                        dt.Columns.Add(entry.Key[1], entry.Value.GetType());

                    // Create row
                    DataRow row = dt.NewRow();
                    foreach (var entry in data.Where(e => e.Key != null && e.Key[0] == tableName))
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
                        // Handle bool type
                        else if (colType == typeof(bool))
                        {
                            if ((bool)dt.Rows[0][col] == true)
                                rowValuesReady.Add("1");
                            else rowValuesReady.Add("0");
                        }
                        else // handle numeric types
                        {
                            rowValuesReady.Add(dt.Rows[0][col].ToString().Replace(',', '.'));
                        }
                    }

                    // Generate row in query
                    sql += "(" + string.Join(", ", rowValuesReady) + ");" + Environment.NewLine;
                }
                return sql;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}", Logger.Status.Error, true);
                return "";
            }
        }

        // Loot is DDC based, loop through the entries
        public static string Loot(LootPage loot)
        {
            // Loot table names are relative to a setting, use special table name in gtk
            string sp = ((ComboBoxItem)loot.lootTypeCb.SelectedValue).Content.ToString();

            // Loot supports multiple rows per "creation", hence the loop
            string sql = String.Empty;
            foreach (LootRowControl row in loot.lootRowSp.Children)
            {
                int entry = 0;
                try // Loot has an unusual system, need to manually parse the int
                {
                    entry = int.Parse(loot.entryTb.Text);
                }
                catch
                {
                    Logger.Log("Target Entry ID was not a numeric value.", Logger.Status.Error, true);
                    return ""; // export/save won't happen
                }

                sql += GenerateSql(new Dictionary<string[], dynamic>()
                {
                    { Profile.gtk(C.Loot, "Entry", sp),  entry},
                    { Profile.gtk(C.Loot, "Item", sp), row.Item },
                    { Profile.gtk(C.Loot, "Chance", sp), row.Chance },
                    { Profile.gtk(C.Loot, "QuestRequired", sp), row.QuestRequired },
                    { Profile.gtk(C.Loot, "MinCount", sp), row.MinCount },
                    { Profile.gtk(C.Loot, "MaxCount", sp), row.MaxCount },
                }) + Environment.NewLine;

            }                
            return sql;
        }
    }
}
