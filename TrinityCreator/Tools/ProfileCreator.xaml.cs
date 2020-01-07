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

namespace TrinityCreator.Tools
{
    /// <summary>
    /// Interaction logic for ProfileCreator.xaml
    /// </summary>
    public partial class ProfileCreator : Window
    {
        public ProfileCreator()
        {
            InitializeComponent();
            DisplayEntries();
            DataContext = this;
        }


        List<ProfileCreatorEntry> ItemElements;
        List<ProfileCreatorEntry> LootElements;
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
        }

        private string GenerateJson()
        {
            // Place data from UI in EditingProfile
            EditingProfile.Loot = ToProfileFormat(LootElements);

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
                    Logger.Status.Error, true);
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

        private void copyBtn_Click(object sender, RoutedEventArgs e)
        {
            string json = GenerateJson();
            if (json != null) 
            {
                Clipboard.SetDataObject(json);
                Logger.Log("Profile copied to clipboard.", Logger.Status.Info, true);
            }
        }
    }
}
