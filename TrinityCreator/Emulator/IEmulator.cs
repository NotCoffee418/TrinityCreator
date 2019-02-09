using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;

namespace TrinityCreator.Emulator
{
    interface IEmulator
    {
        int ID { get; set; }
        string GenerateQuery(TrinityItem item);
        string GenerateQuery(TrinityQuest quest);
        string GenerateQuery(TrinityCreature creature);
        string GenerateQuery(LootPage loot);
        string GenerateQuery(VendorPage vendor);
        Tuple<string, string> GetIdColumnName(string v);
    }
}
