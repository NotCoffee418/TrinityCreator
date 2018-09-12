using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;
using System.Windows.Controls;

namespace TrinityCreator.Emulator
{
    class cMangos335a : IEmulator
    {
        public cMangos335a()
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
            return SqlQuery.GenerateInsert("creature_template", CreatureTemplate(creature)) +
               SqlQuery.GenerateInsert("creature_template_addon", CreatureTemplateAddon(creature));
        }

        public string GenerateQuery(LootPage loot)
        {
            string result = "";
            foreach (var l in LootTemplates(loot))
                result += SqlQuery.GenerateInsert(((ComboBoxItem)loot.lootTypeCb.SelectedValue).Content.ToString() + "_loot_template", l);
            return result;
        }

        public string GenerateQuery(VendorPage vendor)
        {
            return SqlQuery.GenerateInsert("npc_vendor", Vendor(vendor));
        }

        private Dictionary<string, string> ItemTemplate(TrinityItem item)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"name", SqlQuery.CleanText(item.Name)},
                { "entry", item.EntryId.ToString()},

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
                {"ExtraFlags", item.FlagsExtra.BitmaskValue.ToString()},
                {"StatsCount", item.StatsCount.ToString()},
                {"ItemLevel", item.ItemLevel.ToString()},
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

            if (item.InventoryType.Id == 15 || item.InventoryType.Id == 26)
                kvplist.Add("RangedModRange", "100");

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

        private Dictionary<string, string> CreatureTemplate(TrinityCreature creature)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"entry", creature.Entry.ToString()},
                {"name", SqlQuery.CleanText(creature.Name)},
                {"subname", SqlQuery.CleanText(creature.Subname)},
                {"minlevel", creature.MinLevel.ToString()},
                {"maxlevel", creature.MaxLevel.ToString()},
                {"FactionAlliance", creature.Faction.ToString()},
                {"FactionHorde", creature.Faction.ToString()}, // to do : pouvoir choisir 2 factions différentes
                {"NpcFlags", creature.NpcFlags.BitmaskValue.ToString()},
                {"SpeedWalk", creature.SpeedWalk.ToString()},
                {"SpeedRun", creature.SpeedRun.ToString()},
                {"scale", creature.Scale.ToString()},
                {"rank", creature.Rank.Id.ToString()},
                {"DamageSchool", creature.DmgSchool.Id.ToString()},
                {"MeleeBaseAttackTime", creature.BaseAttackTime.ToString()},
                {"RangedBaseAttackTime", creature.RangeAttackTime.ToString()},
                {"UnitClass", creature._UnitClass.Id.ToString()},
                {"UnitFlags", creature.UnitFlags.BitmaskValue.ToString()},
                //{"unit_flags2", creature.UnitFlags2.BitmaskValue.ToString()},
                {"DynamicFlags", creature.DynamicFlags.BitmaskValue.ToString()},
                {"family", creature.Family.Id.ToString()},
                {"trainertype", creature.Trainer.TrainerType.ToString()},
                {"trainerspell", creature.Trainer.TrainerSpell.ToString()},
                {"trainerclass", creature.Trainer.TrainerClass.ToString()},
                {"trainerrace", creature.Trainer.TrainerRace.ToString()},
                {"CreatureType", creature._CreatureType.Id.ToString()},
                //{"InhabitType", creature.TypeFlags.BitmaskValue.ToString()},
                {"lootid", creature.LootId.ToString()},
                {"pickpocketlootid", creature.PickpocketLoot.ToString()},
                {"skinninglootid", creature.SkinLoot.ToString()},
                {"PetSpellDataId", creature.PetDataId.ToString()},
                {"VehicleTemplateId", creature.VehicleId.ToString()},
                {"minlootgold", creature.MinGold.Amount.ToString()},
                {"maxlootgold", creature.MaxGold.Amount.ToString()},
                {"AIName", SqlQuery.CleanText(creature.AIName.Description)},
                {"MovementType", creature.Movement.Id.ToString()},
                {"InhabitType", creature.Inhabit.BitmaskValue.ToString()},
                //{"HoverHeight", creature.HoverHeight.ToString()},
                {"HealthMultiplier", creature.HealthModifier.ToString()},
                {"PowerMultiplier", creature.ManaModifier.ToString()},
                {"ArmorMultiplier", creature.ArmorModifier.ToString()},
                {"DamageMultiplier", creature.DamageModifier.ToString()},
                {"ExperienceMultiplier", creature.ExperienceModifier.ToString()},
                {"RacialLeader", Convert.ToInt16(creature.RacialLeader).ToString()},
                {"RegenerateStats", Convert.ToInt16(creature.RegenHealth).ToString()},
                {"mechanicimmunemask", creature.MechanicImmuneMask.BitmaskValue.ToString()},
                {"ExtraFlags", creature.FlagsExtra.BitmaskValue.ToString()},
                //{"exp", creature.Exp.ToString()},
            };

            creature.ModelIds.AddValues(kvplist);
            creature.Resistances.AddValues(kvplist, "resistance");
            creature.DifficultyEntry.AddValues(kvplist, "");
            creature.Spells.AddValues(kvplist);
            return kvplist;
        }

        private Dictionary<string, string> CreatureTemplateAddon(TrinityCreature creature)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"entry", creature.Entry.ToString()},
                {"mount", creature.Mount.ToString()},
                {"bytes1", creature.Bytes1.BitmaskValue.ToString()},
                {"emote", creature.Emote.ToString()},
            };

            creature.Auras.AddValues(kvplist, "auras", ' ');

            return kvplist;
        }


        private Dictionary<string, string>[] LootTemplates(LootPage loot)
        {
            List<Dictionary<string,string>> result = new List<Dictionary<string,string>>();
            foreach (LootRowControl row in loot.lootRowSp.Children)
            {
                var kvplist = new Dictionary<string, string>
                {
                    {"Entry", loot.entryTb.Text},
                    {"Item", row.Item.ToString()},
                    {"Chance", row.Chance.ToString()},
                    {"QuestRequired", Convert.ToInt16(row.QuestRequired).ToString()},
                    {"MinCount", row.MinCount.ToString()},
                    {"MaxCount", row.MaxCount.ToString()},
                };
                result.Add(kvplist);
            }
            return result.ToArray();
        }

        public Dictionary<string, string> Vendor(VendorPage vendor)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"entry", vendor.npcTb.Text},
                {"slot", vendor.slotTb.Text},
                {"item", vendor.itemTb.Text},
                {"maxcount", vendor.maxcountTb.Text},
                {"incrtime", vendor.incrTimeTb.Text},
                {"extendedcost", vendor.extendedCostTb.Text},
            };
            return kvplist;
        }
    }
}
