using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;

namespace TrinityCreator.Emulator
{
    class Trinity335a : IEmulator
    {
        public Trinity335a()
        {
            ID = 0;
        }

        public int ID { get; set; }

        public string GenerateQuery(TrinityCreature creature)
        {
            throw new NotImplementedException();
        }

        public string GenerateQuery(TrinityItem item)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"entry", item.EntryId.ToString()},
                {"name", SqlQuery.CleanText(item.Name)},
                {"description", SqlQuery.CleanText(item.Quote)},
                {"class", item.Class.Id.ToString()},
                {"subclass", item.ItemSubClass.Id.ToString()},
                {"displayid", item.DisplayId.ToString()},
                {"Quality", item.Quality.Id.ToString()},
                {"bonding", item.Binds.Id.ToString()},
                {"RequiredLevel", item.MinLevel.ToString()},
                {"maxcount", item.MaxAllowed.ToString()},
                {"AllowableClass", item.AllowedClass.BitmaskValue.ToString()},
                {"AllowableRace", item.AllowedRace.BitmaskValue.ToString()},
                {"BuyPrice", item.ValueBuy.ToString()},
                {"SellPrice", item.ValueSell.ToString()},
                {"InventoryType", item.InventoryType.Id.ToString()},
                {"Material", item.ItemSubClass.Material.Id.ToString()},
                {"sheath", item.InventoryType.Sheath.ToString()},
                {"Flags", item.Flags.BitmaskValue.ToString()},
                {"BuyCount", item.BuyCount.ToString()},
                {"stackable", item.Stackable.ToString()},
                {"ContainerSlots", item.ContainerSlots.ToString()},
                {"dmg_min1", item.DamageInfo.MinDamage.ToString()},
                {"dmg_max1", item.DamageInfo.MaxDamage.ToString()},
                {"dmg_type1", item.DamageInfo.Type.Id.ToString()},
                {"delay", item.DamageInfo.Speed.ToString()},
                {"MaxDurability", item.Durability.ToString()},
                {"ammo_type", item.AmmoType.ToString()},
                {"armor", item.Armor.ToString()},
                {"block", item.Block.ToString()},
                {"BagFamily", item.BagFamily.BitmaskValue.ToString()},
                {"socketBonus", item.SocketBonus.Id.ToString()},
                {"FlagsExtra", item.FlagsExtra.BitmaskValue.ToString()},
            };
            item.GemSockets.AddValues(kvplist, "socketColor_", "socketContent_");            
            item.Stats.AddValues(kvplist, "stat_type", "stat_value");

            try // resistances
            {
                // loops unique keys
                foreach (var kvp in item.Resistances.GetUserInput())
                {
                    var type = (DamageType)kvp.Key;
                    var value = int.Parse(kvp.Value); // validate int
                    kvplist.Add(type.Description + "_res", value.ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid value in magic resistance.");
            }
            
            return SqlQuery.GenerateInsert("item_template", kvplist);
        }

        public string GenerateQuery(TrinityQuest quest)
        {
            throw new NotImplementedException();
        }
    }
}
