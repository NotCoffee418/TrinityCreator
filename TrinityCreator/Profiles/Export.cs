using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace TrinityCreator.Profiles
{
    public class Export
    {
        // Creation type, used for requests to profile
        public enum C
        {
            Creature,
            Quest,
            Item,
            Loot,
            Vendor
        }


        /// <summary>
        /// Generate SQL insert based on string[0] (table name), sting[1] column name and dynamic value
        /// </summary>
        /// <param name="data">Dictionary of string[] generated through Profile.gtk() and value</param>
        /// <returns></returns>
        private static string GenerateSql(List<ExpKvp> data)
        {
            try
            {
                // Find table names
                IEnumerable<string> tableNames = data
                    .Where(e => e.IsValid)
                    .GroupBy(e => e.SqlTableName)
                    .Select(grp => grp.First().SqlTableName);

                // Prepare datatables
                List<DataTable> dtList = new List<DataTable>();
                
                // Generate Datatables
                foreach (var tableName in tableNames)
                {
                    DataTable dt = new DataTable(tableName);
                    // Create columns
                    foreach (var entry in data.Where(e => e.IsValid && e.SqlTableName == tableName))
                        dt.Columns.Add(entry.SqlKey, entry.Value.GetType());

                    // Create row
                    DataRow row = dt.NewRow();
                    foreach (var entry in data.Where(e => e.IsValid && e.SqlTableName == tableName))
                        row[entry.SqlKey] = entry.Value;
                    dt.Rows.Add(row);

                    // Add to result tables
                    dtList.Add(dt);
                }

                // Build mysql insert string (manually for now)
                string sql = String.Empty;
                foreach (var dt in dtList)
                {
                    string[] colNames = dt.Columns.Cast<DataColumn>()
                        .Select(x => x.ColumnName)
                        .ToArray();

                    // Generate query
                    sql += "INSERT INTO " + dt.TableName +
                        " (" + string.Join(", ", colNames) + ") VALUES ";

                    // Datermine how to write values into the query based on type
                    List<string> rowValuesReady = new List<string>();
                    foreach (string col in colNames)
                    {
                        Type colType = dt.Columns[col].DataType;

                        // Handle string-y types
                        if (colType == typeof(String) || colType == typeof(string))
                        {
                            // Wow client's interpretation of newline is $B
                            string valStr = ((string)dt.Rows[0][col])
                                .Replace(Environment.NewLine, "$B")
                                .Replace("\n", "$B");

                            // Clean the string & add it.
                            rowValuesReady.Add("'" + MySqlHelper.EscapeString(valStr) + "'");
                        }
                        // Handle bool type
                        else if (colType == typeof(bool))
                        {
                            if ((bool)dt.Rows[0][col] == true)
                                rowValuesReady.Add("1");
                            else rowValuesReady.Add("0");
                        }
                        else // handle numeric types
                        {
                            rowValuesReady.Add(dt.Rows[0][col].ToString().Replace(',', '.'));
                        }
                    }

                    // Generate row in query
                    sql += "(" + string.Join(", ", rowValuesReady) + ");" + Environment.NewLine;
                }
                return sql;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}", Logger.Status.Error, true);
                return "";
            }
        }

        public static string Item(TrinityItem item)
        {
            var data = new List<ExpKvp>()
            {
                new ExpKvp("EntryId", item.EntryId, C.Item),
                new ExpKvp("Name", item.Name, C.Item),
                new ExpKvp("Quote", item.Quote, C.Item),
                new ExpKvp("Class", item.Class.Id, C.Item),
                new ExpKvp("ItemSubClass", item.ItemSubClass.Id, C.Item),
                new ExpKvp("Quality", item.Quality.Id, C.Item),
                new ExpKvp("DisplayId", item.DisplayId, C.Item),
                new ExpKvp("Binds", item.Binds.Id, C.Item),
                new ExpKvp("MinLevel", item.MinLevel, C.Item),
                new ExpKvp("MaxAllowed", item.MaxAllowed, C.Item),
                new ExpKvp("AllowedClass", item.AllowedClass.BitmaskValue, C.Item),
                new ExpKvp("AllowedRace", item.AllowedRace.BitmaskValue, C.Item),
                new ExpKvp("ValueBuy", item.ValueBuy.Amount, C.Item),
                new ExpKvp("ValueSell", item.ValueSell.Amount, C.Item),
                new ExpKvp("InventoryType", item.InventoryType.Id, C.Item),
                new ExpKvp("Flags", item.Flags.BitmaskValue, C.Item),
                new ExpKvp("FlagsExtra", item.FlagsExtra.BitmaskValue, C.Item),
                new ExpKvp("BuyCount", item.BuyCount, C.Item),
                new ExpKvp("Stackable", item.Stackable, C.Item),
                new ExpKvp("ContainerSlots", item.ContainerSlots, C.Item),
                new ExpKvp("MinDamage", item.DamageInfo.MinDamage, C.Item),
                new ExpKvp("MaxDamage", item.DamageInfo.MaxDamage, C.Item),
                new ExpKvp("AttackSpeed", item.DamageInfo.Speed, C.Item),
                new ExpKvp("DamageType", item.DamageInfo.Type.Id, C.Item),
                new ExpKvp("AmmoType", item.AmmoType, C.Item),
                new ExpKvp("Durability", item.Durability, C.Item),
                new ExpKvp("SocketBonus", item.SocketBonus.Id, C.Item),
                new ExpKvp("StatsCount", item.StatsCount, C.Item),
                new ExpKvp("Armor", item.Armor, C.Item),
                new ExpKvp("Block", item.Block, C.Item),
                new ExpKvp("BagFamily", item.BagFamily.BitmaskValue, C.Item),
                new ExpKvp("ItemLevel", item.ItemLevel, C.Item),
                new ExpKvp("RangedModRange", item.RangedModRange, C.Item),
            };

            try // Handle resistances
            {
                foreach (var kvp in item.Resistances.GetUserInput())
                {
                    // Translates to HolyResistance, FireResistance...
                    string appKey = ((DamageType)kvp.Key).Description + "Resistance";
                    int value;
                    if (!int.TryParse(kvp.Value, out value))
                    {
                        Logger.Log($"Invalid value enetered for {appKey}. Query was not saved.", Logger.Status.Error, true);
                        return String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Logger.Log("Something went wrong parsing resistances data. Query was not saved.", Logger.Status.Error, true);
                return String.Empty;
            }
            

            try // Handle gem sockets
            {
                // Translates to two columns: SocketColor1, SocketContent1, SocketColor2...
                var gemData = new Dictionary<string, string>();
                item.GemSockets.AddValues(gemData, "SocketColor", "SocketContent");
                foreach (var gemColumn in gemData)
                    data.Add(new ExpKvp(gemColumn.Key, gemColumn.Value, C.Item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Logger.Log("Something went wrong parsing gem socket data. Query was not saved.", Logger.Status.Error, true);
                return String.Empty;
            }

            try // Handle Stats
            {
                // Translates to two columns: StatType1, StatValue1, StatType2...
                var statData = new Dictionary<string, string>();
                item.Stats.AddValues(statData, "StatType", "StatValue");
                foreach (var statColumn in statData)
                    data.Add(new ExpKvp(statColumn.Key, statColumn.Value, C.Item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Logger.Log("Something went wrong parsing gem socket data. Query was not saved.", Logger.Status.Error, true);
                return String.Empty;
            }


            return GenerateSql(data);
        }

        public static string Quest(TrinityQuest quest)
        {
            var data = new List<ExpKvp>()
            {
                new ExpKvp("EntryId", quest.EntryId, C.Quest),
                new ExpKvp("QuestSort", quest.PQuestSort, C.Quest),
                new ExpKvp("QuestInfo", quest.PQuestInfo.Id, C.Quest),
                new ExpKvp("SuggestedGroupNum", quest.SuggestedGroupNum, C.Quest),
                new ExpKvp("Flags", quest.Flags.BitmaskValue, C.Quest),
                new ExpKvp("SpecialFlags", quest.SpecialFlags.BitmaskValue, C.Quest),
                new ExpKvp("LogTitle", quest.LogTitle, C.Quest),
                new ExpKvp("LogDescription", quest.LogDescription, C.Quest),
                new ExpKvp("QuestDescription", quest.QuestDescription, C.Quest),
                new ExpKvp("AreaDescription", quest.AreaDescription, C.Quest),
                new ExpKvp("QuestCompletionLog", quest.QuestCompletionLog, C.Quest),
                new ExpKvp("RewardText", quest.RewardText, C.Quest),
                new ExpKvp("IncompleteText", quest.IncompleteText, C.Quest),
                new ExpKvp("PrevQuest", quest.PrevQuest, C.Quest),
                new ExpKvp("NextQuest", quest.NextQuest, C.Quest),
                //new ExpKvp("ExclusiveGroup", quest.ExclusiveGroup.???, C.Quest),
                new ExpKvp("Questgiver", quest.Questgiver, C.Quest),
                new ExpKvp("QuestCompleter", quest.QuestCompleter, C.Quest),
                new ExpKvp("QuestLevel", quest.QuestLevel, C.Quest),
                new ExpKvp("MinLevel", quest.MinLevel, C.Quest),
                new ExpKvp("MaxLevel", quest.MaxLevel, C.Quest),
                new ExpKvp("AllowableClass", quest.AllowableClass.BitmaskValue, C.Quest),
                new ExpKvp("AllowableRace", quest.AllowableRace.BitmaskValue, C.Quest),
                new ExpKvp("StartItem", quest.StartItem, C.Quest),
                new ExpKvp("ProvidedItemCount", quest.ProvidedItemCount, C.Quest),
                new ExpKvp("SourceSpell", quest.SourceSpell, C.Quest),
                new ExpKvp("PoiCoordinateX", quest.PoiCoordinate.X, C.Quest),
                new ExpKvp("PoiCoordinateY", quest.PoiCoordinate.Y, C.Quest),
                new ExpKvp("PoiCoordinateZ", quest.PoiCoordinate.Z, C.Quest),
                new ExpKvp("PoiCoordinateMap", quest.PoiCoordinate.MapId, C.Quest),
                new ExpKvp("TimeAllowed", quest.TimeAllowed, C.Quest),
                new ExpKvp("RequiredPlayerKills", quest.RequiredPlayerKills, C.Quest),
                new ExpKvp("RewardXpDifficulty", quest.RewardXpDifficulty.Id, C.Quest), // Emulator support? See issue #72
                new ExpKvp("RewardMoney", quest.RewardMoney.Amount, C.Quest),
                new ExpKvp("RewardSpell", quest.RewardSpell, C.Quest),
                new ExpKvp("RewardHonor", quest.RewardHonor, C.Quest),
                //new ExpKvp("RewardMailTemplateId", quest.RewardMailTemplateId.???, C.Quest),
                //new ExpKvp("RewardMailDelay", quest.RewardMailDelay, C.Quest),
                new ExpKvp("RewardTitle", quest.RewardTitle, C.Quest),
                new ExpKvp("RewardArenaPoints", quest.RewardArenaPoints, C.Quest),
                new ExpKvp("RewardTalents", quest.RewardTalents, C.Quest),
            };

            try // RequiredItem & RequiredItemCount (max 6)
            {
                // Translates to two columns: RequiredItem1, RequiredItemCount1, RequiredItem2...
                var requiredItemData = new Dictionary<string, string>();
                quest.RequiredItems.AddValues(requiredItemData, "RequiredItem", "RequiredItemCount");
                foreach (var reqItemCol in requiredItemData)
                    data.Add(new ExpKvp(reqItemCol.Key, reqItemCol.Value, C.Item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Logger.Log("Something went wrong parsing RequiredItem data. Query was not saved.", Logger.Status.Error, true);
                return String.Empty;
            }

            try // RequiredNpcOrGo & RequiredNpcOrGoCount (max 6)
            {
                // Translates to two columns: RequiredNpcOrGo1, RequiredNpcOrGoCount1, RequiredNpcOrGoItem2...
                var requiredNpcOrGoData = new Dictionary<string, string>();
                quest.RequiredItems.AddValues(requiredNpcOrGoData, "RequiredNpcOrGoItem", "RequiredNpcOrGoCount");
                foreach (var reqNpcOrGoCol in requiredNpcOrGoData)
                    data.Add(new ExpKvp(reqNpcOrGoCol.Key, reqNpcOrGoCol.Value, C.Item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Logger.Log("Something went wrong parsing RequiredNpcOrGo data. Query was not saved.", Logger.Status.Error, true);
                return String.Empty;
            }

            try // RewardItem & RewardItemAmount (max 4)
            {
                // Translates to two columns: RewardItem1, RewardItemAmount1, RewardItemItem2...
                var rewardItemData = new Dictionary<string, string>();
                quest.RequiredItems.AddValues(rewardItemData, "RewardItem", "RewardItemAmount");
                foreach (var rewardItemCol in rewardItemData)
                    data.Add(new ExpKvp(rewardItemCol.Key, rewardItemCol.Value, C.Item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Logger.Log("Something went wrong parsing RewardItem data. Query was not saved.", Logger.Status.Error, true);
                return String.Empty;
            }

            try // RewardChoiceItemID & RewardChoiceItemAmount (max 6)
            {
                // Translates to two columns: RewardChoiceItemID1, RewardChoiceItemAmount1, RewardChoiceItemID2...
                var rewardItemChoiceData = new Dictionary<string, string>();
                quest.RequiredItems.AddValues(rewardItemChoiceData, "RewardChoiceItemID", "RewardChoiceItemAmount");
                foreach (var rewardItemChoiceCol in rewardItemChoiceData)
                    data.Add(new ExpKvp(rewardItemChoiceCol.Key, rewardItemChoiceCol.Value, C.Item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Logger.Log("Something went wrong parsing RewardChoiceItem data. Query was not saved.", Logger.Status.Error, true);
                return String.Empty;
            }

            try // UNUSUAL!! - FactionRewardID & RewardFactionOverride (max 4)
            {
                // Translates to two columns: FactionRewardID1, RewardFactionOverride1, FactionRewardID2...
                var factionRewardData = new Dictionary<string, string>();
                // Value output is *100 (TrinityCore support)! see issue #72
                quest.RequiredItems.AddValues(factionRewardData, "RewardChoiceItemID", "RewardChoiceItemAmount", 100); 
                foreach (var factionRewardDataCol in factionRewardData)
                    data.Add(new ExpKvp(factionRewardDataCol.Key, factionRewardDataCol.Value, C.Item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                Logger.Log("Something went wrong parsing FactionReward data. Query was not saved.", Logger.Status.Error, true);
                return String.Empty;
            }

            return GenerateSql(data);
        }

        // Loot is DDC based, loop through the entries
        public static string Loot(LootPage loot)
        {
            // Loot table names are relative to a setting, use special table name in gtk
            string sp = ((ComboBoxItem)loot.lootTypeCb.SelectedValue).Content.ToString();

            // Loot supports multiple rows per "creation", hence the loop
            string sql = String.Empty;
            foreach (LootRowControl row in loot.lootRowSp.Children)
            {
                int entry = 0;
                try // Loot has an unusual system, need to manually parse the int
                {
                    entry = int.Parse(loot.entryTb.Text);
                }
                catch
                {
                    Logger.Log("Target Entry ID was not a numeric value.", Logger.Status.Error, true);
                    return ""; // export/save won't happen
                }

                sql += GenerateSql(new List<ExpKvp>()
                {
                    new ExpKvp("Entry", entry, C.Loot, sp),
                    new ExpKvp("Item", row.Item, C.Loot, sp),
                    new ExpKvp("Chance", row.Chance, C.Loot, sp),
                    new ExpKvp("QuestRequired", row.QuestRequired, C.Loot, sp),
                    new ExpKvp("MinCount", row.MinCount, C.Loot, sp),
                    new ExpKvp("MaxCount", row.MaxCount, C.Loot, sp),
                }) + Environment.NewLine;

            }                
            return sql;
        }
    }
}
