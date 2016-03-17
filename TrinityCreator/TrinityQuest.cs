using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    class TrinityQuest : INotifyPropertyChanged
    {
        private BitmaskStackPanel _requiredClasses;
        private BitmaskStackPanel _requiredRaces;
        private BitmaskStackPanel _flags;
        private BitmaskStackPanel _specialFlags;
        private DynamicDataControl _rewardItems;
        private DynamicDataControl _rewardChoiceItems;
        private DynamicDataControl _factionRewards;
        private DynamicDataControl _requiredNpcOrGos;
        private DynamicDataControl _requiredItems;


        #region Quest info
        public int EntryId { get; set; }
        public QuestSort PQuestSort { get; set; }
        public QuestInfo PQuestInfo { get; set; }
        public int SuggestedGroupNum { get; set; }
        public BitmaskStackPanel Flags
        {
            get
            {
                if (_flags == null)
                    _flags = BitmaskStackPanel.GetQuestFlags();
                return _flags;
            }
            set
            {
                _flags = value;
            }
        }
        public BitmaskStackPanel SpecialFlags
        {
            get
            {
                if (_specialFlags == null)
                    _specialFlags = BitmaskStackPanel.GetQuestSpecialFlags();
                return _specialFlags;
            }
            set
            {
                _specialFlags = value;
            }
        }

        public string LogTitle { get; set; }
        public string LogDescription { get; set; }
        public string QuestDescription { get; set; }
        public string AreaDescription { get; set; }
        public string QuestCompletionLog { get; set; }

        #endregion


        #region Quest chain data
        public int PrevQuestId { get; set; }
        public int NextQuestId { get; set; }
        //public int ExclusiveGroup { get; set; } // Implement if it can be simplified
        public int NextQuestIdChain { get; set; } // WARNING: This is the completer NPC or GO, not quest ID
        #endregion


        #region Prerequisites
        public int QuestLevel { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        //public int RequiredMinRepFaction { get; set; } // Prerequisite
        //public int RequiredMinRepValue { get; set; }
        //public int RequiredMaxRepFaction { get; set; } // Prerequisite
        //public int RequiredMaxRepValue { get; set; } // Prerequisite
        //public int RequiredSkillId { get; set; }
        //public int RequiredSkillPoints { get; set; }
        public BitmaskStackPanel AllowableClasses
        {
            get
            {
                if (_requiredClasses == null)
                    _requiredClasses = BitmaskStackPanel.GetClassFlags(0);
                return _requiredClasses;
            }
            set { _requiredClasses = value; }
        }
        public BitmaskStackPanel AllowableRaces
        {
            get
            {
                if (_requiredRaces == null)
                    _requiredRaces = BitmaskStackPanel.GetRaceFlags(0);
                return _requiredRaces;
            }
            set { _requiredRaces = value; }
        }
        #endregion


        #region On quest start
        public int StartItem { get; set; } // item given on accept
        public int ProvidedItemCount { get; set; } // Amount of StartItem
        public int SourceSpell { get; set; } // Cast on player on accept
        public Coordinate PoiCoordinate { get; set; }
        #endregion


        #region Objectives
        public int TimeAllowed { get; set; }
        //public int RepObjectiveFaction { get; set; }
        //public int RepObjectiveValue { get; set; }
        public int RequiredPlayerKills { get; set; }
        public DynamicDataControl RequiredItems
        {
            get
            {
                if (_requiredItems == null)
                    _requiredItems = new DynamicDataControl(4, "Item ID", "Amount");
                return _requiredItems;
            }
            set { _requiredItems = value; }
        }
        public DynamicDataControl RequiredNpcOrGos
        {
            get
            {
                if (_requiredNpcOrGos == null)
                    _requiredNpcOrGos = new DynamicDataControl(4, "NPC or GObject ID", "Amount");
                return _requiredNpcOrGos;
            }
            set { _requiredNpcOrGos = value; }
        }
        public int RequiredSpell { get; set; } // requires spell cast on RequiredNpcOrGoIds instead of kill
        #endregion


        #region Rewards
        public QuestXp RewardXpDifficulty { get; set; }
        public Currency RewardMoney { get; set; }
        public Currency RewardBonusMoney { get; set; } // ?????
        public int RewardSpell { get; set; }
        public int RewardSpellCast { get; set; }
        public int RewardHonor { get; set; }
        public int RewardHonorMultiplier => RewardHonor == 0 ? 0 : 1;
        //public int RewardMailTemplateId { get; set; } // Implement when DB is set up properly
        //public int RewardMailDelay { get; set; }
        public PlayerTitle RewardTitle { get; set; }
        public int RewardArenaPoints { get; set; }
        public DynamicDataControl RewardItems
        {
            get
            {
                if (_rewardItems == null)
                    _rewardItems = new DynamicDataControl(maxLines:4, header1:"Item ID", header2:"Amount", defaultValue:"0");
                return _rewardItems;
            }
            set { _rewardItems = value; }
        }
        public DynamicDataControl RewardChoiceItems // additional items to choose one from
        {
            get
            {
                if (_rewardChoiceItems == null)
                    _rewardChoiceItems = new DynamicDataControl(maxLines: 6, header1: "Choice Item ID", header2: "Amount", defaultValue: "0");
                return _rewardChoiceItems;
            }
            set { _rewardChoiceItems = value; }
        }
        public DynamicDataControl FactionRewards
        {
            get
            {
                if (_factionRewards == null)
                    _factionRewards = new DynamicDataControl(maxLines:5, header1:"Faction ID", header2: "Amount", defaultValue:"0");
                return _factionRewards;;
            }
            set { _factionRewards = value; }
        }

        #endregion

        private Dictionary<string, string> GenerateQueryValues()
        {
            var kvplist = new Dictionary<string, string>
            {
                {"entry", EntryId.ToString()},
                {"QuestType", "2"},
                {"QuestLevel", QuestLevel.ToString()},
                {"MinLevel", MinLevel.ToString()},
                {"QuestSortID", PQuestSort.Id.ToString()},
                {"QuestInfoID", PQuestInfo.Id.ToString()},
                {"SuggestedGroupNum", SuggestedGroupNum.ToString()},
                {"TimeAllowed", TimeAllowed.ToString()},
                {"AllowableRaces", AllowableRaces.BitmaskValue.ToString()},
                {"NextQuestIdChain", NextQuestIdChain.ToString()},
                {"RewardXPDifficulty", RewardXpDifficulty.Id.ToString()},
                {"RewardMoney", RewardMoney.Amount.ToString()},
                {"RewardBonusMoney", RewardBonusMoney.Amount.ToString()},
                {"RewardSpell", RewardSpell.ToString()},
                {"RewardSpellCast", RewardSpellCast.ToString()},
                {"RewardHonor", RewardHonor.ToString()},
                {"RewardHonorMultiplier", RewardHonorMultiplier.ToString()},
                {"StartItem", RewardHonor.ToString()},
                {"SourceSpell", SourceSpell.ToString()},
                {"Flags", Flags.BitmaskValue.ToString()},
                {"RewardTitle", RewardTitle.Id.ToString()},
                {"RequiredPlayerKills", RequiredPlayerKills.ToString()},
                {"RewardArenaPoints", RewardArenaPoints.ToString()},
                {"POIContinent", PoiCoordinate.MapId.ToString()},
                {"POIx", PoiCoordinate.X.ToString()},
                {"POIy", PoiCoordinate.Y.ToString()},
                {"LogTitle", LogTitle},
                {"LogDescription", LogDescription},
                {"QuestDescription", QuestDescription},
                {"AreaDescription", AreaDescription},
                {"QuestCompletionLog", QuestCompletionLog},
                {"RequiredSpell", RequiredSpell.ToString()},
            };

            try // Reward Items
            {
                int i = 0;
                foreach (KeyValuePair<object, string> line in RewardItems.GetUserInput())
                {
                    // parse to validate
                    int itemId = int.Parse((string)line.Key);
                    int itemCount = int.Parse(line.Value);
                    i++;

                    kvplist.Add("RewardItemId" + i, itemId.ToString());
                    kvplist.Add("RewardItemCount" + i, itemCount.ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid data in Item Rewards.");
            }

            try // Reward Choice Items
            {
                int i = 0;
                foreach (KeyValuePair<object, string> line in RewardChoiceItems.GetUserInput())
                {
                    // parse to validate
                    int itemId = int.Parse((string)line.Key);
                    int itemCount = int.Parse(line.Value);
                    i++;

                    kvplist.Add("RewardChoiceItemId" + i, itemId.ToString());
                    kvplist.Add("RewardChoiceItemCount" + i, itemCount.ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid data in Item Choice Rewards.");
            }

            try // Faction Rewards
            {
                int i = 0;
                foreach (KeyValuePair<object, string> line in FactionRewards.GetUserInput())
                {
                    // parse to validate
                    int factionId = int.Parse((string)line.Key);
                    int repCount = int.Parse(line.Value) * 100;
                    i++;

                    kvplist.Add("RewardFactionId" + i, factionId.ToString());
                    kvplist.Add("RewardFactionValueIdOverride" + i, repCount.ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid data in Faction Rewards.");
            }

            try // Required Npc or Go
            {
                int i = 0;
                foreach (KeyValuePair<object, string> line in RequiredNpcOrGos.GetUserInput())
                {
                    // parse to validate
                    int npcGoId = int.Parse((string)line.Key);
                    int npcGoCount = int.Parse(line.Value);
                    i++;

                    kvplist.Add("RequiredNpcOrGo" + i, npcGoId.ToString());
                    kvplist.Add("RequiredNpcOrGoCount" + i, npcGoCount.ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid data in Npc or Go requirements.");
            }

            try // Required Npc or Go
            {
                int i = 0;
                foreach (KeyValuePair<object, string> line in RequiredNpcOrGos.GetUserInput())
                {
                    // parse to validate
                    int npcGoId = int.Parse((string)line.Key);
                    int npcGoCount = int.Parse(line.Value);
                    i++;

                    kvplist.Add("RequiredItemId" + i, npcGoId.ToString());
                    kvplist.Add("RequiredItemCount" + i, npcGoCount.ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid data in Item requirements.");
            }

            return kvplist;
        }


        private Dictionary<string, string> GenerateAddonQueryValues()
        {
            var kvplist = new Dictionary<string, string>
            {
                {"ID", EntryId.ToString()},
                {"MaxLevel", MaxLevel.ToString()},
                {"AllowableClasses", AllowableClasses.BitmaskValue.ToString()},
                {"PrevQuestId", PrevQuestId.ToString()},
                {"NextQuestId", NextQuestId.ToString()},
                //{"RewardMailTemplateId", RewardMailTemplateId.ToString()},
                //{"RewardMailDelay", RewardMailDelay.ToString()},
                {"ProvidedItemCount", ProvidedItemCount.ToString()},
                {"SpecialFlags", SpecialFlags.BitmaskValue.ToString()},
            };

            return kvplist;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
