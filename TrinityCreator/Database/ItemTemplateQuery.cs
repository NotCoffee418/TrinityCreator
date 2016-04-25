using System;
using System.Data;

namespace TrinityCreator.Database
{
    internal class ItemTemplateQuery : Connection
    {
        internal static ItemPage GetItemById(int entryId)
        {
            Open();
            ItemPage result = new ItemPage();
            var query = "SELECT * FROM item_template WHERE entry = " + entryId;
            var dt = ExecuteQuery(query);
            if (dt.Rows.Count > 0)
                result = new ItemPage(dt.Rows[0]);
            return result;
        }

        /// <summary>
        /// Used for displayid lookup
        /// </summary>
        /// <param name="partialName"></param>
        /// <returns></returns>
        internal static DataTable FindItemsByName(string partialName)
        {
            return ExecuteQuery("SELECT entry, displayid, name FROM item_template WHERE name LIKE '%" + partialName + "%' ORDER BY entry LIMIT 200;");
        }
    }
}