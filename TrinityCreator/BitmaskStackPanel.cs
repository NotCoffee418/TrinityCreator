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
                    if (cb.Tag.ToString() != "")
                        bitmask += (uint)cb.Tag;
                }
                return bitmask;
            }
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
