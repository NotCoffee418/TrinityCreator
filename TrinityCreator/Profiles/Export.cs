using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Loot is DDC based, loop through the entries
        public static string LootSql(LootPage loot)
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
