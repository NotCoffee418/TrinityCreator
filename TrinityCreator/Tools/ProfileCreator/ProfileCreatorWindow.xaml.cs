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
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Reflection;

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
        List<ProfileCreatorEntry> CustomFieldsElements;
        List<ProfileCreatorDefinition> DefinitionElements;
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
                new ProfileCreatorEntry("Material"),
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
                new ProfileCreatorEntry("Sheath"),
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
                new ProfileCreatorEntry("InhabitType", "Old System (bitmask) - 1=ground, 2=water, 4=flying, 8=rooted"),
                new ProfileCreatorEntry("InhabitCreatureID", "Modern System - using creature_template_movement table"),
                new ProfileCreatorEntry("InhabitGround", "Modern System - using creature_template_movement table"),
                new ProfileCreatorEntry("InhabitWater", "Modern System - using creature_template_movement table"),
                new ProfileCreatorEntry("InhabitFlight", "Modern System - using creature_template_movement table"),
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


                new ProfileCreatorEntry("RewardXpDifficulty", "Use this field or RewardXpRaw, not both."),
                new ProfileCreatorEntry("RewardXpRaw", "Use this field or RewardXpDifficulty, not both."),
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
                new ProfileCreatorEntry("RewardFactionID1"),
                new ProfileCreatorEntry("RewardFactionOverride1", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
                new ProfileCreatorEntry("RewardFactionID2"),
                new ProfileCreatorEntry("RewardFactionOverride2", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
                new ProfileCreatorEntry("RewardFactionID3"),
                new ProfileCreatorEntry("RewardFactionOverride3", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
                new ProfileCreatorEntry("RewardFactionID4"),
                new ProfileCreatorEntry("RewardFactionOverride4", "Specific rep input. User input value is multiplied by 100 (Trinitycore Support). Open ticket is this causes problems for your emulator."),
                new ProfileCreatorEntry("RewardFactionID5"),
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

            // Custom Fields
            CustomFieldsElements = new List<ProfileCreatorEntry>();


            // LookupTool settings
            DefinitionElements = new List<ProfileCreatorDefinition>()
            {
                new ProfileCreatorDefinition("GameObjectTableName", "(Optional) gameobject_template or equivalent"),
                new ProfileCreatorDefinition("GameObjectIdColumn", "(Optional) Id column of the gameobject_template or equivalent table"),
                new ProfileCreatorDefinition("GameObjectDisplayIdColumn", "(Optional) DisplayId column of the gameobject_template or equivalent table"),
                new ProfileCreatorDefinition("GameObjectNameColumn", "(Optional) Object Name column of the gameobject_template or equivalent table"),
                new ProfileCreatorDefinition("SpellTableName", "(Optional) spell_dbc or equivalent table. Used for lookup tool."),
                new ProfileCreatorDefinition("SpellIdColumn", "(Optional) spell_dbc or equivalent's id column"),
                new ProfileCreatorDefinition("SpellNameColumn", "spell_dbc or equivalent's name column"),
                new ProfileCreatorDefinition("RewardFactionOverrideModifier", "RewardFactionOverride: This should be 1 or 100. Modern Trinity will divide input value by 100 for some reason. Put 1 if database uses raw rep value, put 100 if your emulator divides input by 100."),
            };
            foreach (var e in DefinitionElements)
                definitionsSp.Children.Add(e);
        }

        private string GenerateJson()
        {
            // Validate game version
            int x;
            if (EditingProfile.GameVersion == null || !EditingProfile.GameVersion.Contains('.') || !int.TryParse(EditingProfile.GameVersion.Split('.')[0], out x))
            {
                Logger.Log("Profile Creation Error: GameVersion must conform to structure. Example: 3.3.5a. See documentation for more info. Profile will export with game version 0.0.",
                    Logger.Status.Error, true);
                EditingProfile.GameVersion = "0.0";
            }

            // Define profile's TestedBuild
            EditingProfile.TestedBuild = Assembly.GetExecutingAssembly().GetName().Version.Revision;

            // Place data from UI in EditingProfile
            EditingProfile.Item = ToProfileFormat(ItemElements);
            EditingProfile.Quest = ToProfileFormat(QuestElements);
            EditingProfile.Creature = ToProfileFormat(CreatureElements);
            EditingProfile.Loot = ToProfileFormat(LootElements);
            EditingProfile.Vendor = ToProfileFormat(VendorElements);
            EditingProfile.CustomFields = ToProfileFormat(CustomFieldsElements);
            EditingProfile.Definitions = ToProfileFormat(DefinitionElements);

            // Validate that addon tables have an ID specified in customfields
            ValidateAddonTableCustoms(EditingProfile.Item, Export.C.Item);
            ValidateAddonTableCustoms(EditingProfile.Quest, Export.C.Quest);
            ValidateAddonTableCustoms(EditingProfile.Creature, Export.C.Creature);
            ValidateAddonTableCustoms(EditingProfile.Loot, Export.C.Loot);
            ValidateAddonTableCustoms(EditingProfile.Vendor, Export.C.Vendor);

            // Convert to json & beautify
            return JsonConvert.SerializeObject(EditingProfile, Formatting.Indented);
        }

        /// <summary>
        /// Throws warning if an addon table doesn't have table ID defined in customs
        /// It doesn't validate the values. Only checks for tablename in customs.
        /// </summary>
        /// <param name="item"></param>
        private bool ValidateAddonTableCustoms(Dictionary<string, Dictionary<string, string>> item, Export.C c)
        {
            bool result = true;
            if (item.Count == 1)
                return true;

            // Exceptions to the rule. These tables act differently and have a dedicated column in the profile
            // Lists appkeys in tables which are treated as exceptions.
            string[] exceptionalAppKeys = new string[] {
                "ResistanceCreatureId",
                "SpellCreatureID",
                "InhabitCreatureID",
            };
            
            foreach (var table in item.Keys)
            {
                string primaryAppKey = ProfileHelper.GetToolIdAppKey(c);
                if (item[table].ContainsKey(primaryAppKey))
                    continue; // this is the primary table & requires no custom

                // Check exceptions to the rule
                bool foundExceptional = false;
                foreach (var except in exceptionalAppKeys)
                    if (item[table].ContainsKey(except))
                    {
                        foundExceptional = true;
                        continue; // Exceptional, move on
                    }
                if (foundExceptional)
                    continue;                    

                // Throw warning if no custom was found for this table
                if (!EditingProfile.CustomFields.ContainsKey(table))
                {
                    Logger.Log($"Warning: You need to define custom keys for all addon tables. Custom key missing for '{table}'. See documentation for more info.",
                        Logger.Status.Warning, true);
                    result = false;
                }
            }
            return result;
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
        private Dictionary<string, string> ToProfileFormat(List<ProfileCreatorDefinition> sectionList)
        {
            var result = new Dictionary<string, string>();
            foreach (var pDef in sectionList)
                if (pDef.IsIncluded)
                    result.Add(pDef.DefinitionKey, pDef.DefinitionValue);
            return result;
        }

        // Load an existing profile into the UI
        private void LoadProfile(Profile p)
        {
            // Load metadata
            EditingProfile = p;
            DataContext = EditingProfile;

            // Call partial loads for each creator
            _PartialLoad(creatureSp, p.Creature);
            _PartialLoad(questSp, p.Quest);
            _PartialLoad(itemSp, p.Item);
            _PartialLoad(lootSp, p.Loot);
            _PartialLoad(vendorSp, p.Vendor);
            _PartialLoadDefinitions(definitionsSp, p.Definitions);

            // Manually set metadata fields because faster

            // Add Custom Fields
            customFieldsSp.Children.Clear();
            CustomFieldsElements.Clear();
            foreach (var custFieldPair in p.CustomFields)
            {
                foreach (var keys in custFieldPair.Value)
                {
                    // Prepare the ProfileCreatorEntry
                    var ne = new ProfileCreatorEntry();
                    ne.AppKey = keys.Key;
                    ne.SqlKey = keys.Value;
                    ne.TableName = custFieldPair.Key;
                    ne.IsIncluded = true;

                    // Add them to UI
                    CustomFieldsElements.Add(ne);
                    customFieldsSp.Children.Add(ne);
                }
            }
        }

        // Used by LoadProfile, shouldn't be used seperately.
        private void _PartialLoad(StackPanel sp, Dictionary<string, Dictionary<string, string>> pData)
        {
            try
            {
                // Careful, this assumes that the stackpanel's only children are ProfileCreatorEntries
                foreach (object pceObj in sp.Children)
                {
                    ProfileCreatorEntry pce = (ProfileCreatorEntry)pceObj;

                    // Attempt to find a defined sqlKey for this appKEy in the profile segment.
                    var matchTables = pData.Where(dd => dd.Value
                        .Where(keys => keys.Key == pce.AppKey).Count() > 0
                    );

                    // No match found, clear the sqlKey and tablename and disable
                    if (matchTables.Count() == 0)
                    {
                        pce.SqlKey = "";
                        pce.TableName = "";
                        pce.IsIncluded = false;
                    }
                    else // Match found, set known values to ui
                    {
                        pce.TableName = matchTables.First().Key;
                        pce.SqlKey = matchTables.First().Value.Where(kvp => kvp.Key == pce.AppKey).First().Value; // I'm too deep in to change the structure now :P
                        pce.IsIncluded = true;
                    }
                }
            }
            catch
            {
                Logger.Log("There was a problem loading this profile into the UI. Is the profile valid for this version of TrinityCreator?", Logger.Status.Error);
            }
        }

        private void _PartialLoadDefinitions(StackPanel sp, Dictionary<string, string> pData)
        {
            try
            {
                // Careful, this assumes that the stackpanel's only children are ProfileCreatorEntries
                foreach (object pceObj in sp.Children)
                {
                    ProfileCreatorDefinition pce = (ProfileCreatorDefinition)pceObj;

                    if (pData == null) // Happens when loading alpha profiles
                    {
                        pce.DefinitionValue = "";
                        pce.IsIncluded = false;
                        continue;
                    }

                    // Attempt to find a defined sqlKey for this appKEy in the profile segment.
                    var matches = pData.Where(kvp => kvp.Key == pce.DefinitionKey);

                    // No match found, clear the sqlKey and tablename and disable
                    if (matches.Count() == 0)
                    {
                        pce.DefinitionValue = "";
                        pce.IsIncluded = false;
                    }
                    else // Match found, set known values to ui
                    {
                        pce.DefinitionValue = matches.First().Value;
                        pce.IsIncluded = true;
                    }
                }
            }
            catch
            {
                Logger.Log("There was a problem loading this profile into the UI. Is the profile valid for this version of TrinityCreator?", Logger.Status.Error, true);
            }
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
        
        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            // Select file dialog
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "json";
            ofd.FilterIndex = 0;
            ofd.Filter = "Profile Files (*.json)|*.json";
            ofd.InitialDirectory = System.IO.Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDoc‌​uments), "TrinityCreator", "Profiles");
            if (ofd.ShowDialog() != true)
                return; // No file or invalid file selected, do nothing

            Profile p = Profile.LoadFile(ofd.FileName, true);
            LoadProfile(p);
        }


        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            string json = GenerateJson();
            if (json != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = "json";
                sfd.FilterIndex = 0;
                sfd.Filter = "Profile Files (*.json)|*.json";
                sfd.InitialDirectory = System.IO.Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDoc‌​uments), "TrinityCreator", "Profiles");
                if (sfd.ShowDialog() == true)
                {
                    File.WriteAllText(sfd.FileName, json);
                    Logger.Log("Profile saved to file.", Logger.Status.Info, true);
                }                
            }
        }

        private void addCustomFieldBtn_Click(object sender, RoutedEventArgs e)
        {
            var ne = new ProfileCreatorEntry();
            CustomFieldsElements.Add(ne);
            customFieldsSp.Children.Add(ne);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
