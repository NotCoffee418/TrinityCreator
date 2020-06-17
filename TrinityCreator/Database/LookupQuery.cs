using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
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
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Item);
            string entryId = ProfileHelper.GetSqlKey(Export.C.Item, "EntryId");
            string displayId = ProfileHelper.GetSqlKey(Export.C.Item, "DisplayId");
            string name = ProfileHelper.GetSqlKey(Export.C.Item, "Name");

            return ExecuteQuery(
                $"SELECT {entryId}, {displayId}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable FindQuestByName(string partialName)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Quest);
            string entryId = ProfileHelper.GetSqlKey(Export.C.Quest, "EntryId");
            string logTitle = ProfileHelper.GetSqlKey(Export.C.Quest, "LogTitle");

            return ExecuteQuery(
                $"SELECT {entryId}, {logTitle} FROM {tableName} WHERE {logTitle} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY Id DESC LIMIT 200;");
        }

        internal static DataTable FindCreatureByName(string partialName)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Creature);
            string entry = ProfileHelper.GetSqlKey(Export.C.Creature, "Entry");
            string modelId1 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId1");
            string name = ProfileHelper.GetSqlKey(Export.C.Creature, "Name");

            return ExecuteQuery(
                $"SELECT {entry}, {modelId1}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable FindGoByName(string partialName)
        {
            string tableName = ProfileHelper.GetDefinitionValue("GameObjectTableName");
            string id = ProfileHelper.GetDefinitionValue("GameObjectIdColumn");
            string displayId = ProfileHelper.GetDefinitionValue("GameObjectDisplayIdColumn");
            string name = ProfileHelper.GetDefinitionValue("GameObjectNameColumn");

            return ExecuteQuery(
                $"SELECT {id}, {displayId}, {name} FROM {tableName} WHERE {name} LIKE '%" + MySqlHelper.EscapeString(partialName) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable GetSpells(string partialName)
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
        internal static int GetNextId(Export.C toolType)
        {
            if (Open(requestConfig:false))
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
            return 0;
        }

        internal static int GetCreatureIdFromDisplayId(int targetDisplayId)
        {

            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Creature);
            string entry = ProfileHelper.GetSqlKey(Export.C.Creature, "Entry");
            string modelId1 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId1");
            string modelId2 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId2");
            string modelId3 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId3");
            string modelId4 = ProfileHelper.GetSqlKey(Export.C.Creature, "ModelId4");

            object result = ExecuteScalar(
                $"SELECT {entry} FROM creature_template WHERE {modelId1} = {targetDisplayId} OR {modelId2} = {targetDisplayId} OR {modelId3} = {targetDisplayId} OR {modelId4} = {targetDisplayId};");
            if (result == null)
                return 0;
            else return Convert.ToInt32(result);
        }

        internal static int GetItemDisplayFromEntry(int targetEntryId)
        {
            string tableName = ProfileHelper.GetPrimaryTable(Export.C.Item);
            string entryId = ProfileHelper.GetSqlKey(Export.C.Item, "EntryId");
            string displayId = ProfileHelper.GetSqlKey(Export.C.Item, "DisplayId");
            string name = ProfileHelper.GetSqlKey(Export.C.Item, "Name");

            object result = ExecuteScalar(
                $"SELECT {displayId} FROM {tableName} WHERE {entryId} = {targetEntryId};");
            if (result == null)
                return 0;
            else return Convert.ToInt32(result);
        }
    }
}