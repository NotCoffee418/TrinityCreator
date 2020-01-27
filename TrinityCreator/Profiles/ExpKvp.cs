using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Helpers;

namespace TrinityCreator.Profiles
{
    /// <summary>
    /// Exportable key and value information for SQL exports
    /// </summary>
    public class ExpKvp
    {
        /// <summary>
        /// Constructor for default fields. Runs gtk to define properties
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="value"></param>
        /// <param name="c"></param>
        /// <param name="specialTableName"></param>
        public ExpKvp(string appKey, dynamic value, Export.C c, string specialTableName = "")
        {
            Value = value;
            gtk(c, appKey, specialTableName);
        }

        /// <summary>
        /// Constructor for custom fields. Manually define key, value and table
        /// </summary>
        /// <param name="sqlKey"></param>
        /// <param name="value"></param>
        /// <param name="table"></param>
        public ExpKvp(string sqlKey, dynamic value, string table)
        {
            SqlKey = sqlKey;
            Value = value;
            SqlTableName = table;
            IsValid = true;
        }

        /// <summary>
        /// True: Key can be exported, there is a corresponding sqlKey to the given appKey and it's been defined.
        /// </summary>
        public bool IsValid { get; private set; }

        public string SqlTableName { get; private set; }

        public string SqlKey { get; private set; }

        public dynamic Value { get; private set; }

        /// <summary>
        /// Stands for "Get Table name and Key"
        /// Returns the correct key for the current profile based on the app's equivalent of that key
        /// </summary>
        /// <param name="creationType"></param>
        /// <param name="appKey"></param>
        /// <param name="specialTableName">For table names that vary based on a setting, replaces %t with this string</param>
        /// <returns>:0:Table - 1: Sql key</returns>
        public void gtk(Export.C creationType, string appKey, string specialTableName = "")
        {
            // Select correct dictionary to searchfor creation
            Dictionary<string, Dictionary<string, string>> targetDict = null;
            switch (creationType)
            {
                case Export.C.Creature:
                    targetDict = Profile.Active.Creature;
                    break;
                case Export.C.Quest:
                    targetDict = Profile.Active.Quest;
                    break;
                case Export.C.Item:
                    targetDict = Profile.Active.Item;
                    break;
                case Export.C.Loot:
                    targetDict = Profile.Active.Loot;
                    break;
                case Export.C.Vendor:
                    targetDict = Profile.Active.Vendor;
                    break;
            }

            // Check if the key exists in the profile
            var table = targetDict.Where(t =>
                // equals in this case is equivalent to null check
                t.Value.Where(keysDic => keysDic.Key.ToLower() == appKey.ToLower()).FirstOrDefault().Key != null
            ).FirstOrDefault();

            // Key didn't exist in any table, ignore column
            IsValid = table.Key != null;
            if (!IsValid)
            {
                Logger.Log($"Notice: Export: {creationType.ToString()}.{appKey} not in profile. This isn't always an problem.");
                return;
            }

            // Key exists, generate result
            SqlTableName = table.Key; // Return table name

            // Handle unusual table names
            if (specialTableName != "")
                SqlTableName = SqlTableName.Replace("%t", specialTableName);

            // Find appkey again and put sqlkey in result
            SqlKey = table.Value.Where(keys => keys.Key.ToLower() == appKey.ToLower()).First().Value;
        }
    }
}
