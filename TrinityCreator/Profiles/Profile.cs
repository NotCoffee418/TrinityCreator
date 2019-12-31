using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.Profiles
{
    class Profile
    {
        #region Static 
        static Profile _activeProfile = null;

        public static Profile Active {
            get { return _activeProfile; }
            private set { _activeProfile = value; }
        }

        public static Profile LoadFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public static List<Profile> ListProfiles()
        {
            throw new NotImplementedException();
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
