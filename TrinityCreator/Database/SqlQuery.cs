using System;
using System.Data;

namespace TrinityCreator.Database
{
    internal class SqlQuery : Connection
    {
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