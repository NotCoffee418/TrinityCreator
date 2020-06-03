using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrinityCreator.Helpers;

namespace TrinityCreator.Profiles
{
    public class Profile
    {
        public string Name { get; set; }

        public string EmulatorName { get; set; }

        public string GameVersion { get; set; }

        public string DatabaseVersion { get; set; }

        public int Revision { get; set; }

        public string Author { get; set; }

        public string UpdateUrl { get; set; }

        public int TestedBuild { get; set; }

        [JsonIgnore]
        public string LocalPath { get; set; }

        // <tableName, <appKey, sqlKey>>
        public Dictionary<string, Dictionary<string, string>> Creature { get; set; }
        public Dictionary<string, Dictionary<string, string>> Quest { get; set; }
        public Dictionary<string, Dictionary<string, string>> Item { get; set; }
        public Dictionary<string, Dictionary<string, string>> Loot { get; set; }
        public Dictionary<string, Dictionary<string, string>> Vendor { get; set; }
        public Dictionary<string, Dictionary<string, string>> CustomFields { get; set; }
        public Dictionary<string, string> CustomDefaultValues { get; set; } // NIY
        public Dictionary<string, string> Definitions { get; set; }

        #region Static 
        static Profile _activeProfile = null;

        public static Profile Active {
            get { return _activeProfile; }
            set {
                if (value == null)
                    Logger.Log($"Profile: Changing Profile.Active to NULL.");
                else Logger.Log($"Profile: Changing Profile.Active to '{value.Name}'.");

                // Validate version
                if (value.TestedBuild > Assembly.GetExecutingAssembly().GetName().Version.Revision)
                    Logger.Log("Profile: The active profile was made for a newer version of Trinity Creator." +
                         Environment.NewLine + "Values may export correctly until you update the application.",
                        Logger.Status.Warning, showMessageBox:true);

                _activeProfile = value;
                if (ActiveProfileChangedEvent != null)
                    ActiveProfileChangedEvent(Profile.Active, new EventArgs());
            }
        }


        // Fires when Profile.Active changes
        public static event EventHandler ActiveProfileChangedEvent;


        public static Profile LoadFile(string filePath, bool showError = true)
        {
            try
            {
                Logger.Log($"Profile: LoadFile: {filePath}");
                string json = File.ReadAllText(filePath);
                Profile p = JsonConvert.DeserializeObject<Profile>(json);
                p.LocalPath = filePath;
                return p;
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to load profile '" + filePath + ". This profile may be corrupt or for a different version of TrinityCreator. Please update TrinityCreator and try again.", Logger.Status.Error, showError);
                return null;
            }
        }

        public static List<Profile> ListProfiles()
        {
            // Prepare directories
            string sysProfilesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NotCoffee418", "TrinityCreator", "Profiles");
            string usrProfilesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TrinityCreator", "Profiles");
            if (!Directory.Exists(usrProfilesDir))
                Directory.CreateDirectory(usrProfilesDir);
            if (!Directory.Exists(sysProfilesDir))
                Directory.CreateDirectory(sysProfilesDir);

            // Load all profiles:
            List<Profile> allProfiles = new List<Profile>();
            List<string> allFiles = new List<string>();
            allFiles.AddRange(Directory.GetFiles(sysProfilesDir));
            allFiles.AddRange(Directory.GetFiles(usrProfilesDir));
            foreach (string path in allFiles)
            {
                if (Path.GetExtension(path).ToLower() != ".json")
                    continue; // Not a valid profile file
                allProfiles.Add(LoadFile(path));
            }

            // Warn user when user created profile has the same name as default profile (can create issues)
            List<string> fileNames = new List<string>();
            foreach (string path in allFiles)
            {
                string fn = Path.GetFileName(path).ToLower();
                if (fileNames.Contains(fn))
                    Logger.Log($"User-created profile {fn} has the same filename as a default profile.{Environment.NewLine}" + 
                        $"Please rename or remove the user-created profile.{Environment.NewLine}" +
                        $"User-created profiles can be found in My Documents\\TrinityCreator\\Profiles", Logger.Status.Warning, true);
                fileNames.Add(fn);
            }

            // return result
            return allProfiles;
        }
        #endregion

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType().Equals(typeof(Profile)) &&
                JsonConvert.SerializeObject((Profile)obj) == JsonConvert.SerializeObject(this);
        }

        public Dictionary<string, Dictionary<string, string>> GetToolDict(Export.C cType)
        {
            Dictionary<string, Dictionary<string, string>> targetDict = null;
            switch (cType)
            {
                case Export.C.Item:
                    targetDict = this.Item;
                    break;
                case Export.C.Quest:
                    targetDict = this.Quest;
                    break;
                case Export.C.Creature:
                    targetDict = this.Creature;
                    break;
                case Export.C.Loot:
                    targetDict = this.Loot;
                    break;
                case Export.C.Vendor:
                    targetDict = this.Vendor;
                    break;
                default:
                    Logger.Log($"Creator type '{cType}' is not defined in Profile.IsKeyDefined(). Please report this issue on GitHub.", 
                        Logger.Status.Error, true);
                    return null;
            }
            return targetDict;
        }

        /// <summary>
        /// Determines if an appkey is correctly defined in this profile.
        /// </summary>
        /// <param name="cType">Export.C type</param>
        /// <param name="appKey">Valid AppKey</param>
        /// <returns></returns>
        internal bool IsKeyDefined(Export.C cType, string appKey)
        {
            var targetDir = GetToolDict(cType);
            foreach (var subD in targetDir) // dictionary in table
                if (subD.Value.ContainsKey(appKey)) // Has defiend appkey
                {
                    string match;
                    subD.Value.TryGetValue(appKey, out match);
                    if (match != null && match != string.Empty)
                        return true;
                    else
                    {
                        Logger.Log($"Profile Error: Appkey '{cType}.{appKey}' is has an invalid definition."+
                            "Disable the AppKey, define a valid SqlKey or report this issue on GitHub.", Logger.Status.Error, true);
                        return false;
                    }
                }

            // None found, it's disabled
            return false;
        }

        /// <summary>
        /// Returns visibility if an appkey is correctly defined in this profile.
        /// </summary>
        /// <param name="cType">Export.C type</param>
        /// <param name="appKey">Valid AppKey</param>
        /// <returns></returns>
        internal Visibility VisibileIfKeyDefined(Export.C cType, string appKey)
        {
            return IsKeyDefined(cType, appKey) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
