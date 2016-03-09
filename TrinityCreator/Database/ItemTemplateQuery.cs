using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace TrinityCreator.Database
{
    class ItemTemplateQuery : Connection
    {
        /// <summary>
        /// Comma seperated list with the required values in the correct order
        /// (* could cause problems with core modifications)
        /// </summary>
        /// <returns></returns>
        /// 
        private static string[] GetRowValues()
        {
            return new string[]
            {

            };
        }

        internal static ItemPage GetItemById(int entryId)
        {
            ItemPage result = null;
            string query = string.Format("SELECT {0} FROM item_template WHERE entry = {1};", 
                string.Join(", ", GetRowValues()), entryId);
            DataTable dt = ExecuteQuery(query);
            if (dt.Rows.Count > 0)
                result = new ItemPage(dt.Rows[0]);
            return result;           
        }

        internal static Item[] FindItemsByName(string partialName)
        {
            throw new NotImplementedException();
        }
        

    }
}
