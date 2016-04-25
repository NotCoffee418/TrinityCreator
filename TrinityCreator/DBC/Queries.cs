using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.DBC
{
    class Queries
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

            return result;
        }

        public static DataTable GetAreaTableNames()
        {
            return DbcHandler.LoadDbc("AreaTable").DefaultView.ToTable(false, "m_ID", "m_AreaName_lang");
        }

        public static DataTable GetQuestSortNames()
        {
            return DbcHandler.LoadDbc("QuestSort").DefaultView.ToTable(false, "id", "name");
        }
    }
}
