using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using TrinityCreator.Helpers;
using TrinityCreator.Profiles;

namespace TrinityCreator.Database
{
    internal class LookupQuery : Connection
    {
        internal static DataTable FindItemsByName(string partialName)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Profiles.Export.C.Item);
            string entryId = ProfileHelper.GetSqlKey(Profiles.Export.C.Item, "EntryId");
            string displayId = ProfileHelper.GetSqlKey(Profiles.Export.C.Item, "DisplayId");
            string name = ProfileHelper.GetSqlKey(Profiles.Export.C.Item, "Name");

            return ExecuteQuery(
                $"SELECT {entryId}, {displayId}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable FindQuestByName(string partialName)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Profiles.Export.C.Quest);
            string entryId = ProfileHelper.GetSqlKey(Profiles.Export.C.Quest, "EntryId");
            string logTitle = ProfileHelper.GetSqlKey(Profiles.Export.C.Quest, "LogTitle");

            return ExecuteQuery(
                $"SELECT {entryId}, {logTitle} FROM {tableName} WHERE {logTitle} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY Id DESC LIMIT 200;");
        }

        internal static DataTable FindCreatureByName(string partialName)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Profiles.Export.C.Creature);
            string entry = ProfileHelper.GetSqlKey(Profiles.Export.C.Creature, "Entry");
            string modelId1 = ProfileHelper.GetSqlKey(Profiles.Export.C.Creature, "ModelId1");
            string name = ProfileHelper.GetSqlKey(Profiles.Export.C.Creature, "Name");

            return ExecuteQuery(
                $"SELECT {entry}, {modelId1}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable FindGoByName(string partialName)
        {
            return ExecuteQuery(
                "SELECT entry, displayId, name FROM gameobject_template WHERE name LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable GetSpells(string partialName)
        {
            return ExecuteQuery(
                "SELECT Id, Comment FROM spell_dbc WHERE Comment LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY Id DESC LIMIT 200;");
        }

        /// <summary>
        /// Returns next ID for the selected tool
        /// </summary>
        /// <param name="toolType"></param>
        /// <returns></returns>
        internal static int GetNextId(Export.C toolType)
        {
            string toolIdAppKey = ProfileHelper.GetToolIdAppKey(toolType);
            string toolSqlKey = ProfileHelper.GetSqlKey(toolType, toolIdAppKey);
            string tableName = ProfileHelper.GetPrimaryTable(toolType);

            string query = $"SELECT MAX({toolSqlKey}) FROM {tableName}";
            object result = ExecuteScalar(query, requestConfig:false);
            if (result == null || result is DBNull)
                return 0;
            else return Convert.ToInt32(result) + 1;
        }

        internal static int GetCreatureIdFromDisplayId(int displayId)
        {
            object result = ExecuteScalar(string.Format(
                "SELECT entry FROM creature_template WHERE modelid1 = {0} OR modelid2 = {0} OR modelid3 = {0} OR modelid4 = {0};", displayId));
            if (result == null)
                return 0;
            else return Convert.ToInt32(result);
        }

        internal static int GetItemDisplayFromEntry(int entry)
        {
            object result = ExecuteScalar(string.Format(
                "SELECT displayid FROM item_template WHERE entry = {0};", entry));
            if (result == null)
                return 0;
            else return Convert.ToInt32(result);
        }
    }
}