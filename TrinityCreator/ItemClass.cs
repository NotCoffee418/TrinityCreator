using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class ItemClass : IKeyValue
    {
        public ItemClass(int id, string description)
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

        public ItemSubClass[] AvailableSubClasses
        {
            get
            {
                switch (Id)
                {
                    case 0:
                        return ItemSubClass.GetConsumableList();
                    case 1:
                        return ItemSubClass.GetContainerList();
                    case 2:
                        return ItemSubClass.GetWeaponList();
                    case 3:
                        return ItemSubClass.GetGemList();
                    case 4:
                        return ItemSubClass.GetArmorList();
                    case 5:
                        return ItemSubClass.GetReagentList();
                    case 6:
                        return ItemSubClass.GetProjectileList();
                    case 7:
                        return ItemSubClass.GetTradeGoodsList();
                    case 9:
                        return ItemSubClass.GetRecipeList();
                    case 12:
                        return ItemSubClass.GetQuestList();
                    case 13:
                        return ItemSubClass.GetKeyList();
                    case 15:
                        return ItemSubClass.GetMiscellaneousList();
                    case 16:
                        return ItemSubClass.GetGlyphList();
                    default:
                        throw new Exception("No correct item class was given to list subclasses.");
                }
            }
        }

        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// Return list of supported itemclasses
        /// </summary>
        /// <returns></returns>
        public static ItemClass[] GetClassList()
        {
            return new ItemClass[]
            {
                new ItemClass(2, "Weapon"),
                new ItemClass(4, "Armor"),
                new ItemClass(0, "Consumable"),
                new ItemClass(1, "Container"),
                new ItemClass(3, "Gem"),
                new ItemClass(5, "Reagent"),
                new ItemClass(6, "Projectile"),
                new ItemClass(7, "Trade Goods"),
                new ItemClass(9, "Recipe"),
                new ItemClass(11, "Quiver"),
                new ItemClass(12, "Quest"),
                new ItemClass(13, "Key"),
                new ItemClass(15, "Miscellaneous"),
                new ItemClass(16, "Glyph"),
            };
        }
    }
}
