using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;
using TrinityCreator.Emulator;

namespace TrinityCreator
{
    public class TrinityQuest : INotifyPropertyChanged
    {
        public TrinityQuest()
        {
            var espvars = Emulator.EmulatorHandler.SelectedEmulator.GetIdColumnName("Quest");
            EntryId = SqlQuery.GetNextId(espvars.Item1, espvars.Item2);
        }

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
        private XmlKeyValue _pQuestInfo;
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
        private XmlKeyValue _rewardXpDifficulty;
        private Currency _rewardMoney;
        private int _rewardSpell;
        private int _rewardHonor;
        private int _rewardTitle;
        private int _rewardArenaPoints;
        private int _questgiver;
        private string _rewardText;
        private int _rewardTalents;
        private string _incompleteText;


        #region Quest info

        public int EntryId
        {
            get { return _entryId; }
            set
            {
                _entryId = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("EntryId");
            }
        }

        public int PQuestSort
        {
            get { return _pQuestSort; }
            set
            {
                _pQuestSort = DataType.LimitLength(value, "smallint(6)");
                RaisePropertyChanged("PQuestSort");
            }
        }

        public XmlKeyValue PQuestInfo
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
                _suggestedGroupNum = DataType.LimitLength(value, "tinyint(3)");
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
                _logTitle = DataType.LimitLength(value, 65535); 
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
                _logDescription = DataType.LimitLength(value, 65535);
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
                _questDescription = DataType.LimitLength(value, 65535);
                RaisePropertyChanged("QuestDescription");
            }
        }

        public string AreaDescription
        {
            get { return _areaDescription; }
            set
            {
                _areaDescription = DataType.LimitLength(value, 65535);
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
                _questCompletionLog = DataType.LimitLength(value, 65535);
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
                _rewardText = DataType.LimitLength(value, 65535);
                RaisePropertyChanged("RewardText");
            }
        }

        public string IncompleteText
        {
            get
            {
                if (_incompleteText == null)
                    _incompleteText = "Greetings $N. How goes the quest?";
                return _incompleteText;
            }
            set
            {
                _incompleteText = DataType.LimitLength(value, 65535);
                RaisePropertyChanged("IncompleteText");
            }
        }
        #endregion


        #region Quest chain data
        public int PrevQuest
        {
            get { return _prevQuest; }
            set
            {
                _prevQuest = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("PrevQuest");
            }
        }
        public int NextQuest {
            get { return _nextQuest; }
            set
            {
                _nextQuest = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("NextQuest");
            }
        }
        //public int ExclusiveGroup { get; set; } // Implement if it can be simplified        
        public int Questgiver
        {
            get { return _questgiver; }
            set
            {
                _questgiver = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Questgiver");
            }
        }
        public int QuestCompleter
        {
            get { return _questCompleter; }
            set
            {
                _questCompleter = DataType.LimitLength(value, "mediumint(8)");
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
                _questLevel = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("QuestLevel");
            }
        }
        public int MinLevel {
            get { return _minLevel; }
            set
            {
                _minLevel = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("MinLevel");
            }
        }
        public int MaxLevel {
            get { return _maxLevel; }
            set
            {
                _maxLevel = DataType.LimitLength(value, "tinyint(3)");
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
                _startItem = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("StartItem");
            }
        } // item given on accept
        public int ProvidedItemCount {
            get { return _providedItemCount; }
            set
            {
                _providedItemCount = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("ProvidedItemCount");
            }
        } // Amount of StartItem
        public int SourceSpell {
            get { return _sourceSpell; }
            set
            {
                _sourceSpell = DataType.LimitLength(value, "mediumint(8)");
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
                _timeAllowed = DataType.LimitLength(value, "int(10)");
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
                _requiredPlayerKills = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("RequiredPlayerKills");
            }
        }
        public DynamicDataControl RequiredItems
        {
            get
            {
                if (_requiredItems == null)
                    _requiredItems = new DynamicDataControl(4, "Item ID", "Amount", keyMySqlDt:"mediumint(8)", valueMySqlDt:"smallint(5)");
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
                    _requiredNpcOrGos = new DynamicDataControl(4, "NPC or GObject ID", "Amount", keyMySqlDt: "mediumint(8)", valueMySqlDt: "smallint(5)");
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
        public XmlKeyValue RewardXpDifficulty 
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
                _rewardSpell = DataType.LimitLength(value, "int(11)"); ;
                RaisePropertyChanged("RewardSpell");
            }
        }
        public int RewardHonor {
            get { return _rewardHonor; }
            set
            {
                _rewardHonor = DataType.LimitLength(value, "int(11)"); ;
                RaisePropertyChanged("RewardHonor");
            }
        }
        //public int RewardMailTemplateId { get; set; } // Implement when DB is set up properly
        //public int RewardMailDelay { get; set; }
        public int RewardTitle {
            get { return _rewardTitle; }
            set
            {
                _rewardTitle = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("RewardTitle");
            }
        }
        public int RewardArenaPoints {
            get { return _rewardArenaPoints; }
            set
            {
                _rewardArenaPoints = DataType.LimitLength(value, "smallint(5)");
                RaisePropertyChanged("RewardArenaPoints");
            }
        }
        public int RewardTalents
        {
            get { return _rewardTalents; }
            set
            {
                _rewardTalents = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("RewardTalents");
            }
        }
        public DynamicDataControl RewardItems
        {
            get
            {
                if (_rewardItems == null)
                    _rewardItems = new DynamicDataControl(maxLines:4, header1:"Item ID", header2:"Amount", defaultValue:"0", keyMySqlDt:"mediumint(8)", valueMySqlDt: "smallint(5)");
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
                    _rewardChoiceItems = new DynamicDataControl(maxLines: 6, header1: "Choice Item ID", header2: "Amount", defaultValue: "0", keyMySqlDt: "mediumint(8)", valueMySqlDt: "smallint(5)");
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
                    _factionRewards = new DynamicDataControl(maxLines:5, header1:"Faction ID", header2: "Amount", defaultValue:"0", keyMySqlDt: "smallint(5)", valueMySqlDt: "mediumint(8)");
                return _factionRewards;
            }
            set
            {
                _factionRewards = value;
                RaisePropertyChanged("FactionRewards");
            }
        }
        #endregion       
        
        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
