using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class TrinityQuest : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private BitmaskStackPanel _requiredClasses;
        private BitmaskStackPanel _requiredRaces;
        private BitmaskStackPanel _flags;
        private BitmaskStackPanel _specialFlags;
        private DynamicDataControl _rewardItems;
        private DynamicDataControl _rewardChoiceItems;
        private DynamicDataControl _factionRewards;
        private DynamicDataControl _requiredNpcOrGos;
        private DynamicDataControl _requiredItems;
        private string _logTitle = null;
        private int _entryId;
        private int _pQuestSort;
        private QuestInfo _pQuestInfo;
        private int _suggestedGroupNum;
        private string _logDescription = null;
        private string _questDescription = null;
        private string _areaDescription;
        private string _questCompletionLog;
        private TrinityQuest _prevQuest;
        private TrinityQuest _nextQuest;
        private int _nextQuestIdChain;
        private int _questLevel;
        private int _minLevel;
        private int _maxLevel;
        private int _startItem;
        private int _providedItemCount;
        private int _sourceSpell;
        private Coordinate _poiCoordinate;
        private int _timeAllowed;
        private int _requiredPlayerKills;
        private int _requiredSpell;
        private QuestXp _rewardXpDifficulty;
        private Currency _rewardMoney;
        private int _rewardSpell;
        private int _rewardSpellCast;
        private int _rewardHonor;
        private PlayerTitle _rewardTitle;
        private int _rewardArenaPoints;


        #region Quest info

        public int EntryId
        {
            get { return _entryId; }
            set
            {
                _entryId = value;
                RaisePropertyChanged("EntryId");
            }
        }

        public int PQuestSort
        {
            get { return _pQuestSort; }
            set
            {
                _pQuestSort = value;
                RaisePropertyChanged("PQuestSort");
            }
        }

        public QuestInfo PQuestInfo
        {
            get { return _pQuestInfo; }
            set
            {
                _pQuestInfo = value;
                RaisePropertyChanged("PQuestInfo");
            }
        }

        public int SuggestedGroupNum
        {
            get { return _suggestedGroupNum; }
            set
            {
                _suggestedGroupNum = value;
                RaisePropertyChanged("SuggestedGroupNum");
            }
        }
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

        public string LogTitle
        {
            get
            {
                if (_logTitle == null) // on initial load
                    _logTitle = "Enter quest name here";
                return _logTitle;
            }
            set
            {
                _logTitle = value; 
                RaisePropertyChanged("LogTitle");
            }
        }

        public string LogDescription
        {
            get {
                if (_logDescription == null)
                    _logDescription = "Short description & objectives";
                return _logDescription; 
            }
            set
            {
                _logDescription = value;
                RaisePropertyChanged("LogDescription");
            }
        }

        public string QuestDescription
        {
            get
            {
                if (_questDescription == null)
                    _questDescription = "Long quest description here";
                return _questDescription; 
            }
            set
            {
                _questDescription = value;
                RaisePropertyChanged("QuestDescription");
            }
        }

        public string AreaDescription
        {
            get { return _areaDescription; }
            set
            {
                _areaDescription = value;
                RaisePropertyChanged("AreaDescription");
            }
        }
        public string QuestCompletionLog {
            get
            {
                if (_questCompletionLog == null)
                    _questCompletionLog = "This text displays when turning in the quest.";
                return _questCompletionLog;
            }
            set
            {
                _questCompletionLog = value;
                RaisePropertyChanged("QuestCompletionLog");
            }
        }

        #endregion


        #region Quest chain data
        public TrinityQuest PrevQuest
        {
            get { return _prevQuest; }
            set
            {
                _prevQuest = value;
                RaisePropertyChanged("PrevQuest");
            }
        }
        public TrinityQuest NextQuest {
            get { return _nextQuest; }
            set
            {
                _nextQuest = value;
                RaisePropertyChanged("NextQuest");
            }
        }
        //public int ExclusiveGroup { get; set; } // Implement if it can be simplified
        public int NextQuestIdChain {
            get { return _nextQuestIdChain; }
            set
            {
                _nextQuestIdChain = value;
                RaisePropertyChanged("NextQuestIdChain");
            }
        } // WARNING: This is the completer NPC or GO, not quest ID
        #endregion


        #region Prerequisites
        public int QuestLevel
        {
            get { return _questLevel; }
            set
            {
                _questLevel = value;
                RaisePropertyChanged("QuestLevel");
            }
        }
        public int MinLevel {
            get { return _minLevel; }
            set
            {
                _minLevel = value;
                RaisePropertyChanged("MinLevel");
            }
        }
        public int MaxLevel {
            get { return _maxLevel; }
            set
            {
                _maxLevel = value;
                RaisePropertyChanged("MaxLevel");
            }
        }
        //public int RequiredMinRepFaction { get; set; } // Prerequisite
        //public int RequiredMinRepValue { get; set; }
        //public int RequiredMaxRepFaction { get; set; } // Prerequisite
        //public int RequiredMaxRepValue { get; set; } // Prerequisite
        //public int RequiredSkillId { get; set; }
        //public int RequiredSkillPoints { get; set; }
        public BitmaskStackPanel AllowableClass
        {
            get
            {
                if (_requiredClasses == null)
                    _requiredClasses = BitmaskStackPanel.GetClassFlags(0);
                return _requiredClasses;
            }
            set
            {
                _requiredClasses = value;
                RaisePropertyChanged("AllowableClasses");
            }
        }
        public BitmaskStackPanel AllowableRace
        {
            get
            {
                if (_requiredRaces == null)
                    _requiredRaces = BitmaskStackPanel.GetRaceFlags(0);
                return _requiredRaces;
            }
            set
            {
                _requiredRaces = value;
                RaisePropertyChanged("AllowableRaces");
            }
        }
        #endregion


        #region On quest start
        public int StartItem { 
            get { return _startItem; }
            set
            {
                _startItem = value;
                RaisePropertyChanged("StartItem");
            }
        } // item given on accept
        public int ProvidedItemCount {
            get { return _providedItemCount; }
            set
            {
                _providedItemCount = value;
                RaisePropertyChanged("ProvidedItemCount");
            }
        } // Amount of StartItem
        public int SourceSpell {
            get { return _sourceSpell; }
            set
            {
                _sourceSpell = value;
                RaisePropertyChanged("SourceSpell");
            }
        } // Cast on player on accept
        public Coordinate PoiCoordinate {
            get {
                if (_poiCoordinate == null)
                    _poiCoordinate = new Coordinate();
                return _poiCoordinate;
            }
            set
            {
                _poiCoordinate = value;
                RaisePropertyChanged("PoiCoordinate");
            }
        }
        #endregion


        #region Objectives
        public int TimeAllowed
        {
            get { return _timeAllowed; }
            set
            {
                _timeAllowed = value;
                RaisePropertyChanged("TimeAllowed");
            }
        }
        //public int RepObjectiveFaction { get; set; }
        //public int RepObjectiveValue { get; set; }
        public int RequiredPlayerKills 
        {
            get { return _requiredPlayerKills; }
            set
            {
                _requiredPlayerKills = value;
                RaisePropertyChanged("RequiredPlayerKills");
            }
        }
        public DynamicDataControl RequiredItems
        {
            get
            {
                if (_requiredItems == null)
                    _requiredItems = new DynamicDataControl(4, "Item ID", "Amount");
                return _requiredItems;
            }
            set
            {
                _requiredItems = value;
                RaisePropertyChanged("RequiredItems");
            }
        }
        public DynamicDataControl RequiredNpcOrGos
        {
            get
            {
                if (_requiredNpcOrGos == null)
                    _requiredNpcOrGos = new DynamicDataControl(4, "NPC or GObject ID", "Amount");
                return _requiredNpcOrGos;
            }
            set
            {
                _requiredNpcOrGos = value;
                RaisePropertyChanged("RequiredNpcOrGos");
            }
        }
        public int RequiredSpell 
        {
            get { return _requiredSpell; }
            set
            {
                _requiredSpell = value;
                RaisePropertyChanged("RequiredSpell");
            }
        } // requires spell cast on RequiredNpcOrGoIds instead of kill
        #endregion


        #region Rewards
        public QuestXp RewardXpDifficulty 
        {
            get { return _rewardXpDifficulty; }
            set
            {
                _rewardXpDifficulty = value;
                RaisePropertyChanged("RewardXpDifficulty");
            }
        }
        public Currency RewardMoney
        {
            get { return _rewardMoney; }
            set
            {
                _rewardMoney = value;
                RaisePropertyChanged("RewardMoney");
            }
        }
        public int RewardSpell
        {
            get { return _rewardSpell; }
            set
            {
                _rewardSpell = value;
                RaisePropertyChanged("RewardSpell");
            }
        }
        public int RewardSpellCast {
            get { return _rewardSpellCast; }
            set
            {
                _rewardSpellCast = value;
                RaisePropertyChanged("RewardSpellCast");
            }
        }
        public int RewardHonor {
            get { return _rewardHonor; }
            set
            {
                _rewardHonor = value;
                RaisePropertyChanged("RewardHonor");
                RaisePropertyChanged("RewardHonorMultiplier");
            }
        }
        public int RewardHonorMultiplier
        {
            get { return RewardHonor == 0 ? 0 : 1; }
        }
        //public int RewardMailTemplateId { get; set; } // Implement when DB is set up properly
        //public int RewardMailDelay { get; set; }
        public PlayerTitle RewardTitle {
            get { return _rewardTitle; }
            set
            {
                _rewardTitle = value;
                RaisePropertyChanged("RewardTitle");
            }
        }
        public int RewardArenaPoints {
            get { return _rewardArenaPoints; }
            set
            {
                _rewardArenaPoints = value;
                RaisePropertyChanged("RewardArenaPoints");
            }
        }
        public DynamicDataControl RewardItems
        {
            get
            {
                if (_rewardItems == null)
                    _rewardItems = new DynamicDataControl(maxLines:4, header1:"Item ID", header2:"Amount", defaultValue:"0");
                return _rewardItems;
            }
            set
            {
                _rewardItems = value;
                RaisePropertyChanged("RewardItems");
            }
        }
        public DynamicDataControl RewardChoiceItems // additional items to choose one from
        {
            get
            {
                if (_rewardChoiceItems == null)
                    _rewardChoiceItems = new DynamicDataControl(maxLines: 6, header1: "Choice Item ID", header2: "Amount", defaultValue: "0");
                return _rewardChoiceItems;
            }
            set
            {
                _rewardChoiceItems = value;
                RaisePropertyChanged("RewardChoiceItems");
            }
        }
        public DynamicDataControl FactionRewards
        {
            get
            {
                if (_factionRewards == null)
                    _factionRewards = new DynamicDataControl(maxLines:5, header1:"Faction ID", header2: "Amount", defaultValue:"0");
                return _factionRewards;
            }
            set
            {
                _factionRewards = value;
                RaisePropertyChanged("FactionRewards");
            }
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
                {"QuestSortID", PQuestSort.ToString()},
                {"QuestInfoID", PQuestInfo.Id.ToString()},
                {"SuggestedGroupNum", SuggestedGroupNum.ToString()},
                {"TimeAllowed", TimeAllowed.ToString()},
                {"AllowableRaces", AllowableRace.BitmaskValue.ToString()},
                {"NextQuestIdChain", NextQuestIdChain.ToString()},
                {"RewardXPDifficulty", RewardXpDifficulty.Id.ToString()},
                {"RewardMoney", RewardMoney.Amount.ToString()},
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
                {"AllowableClasses", AllowableClass.BitmaskValue.ToString()},
                {"PrevQuestId", PrevQuest.EntryId.ToString()},
                {"NextQuestId", NextQuest.EntryId.ToString()},
                //{"RewardMailTemplateId", RewardMailTemplateId.ToString()},
                //{"RewardMailDelay", RewardMailDelay.ToString()},
                {"ProvidedItemCount", ProvidedItemCount.ToString()},
                {"SpecialFlags", SpecialFlags.BitmaskValue.ToString()},
            };

            return kvplist;
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public string GenerateSqlQuery()
        {
            var kvplist1 = GenerateQueryValues();
            var kvplist2 = GenerateAddonQueryValues();

            // generate query1
            var query1 = "INSERT INTO quest_template (";
            query1 += string.Join(", ", kvplist1.Keys);
            query1 += ") VALUES (";
            query1 += string.Join(", ", kvplist1.Values) + ");" + Environment.NewLine;

            // generate query2
            var query2 = "INSERT INTO quest_template_addon (";
            query2 += string.Join(", ", kvplist2.Keys);
            query2 += ") VALUES (";
            query2 += string.Join(", ", kvplist2.Values) + ");" + Environment.NewLine;

            
            return query1 + Environment.NewLine + query2;
        }
    }
}
