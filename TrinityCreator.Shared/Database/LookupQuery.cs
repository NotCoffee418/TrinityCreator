using MySqlConnector;
using System;
using System.Data;
using TrinityCreator.Shared.Helpers;
using TrinityCreator.Shared.Profiles;

namespace TrinityCreator.Shared.Database
{
    public class LookupQuery : Connection
    {
        public static (DataTable, bool) FindItemsByName(string partialName)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Item);
            string entryId = ProfileHelper.GetSqlKey(Export.C.Item, "EntryId");
            string displayId = ProfileHelper.GetSqlKey(Export.C.Item, "DisplayId");
            string name = ProfileHelper.GetSqlKey(Export.C.Item, "Name");

            return ExecuteQuery(
                $"SELECT {entryId}, {displayId}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        public static (DataTable, bool) FindQuestByName(string partialName)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Quest);
            string entryId = ProfileHelper.GetSqlKey(Export.C.Quest, "EntryId");
            string logTitle = ProfileHelper.GetSqlKey(Export.C.Quest, "LogTitle");

            return ExecuteQuery(
                $"SELECT {entryId}, {logTitle} FROM {tableName} WHERE {logTitle} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY Id DESC LIMIT 200;");
        }

        public static (DataTable, bool) FindCreatureByName(string partialName)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Creature);
            string entry = ProfileHelper.GetSqlKey(Export.C.Creature, "Entry");
            string modelId1 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId1");
            string name = ProfileHelper.GetSqlKey(Export.C.Creature, "Name");

            return ExecuteQuery(
                $"SELECT {entry}, {modelId1}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        public static (DataTable, bool) FindGoByName(string partialName)
        {
            string tableName = ProfileHelper.GetDefinitionValue("GameObjectTableName");
            string id = ProfileHelper.GetDefinitionValue("GameObjectIdColumn");
            string displayId = ProfileHelper.GetDefinitionValue("GameObjectDisplayIdColumn");
            string name = ProfileHelper.GetDefinitionValue("GameObjectNameColumn");

            return ExecuteQuery(
                $"SELECT {id}, {displayId}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        public static (DataTable, bool) GetSpells(string partialName)
        {
            string tableName = ProfileHelper.GetDefinitionValue("SpellTableName");
            string id = ProfileHelper.GetDefinitionValue("SpellIdColumn");
            string name = ProfileHelper.GetDefinitionValue("SpellNameColumn");

            return ExecuteQuery(
                $"SELECT {id}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY Id DESC LIMIT 200;");
        }

        /// <summary>
        /// Returns next ID for the selected tool
        /// </summary>
        /// <param name="toolType"></param>
        /// <returns></returns>
        public static int GetNextId(Export.C toolType)
        {
            if (Open(requestConfig:false))
            {
                string toolIdAppKey = ProfileHelper.GetToolIdAppKey(toolType);
                string toolSqlKey = ProfileHelper.GetSqlKey(toolType, toolIdAppKey);
                string tableName = ProfileHelper.GetPrimaryTable(toolType);

                string query = $"SELECT MAX({toolSqlKey}) FROM {tableName}";
                (object result, bool isValid) = ExecuteScalar(query, requestConfig:false);
                if (!isValid || result == null || result is DBNull)
                    return 0;
                else return Convert.ToInt32(result) + 1;
            }
            return 0;
        }

        public static int GetCreatureIdFromDisplayId(int targetDisplayId)
        {

            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Creature);
            string entry = ProfileHelper.GetSqlKey(Export.C.Creature, "Entry");
            string modelId1 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId1");
            string modelId2 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId2");
            string modelId3 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId3");
            string modelId4 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId4");

            (object result, bool isSuccess) = ExecuteScalar(
                $"SELECT {entry} FROM creature_template WHERE {modelId1} = {targetDisplayId} OR {modelId2} = {targetDisplayId} OR {modelId3} = {targetDisplayId} OR {modelId4} = {targetDisplayId};");
            if (!isSuccess || result == null)
                return 0;
            else return Convert.ToInt32(result);
        }

        public static int GetItemDisplayFromEntry(int targetEntryId)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Item);
            string entryId = ProfileHelper.GetSqlKey(Export.C.Item, "EntryId");
            string displayId = ProfileHelper.GetSqlKey(Export.C.Item, "DisplayId");
            string name = ProfileHelper.GetSqlKey(Export.C.Item, "Name");

            (object result, bool isSuccess) = ExecuteScalar(
                $"SELECT {displayId} FROM {tableName} WHERE {entryId} = {targetEntryId};");
            if (!isSuccess || result == null)
                return 0;
            else return Convert.ToInt32(result);
        }

        /// <summary>
        /// Attempt to find a blizzlike required skill and DE loot template in database
        /// </summary>
        /// <param name="qualityValue">Item quality int value</param>
        /// <param name="reqLevelValue">Minimum level</param>
        /// <returns>Tuple<DisenchantID, RequiredDisenchantSkill></returns>
        public static int GetDisenchantLootId(int qualityValue, int reqLevelValue)
        {
            Logger.Log($"LookupQuery.GetDisenchantData({qualityValue}, {reqLevelValue})");
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Item);
            string DisenchantID = ProfileHelper.GetSqlKey(Export.C.Item, "DisenchantLootId");
            string MinLevel = ProfileHelper.GetSqlKey(Export.C.Item, "MinLevel");
            string Quality = ProfileHelper.GetSqlKey(Export.C.Item, "Quality");

            // Handle invalid (caused by missing value in profile)
            string invalid = "InvalidAppKey";
            if (tableName == "InvalidTable" || DisenchantID == invalid || MinLevel == invalid || Quality == invalid)
            {
                Logger.Log("LookupQuery.GetDisenchantData: A required appkey for this feature is not assigned in the profile.", Logger.Status.Warning, true);
                return 0;
            }

            // Match level & quality, Find closest level match at level or lower, ASC sort DisenchantID to get blizzlike.
            string query = $"SELECT {DisenchantID} FROM {tableName} " +
                $"WHERE {MinLevel} <= {reqLevelValue} AND {Quality} = {qualityValue} AND {DisenchantID} > 0 " +
                $"GROUP BY {Quality}, {MinLevel} ORDER BY {MinLevel} DESC, {DisenchantID} ASC LIMIT 1;";

            // Execute
            (DataTable result, bool isValid) = ExecuteQuery(query, true);

            // Return invalid result as 0
            if (!isValid || result == null || result.Rows.Count == 0)
            {
                Logger.Log("LookupQuery.GetDisenchantData: No results or null result. Returning 0,0.");
                return 0;
            }

            try
            {
                // Parse values to int & return
                return Convert.ToInt32(result.Rows[0][DisenchantID]);
            }
            catch
            {
                Logger.Log("LookupQuery.GetDisenchantData: Failed to parse result to int. Please report this issue.");
                return 0;
            }
        }
    }
}