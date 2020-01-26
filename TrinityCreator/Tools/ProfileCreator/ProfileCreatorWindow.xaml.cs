using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrinityCreator.Profiles;
using Newtonsoft.Json;
using TrinityCreator.Helpers;

namespace TrinityCreator.Tools.ProfileCreator
{
    /// <summary>
    /// Interaction logic for ProfileCreator.xaml
    /// </summary>
    public partial class ProfileCreatorWindow : Window
    {
        public ProfileCreatorWindow()
        {
            InitializeComponent();
            DisplayEntries();
            DataContext = EditingProfile;
        }


        List<ProfileCreatorEntry> ItemElements;
        List<ProfileCreatorEntry> QuestElements;
        List<ProfileCreatorEntry> CreatureElements;
        List<ProfileCreatorEntry> LootElements;
        List<ProfileCreatorEntry> VendorElements;
        List<ProfileCreatorSetting> LookupToolElements;
        List<ProfileCreatorEntry> CustomFieldsElements;
        Profile EditingProfile = new Profile();


        /// <summary>
        /// Display all appKeys supported by the application with the ability to set it's sql key.
        /// Should only be called once while opening the window
        /// </summary>
        private void DisplayEntries()
        {
            // Item Entries
            ItemElements = new List<ProfileCreatorEntry>()
            {
                new ProfileCreatorEntry("EntryId"),
                new ProfileCreatorEntry("Name"),
                new ProfileCreatorEntry("Quote"),
                new ProfileCreatorEntry("Class"),
                new ProfileCreatorEntry("ItemSubClass"),
                new ProfileCreatorEntry("Quality"),
                new ProfileCreatorEntry("DisplayId"),
                new ProfileCreatorEntry("Binds", "eg. Bind on Pickup"),
                new ProfileCreatorEntry("MinLevel"),
                new ProfileCreatorEntry("MaxAllowed"),
                new ProfileCreatorEntry("AllowedClass", "Bitmask using 3.3.5a values"),
                new ProfileCreatorEntry("AllowedRace", "Bitmask using 3.3.5a values"),
                new ProfileCreatorEntry("ValueBuy", "Buy price in copper"),
                new ProfileCreatorEntry("ValueSell", "Sell price in copper"),
                new ProfileCreatorEntry("InventoryType"),
                new ProfileCreatorEntry("Flags", "Assumes 3.3.5a flags."),
                new ProfileCreatorEntry("FlagsExtra", "Assumes 3.3.5a extra flags."),
                new ProfileCreatorEntry("BuyCount"),
                new ProfileCreatorEntry("Stackable", "Stack size?"),
                new ProfileCreatorEntry("ContainerSlots"),
                new ProfileCreatorEntry("MinDamage"),
                new ProfileCreatorEntry("MaxDamage"),
                new ProfileCreatorEntry("AttackSpeed"),
                new ProfileCreatorEntry("DamageType", "Physical or magic school"),
                new ProfileCreatorEntry("AmmoType"),
                new ProfileCreatorEntry("Durability"),
                new ProfileCreatorEntry("Armor"),
                new ProfileCreatorEntry("Block"),
                new ProfileCreatorEntry("BagFamily"),
                new ProfileCreatorEntry("ItemLevel", "WotLK+ Item Level. Not the required player level."),
                new ProfileCreatorEntry("RangedModRange", "Always 100 if weapon is ranged. Otherwise it's ignored."),

                // These are not hardcoded in export but in DamageType.GetDamageTypes()
                new ProfileCreatorEntry("HolyResistance"),
                new ProfileCreatorEntry("FireResistance"),
                new ProfileCreatorEntry("NatureResistance"),
                new ProfileCreatorEntry("FrostResistance"),
                new ProfileCreatorEntry("ShadowResistance"),
                new ProfileCreatorEntry("ArcaneResistance"),

                // Gems
                new ProfileCreatorEntry("SocketColor1", "Gem socket color. Disable <3.3.5a"),
                new ProfileCreatorEntry("SocketContent1", "Gem socket content. Disable <3.3.5a"),
                new ProfileCreatorEntry("SocketColor2", "Gem socket color. Disable <3.3.5a"),
                new ProfileCreatorEntry("SocketContent2", "Gem socket content. Disable <3.3.5a"),
                new ProfileCreatorEntry("SocketColor3", "Gem socket color. Disable <3.3.5a"),
                new ProfileCreatorEntry("SocketContent3", "Gem socket content. Disable <3.3.5a"),
                new ProfileCreatorEntry("SocketBonus"),

                // Stats
                new ProfileCreatorEntry("StatType1"),
                new ProfileCreatorEntry("StatValue1"),
                new ProfileCreatorEntry("StatType2"),
                new ProfileCreatorEntry("StatValue2"),
                new ProfileCreatorEntry("StatType3"),
                new ProfileCreatorEntry("StatValue3"),
                new ProfileCreatorEntry("StatType4"),
                new ProfileCreatorEntry("StatValue4"),
                new ProfileCreatorEntry("StatType5"),
                new ProfileCreatorEntry("StatValue5"),
                new ProfileCreatorEntry("StatType6"),
                new ProfileCreatorEntry("StatValue6"),
                new ProfileCreatorEntry("StatType7"),
                new ProfileCreatorEntry("StatValue7"),
                new ProfileCreatorEntry("StatType8"),
                new ProfileCreatorEntry("StatValue8"),
                new ProfileCreatorEntry("StatType9"),
                new ProfileCreatorEntry("StatValue9"),
                new ProfileCreatorEntry("StatType10"),
                new ProfileCreatorEntry("StatValue10"),
                new ProfileCreatorEntry("StatsCount"),
            };
            foreach (var e in ItemElements)
                itemSp.Children.Add(e);

            // Creature Entries
            CreatureElements = new List<ProfileCreatorEntry>()
            {
                new ProfileCreatorEntry("Entry"),
                new ProfileCreatorEntry("ModelId1"),
                new ProfileCreatorEntry("ModelId2"),
                new ProfileCreatorEntry("ModelId3"),
                new ProfileCreatorEntry("ModelId4"),
                new ProfileCreatorEntry("Name"),
                new ProfileCreatorEntry("Subname"),
                new ProfileCreatorEntry("MinLevel"),
                new ProfileCreatorEntry("MaxLevel"),
                new ProfileCreatorEntry("Faction"),
                new ProfileCreatorEntry("NpcFlags"),
                new ProfileCreatorEntry("SpeedWalk"),
                new ProfileCreatorEntry("SpeedRun"),
                new ProfileCreatorEntry("Scale"),
                new ProfileCreatorEntry("Rank", "Normal, elite, boss..."),
                new ProfileCreatorEntry("DmgSchool", "physical, holy, ..."),
                new ProfileCreatorEntry("BaseAttackTime", "in miliseconds"),
                new ProfileCreatorEntry("RangeAttackTime", "in miliseconds"),
                new ProfileCreatorEntry("UnitClass", "warrior, paladin, rogue, mage"),
                new ProfileCreatorEntry("UnitFlags"),
                new ProfileCreatorEntry("UnitFlags2"),
                new ProfileCreatorEntry("DynamicFlags"),
                new ProfileCreatorEntry("Family"),
                new ProfileCreatorEntry("TrainerType"),
                new ProfileCreatorEntry("TrainerSpell"),
                new ProfileCreatorEntry("TrainerClass"),
                new ProfileCreatorEntry("TrainerRace"),
                new ProfileCreatorEntry("CreatureType"),
                new ProfileCreatorEntry("TypeFlags"),
                new ProfileCreatorEntry("LootId", "ID of the creature_loot_template table to be used"),
                new ProfileCreatorEntry("PickpocketLoot"),
                new ProfileCreatorEntry("SkinLoot"),
                new ProfileCreatorEntry("HolyResistance", "Old resistance system. Place value in the column with this name. Only use old or new system, not both."),
                new ProfileCreatorEntry("FireResistance", "Old resistance system. Place value in the column with this name. Only use old or new system, not both."),
                new ProfileCreatorEntry("NatureResistance", "Old resistance system. Place value in the column with this name. Only use old or new system, not both."),
                new ProfileCreatorEntry("FrostResistance", "Old resistance system. Place value in the column with this name. Only use old or new system, not both."),
                new ProfileCreatorEntry("ShadowResistance", "Old resistance system. Place value in the column with this name. Only use old or new system, not both."),
                new ProfileCreatorEntry("ArcaneResistance", "Old resistance system. Place value in the column with this name. Only use old or new system, not both."),
                new ProfileCreatorEntry("ResistanceCreatureId", "Modern system, creates rows in creature_template_resistance or equivalent. See documentation. Dont use both systems."),
                new ProfileCreatorEntry("ResistanceSchool", "Modern system, creates rows in creature_template_resistance or equivalent. See documentation. Dont use both systems."),
                new ProfileCreatorEntry("ResistanceAmount", "Modern system, creates rows in creature_template_resistance or equivalent. See documentation. Dont use both systems."),
                new ProfileCreatorEntry("PetDataId", "Modern system, creates rows in creature_template_resistance or equivalent. See documentation. Dont use both systems."),
                new ProfileCreatorEntry("VehicleId", "Modern system, creates rows in creature_template_resistance or equivalent. See documentation. Dont use both systems."),
                new ProfileCreatorEntry("MinGold", "Modern system, creates rows in creature_template_resistance or equivalent. See documentation. Dont use both systems."),
                new ProfileCreatorEntry("MaxGold", "Modern system, creates rows in creature_template_resistance or equivalent. See documentation. Dont use both systems."),
                new ProfileCreatorEntry("AiName", "Modern system, creates rows in creature_template_resistance or equivalent. See documentation. Dont use both systems."),
                new ProfileCreatorEntry("MovementType", "0=idle, 1=random, 2=waypoint movement"),
                new ProfileCreatorEntry("InhabitType", "bitmask 1=ground, 2=water, 4=flying, 8=rooted"),
                new ProfileCreatorEntry("HoverHeight", "requires MOVEMENTFLAG_DISABLE_GRAVITY"),
                new ProfileCreatorEntry("HealthModifier", "1f by default"),
                new ProfileCreatorEntry("ManaModifier", "1f by default"),
                new ProfileCreatorEntry("DamageModifier", "1f by default"),
                new ProfileCreatorEntry("ArmorModifier", "1f by default"),
                new ProfileCreatorEntry("ExperienceModifier", "1f by default"),
                new ProfileCreatorEntry("RacialLeader"),
                new ProfileCreatorEntry("RegenHealth", "bool, 0 or 1 in db"),
                new ProfileCreatorEntry("MechanicImmuneMask"),
                new ProfileCreatorEntry("FlagsExtra"),
                new ProfileCreatorEntry("Mount"),
                new ProfileCreatorEntry("Bytes1"),
                new ProfileCreatorEntry("Emote"),
                new ProfileCreatorEntry("Auras", "space seperated list of spell IDs placed in one column."),
                new ProfileCreatorEntry("Spell1", "Old spell system, simply puts up to 8 spells in different columns. Disable if modern system."),
                new ProfileCreatorEntry("Spell2", "Old spell system, simply puts up to 8 spells in different columns. Disable if modern system."),
                new ProfileCreatorEntry("Spell3", "Old spell system, simply puts up to 8 spells in different columns. Disable if modern system."),
                new ProfileCreatorEntry("Spell4", "Old spell system, simply puts up to 8 spells in different columns. Disable if modern system."),
                new ProfileCreatorEntry("Spell5", "Old spell system, simply puts up to 8 spells in different columns. Disable if modern system."),
                new ProfileCreatorEntry("Spell6", "Old spell system, simply puts up to 8 spells in different columns. Disable if modern system."),
                new ProfileCreatorEntry("Spell7", "Old spell system, simply puts up to 8 spells in different columns. Disable if modern system."),
                new ProfileCreatorEntry("Spell8", "Old spell system, simply puts up to 8 spells in different columns. Disable if modern system."),
                new ProfileCreatorEntry("SpellCreatureID", "WARNING: Read Documentation!! Alternative spell system (TC 2020 uses this)."),
                new ProfileCreatorEntry("SpellSpell", "WARNING: Read Documentation!! Alternative spell system (TC 2020 uses this)."),
                new ProfileCreatorEntry("SpellIndex", "WARNING: Read Documentation!! Alternative spell system (TC 2020 uses this)."),
                new ProfileCreatorEntry("DifficultyEntry1"),
                new ProfileCreatorEntry("DifficultyEntry2"),
                new ProfileCreatorEntry("DifficultyEntry3"),
                new ProfileCreatorEntry("MinDmg"),
                new ProfileCreatorEntry("MaxDmg"),
                new ProfileCreatorEntry("Weapon1"),
                new ProfileCreatorEntry("Weapon2"),
                new ProfileCreatorEntry("Weapon3"),
                new ProfileCreatorEntry("MinLevelHealth"),
                new ProfileCreatorEntry("MaxLevelHealth"),
                new ProfileCreatorEntry("MinLevelMana"),
                new ProfileCreatorEntry("MaxLevelMana"),
                new ProfileCreatorEntry("MinMeleeDmg"),
                new ProfileCreatorEntry("MaxMeleeDmg"),
                new ProfileCreatorEntry("MinRangedDmg"),
                new ProfileCreatorEntry("MaxRangedDmg"),
                new ProfileCreatorEntry("Armor"),
                new ProfileCreatorEntry("MeleeAttackPower"),
                new ProfileCreatorEntry("RangedAttackPower"),
                new ProfileCreatorEntry("Civilian"),
            };
            foreach (var e in CreatureElements)
                creatureSp.Children.Add(e);

            // Quest Entries
            QuestElements = new List<ProfileCreatorEntry>()
            {
                new ProfileCreatorEntry("EntryId"),
                new ProfileCreatorEntry("QuestSort"),
                new ProfileCreatorEntry("QuestInfo"),
                new ProfileCreatorEntry("SuggestedGroupNum"),
                new ProfileCreatorEntry("Flags"),
                new ProfileCreatorEntry("SpecialFlags"),
                new ProfileCreatorEntry("LogTitle"),
                new ProfileCreatorEntry("LogDescription"),
                new ProfileCreatorEntry("QuestDescription"),
                new ProfileCreatorEntry("AreaDescription"),
                new ProfileCreatorEntry("QuestCompletionLog"),
                new ProfileCreatorEntry("RewardText"),
                new ProfileCreatorEntry("IncompleteText"),

                new ProfileCreatorEntry("PrevQuest"),
                new ProfileCreatorEntry("NextQuest"),
                //new ProfileCreatorEntry("ExclusiveGroup"),
                new ProfileCreatorEntry("Questgiver"),
                new ProfileCreatorEntry("QuestCompleter"),

                new ProfileCreatorEntry("QuestLevel"),
                new ProfileCreatorEntry("MinLevel"),
                new ProfileCreatorEntry("MaxLevel"),
                //new ProfileCreatorEntry("RequiredMinRepFaction"),
                //new ProfileCreatorEntry("RequiredMinRepValue"),
                //new ProfileCreatorEntry("RequiredMaxRepFaction"),
                //new ProfileCreatorEntry("RequiredMaxRepValue"),
                //new ProfileCreatorEntry("RequiredSkillId"),
                //new ProfileCreatorEntry("RequiredSkillPoints"),
                new ProfileCreatorEntry("AllowableClass"),
                new ProfileCreatorEntry("AllowableRace"),

                new ProfileCreatorEntry("StartItem", "Item ID of item provided on accept"),
                new ProfileCreatorEntry("ProvidedItemCount", "Amount of StartItem you get"),
                new ProfileCreatorEntry("SourceSpell"),
                new ProfileCreatorEntry("PoiCoordinateX"),
                new ProfileCreatorEntry("PoiCoordinateY"),
                new ProfileCreatorEntry("PoiCoordinateZ"),
                new ProfileCreatorEntry("PoiCoordinateMap"),

                new ProfileCreatorEntry("TimeAllowed"),
                new ProfileCreatorEntry("RequiredPlayerKills"),
                new ProfileCreatorEntry("RequiredItem1"),
                new ProfileCreatorEntry("RequiredItemCount1"),
                new ProfileCreatorEntry("RequiredItem2"),
                new ProfileCreatorEntry("RequiredItemCount2"),
                new ProfileCreatorEntry("RequiredItem3"),
                new ProfileCreatorEntry("RequiredItemCount3"),
                new ProfileCreatorEntry("RequiredItem4"),
                new ProfileCreatorEntry("RequiredItemCount4"),
                new ProfileCreatorEntry("RequiredItem5"),
                new ProfileCreatorEntry("RequiredItemCount5"),
                new ProfileCreatorEntry("RequiredItem6"),
                new ProfileCreatorEntry("RequiredItemCount6"),
                new ProfileCreatorEntry("RequiredNpcOrGo1"),
                new ProfileCreatorEntry("RequiredNpcOrGoCount1"),
                new ProfileCreatorEntry("RequiredNpcOrGo2"),
                new ProfileCreatorEntry("RequiredNpcOrGoCount2"),
                new ProfileCreatorEntry("RequiredNpcOrGo3"),
                new ProfileCreatorEntry("RequiredNpcOrGoCount3"),
                new ProfileCreatorEntry("RequiredNpcOrGo4"),
                new ProfileCreatorEntry("RequiredNpcOrGoCount4"),
                new ProfileCreatorEntry("RequiredNpcOrGo5"),
                new ProfileCreatorEntry("RequiredNpcOrGoCount5"),
                new ProfileCreatorEntry("RequiredNpcOrGo6"),
                new ProfileCreatorEntry("RequiredNpcOrGoCount6"),


                new ProfileCreatorEntry("RewardXpDifficulty"),
                new ProfileCreatorEntry("RewardMoney"),
                new ProfileCreatorEntry("RewardSpell"),
                new ProfileCreatorEntry("RewardHonor"),
                //new ProfileCreatorEntry("RewardMailTemplateId"),
                //new ProfileCreatorEntry("RewardMailDelay"),
                new ProfileCreatorEntry("RewardTitle"),
                new ProfileCreatorEntry("RewardArenaPoints"),
                new ProfileCreatorEntry("RewardTalents"),
                new ProfileCreatorEntry("RewardItem1"),
                new ProfileCreatorEntry("RewardItemAmount1"),
                new ProfileCreatorEntry("RewardItem2"),
                new ProfileCreatorEntry("RewardItemAmount2"),
                new ProfileCreatorEntry("RewardItem3"),
                new ProfileCreatorEntry("RewardItemAmount3"),
                new ProfileCreatorEntry("RewardItem4"),
                new ProfileCreatorEntry("RewardItemAmount4"),
                new ProfileCreatorEntry("RewardChoiceItemID1"),
                new ProfileCreatorEntry("RewardChoiceItemAmount1"),
                new ProfileCreatorEntry("RewardChoiceItemID2"),
                new ProfileCreatorEntry("RewardChoiceItemAmount2"),
                new ProfileCreatorEntry("RewardChoiceItemID3"),
                new ProfileCreatorEntry("RewardChoiceItemAmount3"),
                new ProfileCreatorEntry("RewardChoiceItemID4"),
                new ProfileCreatorEntry("RewardChoiceItemAmount4"),
                new ProfileCreatorEntry("RewardChoiceItemID5"),
                new ProfileCreatorEntry("RewardChoiceItemAmount5"),
                new ProfileCreatorEntry("RewardChoiceItemID6"),
                new ProfileCreatorEntry("RewardChoiceItemAmount6"),
                new ProfileCreatorEntry("FactionRewardID1"),
                new ProfileCreatorEntry("RewardFactionOverride1", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
                new ProfileCreatorEntry("FactionRewardID2"),
                new ProfileCreatorEntry("RewardFactionOverride2", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
                new ProfileCreatorEntry("FactionRewardID3"),
                new ProfileCreatorEntry("RewardFactionOverride3", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
                new ProfileCreatorEntry("FactionRewardID4"),
                new ProfileCreatorEntry("RewardFactionOverride4", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
                new ProfileCreatorEntry("FactionRewardID5"),
                new ProfileCreatorEntry("RewardFactionOverride5", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
            };
            foreach (var e in QuestElements)
                questSp.Children.Add(e);

            // Loot Entries
            LootElements = new List<ProfileCreatorEntry>()
            {
                new ProfileCreatorEntry("Entry"),
                new ProfileCreatorEntry("Item"),
                new ProfileCreatorEntry("Chance"),
                new ProfileCreatorEntry("QuestRequired"),
                new ProfileCreatorEntry("MinCount"),
                new ProfileCreatorEntry("MaxCount"),
            };
            foreach (var e in LootElements)
                lootSp.Children.Add(e);

            // Vendor Entries
            VendorElements = new List<ProfileCreatorEntry>()
            {
                new ProfileCreatorEntry("NpcEntry"),
                new ProfileCreatorEntry("Item"),
                new ProfileCreatorEntry("Slot"),
                new ProfileCreatorEntry("MaxCount"),
                new ProfileCreatorEntry("IncrTime"),
                new ProfileCreatorEntry("ExtendedCost"),
            };
            foreach (var e in VendorElements)
                vendorSp.Children.Add(e);

            // LookupTool settings
            LookupToolElements = new List<ProfileCreatorSetting>()
            {
            };
            foreach (var e in LookupToolElements)
                lookupToolSp.Children.Add(e);

            // Custom Fields
            CustomFieldsElements = new List<ProfileCreatorEntry>();
        }

        private string GenerateJson()
        {
            // Place data from UI in EditingProfile
            EditingProfile.Item = ToProfileFormat(ItemElements);
            EditingProfile.Quest = ToProfileFormat(QuestElements);
            EditingProfile.Creature = ToProfileFormat(CreatureElements);
            EditingProfile.Loot = ToProfileFormat(LootElements);
            EditingProfile.Vendor = ToProfileFormat(VendorElements);
            EditingProfile.LookupTool = ToProfileFormat(LookupToolElements);
            EditingProfile.CustomFields = ToProfileFormat(CustomFieldsElements);

            // Convert to json & beautify
            return JsonConvert.SerializeObject(EditingProfile, Formatting.Indented);
        }

        /// <summary>
        /// Converts a section (eg. Loot) from ProfileCreatorEntry to data that's compatible with Profile
        /// </summary>
        /// <param name="sectionList"></param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, string>> ToProfileFormat(List<ProfileCreatorEntry> sectionList)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();

            // Check if input data is valid (or.. won't cause the application to crash)
            var issues = sectionList.Where(e => (e.IsIncluded && 
                (e.SqlKey == null || e.SqlKey == String.Empty || e.TableName == null || e.TableName == String.Empty)));     
            
            if (issues.Count() > 0)
            {
                Logger.Log($"You didn't enter an SqlKey or TableName for: " + string.Join(", ", issues.Select(e => e.AppKey)),
                    Logger.Status.Warning, true);
                return result;
            }
                

            // List distinct table names
            IEnumerable<string> tableNames = sectionList
                    .Where(e => e.IsIncluded)
                    .GroupBy(e => e.TableName.ToLower())
                    .Select(grp => grp.First().TableName);

            // Create each "table"
            foreach (var tableName in tableNames)
            {
                // Find entries in this table & add them to tableDict
                var tableDict = new Dictionary<string, string>();
                foreach(var entry in sectionList.Where(e => e.IsIncluded && e.TableName.ToLower() == tableName.ToLower()))
                    tableDict.Add(entry.AppKey, entry.SqlKey);

                // Add all fields in this table to result
                result.Add(tableName, tableDict);
            }

            return result;
        }

        /// <summary>
        /// Converts a settings section. This is simply keyvalue, no table name specified.
        /// Can be used for anything that's not binding something directly to a trinity table.
        /// </summary>
        /// <param name="sectionList"></param>
        /// <returns></returns>
        private Dictionary<string, string> ToProfileFormat(List<ProfileCreatorSetting> sectionList)
        {
            var result = new Dictionary<string, string>();
            foreach (var pSetting in sectionList)
                if (pSetting.IsIncluded)
                    result.Add(pSetting.SettingKey, pSetting.SettingValue);
            return result;
        }

        private void copyBtn_Click(object sender, RoutedEventArgs e)
        {
            string json = GenerateJson();
            if (json != null) 
            {
                Clipboard.SetDataObject(json);
                Logger.Log("Profile copied to clipboard.", Logger.Status.Info, true);
            }
        }

        private void addCustomFieldBtn_Click(object sender, RoutedEventArgs e)
        {
            var ne = new ProfileCreatorEntry();
            CustomFieldsElements.Add(ne);
            customFieldsSp.Children.Add(ne);
        }
    }
}
