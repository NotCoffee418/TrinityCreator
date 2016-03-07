using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TrinityCreator
{
    public class BitmaskStackPanel : StackPanel
    {
        public BitmaskStackPanel(string name, BitmaskCheckBox[] checkboxes)
        {
            Name = name;
            _checkboxes = checkboxes;

            foreach (var bmcb in Checkboxes)
                Children.Add(bmcb);
        }

        private BitmaskCheckBox[] _checkboxes;

        public BitmaskCheckBox[] Checkboxes
        {
            get
            {
                return _checkboxes;
            }
        }

        public uint BitmaskValue
        {
            get
            {
                uint bitmask = 0;
                foreach (CheckBox cb in Checkboxes)
                {
                    if (cb.IsChecked == true)
                        bitmask += (uint)cb.Tag;
                }
                return bitmask;
            }
        }

        /// <summary>
        /// Gets clean comma seperated text from checked BitmaskCheckboxes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> list = new List<string>();
            foreach (BitmaskCheckBox c in Children)
                if (c.IsChecked == true)
                    list.Add(c.Content.ToString());

            if (list.Count == 0)
                list.Add("All");

            return string.Join(", ", list);
        }


        public static BitmaskStackPanel GetItemFlags()
        {
            var cbs = new BitmaskCheckBox[] {
                new BitmaskCheckBox(2, "Conjured item"),
                new BitmaskCheckBox(4, "Openable"),
                new BitmaskCheckBox(8, "Display green \"Heroic\" text"),
                new BitmaskCheckBox(128, "No default 30sec cooldown on equip"),
                new BitmaskCheckBox(512, "Wrapper: Can wrap other items"),
                new BitmaskCheckBox(2048, "Party loot: can be looted by all"),
                new BitmaskCheckBox(4096, "Item is refundable"),
                new BitmaskCheckBox(8192, "Arena or guild charter"),
                new BitmaskCheckBox(262144, "Item can be prospected"),
                new BitmaskCheckBox(524288, "Unique equipped"),
                new BitmaskCheckBox(2097152, "Item can be used during arena match"),
                new BitmaskCheckBox(4194304, "Throwable"),
                new BitmaskCheckBox(8388608, "Can be used in shapeshift forms"),
                new BitmaskCheckBox(33554432, "Profession recipes: can only be looted if you meet requirements and don't already know it"),
                new BitmaskCheckBox(67108864, "Cannot be used in arena"),
                new BitmaskCheckBox(134217728, "Bind to Account", visibility: Visibility.Collapsed),  // auto updated on item quality change
            };

            return new BitmaskStackPanel("itemFlagsSp", cbs);
        }

        internal static BitmaskStackPanel GetItemFlagsExtra()
        {
            var cbs = new BitmaskCheckBox[] {
                new BitmaskCheckBox(1, "Horde Only"),
                new BitmaskCheckBox(2, "Alliance Only"),
                new BitmaskCheckBox(4, "Item for ExtendedCost Vendor"),
                new BitmaskCheckBox(768, "Disable need roll"),
                new BitmaskCheckBox(131072, "BNET_ACCOUNT_BOUND", visibility: Visibility.Collapsed), // auto updated on item quality change
                new BitmaskCheckBox(2097152, "Cannot be transmog item"),
                new BitmaskCheckBox(4194304, "Cannot transmog this item"),
                new BitmaskCheckBox(8388608, "Can transmog"),    
            };

            return new BitmaskStackPanel("itemFlagsExtraSp", cbs);
        }
        
        public static BitmaskStackPanel GetClassFlags()
        {
            var cbs = new BitmaskCheckBox[] {
                new BitmaskCheckBox(1, "Warrior"),
                new BitmaskCheckBox(2, "Paladin"),
                new BitmaskCheckBox(4, "Hunter"),
                new BitmaskCheckBox(8, "Rogue"),
                new BitmaskCheckBox(16, "Priest"),
                new BitmaskCheckBox(32, "Death Knight"),
                new BitmaskCheckBox(64, "Shaman"),
                new BitmaskCheckBox(128, "Mage"),
                new BitmaskCheckBox(256, "Warlock"),
                new BitmaskCheckBox(512, "Monk (5.x)"),
                new BitmaskCheckBox(1024, "Druid"),
            };

            return new BitmaskStackPanel("classesSp", cbs);
        }

        public static BitmaskStackPanel GetRaceFlags()
        {
            var cbs = new BitmaskCheckBox[] {
                new BitmaskCheckBox(1, "Human"),
                new BitmaskCheckBox(2, "Orc"),
                new BitmaskCheckBox(4, "Dwarf"),
                new BitmaskCheckBox(8, "Night Elf"),
                new BitmaskCheckBox(16, "Undead"),
                new BitmaskCheckBox(32, "Tauren"),
                new BitmaskCheckBox(64, "Gnome"),
                new BitmaskCheckBox(128, "Troll"),
                new BitmaskCheckBox(512, "Blood Elf"),
                new BitmaskCheckBox(1024, "Draenei"),
                new BitmaskCheckBox(256, "Goblin (4.x)+"),
                new BitmaskCheckBox(2097152, "Worgen (4.x)+"),
                new BitmaskCheckBox(58720256, "Pandaren (5.x)"),
            };

            return new BitmaskStackPanel("racesSp", cbs);
        }

        public static BitmaskStackPanel GetBagFamilies()
        {
            var cbs = new BitmaskCheckBox[] {
                new BitmaskCheckBox(1, "Arrows"),
                new BitmaskCheckBox(2, "Bullets"),
                new BitmaskCheckBox(4, "Soul Shards"),
                new BitmaskCheckBox(8, "Leatherworking Supplies"),
                new BitmaskCheckBox(16, "Inscription Supplies"),
                new BitmaskCheckBox(32, "Herbs"),
                new BitmaskCheckBox(64, "Enchanting Supplies"),
                new BitmaskCheckBox(128, "Engineering Supplies"),
                new BitmaskCheckBox(256, "Keys"),
                new BitmaskCheckBox(512, "Gems"),
                new BitmaskCheckBox(1024, "Mining Supplies"),
                new BitmaskCheckBox(2048, "Soulbound Equipment"),
                new BitmaskCheckBox(4096, "Vanity Pets"),
                new BitmaskCheckBox(8192, "Currency Tokens"),
                new BitmaskCheckBox(16384, "Quest Items"),
            };

            return new BitmaskStackPanel("bagFamilySp", cbs);
        }
    }

    public class BitmaskCheckBox : CheckBox
    {
        public BitmaskCheckBox(uint value, string details, bool isChecked = false, Visibility visibility = Visibility.Visible)
        {
            Tag = value;
            Content = details;
            IsChecked = isChecked;
            Visibility = visibility;
        }
    }
}
