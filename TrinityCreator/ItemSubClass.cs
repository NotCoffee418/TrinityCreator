using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class ItemSubClass
    {
        public ItemSubClass(int id, string description)
        {
            Id = id;
            Description = description;
        }

        private int _id;
        private string _description;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// Return list of supported subclasses for the used class
        /// </summary>
        /// <returns></returns>
        public static ItemSubClass[] GetSubclassList(ItemClass itemclass)
        {
            switch (itemclass.Id)
            {
                case 0:
                    return GetConsumableList();
                case 1:
                    return GetContainerList();
                case 2:
                    return GetWeaponList();
                case 3:
                    return GetGemList();
                case 4:
                    return GetArmorList();
                case 5:
                    return GetReagentList();
                case 6:
                    return GetProjectileList();
                case 7:
                    return GetTradeGoodsList();
                case 9:
                    return GetRecipeList();
                case 12:
                    return GetQuestList();
                case 13:
                    return GetKeyList();
                case 15:
                    return GetMiscellaneousList();
                case 16:
                    return GetGlyphList();
                default:
                    throw new Exception("No correct item class was given to list subclasses.");
            }
        }

        public static ItemSubClass[] GetGlyphList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(1, "Warrior"),
                new ItemSubClass(2, "Paladin"),
                new ItemSubClass(3, "Hunter"),
                new ItemSubClass(4, "Rogue"),
                new ItemSubClass(5, "Priest"),
                new ItemSubClass(6, "Death Knight"),
                new ItemSubClass(7, "Shaman"),
                new ItemSubClass(8, "Mage"),
                new ItemSubClass(9, "Warlock"),
                new ItemSubClass(11, "Druid"),
            };
        }

        public static ItemSubClass[] GetMiscellaneousList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "Junk"),
                new ItemSubClass(1, "Reagent"),
                new ItemSubClass(2, "Pet"),
                new ItemSubClass(3, "Holiday"),
                new ItemSubClass(4, "Other"),
                new ItemSubClass(5, "Mount"),
            };
        }

        public static ItemSubClass[] GetKeyList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "Key"),
                new ItemSubClass(1, "Lockpick"),
            };
        }

        public static ItemSubClass[] GetQuestList()
        {
            return new ItemSubClass[]
           {
                new ItemSubClass(0, "Quest")
           };
        }

        public static ItemSubClass[] GetRecipeList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "Book"),
                new ItemSubClass(1, "Leatherworking"),
                new ItemSubClass(2, "Tailoring"),
                new ItemSubClass(3, "Engineering"),
                new ItemSubClass(4, "Blacksmithing"),
                new ItemSubClass(5, "Cooking"),
                new ItemSubClass(6, "Alchemy"),
                new ItemSubClass(7, "First Aid"),
                new ItemSubClass(8, "Enchanting"),
                new ItemSubClass(9, "Fishing"),
                new ItemSubClass(10, "Jewelcrafting"),
            };
        }

        public static ItemSubClass[] GetTradeGoodsList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "Trade Goods"),
                new ItemSubClass(1, "Parts"),
                new ItemSubClass(2, "Explosives"),
                new ItemSubClass(3, "Devices"),
                new ItemSubClass(4, "Jewelcrafting"),
                new ItemSubClass(5, "Cloth"),
                new ItemSubClass(6, "Leather"),
                new ItemSubClass(7, "Metal & Stone"),
                new ItemSubClass(8, "Meat"),
                new ItemSubClass(9, "Herb"),
                new ItemSubClass(10, "Elemental"),
                new ItemSubClass(11, "Other"),
                new ItemSubClass(12, "Enchanting"),
                new ItemSubClass(13, "Materials"),
                new ItemSubClass(14, "Armor Enchantment"),
                new ItemSubClass(15, "Weapon Enchantment"),
            };
        }

        public static ItemSubClass[] GetProjectileList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(2, "Arrow"),
                new ItemSubClass(3, "Bullet"),
            };
        }

        public static ItemSubClass[] GetReagentList()
        {
            return new ItemSubClass[]
           {
                new ItemSubClass(0, "Reagent")
           };
        }

        public static ItemSubClass[] GetArmorList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "Miscellaneous"),
                new ItemSubClass(1, "Cloth"),
                new ItemSubClass(2, "Leather"),
                new ItemSubClass(3, "Mail"),
                new ItemSubClass(4, "Plate"),
                //new ItemSubClass(5, "Buckler(OBSOLETE)"),
                new ItemSubClass(6, "Shield"),
                new ItemSubClass(7, "Libram"),
                new ItemSubClass(8, "Idol"),
                new ItemSubClass(9, "Totem"),
                new ItemSubClass(10, "Sigil"),
            };
        }

        public static ItemSubClass[] GetGemList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "Red"),
                new ItemSubClass(1, "Blue"),
                new ItemSubClass(2, "Yellow"),
                new ItemSubClass(3, "Purple"),
                new ItemSubClass(4, "Green"),
                new ItemSubClass(5, "Orange"),
                new ItemSubClass(6, "Meta"),
                new ItemSubClass(7, "Simple"),
                new ItemSubClass(8, "Prismatic"),
            };
        }

        public static ItemSubClass[] GetWeaponList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "1h Axe"),
                new ItemSubClass(1, "2h Axe"),
                new ItemSubClass(2, "Bow"),
                new ItemSubClass(3, "Gun"),
                new ItemSubClass(4, "1h Mace"),
                new ItemSubClass(5, "2h Mace"),
                new ItemSubClass(6, "Polearm"),
                new ItemSubClass(7, "1h Sword"),
                new ItemSubClass(8, "2h Sword"),
                new ItemSubClass(10,"Staff"),
                //new ItemSubClass(11,"Exotic"), // no idea what this is
                //new ItemSubClass(12,"Exotic"),
                new ItemSubClass(13,"Fist Weapon"),
                new ItemSubClass(14,"Miscellaneous"),
                new ItemSubClass(15,"Dagger"),
                new ItemSubClass(16,"Thrown"),
                new ItemSubClass(17,"Spear"),
                new ItemSubClass(18,"Crossbow"),
                new ItemSubClass(19,"Wand"),
                new ItemSubClass(20,"Fishing Pole"),
            };
        }

        public static ItemSubClass[] GetContainerList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "Bag"),
                new ItemSubClass(1, "Soul Bag"),
                new ItemSubClass(2, "Herb Bag"),
                new ItemSubClass(3, "Enchanting Bag"),
                new ItemSubClass(4, "Engeneering Bag"),
                new ItemSubClass(5, "Gem Bag"),
                new ItemSubClass(6, "Mining Bag"),
                new ItemSubClass(7, "Leatherworking Bag"),
                new ItemSubClass(8, "Inscription  Bag"),
            };
        }

        public static ItemSubClass[] GetConsumableList()
        {
            return new ItemSubClass[]
            {
                new ItemSubClass(0, "Consumable"),
                new ItemSubClass(1, "Potion"),
                new ItemSubClass(2, "Elixir"),
                new ItemSubClass(3, "Flask"),
                new ItemSubClass(4, "Scroll"),
                new ItemSubClass(5, "Food & Drink"),
                new ItemSubClass(6, "Item Enhancement"),
                new ItemSubClass(7, "Bandage"),
                new ItemSubClass(8, "Other"),
            };
        }
    }
}
