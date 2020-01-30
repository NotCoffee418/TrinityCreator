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

        /// <summary>
        /// Checks if an appKey exists in the profile
        /// This will not work for lookuptool
        /// </summary>
        /// <param name="toolName">Creature, Quest etc</param>
        /// <param name="appKey">Name of the application key requested</param>
        /// <returns></returns>
        public static bool HasAppKey(Export.C tool, string appKey)
        {
            try
            {
                // Return true if appkey exists in targetTool
                var targetTool = GetToolDataFromC(tool);
                return targetTool.Values.Where(keys => keys.ContainsKey(appKey)).Count() > 0;
            }
            catch
            {
                Logger.Log($"Application Error: Profile.HasAppKey failed to ({tool.ToString()}, {appKey}. Please report this issue.", Logger.Status.Error, true);
                return false;
            }
        }

        public static string GetToolIdAppKey(Export.C toolType)
        {
            // Get the ID string appkey for this tool
            string idAppKey = "fixthis";
            switch (toolType)
            {
                case Export.C.Creature:
                    idAppKey = "Entry";
                    break;
                case Export.C.Item:
                    idAppKey = "EntryId";
                    break;
                case Export.C.Loot:
                    idAppKey = "Entry";
                    break;
                case Export.C.Quest:
                    idAppKey = "EntryId";
                    break;
                case Export.C.Vendor:
                    idAppKey = "NpcEntry";
                    break;
            }
            return idAppKey;
        }

        public static string GetPrimaryTable(Export.C toolType)
        {
            string idAppKey = GetToolIdAppKey(toolType);

            try // Find the table name & return it
            {
                var targetTool = GetToolDataFromC(toolType);
                return targetTool.Where(kp => kp.Value
                    .Where(keys => keys.Key == idAppKey).Count() > 0
                ).First().Key;
            }
            catch
            {
                Logger.Log("Profile Error: Lookup tool failed to find primary table. Profile is not set up correctly.");
                return "InvalidTable";
            }
            
        }

        public static string GetSqlKey(Export.C toolType, string appKey)
        {
            try 
            {
                // Find the table containing the appkey
                Logger.Log($"ProfileHelper: GetSqlKey({toolType.ToString()}, {appKey})");
                var targetTool = GetToolDataFromC(toolType);
                var table = targetTool.Where(kp => kp.Value
                    .Where(keys => keys.Key == appKey).Count() > 0
                ).First().Value;

                // Find it again & return sqlkey
                string result = table.Where(keys => keys.Key == appKey).First().Value;
                Logger.Log("ProfileHelper: GetSqlKey found SQLKey: {result}");
                return result;
            }
            catch
            {
                Logger.Log($"Profile Error: Lookup tool failed to find sqlKey for appKey {appKey}. returning 'InvalidAppKey'");
                return "InvalidAppKey";
            }
        }

        public static string GetDefinitionValue(string defKey)
        {
            try
            {
                Logger.Log($"ProfileHelper: GetDefinitionValue({defKey})");
                return Profile.Active.Definitions[defKey];
            }
            catch
            {
                Logger.Log($"Profile Error: Failed to find value for definition with key {defKey}. Returning 'InvalidDefinition'");
                return "InvalidDefinition";
            }
        }

        public static Dictionary<string, Dictionary<string, string>> GetToolDataFromC(Export.C toolType)
        {
            try
            {
                // Find the correct Profile property (equest, creature etc) based on C
                return (Dictionary<string, Dictionary<string, string>>)Profile.Active.GetType().GetProperty(toolType.ToString()).GetValue(Profile.Active, null);
            }
            catch 
            {
                Logger.Log($"Application Error: Profile.GetToolDataFromC() failed on {toolType.ToString()} Please report this issue.", Logger.Status.Error, true);
                return null; // should be impossible anyway
            }
        }
    }
}
