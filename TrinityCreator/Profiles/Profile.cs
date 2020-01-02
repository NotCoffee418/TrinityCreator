using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.Profiles
{
    public class Profile
    {
        #region Static 
        static Profile _activeProfile = null;

        public static Profile Active {
            get { return _activeProfile; }
            set { 
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
            string sysProfilesDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Profiles");
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

        public string Name { get; set; }

        public string EmulatorName { get; set; }

        public string GameVersion { get; set; }

        public string DatabaseVersion { get; set; }

        public int Revision { get; set; }

        public string Author { get; set; }

        public string UpdateUrl { get; set; }

        [JsonIgnore] 
        public string LocalPath { get; set; }

        // Tablename: application keys, db keys
        public Dictionary<string, Dictionary<string,string>> Creature { get; set; }
        public Dictionary<string, Dictionary<string, string>> Quest { get; set; }
        public Dictionary<string, Dictionary<string, string>> Item { get; set; }
        public Dictionary<string, Dictionary<string, string>> Loot { get; set; }
        public Dictionary<string, Dictionary<string, string>> Vendor { get; set; }

        /// <summary>
        /// Stands for "Get Table name and Key"
        /// Returns the correct key for the current profile based on the app's equivalent of that key
        /// </summary>
        /// <param name="creationType"></param>
        /// <param name="appKey"></param>
        /// <param name="specialTableName">For table names that vary based on a setting, replaces %t with this string</param>
        /// <returns>:0:Table - 1: Sql key</returns>
        public static String[] gtk(Export.C creationType, string appKey, string specialTableName = "")
        {
            // Select correct dictionary to searchfor creation
            Dictionary<string, Dictionary<string, string>> targetDict = null;
            switch (creationType)
            {
                case Export.C.Creature:
                    targetDict = Profile.Active.Creature;
                    break;
                case Export.C.Quest:
                    targetDict = Profile.Active.Quest;
                    break;
                case Export.C.Item:
                    targetDict = Profile.Active.Item;
                    break;
                case Export.C.Loot:
                    targetDict = Profile.Active.Loot;
                    break;
                case Export.C.Vendor:
                    targetDict = Profile.Active.Vendor;
                    break;
            }

            // Check if the key exists in the profile
            var table = targetDict.Where(t =>
                // equals in this case is equivalent to null check
                !t.Value.Where(keysDic => keysDic.Key.ToLower() == appKey.ToLower()).FirstOrDefault().Equals(default(KeyValuePair<string, string>))
            ).FirstOrDefault();

            // Key didn't exist in any table, return null
            if (table.Equals(default(KeyValuePair<string, string>)))
                return null;

            // Key exists, generate result
            var result = new String[2];
            result[0] = table.Key; // Return table name

            // Handle unusual table names
            if (specialTableName != "")
                result[0] = result[0].Replace("%t", specialTableName);

            // Find appkey again and put sqlkey in result
            result[1] = table.Value.Where(keys => keys.Key.ToLower() == appKey.ToLower()).First().Value;

            // Return result
            return result;
        }




        public static string teststructure()
        {
            Profile p = new Profile();
            p.Name = "Test Profile";
            var table1 = new Dictionary<string, string>()
            {
                {"t1lk1", "t1ek1" },
                {"t1lk2", "t1ek2" },
            };
            var table2 = new Dictionary<string, string>()
            {
                {"t2lk1", "t2ek1" },
                {"t2lk2", "t2ek2" },
            };
            p.Creature = new Dictionary<string, Dictionary<string, string>>()
            {
                { "table1", table1 },
                { "table2", table2 },
            };
            p.Quest = new Dictionary<string, Dictionary<string, string>>()
            {
                { "table1", table1 },
                { "table2", table1 },
            };
            p.Item = new Dictionary<string, Dictionary<string, string>>()
            {
                { "table1", table1 },
                { "table2", table2 },
            };
            p.Vendor = new Dictionary<string, Dictionary<string, string>>()
            {
                { "table1", table1 },
                { "table2", table1 },
            };
            p.Loot = new Dictionary<string, Dictionary<string, string>>()
            {
                { "table1", table1 },
                { "table2", table1 },
            };
            return JsonConvert.SerializeObject(p);
            /* 
             {
                  "Name": "Test Profile",
                  "EmulatorName": null,
                  "GameVersion": null,
                  "DatabaseVersion": null,
                  "Revision": 0,
                  "Author": null,
                  "UpdateUrl": null,
                  "Creature": {
                    "table1": {
                      "t1lk1": "t1ek1",
                      "t1lk2": "t1ek2"
                    },
                    "table2": {
                      "t2lk1": "t2ek1",
                      "t2lk2": "t2ek2"
                    }
                  },
                  "Quest": {
                    "table1": {
                      "t1lk1": "t1ek1",
                      "t1lk2": "t1ek2"
                    },
                    "table2": {
                      "t1lk1": "t1ek1",
                      "t1lk2": "t1ek2"
                    }
                  },
                  "Item": {
                    "table1": {
                      "t1lk1": "t1ek1",
                      "t1lk2": "t1ek2"
                    },
                    "table2": {
                      "t2lk1": "t2ek1",
                      "t2lk2": "t2ek2"
                    }
                  },
                  "Loot": {
                    "table1": {
                      "t1lk1": "t1ek1",
                      "t1lk2": "t1ek2"
                    },
                    "table2": {
                      "t1lk1": "t1ek1",
                      "t1lk2": "t1ek2"
                    }
                  },
                  "Vendor": {
                    "table1": {
                      "t1lk1": "t1ek1",
                      "t1lk2": "t1ek2"
                    },
                    "table2": {
                      "t1lk1": "t1ek1",
                      "t1lk2": "t1ek2"
                    }
                  }
                }
              */
        }
    }
}
