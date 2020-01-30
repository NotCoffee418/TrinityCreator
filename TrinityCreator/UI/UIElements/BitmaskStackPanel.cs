using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TrinityCreator.Helpers;

namespace TrinityCreator.UI.UIElements
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


        internal static BitmaskStackPanel GetNpcFlags()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Gossip"),
                new BitmaskCheckBox(2, "Questgiver"),
                new BitmaskCheckBox(16, "Trainer"),
                new BitmaskCheckBox(32, "Class Trainer"),
                new BitmaskCheckBox(64, "Profession Trainer"),
                new BitmaskCheckBox(128, "Vendor"),
                new BitmaskCheckBox(256, "Vendor Ammo"),
                new BitmaskCheckBox(512, "Vendor Food"),
                new BitmaskCheckBox(1024, "Vendor Poison"),
                new BitmaskCheckBox(2048, "Vendor Reagent"),
                new BitmaskCheckBox(4096, "Repairer"),
                new BitmaskCheckBox(8192, "Flight Master"),
                new BitmaskCheckBox(16384, "Only visible to dead players"),
                new BitmaskCheckBox(32768, "Spirit Guide"),
                new BitmaskCheckBox(65536, "Innkeeper"),
                new BitmaskCheckBox(131072, "Banker"),
                new BitmaskCheckBox(262144, "Petitioner"),
                new BitmaskCheckBox(524288, "Tabard Designer"),
                new BitmaskCheckBox(1048576, "Battlemaster"),
                new BitmaskCheckBox(2097152, "Auctioneer"),
                new BitmaskCheckBox(4194304, "Stable Master"),
                new BitmaskCheckBox(8388608, "Guild Banker"),
                new BitmaskCheckBox(16777216, "Spellclick (npc_spellclick_spells table)"),
                new BitmaskCheckBox(67108864, "Mailbox"),
            };
            return new BitmaskStackPanel("npcFlagsSp", cbs);
        }

        public static BitmaskStackPanel GetUnitFlags()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Server Controlled"),
                new BitmaskCheckBox(2 + 128, "Non Attackable"),
                new BitmaskCheckBox(8 + 4096, "PvP Attackable"),
                new BitmaskCheckBox(4, "Disable Move"),
                new BitmaskCheckBox(131072, "Pacified (Creature won't attack)"),
                new BitmaskCheckBox(262144, "Stunned"),
                new BitmaskCheckBox(4194304, "Confused"),
                new BitmaskCheckBox(8388608, "Fleeing"),
                new BitmaskCheckBox(16777216, "Player controlled (Vehicle, Eyes of the Beast, ...)"),
                new BitmaskCheckBox(33554432, "Not selectable"),
                new BitmaskCheckBox(67108864, "Skinnable"),
                new BitmaskCheckBox(134217728, "Is mount"),
                new BitmaskCheckBox(536870912, "Play dead"),
                new BitmaskCheckBox(1073741824, "Sheath weapon"),
            };
            return new BitmaskStackPanel("unitFlagsSp", cbs);
        }

        internal static BitmaskStackPanel GetInhabitTypes()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Ground", true),
                new BitmaskCheckBox(2, "Water", true),
                new BitmaskCheckBox(4, "Flying"),
            };
            return new BitmaskStackPanel("unitFlags2Sp", cbs);
        }

        public static BitmaskStackPanel GetUnitFlags2()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Feign Death"),
                new BitmaskCheckBox(2, "Hide (Only show equipment)"),
                new BitmaskCheckBox(4, "Ignore Reputation (friendly-faction combat)"),
                new BitmaskCheckBox(8, "Comprehend Language"),
                new BitmaskCheckBox(16, "Is mirror image"),
                new BitmaskCheckBox(64, "Force move"),
                new BitmaskCheckBox(128, "Disarm offhand"),
                new BitmaskCheckBox(16384, "Allow enemy interaction"),
                new BitmaskCheckBox(262144, "Allow cheat spells with AttributesEx7"),
            };
            return new BitmaskStackPanel("unitFlags2Sp", cbs);
        }

        public static BitmaskStackPanel GetCreatureDynamicFlags()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Lootable", true),
                new BitmaskCheckBox(2, "Track unit on minimap"),
                new BitmaskCheckBox(4, "Tapped (Grey name, Lua_UnitIsTapped)"),
                new BitmaskCheckBox(8, "Tapped by player"),
                new BitmaskCheckBox(32, "Is dead"),
                new BitmaskCheckBox(128, "Tapped by all thread list (Lua_UnitIsTappedByAllThreatList)"),
            };
            return new BitmaskStackPanel("dynamicFlagsSp", cbs);
        }

        public static BitmaskStackPanel GetCreatureTypeFlags()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Tamable"),
                new BitmaskCheckBox(2, "Ghost"),
                new BitmaskCheckBox(4, "Boss (Level ??)"),
                new BitmaskCheckBox(8, "No parry wound animation"),
                new BitmaskCheckBox(16, "Hide faction tooltip"),
                new BitmaskCheckBox(64, "Spell attackable", true),
                new BitmaskCheckBox(128, "Interact with players when NPC is dead"),
                new BitmaskCheckBox(256, "Herbable"),
                new BitmaskCheckBox(512, "Minable"),
                new BitmaskCheckBox(32768, "Engeneer loot"),
                new BitmaskCheckBox(1024, "Don't show death in combat log"),
                new BitmaskCheckBox(2048, "Mounted combat"),
                new BitmaskCheckBox(4096, "Aid players in combat"),
                new BitmaskCheckBox(8192, "Use pet bar"),
                new BitmaskCheckBox(65536, "Exotic tamable"),
                new BitmaskCheckBox(262144, "Siege weapon"),
                new BitmaskCheckBox(524288, "Takes siege damage"),
                new BitmaskCheckBox(1048576, "Hide nameplate"),
                new BitmaskCheckBox(2097152, "Do not play mounted animation"),
                new BitmaskCheckBox(8388608, "Only interact with it's creator"),
                new BitmaskCheckBox(134217728, "Force single gossip option"),
            };
            return new BitmaskStackPanel("creatureTypeFlagsSp", cbs);
        }

        internal static BitmaskStackPanel GetMechanicImmuneMask()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Charm"),
                new BitmaskCheckBox(2, "Disorient"),
                new BitmaskCheckBox(4, "Disarm"),
                new BitmaskCheckBox(8, "Distract"),
                new BitmaskCheckBox(16, "Fear"),
                new BitmaskCheckBox(32, "Grip"),
                new BitmaskCheckBox(64, "Root"),
                new BitmaskCheckBox(128, "Pacify"),
                new BitmaskCheckBox(256, "Silence"),
                new BitmaskCheckBox(512, "Sleep"),
                new BitmaskCheckBox(1024, "Snare"),
                new BitmaskCheckBox(2048, "Stun"),
                new BitmaskCheckBox(4096, "Freeze"),
                new BitmaskCheckBox(8192, "Knockout"),
                new BitmaskCheckBox(16384, "Bleed"),
                new BitmaskCheckBox(32768, "Bandage"),
                new BitmaskCheckBox(65536, "Polymorph"),
                new BitmaskCheckBox(131072, "Banish"),
                new BitmaskCheckBox(262144, "Shield"),
                new BitmaskCheckBox(524288, "Shackle (undead)"),
                new BitmaskCheckBox(1048576, "Mount"),
                new BitmaskCheckBox(2097152, "Infected"),
                new BitmaskCheckBox(4194304, "Turn Evil"),
                new BitmaskCheckBox(8388608, "Horror"),
                new BitmaskCheckBox(16777216, "Invulnerability (Forbearance, Nether Protection, Diplomatic Immunity only)"),
                new BitmaskCheckBox(33554432, "Interrupt"),
                new BitmaskCheckBox(67108864, "Daze"),
                new BitmaskCheckBox(134217728, "Discovery (create item effect)"),
                new BitmaskCheckBox(268435456, "Immune shield"),
                new BitmaskCheckBox(536870912, "Sapped"),
                new BitmaskCheckBox(1073741824, "Enraged"),
            };
            return new BitmaskStackPanel("mechanicImmuneMaskSp", cbs);
        }

        public static BitmaskStackPanel GetCreatureFlagsExtra()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Instance group bind"),
                new BitmaskCheckBox(2, "Civilian"),
                new BitmaskCheckBox(4, "No parry"),
                new BitmaskCheckBox(8, "No counter-attack at parry"),
                new BitmaskCheckBox(16, "No block"),
                new BitmaskCheckBox(32, "No crush attacks"),
                new BitmaskCheckBox(64, "No XP on kill"),
                new BitmaskCheckBox(128, "Is trigger NPC (invisible)"),
                new BitmaskCheckBox(256, "Immune to Taunt"),
                new BitmaskCheckBox(16384, "World event"),
                new BitmaskCheckBox(32768, "Guard"),
                new BitmaskCheckBox(131072, "No critical strikes"),
                new BitmaskCheckBox(262144, "No weapon skill gain"),
                new BitmaskCheckBox(524288, "Taunt diminish"),
                new BitmaskCheckBox(1048576, "All diminish"),
                new BitmaskCheckBox(2097152, "NPC's can help kill creature & player gets credit"),
                new BitmaskCheckBox(536870912, "Ignore pathfinding (like disabling mmaps for 1 creature)"),
                new BitmaskCheckBox(1073741824, "Immune to knockback"),
            };
            return new BitmaskStackPanel("creatureFlagsExtraSp", cbs);
        }
        public static BitmaskStackPanel GetCreatureBytes1()
        {
            var cbs = new List<BitmaskCheckBox>
            {
                new BitmaskCheckBox(1, "Always stand"),
                new BitmaskCheckBox(2, "Flying creature"),
                new BitmaskCheckBox(4, "Not trackable on minimap"),
                new BitmaskCheckBox(16, "Kneel"),
                new BitmaskCheckBox(48, "Lay down"),
            };
            return new BitmaskStackPanel("creatureBytes1Sp", cbs);
        }

        /// <summary>
        /// Returns true if checkbox with the specific bitmask value is checked
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        internal bool HasValue(int v)
        {
            try
            {
                foreach (BitmaskCheckBox bmcb in Children)
                {
                    if ((long)bmcb.Tag == v) // Attempt to find bmcb with the requested value (defined in Tag)
                        return bmcb.IsChecked == true;
                }
                Logger.Log("BMSP: Failed to find BMCB with requested value in this BMSP.", Logger.Status.Warning, true);
                return false;
            }
            catch
            {
                Logger.Log("Error parsing data from BitmaskStackpanel. Output data may be invalid.", Logger.Status.Error, true);
                return false;
            }
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

        public long GetValue()
        {
            return (long)Tag;
        }
    }
}