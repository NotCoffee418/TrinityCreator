using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;

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
        private int _prevQuest;
        private int _nextQuest;
        private int _questCompleter;
        private int _questLevel;
        private int _minLevel;
        private int _maxLevel;
        private int _startItem;
        private int _providedItemCount;
        private int _sourceSpell;
        private Coordinate _poiCoordinate;
        private int _timeAllowed;
        private int _requiredPlayerKills;
        private QuestXp _rewardXpDifficulty;
        private Currency _rewardMoney;
        private int _rewardSpell;
        private int _rewardHonor;
        private int _rewardTitle;
        private int _rewardArenaPoints;
        private int _questgiver;
        private string _rewardText;


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
                    _logDescription = "Short description";
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
                    _questCompletionLog = "";
                return _questCompletionLog;
            }
            set
            {
                _questCompletionLog = value;
                RaisePropertyChanged("QuestCompletionLog");
            }
        }

        public string RewardText
        {
            get
            {
                if (_rewardText == null)
                    _rewardText = "Good job! Here is your reward.";
                return _rewardText;
            }
            set
            {
                _rewardText = value;
                RaisePropertyChanged("RewardText");
            }
        }
        #endregion


        #region Quest chain data
        public int PrevQuest
        {
            get { return _prevQuest; }
            set
            {
                _prevQuest = value;
                RaisePropertyChanged("PrevQuest");
            }
        }
        public int NextQuest {
            get { return _nextQuest; }
            set
            {
                _nextQuest = value;
                RaisePropertyChanged("NextQuest");
            }
        }
        //public int ExclusiveGroup { get; set; } // Implement if it can be simplified        
        public int Questgiver
        {
            get { return _questgiver; }
            set
            {
                _questgiver = value;
                RaisePropertyChanged("Questgiver");
            }
        }
        public int QuestCompleter
        {
            get { return _questCompleter; }
            set
            {
                _questCompleter = value;
                RaisePropertyChanged("QuestCompleter");
            }
        }
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
            get {
                if (_rewardMoney == null)
                    _rewardMoney = new Currency(0);
                return _rewardMoney;
            }
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
        public int RewardHonor {
            get { return _rewardHonor; }
            set
            {
                _rewardHonor = value;
                RaisePropertyChanged("RewardHonor");
            }
        }
        //public int RewardMailTemplateId { get; set; } // Implement when DB is set up properly
        //public int RewardMailDelay { get; set; }
        public int RewardTitle {
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


        #region Generate Query
        public string GenerateSqlQuery()
        {
            return SqlQuery.GenerateInsert("quest_template", GenerateQueryValues()) +
                SqlQuery.GenerateInsert("quest_template_addon", GenerateAddonQueryValues()) +
                SqlQuery.GenerateInsert("creature_queststarter", GenerateQuestStarterValues()) +
                SqlQuery.GenerateInsert("creature_questender", GenerateQuestEnderValues()) +
                SqlQuery.GenerateInsert("quest_offer_reward", GenerateQuestOfferRewardValues());
        }

        private Dictionary<string, string> GenerateQueryValues()
        {
            var kvplist = new Dictionary<string, string>
            {
                {"ID", EntryId.ToString()},
                {"QuestType", "2"},
                {"QuestLevel", QuestLevel.ToString()},
                {"MinLevel", MinLevel.ToString()},
                {"QuestSortID", PQuestSort.ToString()},
                {"QuestInfoID", PQuestInfo.Id.ToString()},
                {"SuggestedGroupNum", SuggestedGroupNum.ToString()},
                {"TimeAllowed", TimeAllowed.ToString()},
                {"AllowableRaces", AllowableRace.BitmaskValue.ToString()},
                {"RewardXPDifficulty", RewardXpDifficulty.Id.ToString()},
                {"RewardMoney", RewardMoney.Amount.ToString()},
                {"RewardSpell", RewardSpell.ToString()},
                {"RewardDisplaySpell", RewardSpell.ToString()},
                {"RewardHonor", RewardHonor.ToString()},
                {"StartItem", RewardHonor.ToString()},
                {"Flags", Flags.BitmaskValue.ToString()},
                {"RewardTitle", RewardTitle.ToString()},
                {"RequiredPlayerKills", RequiredPlayerKills.ToString()},
                {"RewardArenaPoints", RewardArenaPoints.ToString()},
                {"POIContinent", PoiCoordinate.MapId.ToString()},
                {"POIx", PoiCoordinate.X.ToString()},
                {"POIy", PoiCoordinate.Y.ToString()},
                {"LogTitle", SqlQuery.CleanText(LogTitle)},
                {"LogDescription", SqlQuery.CleanText(LogDescription)},
                {"QuestDescription", SqlQuery.CleanText(QuestDescription)},
                {"AreaDescription", SqlQuery.CleanText(AreaDescription)},
                {"QuestCompletionLog", SqlQuery.CleanText(QuestCompletionLog)},
            };
            
            // DDC values
            RewardItems.AddValues(kvplist, "RewardItem", "RewardAmount");
            RewardChoiceItems.AddValues(kvplist, "RewardChoiceItemID", "RewardChoiceItemQuantity");
            FactionRewards.AddValues(kvplist, "RewardFactionID", "RewardFactionOverride", 100);
            RequiredNpcOrGos.AddValues(kvplist, "RequiredNpcOrGo", "RequiredNpcOrGoCount");
            RequiredItems.AddValues(kvplist, "RequiredItemId", "RequiredItemCount");

            return kvplist;
        }


        private Dictionary<string, string> GenerateAddonQueryValues()
        {
            var kvplist = new Dictionary<string, string>
            {
                {"ID", EntryId.ToString()},
                {"MaxLevel", MaxLevel.ToString()},
                {"AllowableClasses", AllowableClass.BitmaskValue.ToString()},
                {"PrevQuestId", PrevQuest.ToString()},
                {"NextQuestId", NextQuest.ToString()},
                //{"RewardMailTemplateId", RewardMailTemplateId.ToString()},
                //{"RewardMailDelay", RewardMailDelay.ToString()},
                {"ProvidedItemCount", ProvidedItemCount.ToString()},
                {"SpecialFlags", SpecialFlags.BitmaskValue.ToString()},
                {"SourceSpellID", SourceSpell.ToString()},
            };

            return kvplist;
        }


        private Dictionary<string, string> GenerateQuestStarterValues()
        {
            return new Dictionary<string, string>
            {
                {"id", Questgiver.ToString()},
                {"quest", EntryId.ToString()},
            };
        }

        private Dictionary<string, string> GenerateQuestEnderValues()
        {
            return new Dictionary<string, string>
            {
                {"id", QuestCompleter.ToString()},
                {"quest", EntryId.ToString()},
            };
        }

        private Dictionary<string, string> GenerateQuestOfferRewardValues()
        {
            return new Dictionary<string, string>
            {
                {"ID", EntryId.ToString()},
                // Reward emotes
                {"RewardText", EntryId.ToString()},
            };
        }

        #endregion

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
