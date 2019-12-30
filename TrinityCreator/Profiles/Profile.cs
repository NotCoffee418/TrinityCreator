using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.Profiles
{
    class Profile
    {
        static Profile _activeProfile = null;

        public static Profile ActiveProfile {
            get { return _activeProfile; }
            private set { _activeProfile = value; }
        }

        public static void LoadString(string profileJson)
        {

        }

        public static void LoadFile(string filePath)
        {

        }
    }
}
