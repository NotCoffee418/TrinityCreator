using System;

namespace TrinityCreator.Data
{
    public class ItemClass : IKeyValue
    {
        public ItemClass(int id, string description)
            : base(id, description) { }

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
                    case 11:
                        return ItemSubClass.GetQuiverList();
                    case 12:
                        return ItemSubClass.GetQuestList();
                    case 13:
                        return ItemSubClass.GetKeyList();
                    case 15:
                        return ItemSubClass.GetMiscellaneousList();
                    default:
                        throw new Exception("No correct item class was given to list subclasses.");
                }
            }
        }


        /// <summary>
        ///     Return list of supported itemclasses
        /// </summary>
        /// <returns></returns>
        public static ItemClass[] GetClassList()
        {
            return new[]
            {
                new ItemClass(4, "Armor"),
                new ItemClass(2, "Weapon"),
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
                new ItemClass(16, "Glyph")
            };
        }
    }
}