using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TrinityCreator
{
    public class BitmaskStackPanel : StackPanel
    {
        public BitmaskStackPanel(string name, List<BitmaskCheckBox> checkboxes, long defaultValue = 0)
        {
            Name = name;
            Checkboxes = checkboxes;
            DefaultValue = defaultValue;

            foreach (var bmcb in Checkboxes)
            {
                Children.Add(bmcb);
                bmcb.Checked += RaiseBmspChanged;
                bmcb.Unchecked += RaiseBmspChanged;
            }
        }

        List<BitmaskCheckBox> checkboxes;

        public List<BitmaskCheckBox> Checkboxes {
            get { return checkboxes; }
            set { checkboxes = value; }
        }

        public string SelectionText
        {
            get { return ToString(); }
        }

        public long BitmaskValue
        {
            get
            {
                long bitmask = 0;
                var checkedCount = 0;
                foreach (CheckBox cb in Checkboxes)
                {
                    if (cb.IsChecked == true)
                    {
                        bitmask += (long) cb.Tag;
                        checkedCount++;
                    }
                }
                if (checkedCount == 0 && bitmask != DefaultValue)
                    bitmask = DefaultValue;
                return bitmask;
            }
            set { SetBitmaskValue(value); }
        }

        public long DefaultValue { get; set; }

        private void RaiseBmspChanged(object sender, RoutedEventArgs e)
        {
            BmspChanged(sender, e);
        }

        public event EventHandler BmspChanged = delegate { };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmv"></param>
        private void SetBitmaskValue(long bmv)
        {
            var bitValues = new List<long>();
            bitValues.Add(0);
            bitValues.Add(1);
            for (long i = 0; i < 31; i++)
            {
                bitValues.Add(bitValues.Last()*2);
                Console.WriteLine(bitValues.Last());
            }
            bitValues.Reverse();

            foreach (var bitmask in bitValues)
                if (bmv >= bitmask)
                    foreach (var cb in Checkboxes)
                        if ((long) cb.Tag >= bitmask)
                        {
                            cb.IsChecked = true;
                            bmv -= bitmask;
                            break;
                        }
        }

        /// <summary>
        /// Sets IsChecked for one specific flag
        /// </summary>
        public void SetValueIsChecked(long key, bool isChecked)
        {
            foreach (CheckBox c in checkboxes)
            {
                long cbKey = (long)c.Tag;
                if (cbKey == key)
                {
                    c.IsChecked = isChecked;
                    break;
                }
            }
        }

        /// <summary>
        ///     Gets clean comma seperated text from checked BitmaskCheckboxes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var list = new List<string>();
            foreach (BitmaskCheckBox c in Children)
                if (c.IsChecked == true)
                    list.Add(c.Content.ToString());
            return string.Join(", ", list);
        }


        public static BitmaskStackPanel GetItemFlags()
        {
            var cbs = new List<BitmaskCheckBox>
            {
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
                new BitmaskCheckBox(33554432,
                    "Profession recipes: can only be looted if you meet requirements and don't already know it"),
                new BitmaskCheckBox(67108864, "Cannot be used in arena"),
                new BitmaskCheckBox(134217728, "Bind to Account", visibility: Visibility.Collapsed)
                // auto updated on item quality change
            };

            return new BitmaskStackPanel("itemFlagsSp", cbs);
        }

        internal static BitmaskStackPanel GetQuestFlags()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Stay Alive"),
                new BitmaskCheckBox(2, "Party Accept"),
                new BitmaskCheckBox(8, "Sharable", true),
                new BitmaskCheckBox(32, "Epic"),
                new BitmaskCheckBox(64, "Raid"),
                new BitmaskCheckBox(512, "Hidden rewards", tooltip:"Item and monetary rewards are hidden in the initial quest details page and in the quest log but will appear once ready to be rewarded."),
                new BitmaskCheckBox(4096, "Daily"),
                new BitmaskCheckBox(8192, "Repeatable"),
                new BitmaskCheckBox(32768, "Weekly"),
                new BitmaskCheckBox(524288, "Auto Accept"),
            };

            return new BitmaskStackPanel("questFlagsSp", cbs);
        }

        internal static BitmaskStackPanel GetQuestSpecialFlags()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(4, "Auto Accept"), // Bind to Flag 524288
                new BitmaskCheckBox(32 ,"The quest requires RequiredOrNpcGo killcredit but NOT kill (a spell cast)"), // good luck
            };

            return new BitmaskStackPanel("questSpecialFlagsSp", cbs);
        }

        internal static BitmaskStackPanel GetItemFlagsExtra()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Horde Only"),
                new BitmaskCheckBox(2, "Alliance Only"),
                new BitmaskCheckBox(4, "Item for ExtendedCost Vendor"),
                new BitmaskCheckBox(768, "Disable need roll"),
                new BitmaskCheckBox(131072, "BNET_ACCOUNT_BOUND", visibility: Visibility.Collapsed),
                // auto updated on item quality change
                new BitmaskCheckBox(2097152, "Cannot be transmog item"),
                new BitmaskCheckBox(4194304, "Cannot transmog this item"),
                new BitmaskCheckBox(8388608, "Can transmog")
            };

            return new BitmaskStackPanel("itemFlagsExtraSp", cbs);
        }

        public static BitmaskStackPanel GetClassFlags(int defaultValue)
        {
            var cbs = new List<BitmaskCheckBox>
            {
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
                new BitmaskCheckBox(1024, "Druid")
            };

            // default value must be -1!
            return new BitmaskStackPanel("classesSp", cbs, defaultValue);
        }

        public static BitmaskStackPanel GetRaceFlags(int defaultValue)
        {
            var cbs = new List<BitmaskCheckBox>
            {
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
                new BitmaskCheckBox(58720256, "Pandaren (5.x)")
            };

            // default value must be -1!
            return new BitmaskStackPanel("racesSp", cbs, defaultValue);
        }

        public static BitmaskStackPanel GetBagFamilies()
        {
            var cbs = new List<BitmaskCheckBox>
            {
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
                new BitmaskCheckBox(16384, "Quest Items")
            };

            return new BitmaskStackPanel("bagFamilySp", cbs);
        }
    }

    public class BitmaskCheckBox : CheckBox
    {
        public BitmaskCheckBox(long value, string details, bool isChecked = false,
            Visibility visibility = Visibility.Visible, string tooltip = "")
        {
            Tag = value;
            Content = details;
            IsChecked = isChecked;
            Visibility = visibility;
            ToolTip = tooltip;
        }
    }
}