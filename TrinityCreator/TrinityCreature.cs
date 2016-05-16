using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;

namespace TrinityCreator
{
    public class TrinityCreature : INotifyPropertyChanged
    {
        public TrinityCreature()
        {
            Entry = SqlQuery.GetNextId("creature_template", "entry");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int _entry;
        private int _modelId1;
        private int _modelId2;
        private int _modelId3;
        private int _modelId4;
        private string _name;
        private string _subname;
        private int _minLevel;
        private int _maxLevel;
        private int _exp;
        private int _faction;
        private BitmaskStackPanel _npcFlags;
        private double _speedWalk = 2.5;
        private double _speedRun = 7.5;
        private int _scale = 1;
        private CreatureRank _rank;
        private DamageType _dmgSchool;
        private int _baseAttackTime = 1500;
        private int _rangeAttackTime = 2000;
        private UnitClass _unitClass;
        private BitmaskStackPanel _unitFlags;
        private BitmaskStackPanel _unitFlags2;
        private BitmaskStackPanel _dynamicFlags;
        private CreatureFamily _family;
        private TrainerData _trainer;
        private CreatureType _creatureType;
        private BitmaskStackPanel _typeFlags;
        private int _lootId;
        private int _pickpocketLoot;
        private int _skinLoot;
        private DynamicDataControl _resistances;
        private int _petDataId;
        private int _vehicleId;
        private Currency _minGold;
        private Currency _maxGold;
        private AI _aiName;
        private MovementType _movement;
        private InhabitType _inhabit;
        private int _hoverHeight;
        private double _healthModifier;
        private double _manaModifier;
        private double _damageModifier;
        private double _armorModifier;
        private double _experienceModifier;
        private bool _racialLeader;
        private bool _regenHealth;
        private BitmaskStackPanel _mechanicImmuneMask;
        private BitmaskStackPanel _flagsExtra;


        #region
        public int Entry
        {
            get { return _entry; }
            set
            {
                _entry = value;
                RaisePropertyChanged("Entry");
            }
        }
        // difficulty_entry_1-3
        // KillCredit1-2
        public int ModelId1
        {
            get { return _modelId1; }
            set
            {
                _modelId1 = value;
                RaisePropertyChanged("ModelId1");
            }
        }
        public int ModelId2
        {
            get { return _modelId2; }
            set
            {
                _modelId2 = value;
                RaisePropertyChanged("ModelId2");
            }
        }
        public int ModelId3
        {
            get { return _modelId3; }
            set
            {
                _modelId3 = value;
                RaisePropertyChanged("ModelId3");
            }
        }
        public int ModelId4
        {
            get { return _modelId4; }
            set
            {
                _modelId4 = value;
                RaisePropertyChanged("ModelId4");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        public string Subname
        {
            get { return _subname; }
            set
            {
                _subname = value;
                RaisePropertyChanged("Subname");
            }
        }
        // IconName = vehiclecursor, null, ??
        // gossip_menu_id
        public int MinLevel
        {
            get { return _minLevel; }
            set
            {
                _minLevel = value;
                RaisePropertyChanged("MinLevel");
            }
        }
        public int MaxLevel
        {
            get { return _maxLevel; }
            set
            {
                _maxLevel = value;
                RaisePropertyChanged("MaxLevel");
            }
        }
        public int XP
        {
            get { return _exp; }
            set
            {
                _exp = value;
                RaisePropertyChanged("XP");
            }
        }
        public int Faction
        {
            get { return _faction; }
            set
            {
                _faction = value;
                RaisePropertyChanged("Faction");
            }
        }
        public BitmaskStackPanel NpcFlags
        {
            get
            {
                if (_npcFlags == null)
                    _npcFlags = BitmaskStackPanel.GetNpcFlags();
                return _npcFlags;
            }
            set
            {
                _npcFlags = value;
                RaisePropertyChanged("NpcFlags");
            }
        }
        public double SpeedWalk
        {
            get { return _speedWalk; }
            set
            {
                _speedWalk = value;
                RaisePropertyChanged("SpeedWalk");
            }
        }
        public double SpeedRun
        {
            get { return _speedRun; }
            set
            {
                _speedRun = value;
                RaisePropertyChanged("SpeedRun");
            }
        }
        public int Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                RaisePropertyChanged("Scale");
            }
        }
        public CreatureRank Rank
        {
            get { return _rank; }
            set
            {
                _rank = value;
                RaisePropertyChanged("Rank");
            }
        }
        public DamageType DmgSchool
        {
            get { return _dmgSchool; }
            set
            {
                _dmgSchool = value;
                RaisePropertyChanged("DmgSchool");
            }
        }
        public int BaseAttackTime
        {
            get { return _baseAttackTime; }
            set
            {
                _baseAttackTime = value;
                RaisePropertyChanged("BaseAttackTime");
            }
        }
        public int RangeAttackTime
        {
            get { return _rangeAttackTime; }
            set
            {
                _rangeAttackTime = value;
                RaisePropertyChanged("RangeAttackTime");
            }
        }
        public UnitClass _UnitClass
        {
            get { return _unitClass; }
            set
            {
                _unitClass = value;
                RaisePropertyChanged("_UnitClass");
            }
        }
        public BitmaskStackPanel UnitFlags
        {
            get
            {
                if (_unitFlags == null)
                    _unitFlags = BitmaskStackPanel.GetUnitFlags();
                return _unitFlags;
            }
            set { _unitFlags = value; }
        }
        public BitmaskStackPanel UnitFlags2
        {
            get
            {
                if (_unitFlags2 == null)
                    _unitFlags2 = BitmaskStackPanel.GetUnitFlags2();
                return _unitFlags2;
            }
            set { _unitFlags2 = value; }
        }
        public BitmaskStackPanel DynamicFlags
        {
            get
            {
                if (_dynamicFlags == null)
                    _dynamicFlags = BitmaskStackPanel.GetCreatureDynamicFlags();
                return _dynamicFlags;
            }
            set { _dynamicFlags = value; }
        }
        public CreatureFamily Family
        {
            get { return _family; }
            set { _family = value; }
        }
        public TrainerData Trainer
        {
            get { return _trainer; }
            set { _trainer = value; }
        }
        public CreatureType _CreatureType
        {
            get { return _creatureType; }
            set { _creatureType = value; }
        }
        public BitmaskStackPanel TypeFlags
        {
            get {
                if (_typeFlags == null)
                    _typeFlags = BitmaskStackPanel.GetCreatureTypeFlags();
                return _typeFlags;
            }
            set { _typeFlags = value; }
        }
        public int LootId
        {
            get { return _lootId; }
            set { _lootId = value; }
        }
        public int PickpocketLoot
        {
            get { return _pickpocketLoot; }
            set { _pickpocketLoot = value; }
        }
        public int SkinLoot
        {
            get { return _skinLoot; }
            set { _skinLoot = value; }
        }
        public DynamicDataControl Resistances
        {
            get
            {
                if (_resistances == null)
                    _resistances = new DynamicDataControl(DamageType.GetDamageTypes(true), 6, true, defaultValue: "0", valueMySqlDt: "smallint(6)");
                return _resistances;
            }
            set
            {
                _resistances = value;
                RaisePropertyChanged("Resistances");
            }
        }
        public DynamicDataControl Spells
        {
            get
            {
                if (_resistances == null)
                {
                    string[] spellCols = new string[]
                       {
                           "spell1","spell2","spell3","spell4","spell5","spell6","spell7","spell8",
                       };
                    _resistances = new DynamicDataControl(spellCols, 8, true, defaultValue: "0", valueMySqlDt: "mediumint(8)");
                }
                return _resistances;
            }
            set
            {
                _resistances = value;
                RaisePropertyChanged("Resistances");
            }
        }
        public int PetDataId
        {
            get { return _petDataId; }
            set { _petDataId = value; }
        }
        public int VehicleId
        {
            get { return _vehicleId; }
            set { _vehicleId = value; }
        }
        public Currency MinGold
        {
            get
            {
                if (_minGold == null)
                    _minGold = new Currency(0);
                return _minGold;
            }
            set { _minGold = value; }
        }
        public Currency MaxGold
        {
            get
            {
                if (_maxGold == null)
                    _maxGold = new Currency(0);
                return _maxGold;
            }
            set { _maxGold = value; }
        }
        public AI AIName
        {
            get { return _aiName; }
            set { _aiName = value; }
        }
        public MovementType Movement
        {
            get { return _movement; }
            set { _movement = value; }
        }
        public InhabitType Inhabit
        {
            get { return _inhabit; }
            set { _inhabit = value; }
        }
        public int HoverHeight // requires MOVEMENTFLAG_DISABLE_GRAVITY
        {
            get { return _hoverHeight; }
            set { _hoverHeight = value; }
        }
        public double HealthModifier
        {
            get { return _healthModifier; }
            set { _healthModifier = value; }
        }
        public double ManaModifier
        {
            get { return _manaModifier; }
            set { _manaModifier = value; }
        }
        public double DamageModifier
        {
            get { return _damageModifier; }
            set { _damageModifier = value; }
        }
        public double ArmorModifier
        {
            get { return _armorModifier; }
            set { _armorModifier = value; }
        }
        public double ExperienceModifier
        {
            get { return _experienceModifier; }
            set { _experienceModifier = value; }
        }
        public bool RacialLeader
        {
            get { return _racialLeader; }
            set { _racialLeader = value; }
        }
        public bool RegenHealth
        {
            get { return _regenHealth; }
            set { _regenHealth = value; }
        }
        public BitmaskStackPanel MechanicImmuneMask
        {
            get
            {
                if (_mechanicImmuneMask == null)
                    _mechanicImmuneMask = BitmaskStackPanel.GetMechanicImmuneMask();
                return _mechanicImmuneMask;
            }
            set { _mechanicImmuneMask = value; }
        }
        public BitmaskStackPanel FlagsExtra
        {
            get
            {
                if (_flagsExtra == null)
                    _flagsExtra = BitmaskStackPanel.GetCreatureFlagsExtra();
                return _flagsExtra;
            }
            set { _flagsExtra = value; }
        }



        #endregion

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
