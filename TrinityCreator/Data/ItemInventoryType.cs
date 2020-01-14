using System.Collections.Generic;

namespace TrinityCreator.Data
{
    public class ItemInventoryType : IKeyValue
    {
        public ItemInventoryType(int id, string description, int sheath = 0)
            : base(id, description) {
            Sheath = sheath;
        }

        public int Sheath { get; set; }

        internal static ItemInventoryType[] GetAllInventoryTypes()
        {
            var result = new List<ItemInventoryType>();
            result.AddRange(GetNonEquipable());
            result.AddRange(GetArmor());
            result.AddRange(GetOneHandWeapon());
            result.AddRange(GetShield());
            result.AddRange(GetRangedWandGun());
            result.AddRange(GetRangedBow());
            result.AddRange(GetTwoHandWeapon());
            result.AddRange(GetStaff());
            result.AddRange(GetBag());
            result.AddRange(GetThrown());
            result.AddRange(GetAmmo());
            return result.ToArray();
        }

        public static ItemInventoryType[] GetNonEquipable()
        {
            return new[]
            {
                new ItemInventoryType(0, "Non equipable")
            };
        }

        public static ItemInventoryType[] GetOneHandWeapon()
        {
            return new[]
            {
                new ItemInventoryType(13, "One-Hand", 3),
                new ItemInventoryType(21, "Main Hand", 3),
                new ItemInventoryType(22, "Off hand", 6),
                new ItemInventoryType(23, "Holdable (Tome)", 6)
            };
        }

        public static ItemInventoryType[] GetShield()
        {
            return new[]
            {
                new ItemInventoryType(14, "Shield", 4)
            };
        }

        public static ItemInventoryType[] GetRangedWandGun()
        {
            return new[]
            {
                new ItemInventoryType(26, "Ranged")
            };
        }

        public static ItemInventoryType[] GetRangedBow()
        {
            return new[]
            {
                new ItemInventoryType(15, "Ranged")
            };
        }


        public static ItemInventoryType[] GetTwoHandWeapon()
        {
            return new[]
            {
                new ItemInventoryType(17, "Two-Hand", 1)
            };
        }

        public static ItemInventoryType[] GetStaff()
        {
            return new[]
            {
                new ItemInventoryType(17, "Two-Hand", 2)
            };
        }

        public static ItemInventoryType[] GetBag()
        {
            return new[]
            {
                new ItemInventoryType(18, "Bag")
            };
        }

        public static ItemInventoryType[] GetThrown()
        {
            return new[]
            {
                new ItemInventoryType(25, "Thrown")
            };
        }

        public static ItemInventoryType[] GetAmmo()
        {
            return new[]
            {
                new ItemInventoryType(24, "Ammo")
            };
        }

        public static ItemInventoryType[] GetArmor()
        {
            return new[]
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
                new ItemInventoryType(16, "Cloak")
            };
        }
    }
}