using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;

namespace TrinityCreator.Emulator
{
    class cmangosZero : IEmulator
    {
        public cmangosZero()
        {
            ID = 1;
        }

        public int ID { get; set; }


        public string GenerateQuery(TrinityItem item)
        {
            return SqlQuery.GenerateInsert("item_template", ItemTemplate(item));
        }

        public string GenerateQuery(TrinityQuest quest)
        {
            return SqlQuery.GenerateInsert("quest_template", QuestTemplate(quest));
        }

        public string GenerateQuery(TrinityCreature creature)
        {
            return SqlQuery.GenerateInsert("creature_template", CreatureTemplate(creature))
                + SqlQuery.GenerateInsert("creature_template_addon", CreatureTemplateAddon(creature));
        }

        public string GenerateQuery(LootPage loot)
        {
            throw new NotImplementedException();
        }

        public string GenerateQuery(VendorPage vendor)
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
            };
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
                {"entry", quest.EntryId.ToString()},
                {"Method", "2"},
                {"QuestLevel", quest.QuestLevel.ToString()},
                {"MinLevel", quest.MinLevel.ToString()},
                {"ZoneOrSort", quest.PQuestSort.ToString()},
                {"Type", quest.PQuestInfo.Id.ToString()},
                {"RequiredClasses", quest.AllowableClass.BitmaskValue.ToString()},
                {"RequiredRaces", quest.AllowableRace.BitmaskValue.ToString()},
                {"SuggestedPlayers", quest.SuggestedGroupNum.ToString()},
                {"LimitTime", quest.TimeAllowed.ToString()},
                //{"RewardXPDifficulty", quest.RewardXpDifficulty.Id.ToString()},
                {"RewOrReqMoney", quest.RewardMoney.Amount.ToString()},
                {"RewSpellCast", quest.RewardSpell.ToString()},
                //{"RewardHonor", quest.RewardHonor.ToString()},
                //{"RewardTalents", quest.RewardTalents.ToString()},
                {"SrcItemId", quest.StartItem.ToString()},
                {"SrcItemCount", quest.ProvidedItemCount.ToString()},
                {"SrcSpell", quest.SourceSpell.ToString()},
                {"QuestFlags", quest.Flags.BitmaskValue.ToString()},
                {"SpecialFlags", quest.SpecialFlags.BitmaskValue.ToString()},
                {"PrevQuestId", quest.PrevQuest.ToString()},
                {"NextQuestId", quest.NextQuest.ToString()},
                {"NextQuestInChain", quest.QuestCompleter.ToString()}, // completer npc or object
                //{"RewardTitle", quest.RewardTitle.ToString()},
                //{"RequiredPlayerKills", quest.RequiredPlayerKills.ToString()},
                //{"RewardArenaPoints", quest.RewardArenaPoints.ToString()},
                {"PointMapId", quest.PoiCoordinate.MapId.ToString()},
                {"PointX", quest.PoiCoordinate.X.ToString()},
                {"PointY", quest.PoiCoordinate.Y.ToString()},
                {"Title", SqlQuery.CleanText(quest.LogTitle)},
                {"Objectives", SqlQuery.CleanText(quest.LogDescription)},
                {"Details", SqlQuery.CleanText(quest.QuestDescription)},
                //{"AreaDescription", SqlQuery.CleanText(quest.AreaDescription)},
                //{"QuestCompletionLog", SqlQuery.CleanText(quest.QuestCompletionLog)},
                {"OfferRewardText", SqlQuery.CleanText(quest.RewardText)},
                {"RequestItemsText", SqlQuery.CleanText(quest.IncompleteText)},

                // not implemented in creator yet
                //{"RewMailTemplateId", RewardMailTemplateId.ToString()}, 
                //{"RewMailDelaySecs", RewardMailDelay.ToString()},
                //DetailsEmote1-4, DetailsEmoteDelay1-4, IncompleteEmote, CompleteEmote, OfferRewardEmote1-4, OfferRewardEmoteDelay1-4,StartScript,CompleteScript
            };

            // DDC values
            quest.RewardItems.AddValues(kvplist, "RewItemId", "RewItemCount");
            quest.RewardChoiceItems.AddValues(kvplist, "RewChoiceItemId", "RewChoiceItemCount");
            quest.FactionRewards.AddValues(kvplist, "RewRepFaction", "RewRepValue", 100);
            quest.RequiredNpcOrGos.AddValues(kvplist, "ReqCreatureOrGOId", "ReqCreatureOrGOCount");
            quest.RequiredItems.AddValues(kvplist, "ReqItemId", "ReqItemCount");

            return kvplist;
        }

        private Dictionary<string, string> CreatureTemplate(TrinityCreature creature)
        {
           var kvplist = new Dictionary<string, string>
            {
                {"entry", creature.Entry.ToString()},
                {"Name", SqlQuery.CleanText(creature.Name)},
                {"SubName", SqlQuery.CleanText(creature.Subname)},
                {"MinLevel", creature.MinLevel.ToString()},
                {"MaxLevel", creature.MaxLevel.ToString()},
                {"FactionAlliance", creature.Faction.ToString()},
                {"FactionHorde", creature.Faction.ToString()},
                {"Scale", creature.Scale.ToString()},
                {"Family", creature.Family.Id.ToString()},
                {"CreatureType", creature._CreatureType.Id.ToString()},
                {"InhabitType", creature.Inhabit.BitmaskValue.ToString()},
                {"RegenerateStats", Convert.ToInt16(creature.RegenHealth).ToString()},
                {"RacialLeader", creature.RacialLeader.ToString()},
                {"NpcFlags", creature.NpcFlags.BitmaskValue.ToString()},
                {"UnitFlags", creature.UnitFlags.BitmaskValue.ToString()},
                {"DynamicFlags", creature.DynamicFlags.BitmaskValue.ToString()},
                {"ExtraFlags", creature.FlagsExtra.BitmaskValue.ToString()},
                {"CreatureTypeFlags", creature.TypeFlags.BitmaskValue.ToString()},
                {"SpeedWalk", creature.SpeedWalk.ToString()},
                {"SpeedRun", creature.SpeedRun.ToString()},
                {"UnitClass", creature._UnitClass.Id.ToString()},
                {"Rank", creature.Rank.Id.ToString()},
                {"HealthMultiplier", creature.HealthModifier.ToString()},
                {"DamageMultiplier", creature.DamageModifier.ToString()},
                {"ExperienceMultiplier", creature.ExperienceModifier.ToString()},
                {"MinLevelHealth", creature.MinLevelHealth.ToString()},
                {"MaxLevelHealth", creature.MaxLevelHealth.ToString()},
                {"MinLevelMana", creature.MinLevelMana.ToString()},
                {"MaxLevelMana", creature.MaxLevelMana.ToString()},
                {"MinMeleeDmg", creature.MinMeleeDmg.ToString()},
                {"MaxMeleeDmg", creature.MaxMeleeDmg.ToString()},
                {"MinRangedDmg", creature.MinRangedDmg.ToString()},
                {"MaxRangedDmg", creature.MaxRangedDmg.ToString()},
                {"Armor", creature.Armor.ToString()},
                {"MeleeAttackPower", creature.MeleeAttackPower.ToString()},
                {"RangedAttackPower", creature.RangedAttackPower.ToString()},
                {"MeleeBaseAttackTime", creature.BaseAttackTime.ToString()},
                {"RangedBaseAttackTime", creature.BaseAttackTime.ToString()}, // only seperate in cmangzero, using melee
                {"DamageSchool", creature.DmgSchool.Id.ToString()},
                {"MinLootGold", creature.MinGold.ToString()},
                {"MaxLootGold", creature.MaxGold.ToString()},
                {"LootId", creature.LootId.ToString()},
                {"PickpocketLootId", creature.PickpocketLoot.ToString()},
                {"SkinningLootId", creature.SkinLoot.ToString()},
                {"MechanicImmuneMask", creature.MechanicImmuneMask.BitmaskValue.ToString()},
                //{"SchoolImmuneMask", }, <-- not sure if used over individual resistance fields, no comment in db
                {"PetSpellDataId", creature.PetDataId.ToString()},
                {"MovementType", creature.Movement.Id.ToString()},
                {"TrainerType", creature.Trainer.TrainerType.ToString()},
                {"TrainerSpell", creature.Trainer.TrainerSpell.ToString()},
                {"TrainerClass", creature.Trainer.TrainerClass.ToString()},
                {"TrainerRace", creature.Trainer.TrainerRace.ToString()},
                {"Civilian", Convert.ToInt16(creature.Civilian).ToString()},
                
                // Missing relevant: TrainerTemplateId, VendorTemplateId, GossipMenuId, EquipmentTemplateId
            };

            creature.ModelIds.AddValues(kvplist);
            creature.Resistances.AddValues(kvplist, keyPrefix:"", valuePrefix: "Resistance");

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
    }
}
