using System;
using System.Collections.Generic;
using System.Data;

namespace TrinityCreator.Database
{
    internal class SqlQuery : Connection
    {
        /// <summary>
        /// Makes text SQL ready, add quotation, fix newline & fix '
        /// </summary>
        /// <param name="text"></param>
        /// <param name="quotes"></param>
        /// <returns></returns>
        internal static string CleanText(string text, bool quotes = true)
        {
            if (text == null)
                text = "";
            text = text.Replace(Environment.NewLine, "$B").Replace("'", "\\'");
            if (quotes)
                return "'" + text + "'";
            else
                return text;
        }

        /// <summary>
        /// Generate SQL query from Key,Value dictionary
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="d">Dictionary</param>
        /// <returns></returns>
        internal static string GenerateInsert(string tableName, Dictionary<string, string> d)
        {
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2});{3}",
                tableName, string.Join(", ", d.Keys), string.Join(", ", d.Values), Environment.NewLine);
        }

        internal static DataTable FindItemsByName(string partialName)
        {
            return ExecuteQuery(
                "SELECT entry, displayid, name FROM item_template WHERE name LIKE '%" + CleanText(partialName, false) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable FindQuestByName(string partialName)
        {
            return ExecuteQuery(
                "SELECT Id, LogTitle FROM quest_template WHERE LogTitle LIKE '%" + CleanText(partialName, false) + "%' ORDER BY Id DESC LIMIT 200;");
        }

        internal static DataTable FindCreatureByName(string partialName)
        {
            return ExecuteQuery(
                "SELECT entry, modelid1, name FROM creature_template WHERE name LIKE '%" + CleanText(partialName, false) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable FindGoByName(string partialName)
        {
            return ExecuteQuery(
                "SELECT entry, displayId, name FROM gameobject_template WHERE name LIKE '%" + CleanText(partialName, false) + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable GetSpells(string partialName)
        {
            return ExecuteQuery(
                "SELECT Id, Comment FROM spell_dbc WHERE Comment LIKE '%" + CleanText(partialName, false) + "%' ORDER BY Id DESC LIMIT 200;");
        }

        /// <summary>
        /// Returns next ID for the selected table
        /// </summary>
        /// <param name="table">table to check</param>
        /// <param name="primaryKey">usually id or entry</param>
        /// <returns></returns>
        internal static int GetNextId(string table, string primaryKey = "id")
        {
            object result = ExecuteScalar(string.Format("SELECT MAX({0}) FROM {1};", primaryKey, table), requestConfig:false);
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