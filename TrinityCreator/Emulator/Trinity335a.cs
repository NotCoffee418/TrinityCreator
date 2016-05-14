using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;

namespace TrinityCreator.Emulator
{
    class Trinity335a : IEmulator
    {
        public Trinity335a()
        {
            ID = 0;
        }

        public int ID { get; set; }

        public string GenerateQuery(TrinityItem item)
        {
            return SqlQuery.GenerateInsert("item_template", ItemTemplate(item));
        }        

        public string GenerateQuery(TrinityQuest quest)
        {
            return SqlQuery.GenerateInsert("quest_template", QuestTemplate(quest)) +
               SqlQuery.GenerateInsert("quest_template_addon", QuestAddon(quest)) +
               SqlQuery.GenerateInsert("creature_queststarter", QuestStarter(quest)) +
               SqlQuery.GenerateInsert("creature_questender", QuestEnder(quest)) +
               SqlQuery.GenerateInsert("quest_offer_reward", QuestOfferReward(quest)) +
               SqlQuery.GenerateInsert("quest_request_items", QuestRequestItems(quest));
        }

        public string GenerateQuery(TrinityCreature creature)
        {
            throw new NotImplementedException();
        }



        private Dictionary<string, string> ItemTemplate(TrinityItem item)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"entry", item.EntryId.ToString()},
                {"name", SqlQuery.CleanText(item.Name)},
                {"description", SqlQuery.CleanText(item.Quote)},
                {"class", item.Class.Id.ToString()},
                {"subclass", item.ItemSubClass.Id.ToString()},
                {"displayid", item.DisplayId.ToString()},
                {"Quality", item.Quality.Id.ToString()},
                {"bonding", item.Binds.Id.ToString()},
                {"RequiredLevel", item.MinLevel.ToString()},
                {"maxcount", item.MaxAllowed.ToString()},
                {"AllowableClass", item.AllowedClass.BitmaskValue.ToString()},
                {"AllowableRace", item.AllowedRace.BitmaskValue.ToString()},
                {"BuyPrice", item.ValueBuy.ToString()},
                {"SellPrice", item.ValueSell.ToString()},
                {"InventoryType", item.InventoryType.Id.ToString()},
                {"Material", item.ItemSubClass.Material.Id.ToString()},
                {"sheath", item.InventoryType.Sheath.ToString()},
                {"Flags", item.Flags.BitmaskValue.ToString()},
                {"BuyCount", item.BuyCount.ToString()},
                {"stackable", item.Stackable.ToString()},
                {"ContainerSlots", item.ContainerSlots.ToString()},
                {"dmg_min1", item.DamageInfo.MinDamage.ToString()},
                {"dmg_max1", item.DamageInfo.MaxDamage.ToString()},
                {"dmg_type1", item.DamageInfo.Type.Id.ToString()},
                {"delay", item.DamageInfo.Speed.ToString()},
                {"MaxDurability", item.Durability.ToString()},
                {"ammo_type", item.AmmoType.ToString()},
                {"armor", item.Armor.ToString()},
                {"block", item.Block.ToString()},
                {"BagFamily", item.BagFamily.BitmaskValue.ToString()},
                {"socketBonus", item.SocketBonus.Id.ToString()},
                {"FlagsExtra", item.FlagsExtra.BitmaskValue.ToString()},
            };
            item.GemSockets.AddValues(kvplist, "socketColor_", "socketContent_");
            item.Stats.AddValues(kvplist, "stat_type", "stat_value");

            try // resistances
            {
                // loops unique keys
                foreach (var kvp in item.Resistances.GetUserInput())
                {
                    var type = (DamageType)kvp.Key;
                    var value = int.Parse(kvp.Value); // validate int
                    kvplist.Add(type.Description + "_res", value.ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid value in magic resistance.");
            }

            return kvplist;
        }


        private Dictionary<string, string> QuestTemplate(TrinityQuest quest)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"ID", quest.EntryId.ToString()},
                {"QuestType", "2"},
                {"QuestLevel", quest.QuestLevel.ToString()},
                {"MinLevel", quest.MinLevel.ToString()},
                {"QuestSortID", quest.PQuestSort.ToString()},
                {"QuestInfoID", quest.PQuestInfo.Id.ToString()},
                {"SuggestedGroupNum", quest.SuggestedGroupNum.ToString()},
                {"TimeAllowed", quest.TimeAllowed.ToString()},
                {"AllowableRaces", quest.AllowableRace.BitmaskValue.ToString()},
                {"RewardXPDifficulty", quest.RewardXpDifficulty.Id.ToString()},
                {"RewardMoney", quest.RewardMoney.Amount.ToString()},
                {"RewardSpell", quest.RewardSpell.ToString()},
                {"RewardDisplaySpell", quest.RewardSpell.ToString()},
                {"RewardHonor", quest.RewardHonor.ToString()},
                {"RewardTalents", quest.RewardTalents.ToString()},
                {"StartItem", quest.StartItem.ToString()},
                {"Flags", quest.Flags.BitmaskValue.ToString()},
                {"RewardTitle", quest.RewardTitle.ToString()},
                {"RequiredPlayerKills", quest.RequiredPlayerKills.ToString()},
                {"RewardArenaPoints", quest.RewardArenaPoints.ToString()},
                {"POIContinent", quest.PoiCoordinate.MapId.ToString()},
                {"POIx", quest.PoiCoordinate.X.ToString()},
                {"POIy", quest.PoiCoordinate.Y.ToString()},
                {"LogTitle", SqlQuery.CleanText(quest.LogTitle)},
                {"LogDescription", SqlQuery.CleanText(quest.LogDescription)},
                {"QuestDescription", SqlQuery.CleanText(quest.QuestDescription)},
                {"AreaDescription", SqlQuery.CleanText(quest.AreaDescription)},
                {"QuestCompletionLog", SqlQuery.CleanText(quest.QuestCompletionLog)},
            };

            // DDC values
            quest.RewardItems.AddValues(kvplist, "RewardItem", "RewardAmount");
            quest.RewardChoiceItems.AddValues(kvplist, "RewardChoiceItemID", "RewardChoiceItemQuantity");
            quest.FactionRewards.AddValues(kvplist, "RewardFactionID", "RewardFactionOverride", 100);
            quest.RequiredNpcOrGos.AddValues(kvplist, "RequiredNpcOrGo", "RequiredNpcOrGoCount");
            quest.RequiredItems.AddValues(kvplist, "RequiredItemId", "RequiredItemCount");

            return kvplist;
        }

        private Dictionary<string, string> QuestAddon(TrinityQuest quest)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"ID", quest.EntryId.ToString()},
                {"MaxLevel", quest.MaxLevel.ToString()},
                {"AllowableClasses", quest.AllowableClass.BitmaskValue.ToString()},
                {"PrevQuestId", quest.PrevQuest.ToString()},
                {"NextQuestId", quest.NextQuest.ToString()},
                //{"RewardMailTemplateId", RewardMailTemplateId.ToString()},
                //{"RewardMailDelay", RewardMailDelay.ToString()},
                {"ProvidedItemCount", quest.ProvidedItemCount.ToString()},
                {"SpecialFlags", quest.SpecialFlags.BitmaskValue.ToString()},
                {"SourceSpellID", quest.SourceSpell.ToString()},
            };

            return kvplist;
        }

        private Dictionary<string, string> QuestStarter(TrinityQuest quest)
        {
            return new Dictionary<string, string>
            {
                {"id", quest.Questgiver.ToString()},
                {"quest", quest.EntryId.ToString()},
            };
        }

        private Dictionary<string, string> QuestEnder(TrinityQuest quest)
        {
            return new Dictionary<string, string>
            {
                {"id", quest.QuestCompleter.ToString()},
                {"quest", quest.EntryId.ToString()},
            };
        }

        private Dictionary<string, string> QuestOfferReward(TrinityQuest quest)
        {
            return new Dictionary<string, string>
            {
                {"ID", quest.EntryId.ToString()},
                // Reward emotes on complete
                {"RewardText", SqlQuery.CleanText(quest.RewardText)},
            };
        }

        private Dictionary<string, string> QuestRequestItems(TrinityQuest quest)
        {
            return new Dictionary<string, string>
            {
                {"ID", quest.EntryId.ToString()},
                // Reward emotes on talking before complete or incomplete
                {"CompletionText", SqlQuery.CleanText(quest.IncompleteText)},
            };
        }        
    }
}
