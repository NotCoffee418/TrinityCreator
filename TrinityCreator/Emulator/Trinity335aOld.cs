using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;
using System.Windows.Controls;

namespace TrinityCreator.Emulator
{
    class Trinity335aOld : IEmulator
    {
        public Trinity335aOld()
        {
            ID = 2;
        }

        public int ID { get; set; }

        public string GenerateQuery(TrinityItem item)
        {
            return SqlQuery.GenerateInsert("item_template", ItemTemplate(item));
        }        

        public string GenerateQuery(TrinityQuest quest)
        {
            return SqlQuery.GenerateInsert("quest_template", QuestTemplate(quest)) +
               SqlQuery.GenerateInsert("creature_queststarter", QuestStarter(quest));
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
                {"StatsCount", item.StatsCount.ToString()},
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
                {"Method", "2"},
                {"Level", quest.QuestLevel.ToString()},
                {"MinLevel", quest.MinLevel.ToString()},
                {"MaxLevel", quest.MaxLevel.ToString()},
                {"ZoneOrSort", quest.PQuestSort.ToString()},
                {"Type", quest.PQuestInfo.Id.ToString()},
                {"SuggestedPlayers", quest.SuggestedGroupNum.ToString()},
                {"LimitTime", quest.TimeAllowed.ToString()},
                {"RequiredClasses", quest.AllowableClass.BitmaskValue.ToString()},
                {"RequiredRaces", quest.AllowableRace.BitmaskValue.ToString()},
                {"PrevQuestId", quest.PrevQuest.ToString()},
                {"NextQuestId", quest.NextQuest.ToString()},
                {"NextQuestIdChain", quest.QuestCompleter.ToString()},
                {"RewardXPId", quest.RewardXpDifficulty.Id.ToString()},
                {"RewardOrRequiredMoney", quest.RewardMoney.Amount.ToString()},
                {"RewardSpellCast", quest.RewardSpell.ToString()},
                {"RewardHonor", quest.RewardHonor.ToString()},
                {"RewardHonorMultiplier", quest.RewardHonor == 0 ? "0" : "1"},            
                //{"RewardMailTemplateId", RewardMailTemplateId.ToString()},
                //{"RewardMailDelay", RewardMailDelay.ToString()},
                {"SourceItemId", quest.StartItem.ToString()},
                {"SourceItemCount", quest.ProvidedItemCount.ToString()},
                {"SourceSpellId", quest.SourceSpell.ToString()},
                {"Flags", quest.Flags.BitmaskValue.ToString()},
                {"SpecialFlags", quest.SpecialFlags.BitmaskValue.ToString()},
                {"RewardTitleId", quest.RewardTitle.ToString()},
                {"RequiredPlayerKills", quest.RequiredPlayerKills.ToString()},
                {"RewardTalents", quest.RewardTalents.ToString()},
                {"RewardArenaPoints", quest.RewardArenaPoints.ToString()},
                {"PointMapId", quest.PoiCoordinate.MapId.ToString()},
                {"PointX", quest.PoiCoordinate.X.ToString()},
                {"PointY", quest.PoiCoordinate.Y.ToString()},
                {"Title", SqlQuery.CleanText(quest.LogTitle)},
                {"Objectives", SqlQuery.CleanText(quest.LogDescription)},
                {"Details", SqlQuery.CleanText(quest.QuestDescription)},
                {"OfferRewardText", SqlQuery.CleanText(quest.RewardText)},
                {"RequestItemsText", SqlQuery.CleanText(quest.QuestCompletionLog)},
                {"CompletionText", SqlQuery.CleanText(quest.IncompleteText)},
                //{"AreaDescription", SqlQuery.CleanText(quest.AreaDescription)},
            };

            // DDC values
            quest.RewardItems.AddValues(kvplist, "RewardItemId", "RewardItemCount");
            quest.RewardChoiceItems.AddValues(kvplist, "RewardChoiceItemId", "RewardChoiceItemCount");
            quest.FactionRewards.AddValues(kvplist, "RewardFactionId", "RewardFactionValueIdOverride", 100);
            quest.RequiredNpcOrGos.AddValues(kvplist, "RequiredNpcOrGo", "RequiredNpcOrGoCount");
            quest.RequiredItems.AddValues(kvplist, "RequiredItemId", "RequiredItemCount");

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

        private Dictionary<string, string> CreatureTemplate(TrinityCreature creature)
        {
            var kvplist = new Dictionary<string, string>
            {
                {"entry", creature.Entry.ToString()},
                {"name", SqlQuery.CleanText(creature.Name)},
                {"subname", SqlQuery.CleanText(creature.Subname)},
                {"minlevel", creature.MinLevel.ToString()},
                {"maxlevel", creature.MaxLevel.ToString()},
                {"faction", creature.Faction.ToString()},
                {"npcflag", creature.NpcFlags.BitmaskValue.ToString()},
                {"speed_walk", creature.SpeedWalk.ToString()},
                {"speed_run", creature.SpeedRun.ToString()},
                {"scale", creature.Scale.ToString()},
                {"rank", creature.Rank.Id.ToString()},
                {"dmgschool", creature.DmgSchool.Id.ToString()},
                {"BaseAttackTime", creature.BaseAttackTime.ToString()},
                {"RangeAttackTime", creature.RangeAttackTime.ToString()},
                {"unit_class", creature._UnitClass.Id.ToString()},
                {"unit_flags", creature.UnitFlags.BitmaskValue.ToString()},
                {"unit_flags2", creature.UnitFlags2.BitmaskValue.ToString()},
                {"dynamicflags", creature.DynamicFlags.BitmaskValue.ToString()},
                {"family", creature.Family.Id.ToString()},
                {"trainer_type", creature.Trainer.TrainerType.ToString()},
                {"trainer_spell", creature.Trainer.TrainerSpell.ToString()},
                {"trainer_class", creature.Trainer.TrainerClass.ToString()},
                {"trainer_race", creature.Trainer.TrainerRace.ToString()},
                {"type", creature._CreatureType.Id.ToString()},
                {"type_flags", creature.TypeFlags.BitmaskValue.ToString()},
                {"lootid", creature.LootId.ToString()},
                {"pickpocketloot", creature.PickpocketLoot.ToString()},
                {"skinloot", creature.SkinLoot.ToString()},
                {"PetSpellDataId", creature.PetDataId.ToString()},
                {"VehicleId", creature.VehicleId.ToString()},
                {"mingold", creature.MinGold.Amount.ToString()},
                {"maxgold", creature.MaxGold.Amount.ToString()},
                {"AIName", SqlQuery.CleanText(creature.AIName.Description)},
                {"MovementType", creature.Movement.Id.ToString()},
                {"InhabitType", creature.Inhabit.BitmaskValue.ToString()},
                {"HoverHeight", creature.HoverHeight.ToString()},
                {"HealthModifier", creature.HealthModifier.ToString()},
                {"ManaModifier", creature.ManaModifier.ToString()},
                {"ArmorModifier", creature.ArmorModifier.ToString()},
                {"DamageModifier", creature.DamageModifier.ToString()},
                {"ExperienceModifier", creature.ExperienceModifier.ToString()},
                {"RacialLeader", Convert.ToInt16(creature.RacialLeader).ToString()},
                {"RegenHealth", Convert.ToInt16(creature.RegenHealth).ToString()},
                {"mechanic_immune_mask", creature.MechanicImmuneMask.BitmaskValue.ToString()},
                {"flags_extra", creature.FlagsExtra.BitmaskValue.ToString()},
                {"exp", creature.Exp.ToString()},
            };

            creature.ModelIds.AddValues(kvplist);
            creature.Resistances.AddValues(kvplist, "resistance");
            creature.DifficultyEntry.AddValues(kvplist);
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
    }
}
