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
        /// <param name="logTitle"></param>
        /// <returns></returns>
        internal static string CleanText(string text)
        {
            if (text == null)
                text = "";
            return "'" + text.Replace(Environment.NewLine, "$B").Replace("'", "\'") + "'";
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
                "SELECT entry, displayid, name FROM item_template WHERE name LIKE '%" + partialName + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable FindQuestByName(string partialName)
        {
            return ExecuteQuery(
                "SELECT Id, LogTitle FROM quest_template WHERE LogTitle LIKE '%" + partialName + "%' ORDER BY Id DESC LIMIT 200;");
        }

        internal static DataTable FindCreatureByName(string partialName)
        {
            return ExecuteQuery(
                "SELECT entry, name FROM creature_template WHERE name LIKE '%" + partialName + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable FindGoByName(string partialName)
        {
            return ExecuteQuery(
                "SELECT entry, name FROM gameobject_template WHERE name LIKE '%" + partialName + "%' ORDER BY entry DESC LIMIT 200;");
        }

        internal static DataTable GetSpells(string partialName)
        {
            return ExecuteQuery(
                "SELECT Id, Comment FROM spell_dbc WHERE Comment LIKE '%" + partialName + "%' ORDER BY Id DESC LIMIT 200;");
        }
    }
}