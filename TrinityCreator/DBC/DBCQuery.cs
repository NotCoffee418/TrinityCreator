using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.DBC
{
    class DBCQuery
    {
        /// <summary>
        /// Negative values from QuestSort & positive values from AreaTable
        /// </summary>
        /// <returns></returns>
        public static DataTable GetQuestSort()
        {
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn("ID"));
            result.Columns.Add(new DataColumn("Name"));

            // Make quest sort id's negative
            DataRow newRow = null;
            try
            {
                foreach (DataRow dr in GetQuestSortNames().Rows)
                {
                    newRow = result.NewRow();
                    newRow["ID"] = -Math.Abs(Convert.ToInt32(dr[0])); // QuestSort's values should negative
                    newRow["Name"] = dr[1];
                    result.Rows.Add(newRow);
                }

                // Add AreaTable values
                foreach (DataRow dr in GetAreaTableNames().Rows)
                {
                    newRow = result.NewRow();
                    newRow["ID"] = dr[0];
                    newRow["Name"] = dr[1];
                    result.Rows.Add(newRow);
                }
            }
            catch {
                newRow = result.NewRow();
                newRow["ID"] = "DBC not configured.";
                result.Rows.Add(newRow);
            }

            return result;
        }

        public static DataTable GetAreaTableNames()
        {
            return DbcHandler.LoadDbc("AreaTable", 
                new string[] { "m_ID", "m_AreaName_lang" });
        }

        public static DataTable GetQuestSortNames()
        {
            return DbcHandler.LoadDbc("QuestSort", 
                new string[] { "id", "name" });
        }

        internal static DataTable GetSpells()
        {
            return DbcHandler.LoadDbc("Spell", 
                new string[] { "m_ID", "m_name_lang_1", "m_description_lang_1" });
        }

        internal static DataTable GetMap()
        {
            return DbcHandler.LoadDbc("Map", 
                new string[] { "m_ID", "m_MapName_lang1" });
        }

        internal static DataTable GetFaction()
        {
            // basic factions
            var result = DbcHandler.LoadDbc("Faction", new string[] { "m_ID", "m_name_lang_1" });
            result.Rows.Add(35, "Friendly to all");
            result.Rows.Add(168, "Enemy to all");
            result.Rows.Add(7, "Neutral Attackable");
            result.Rows.Add(14, "Unfriendly Attackable");
            return result;
        }

        internal static DataTable GetCharTitles()
        {
            return DbcHandler.LoadDbc("CharTitles", 
                new string[] { "field0", "field2" });
        }


        internal static DataTable GetEmotes()
        {
            return DbcHandler.LoadDbc("Emotes",
                new string[] { "id", "description" });
        }
    }
}
