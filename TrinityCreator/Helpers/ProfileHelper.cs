using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Profiles;

namespace TrinityCreator.Helpers
{
    class ProfileHelper
    {
        public enum Expansion
        {
            Unknown = 0,
            Vanilla = 1,
            BurningCursade = 2,
            WrathOfTheLichKing = 3,
            Cataclysm = 4,
            KungFuPanda = 5,
            GarrisonInDraenor = 6,
            Legion = 7,
            BattleForAzeroth = 8,
            Shadowlands = 9,
            Wow10 = 10,
            Wow11 = 11,
            Wow12 = 12,
            Wow13 = 13,
            Wow14 = 14,
            Wow15 = 15,
            Wow16 = 16,
            Wow17 = 17,
            Wow18 = 18,
            Wow19 = 19,
            Wow20 = 20,
        }

        /// <summary>
        /// Attempts to extract the expansion version from the active profile
        /// </summary>
        /// <returns></returns>
        public static Expansion GetProfileGameVersion()
        {
            try
            {
                int wowVer;
                // Grabs "13" from "13.3.5b" and returns it as it's Expansion enum value, or Expansion.Unknown (0) 
                if (Profile.Active.GameVersion.Length > 0 && int.TryParse(Profile.Active.GameVersion.Split('.')[0], out wowVer))
                    return (Expansion)wowVer;
                else return Expansion.Unknown;
            }
            catch
            {
                Logger.Log("GetProfileGameVersion: Either wow is still pushing expansions in 2040 or this profile's GameVersion is set incorrectly.");
                return Expansion.Unknown;
            }
        }
    }
}
