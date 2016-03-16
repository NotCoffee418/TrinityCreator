using System;

namespace TrinityCreator.Database
{
    internal class ItemTemplateQuery : Connection
    {
        /// <summary>
        ///     Comma seperated list with the required values in the correct order
        ///     (* could cause problems with core modifications)
        /// </summary>
        /// <returns></returns>
        private static string[] GetRowValues()
        {
            return new string[]
            {
            };
        }

        internal static ItemPage GetItemById(int entryId)
        {
            ItemPage result = null;
            var query = $"SELECT {string.Join(", ", GetRowValues())} FROM item_template WHERE entry = {entryId};";
            var dt = ExecuteQuery(query);
            //if (dt.Rows.Count > 0)
            //    result = new ItemPage(dt.Rows[0]);
            return result;
        }

        internal static TrinityItem[] FindItemsByName(string partialName)
        {
            throw new NotImplementedException();
        }
    }
}