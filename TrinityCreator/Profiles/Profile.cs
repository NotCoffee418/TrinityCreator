using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.Profiles
{
    // Structure: {"Name":"Test Profile","EmulatorName":null,"GameVersion":null,"DatabaseVersion":null,"Revision":1,"Author":"NotCoffee418","UpdateUrl":"https://exaple.com/MyProfile.json","Bindings":{"appKey":"exportKey","appKey2":"exportKey2"}}
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

        public Dictionary<string, string> Bindings { get; set; }
    }
}
