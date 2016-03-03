using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class ItemClass
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
                new ItemClass(0, "Consumable"),
                new ItemClass(1, "Container"),
                //new ItemClass(2, "Weapon"),   // Don't list since it has seperate page
                new ItemClass(3, "Gem"),            // todo: Check if supported
                //new ItemClass(4, "Armor"),    // Don't list since it has seperate page
                new ItemClass(5, "Reagent"),
                new ItemClass(6, "Projectile"),
                new ItemClass(7, "Trade Goods"),
                //new ItemClass(8, "Generic"),  // Obsolete
                new ItemClass(9, "Recipe"),         // todo: Check if supported
                //new ItemClass(10, "Money"),   // Obsolete
                //new ItemClass(11, "Quiver"),     // Removed in WotLK?
                new ItemClass(12, "Quest"),
                new ItemClass(13, "Key"),
                //new ItemClass(14, "Permanent"),// Obsolete
                new ItemClass(15, "Miscellaneous"),
                new ItemClass(16, "Glyph"),         // todo: Check if supported
            };
        }

        public static ItemClass[] GetArmorWeaponClasses()
        {
            return new ItemClass[]
            {
                new ItemClass(2, "Weapon"),
                new ItemClass(4, "Armor"),
            };
        }
    }
}
