using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class ItemInventoryType
    {
        public ItemInventoryType(int id, string description, int sheath = 0)
        {
            Id = id;
            Description = description;
            _sheath = sheath;
        }

        private int _id;
        private string _description;
        private int _sheath;

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

        public int Sheath
        {
            get
            {
                return _sheath;
            }
            set
            {
                _sheath = value;
            }
        }

        internal static ItemInventoryType[] GetAllInventoryTypes()
        {
            return new ItemInventoryType[]
            {
                //todo
            };
        }

        public static ItemInventoryType[] GetNonEquipable()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(0, "Non equipable"),
            };
        }

        public static ItemInventoryType[] GetOneHandWeapon()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(13, "Weapon", 3),
                new ItemInventoryType(21, "Main Hand", 3),
                new ItemInventoryType(22, "Off hand", 6),
                //new ItemInventoryType(23, "Holdable (Tome)"),
            };
        }

        public static ItemInventoryType[] GetShield()
        {
            return new ItemInventoryType[]
            {

                new ItemInventoryType(14, "Shield", 4),
            };
        }

        public static ItemInventoryType[] GetRangedWandGun()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(26, "Ranged"),
            };
        }

        public static ItemInventoryType[] GetRangedBow()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(15, "Ranged"),
            };
        }
        

        public static ItemInventoryType[] GetTwoHandWeapon()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(17, "Two-Hand", 1),
            };
        }

        public static ItemInventoryType[] GetStaff()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(17, "Two-Hand", 2),
            };
        }

        public static ItemInventoryType[] GetBag()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(18, "Bag"),
            };
        }

        public static ItemInventoryType[] GetThrown()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(25, "Thrown"),
            };
        }

        public static ItemInventoryType[] GetAmmo()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(24, "Ammo"),
            };
        }

        public static ItemInventoryType[] GetArmor()
        {
            return new ItemInventoryType[]
            {
                new ItemInventoryType(1, "Head"),
                new ItemInventoryType(2, "Neck"),
                new ItemInventoryType(3, "Shoulder"),
                new ItemInventoryType(4, "Shirt"),
                new ItemInventoryType(5, "Chest"),
                new ItemInventoryType(6, "Waist"),
                new ItemInventoryType(7, "Legs"),
                new ItemInventoryType(8, "Feet"),
                new ItemInventoryType(9, "Wrists"),
                new ItemInventoryType(10, "Hands"),
                new ItemInventoryType(11, "Finger"),
                new ItemInventoryType(12, "Trinket"),
            };
        }
    }
}
