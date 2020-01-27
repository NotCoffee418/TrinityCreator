using System.ComponentModel;

namespace TrinityCreator.Data
{
    public class ItemSubClass : IKeyValue, INotifyPropertyChanged
    {
        private ItemInventoryType[] _lockedInventoryType;
        private ItemMaterial _material;
        private string _previewNoteLeft;
        private string _previewNoteRight;

        public ItemSubClass(int id, string description, string previewNoteLeft = "", string previewNoteRight = "",
            ItemInventoryType[] lockedInventoryType = null, ItemMaterial material = null)
        {
            Id = id;
            Description = description;
            PreviewNoteLeft = previewNoteLeft;
            PreviewNoteRight = previewNoteRight;
            LockedInventoryType = lockedInventoryType;
            Material = material;
        }

        public string PreviewNoteLeft
        {
            get
            {
                if (LockedInventoryType == null)
                {
                    if (_previewNoteLeft == "")
                        return Description;
                    return PreviewNoteLeft;
                }
                return LockedInventoryType[0].Description;
            }
            set
            {
                _previewNoteLeft = value;
                RaisePropertyChanged("PreviewNoteLeft");
            }
        }

        public string PreviewNoteRight
        {
            get
            {
                if (_previewNoteRight == "" && _previewNoteRight != PreviewNoteLeft)
                    return Description;
                return _previewNoteRight;
            }
            set
            {
                _previewNoteRight = value;
                RaisePropertyChanged("PreviewNoteRight");
            }
        }

        public ItemInventoryType[] LockedInventoryType
        {
            get
            {
                if (_lockedInventoryType == null)
                    return ItemInventoryType.GetNonEquipable();
                return _lockedInventoryType;
            }
            set
            {
                _lockedInventoryType = value;
                RaisePropertyChanged("LockedInventoryType");
            }
        }

        public ItemMaterial Material
        {
            get
            {
                if (_material == null)
                {
                    return ItemMaterial.GetUndefined();
                }
                return _material;
            }
            set
            {
                _material = value;
                RaisePropertyChanged("Material");
            }
        }

        public int Id
        {
            get { return base.Id; }
            set
            {
                base.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public string Description
        {
            get { return base.Description; }
            set
            {
                base.Description = value;
                RaisePropertyChanged("Description");
            }
        }

        public override string ToString()
        {
            return Description;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public static ItemSubClass[] GetMiscellaneousList()
        {
            return new[]
            {
                new ItemSubClass(0, "Junk"),
                new ItemSubClass(1, "Reagent"),
                new ItemSubClass(2, "Pet"),
                new ItemSubClass(3, "Holiday"),
                new ItemSubClass(4, "Other"),
                new ItemSubClass(5, "Mount")
            };
        }

        public static ItemSubClass[] GetKeyList()
        {
            return new[]
            {
                new ItemSubClass(0, "Key"),
                new ItemSubClass(1, "Lockpick")
            };
        }

        public static ItemSubClass[] GetQuestList()
        {
            return new[]
            {
                new ItemSubClass(0, "Quest")
            };
        }

        public static ItemSubClass[] GetQuiverList()
        {
            return new[]
            {
                new ItemSubClass(2, "Quiver"),
                new ItemSubClass(3, "Ammo Pouch")
            };
        }

        public static ItemSubClass[] GetRecipeList()
        {
            return new[]
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
                new ItemSubClass(10, "Jewelcrafting")
            };
        }

        public static ItemSubClass[] GetTradeGoodsList()
        {
            return new[]
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
                new ItemSubClass(15, "Weapon Enchantment")
            };
        }

        public static ItemSubClass[] GetProjectileList()
        {
            var ammoType = ItemInventoryType.GetAmmo();
            return new[]
            {
                new ItemSubClass(2, "Arrow", lockedInventoryType: ammoType),
                new ItemSubClass(3, "Bullet", lockedInventoryType: ammoType)
            };
        }

        public static ItemSubClass[] GetReagentList()
        {
            return new[]
            {
                new ItemSubClass(0, "Reagent")
            };
        }

        public static ItemSubClass[] GetArmorList()
        {
            var armorType = ItemInventoryType.GetArmor();
            var relicType = ItemInventoryType.GetArmor();

            return new[]
            {
                new ItemSubClass(1, "Cloth", lockedInventoryType: armorType, material: ItemMaterial.GetCloth()),
                new ItemSubClass(2, "Leather", lockedInventoryType: armorType, material: ItemMaterial.GetLeather()),
                new ItemSubClass(3, "Mail", lockedInventoryType: armorType, material: ItemMaterial.GetChain()),
                new ItemSubClass(4, "Plate", lockedInventoryType: armorType, material: ItemMaterial.GetPlate()),
                new ItemSubClass(6, "Shield", lockedInventoryType: ItemInventoryType.GetShield(),
                    material: ItemMaterial.GetPlate()),
                new ItemSubClass(7, "Libram", lockedInventoryType: relicType),
                new ItemSubClass(8, "Idol", lockedInventoryType: relicType),
                new ItemSubClass(9, "Totem", lockedInventoryType: relicType),
                new ItemSubClass(10, "Sigil", lockedInventoryType: relicType),
                new ItemSubClass(0, "Miscellaneous", lockedInventoryType: ItemInventoryType.GetAllInventoryTypes())
            };
        }

        public static ItemSubClass[] GetGemList()
        {
            return new[]
            {
                new ItemSubClass(0, "Red"),
                new ItemSubClass(1, "Blue"),
                new ItemSubClass(2, "Yellow"),
                new ItemSubClass(3, "Purple"),
                new ItemSubClass(4, "Green"),
                new ItemSubClass(5, "Orange"),
                new ItemSubClass(6, "Meta"),
                new ItemSubClass(7, "Simple"),
                new ItemSubClass(8, "Prismatic")
            };
        }

        public static ItemSubClass[] GetWeaponList()
        {
            return new[]
            {
                new ItemSubClass(0, "1h Axe", previewNoteRight: "Axe",
                    lockedInventoryType: ItemInventoryType.GetOneHandWeapon()),
                new ItemSubClass(1, "2h Axe", previewNoteRight: "Axe",
                    lockedInventoryType: ItemInventoryType.GetTwoHandWeapon()),
                new ItemSubClass(2, "Bow", lockedInventoryType: ItemInventoryType.GetRangedBow()),
                new ItemSubClass(3, "Gun", lockedInventoryType: ItemInventoryType.GetRangedWandGun()),
                new ItemSubClass(4, "1h Mace", previewNoteRight: "Mace",
                    lockedInventoryType: ItemInventoryType.GetOneHandWeapon()),
                new ItemSubClass(5, "2h Mace", previewNoteRight: "Mace",
                    lockedInventoryType: ItemInventoryType.GetTwoHandWeapon()),
                new ItemSubClass(6, "Polearm", lockedInventoryType: ItemInventoryType.GetTwoHandWeapon()),
                new ItemSubClass(7, "1h Sword", previewNoteRight: "Sword",
                    lockedInventoryType: ItemInventoryType.GetOneHandWeapon()),
                new ItemSubClass(8, "2h Sword", previewNoteRight: "Sword",
                    lockedInventoryType: ItemInventoryType.GetTwoHandWeapon()),
                new ItemSubClass(10, "Staff", lockedInventoryType: ItemInventoryType.GetStaff()),
                new ItemSubClass(13, "Fist Weapon", lockedInventoryType: ItemInventoryType.GetOneHandWeapon()),
                new ItemSubClass(14, "Miscellaneous", lockedInventoryType: ItemInventoryType.GetAllInventoryTypes()),
                new ItemSubClass(15, "Dagger", lockedInventoryType: ItemInventoryType.GetOneHandWeapon()),
                new ItemSubClass(16, "Thrown", lockedInventoryType: ItemInventoryType.GetThrown()),
                new ItemSubClass(18, "Crossbow", lockedInventoryType: ItemInventoryType.GetRangedBow()),
                new ItemSubClass(19, "Wand", lockedInventoryType: ItemInventoryType.GetRangedWandGun()),
                new ItemSubClass(20, "Fishing Pole", lockedInventoryType: ItemInventoryType.GetTwoHandWeapon())
            };
        }

        public static ItemSubClass[] GetContainerList()
        {
            var bagType = ItemInventoryType.GetBag();
            return new[]
            {
                new ItemSubClass(0, "Bag", lockedInventoryType: bagType),
                new ItemSubClass(1, "Soul Bag", lockedInventoryType: bagType),
                new ItemSubClass(2, "Herb Bag", lockedInventoryType: bagType),
                new ItemSubClass(3, "Enchanting Bag", lockedInventoryType: bagType),
                new ItemSubClass(4, "Engeneering Bag", lockedInventoryType: bagType),
                new ItemSubClass(5, "Gem Bag", lockedInventoryType: bagType),
                new ItemSubClass(6, "Mining Bag", lockedInventoryType: bagType),
                new ItemSubClass(7, "Leatherworking Bag", lockedInventoryType: bagType),
                new ItemSubClass(8, "Inscription  Bag", lockedInventoryType: bagType)
            };
        }

        public static ItemSubClass[] GetConsumableList()
        {
            return new[]
            {
                new ItemSubClass(0, "Consumable", material: ItemMaterial.GetConsumable()),
                new ItemSubClass(1, "Potion", material: ItemMaterial.GetConsumable()),
                new ItemSubClass(2, "Elixir", material: ItemMaterial.GetConsumable()),
                new ItemSubClass(3, "Flask", material: ItemMaterial.GetConsumable()),
                new ItemSubClass(4, "Scroll", material: ItemMaterial.GetConsumable()),
                new ItemSubClass(5, "Food & Drink", material: ItemMaterial.GetConsumable()),
                new ItemSubClass(6, "Item Enhancement", material: ItemMaterial.GetConsumable()),
                new ItemSubClass(7, "Bandage", material: ItemMaterial.GetConsumable()),
                new ItemSubClass(8, "Other", material: ItemMaterial.GetConsumable())
            };
        }
    }
}