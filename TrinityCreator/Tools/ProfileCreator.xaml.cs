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


        List<ProfileCreatorEntry> LootElements;
        Profile EditingProfile = new Profile();


        /// <summary>
        /// Display all appKeys supported by the application with the ability to set it's sql key.
        /// Should only be called once while opening the window
        /// </summary>
        private void DisplayEntries()
        {
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
