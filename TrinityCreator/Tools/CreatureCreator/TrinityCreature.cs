using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Data;
using TrinityCreator.Database;
using TrinityCreator.Profiles;
using TrinityCreator.UI.UIElements;

namespace TrinityCreator.Tools.CreatureCreator
{
    public class TrinityCreature : INotifyPropertyChanged
    {
        public TrinityCreature()
        {
            Entry = LookupQuery.GetNextId(Export.C.Creature);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int _entry;
        private DynamicDataControl _modelIds;
        private string _name;
        private string _subname;
        private int _minLevel = 1;
        private int _maxLevel = 1;
        private int _faction;
        private BitmaskStackPanel _npcFlags;
        private float _speedWalk = 1.1f;
        private float _speedRun = 1.17f;
        private float _scale = 1;
        private XmlKeyValue _rank;
        private DamageType _dmgSchool;
        private int _baseAttackTime = 1500;
        private int _rangeAttackTime = 2000;
        private XmlKeyValue _unitClass;
        private BitmaskStackPanel _unitFlags;
        private BitmaskStackPanel _unitFlags2;
        private BitmaskStackPanel _dynamicFlags;
        private CreatureFamily _family;
        private TrainerData _trainer;
        private XmlKeyValue _creatureType;
        private BitmaskStackPanel _typeFlags;
        private int _lootId;
        private int _pickpocketLoot;
        private int _skinLoot;
        private DynamicDataControl _resistances;
        private int _petDataId;
        private int _vehicleId;
        private Currency _minGold;
        private Currency _maxGold;
        private XmlKeyValue _aiName;
        private XmlKeyValue _movement;
        private BitmaskStackPanel _inhabit;
        private int _hoverHeight = 1;
        private float _healthModifier = 1f;
        private float _manaModifier = 1f;
        private float _damageModifier = 1f;
        private float _armorModifier = 1f;
        private float _experienceModifier = 1f;
        private bool _racialLeader;
        private bool _regenHealth = true;
        private BitmaskStackPanel _mechanicImmuneMask;
        private BitmaskStackPanel _flagsExtra;
        private int _mount;
        private BitmaskStackPanel _bytes1;
        private int _emote;
        private DynamicDataControl _auras;
        private DynamicDataControl _spells;
        private DynamicDataControl _difficultyEntry;
        private int _minDmg;
        private int _maxDmg;
        private int _weapon1;
        private int _weapon2;
        private int _weapon3;
        private int _minLevelHealth;
        private int _maxLevelHealth;
        private int _minLevelMana;
        private int _maxLevelMana;
        private int _minMeleeDmg;
        private int _maxMeleeDmg;
        private int _minRangedDmg;
        private int _maxRangedDmg;
        private int _armor;
        private int _meleeAttackPower;
        private int _rangedAttackPower;
        private bool _civilian;


        #region Properties
        public int Entry
        {
            get { return _entry; }
            set
            {
                _entry = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Entry");
            }
        }
        // difficulty_entry_1-3
        // KillCredit1-2
        public DynamicDataControl ModelIds
        {
            get
            {
                if (_modelIds == null)
                {
                    string[] fields = new string[] {"ModelId1","ModelId2","ModelId3","ModelId4"};
                    _modelIds = new DynamicDataControl(fields, 4, true, defaultValue: "0", valueMySqlDt: "mediumint(8)");
                }
                return _modelIds;
            }
            set
            {
                _modelIds = value;
                RaisePropertyChanged("ModelIds");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = DataType.LimitLength(value, 100);
                RaisePropertyChanged("Name");
            }
        }
        public string Subname
        {
            get { return _subname; }
            set
            {
                _subname = DataType.LimitLength(value, 100);
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
                _minLevel = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("MinLevel");
            }
        }
        public int MaxLevel
        {
            get { return _maxLevel; }
            set
            {
                _maxLevel = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("MaxLevel");
            }
        }
        //exp - expansion table, related to dmg
        public int Faction
        {
            get { return _faction; }
            set
            {
                _faction = DataType.LimitLength(value, "smallint(5)");
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
        public float SpeedWalk
        {
            get { return _speedWalk; }
            set
            {
                _speedWalk = value;
                RaisePropertyChanged("SpeedWalk");
            }
        }
        public float SpeedRun
        {
            get { return _speedRun; }
            set
            {
                _speedRun = value;
                RaisePropertyChanged("SpeedRun");
            }
        }
        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                RaisePropertyChanged("Scale");
            }
        }
        public XmlKeyValue Rank
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
                _baseAttackTime = DataType.LimitLength(value, "int(10)");
                RaisePropertyChanged("BaseAttackTime");
            }
        }
        public int RangeAttackTime
        {
            get { return _rangeAttackTime; }
            set
            {
                _rangeAttackTime = DataType.LimitLength(value, "int(10)");
                RaisePropertyChanged("RangeAttackTime");
            }
        }
        public XmlKeyValue _UnitClass
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
                {
                    _unitFlags = BitmaskStackPanel.GetUnitFlags();
                    _unitFlags.BmspChanged += UnitFlags_BmspChanged;
                }
                return _unitFlags;
            }
            set
            {
                _unitFlags = value;
                RaisePropertyChanged("UnitFlags");
            }
        }
        public BitmaskStackPanel UnitFlags2
        {
            get
            {
                if (_unitFlags2 == null)
                {
                    _unitFlags2 = BitmaskStackPanel.GetUnitFlags2();
                    _unitFlags2.BmspChanged += UnitFlags2_BmspChanged;
                }
                return _unitFlags2;
            }
            set
            {
                _unitFlags2 = value;
                RaisePropertyChanged("UnitFlags2");
            }
        }
        public BitmaskStackPanel DynamicFlags
        {
            get
            {
                if (_dynamicFlags == null)
                {
                    _dynamicFlags = BitmaskStackPanel.GetCreatureDynamicFlags();
                    _dynamicFlags.BmspChanged += DynamicFlags_BmspChanged;
                }
                return _dynamicFlags;
            }
            set
            {
                _dynamicFlags = value;
                RaisePropertyChanged("DynamicFlags");
            }
        }
        public CreatureFamily Family
        {
            get { return _family; }
            set
            {
                _family = value;
                RaisePropertyChanged("Family");
                PetDataId = value.PetSpellDataId;
            }
        }
        public TrainerData Trainer
        {
            get { return _trainer; }
            set
            {
                _trainer = value;
                RaisePropertyChanged("Trainer");
            }
        }
        public XmlKeyValue _CreatureType
        {
            get { return _creatureType; }
            set
            {
                _creatureType = value;
                RaisePropertyChanged("_CreatureType");
            }
        }
        public BitmaskStackPanel TypeFlags
        {
            get {
                if (_typeFlags == null)
                    _typeFlags = BitmaskStackPanel.GetCreatureTypeFlags();
                return _typeFlags;
            }
            set
            {
                _typeFlags = value;
                RaisePropertyChanged("TypeFlags");
            }
        }
        public int LootId
        {
            get { return _lootId; }
            set
            {
                _lootId = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("LootId");
            }
        }
        public int PickpocketLoot
        {
            get { return _pickpocketLoot; }
            set
            {
                _pickpocketLoot = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("PickpocketLoot");
            }
        }
        public int SkinLoot
        {
            get { return _skinLoot; }
            set
            {
                _skinLoot = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("SkinLoot");
            }
        }
        public DynamicDataControl Resistances
        {
            get
            {
                if (_resistances == null)
                    _resistances = new DynamicDataControl(DamageType.GetDamageTypes(true), 6, true, defaultValue: "0", valueMySqlDt: "mediumint(8)");
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
                if (_spells == null)
                {
                    string[] spellCols = new string[]
                       {
                           "Spell1","Spell2","Spell3","Spell4","Spell5","Spell6","Spell7","Spell8",
                       };
                    _spells = new DynamicDataControl(spellCols, 8, true, defaultValue: "0", valueMySqlDt: "smallint(6)");
                }
                return _spells;
            }
            set
            {
                _spells = value;
                RaisePropertyChanged("Spells");
            }
        }
        public int PetDataId
        {
            get { return _petDataId; }
            set
            {
                _petDataId = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("PetDataId");
            }
        }
        public int VehicleId
        {
            get { return _vehicleId; }
            set
            {
                _vehicleId = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("VehicleId");
            }
        }
        public Currency MinGold
        {
            get
            {
                if (_minGold == null)
                    _minGold = new Currency(0);
                return _minGold;
            }
            set
            {
                _minGold = value;
                RaisePropertyChanged("MinGold");
            }
        }
        public Currency MaxGold
        {
            get
            {
                if (_maxGold == null)
                    _maxGold = new Currency(0);
                return _maxGold;
            }
            set
            {
                _maxGold = value;
                RaisePropertyChanged("MaxGold");
            }
        }
        public XmlKeyValue AIName
        {
            get { return _aiName; }
            set
            {
                _aiName = value;
                RaisePropertyChanged("AIName");
            }
        }
        public XmlKeyValue Movement
        {
            get { return _movement; }
            set
            {
                _movement = value;
                RaisePropertyChanged("Movement");
            }
        }
        public BitmaskStackPanel Inhabit
        {
            get {
                if (_inhabit == null)
                    _inhabit = BitmaskStackPanel.GetInhabitTypes();
                return _inhabit; }
            set
            {
                _inhabit = value;
                RaisePropertyChanged("Inhabit");
            }
        }
        public int HoverHeight // requires MOVEMENTFLAG_DISABLE_GRAVITY
        {
            get { return _hoverHeight; }
            set
            {
                _hoverHeight = DataType.LimitLength(value, "smallint(4)");
                RaisePropertyChanged("HoverHeight");
            }
        }
        public float HealthModifier
        {
            get { return _healthModifier; }
            set
            {
                _healthModifier = value;
                RaisePropertyChanged("HealthModifier");
            }
        }
        public float ManaModifier
        {
            get { return _manaModifier; }
            set
            {
                _manaModifier = value;
                RaisePropertyChanged("ManaModifier");
            }
        }
        public float DamageModifier
        {
            get { return _damageModifier; }
            set
            {
                _damageModifier = value;
                RaisePropertyChanged("DamageModifier");
            }
        }
        public float ArmorModifier
        {
            get { return _armorModifier; }
            set
            {
                _armorModifier = value;
                RaisePropertyChanged("ArmorModifier");
            }
        }
        public float ExperienceModifier
        {
            get { return _experienceModifier; }
            set
            {
                _experienceModifier = value;
                RaisePropertyChanged("ExperienceModifier");
            }
        }
        public bool RacialLeader
        {
            get { return _racialLeader; }
            set
            {
                _racialLeader = value;
                RaisePropertyChanged("RacialLeader");
            }
        }
        public bool RegenHealth
        {
            get { return _regenHealth; }
            set
            {
                _regenHealth = value;
                RaisePropertyChanged("RegenHealth");
            }
        }
        public BitmaskStackPanel MechanicImmuneMask
        {
            get
            {
                if (_mechanicImmuneMask == null)
                    _mechanicImmuneMask = BitmaskStackPanel.GetMechanicImmuneMask();
                return _mechanicImmuneMask;
            }
            set
            {
                _mechanicImmuneMask = value;
                RaisePropertyChanged("MechanicImmuneMask");
            }
        }
        public BitmaskStackPanel FlagsExtra
        {
            get
            {
                if (_flagsExtra == null)
                    _flagsExtra = BitmaskStackPanel.GetCreatureFlagsExtra();
                return _flagsExtra;
            }
            set
            {
                _flagsExtra = value;
                RaisePropertyChanged("FlagsExtra");
            }
        }

        // Addon properties
        public int Mount
        {
            get { return _mount; }
            set
            {
                _mount = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Mount");
            }
        }
        public BitmaskStackPanel Bytes1
        {
            get
            {
                if (_bytes1 == null)
                    _bytes1 = BitmaskStackPanel.GetCreatureBytes1();
                return _bytes1;
            }
            set
            {
                _bytes1 = value;
                RaisePropertyChanged("Bytes1");
            }
        }
        public int Emote
        {
            get { return _emote; }
            set
            {
                _emote = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Emote");
            }
        }
        public DynamicDataControl Auras
        {
            get
            {
                if (_auras == null)
                    _auras = new DynamicDataControl(new string[] { "Spell" }, 999, defaultValue: "0", valueMySqlDt: "smallint(6)");
                return _auras;
            }
            set
            {
                _auras = value;
                RaisePropertyChanged("Auras");
            }
        }
        public int Exp // always 2
        {
            get { return 2; }
        }
        public DynamicDataControl DifficultyEntry
        {
            get
            {
                if (_difficultyEntry == null)
                    _difficultyEntry = new DynamicDataControl(new string[] { "DifficultyEntry1", "DifficultyEntry2", "DifficultyEntry3" }, 3, true, defaultValue: "0", valueMySqlDt: "smallint(6)");
                return _difficultyEntry;
            }
            set
            {
                _difficultyEntry = value;
                RaisePropertyChanged("Auras");
            }
        }
        public int MinDmg
        {
            get { return _minDmg; }
            set
            {
                _minDmg = DataType.LimitLength(value, "int(10)");
                RaisePropertyChanged("MinDmg");
            }
        }
        public int MaxDmg
        {
            get { return _maxDmg; }
            set
            {
                _maxDmg = DataType.LimitLength(value, "int(10)");
                RaisePropertyChanged("MaxDmg");
            }
        }
        public int Weapon1
        {
            get { return _weapon1; }
            set
            {
                _weapon1 = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Weapon1");
            }
        }

        public int Weapon2
        {
            get { return _weapon2; }
            set
            {
                _weapon2 = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Weapon2");
            }
        }

        public int Weapon3
        {
            get { return _weapon3; }
            set
            {
                _weapon3 = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Weapon3");
            }
        }

        public int MinLevelHealth
        {
            get { return _minLevelHealth; }
            set
            {
                _minLevelHealth = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MinLevelHealth");
            }
        }
        public int MaxLevelHealth
        {
            get { return _maxLevelHealth; }
            set
            {
                _maxLevelHealth = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MaxLevelHealth");
            }
        }
        public int MinLevelMana
        {
            get { return _minLevelMana; }
            set
            {
                _minLevelMana = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MinLevelMana");
            }
        }
        public int MaxLevelMana
        {
            get { return _maxLevelMana; }
            set
            {
                _maxLevelMana = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MaxLevelMana");
            }
        }
        public int MinMeleeDmg
        {
            get { return _minMeleeDmg; }
            set
            {
                _minMeleeDmg = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MinMeleeDmg");
            }
        }
        public int MaxMeleeDmg
        {
            get { return _maxMeleeDmg; }
            set
            {
                _maxMeleeDmg = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MaxMeleeDmg");
            }
        }
        public int MinRangedDmg
        {
            get { return _minRangedDmg; }
            set
            {
                _minRangedDmg = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MinRangedDmg");
            }
        }
        public int MaxRangedDmg
        {
            get { return _maxRangedDmg; }
            set
            {
                _maxRangedDmg = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MaxRangedDmg");
            }
        }
        public int Armor
        {
            get { return _armor; }
            set
            {
                _armor = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Armor");
            }
        }
        public int MeleeAttackPower
        {
            get { return _meleeAttackPower; }
            set
            {
                _meleeAttackPower = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("MeleeAttackPower");
            }
        }
        public int RangedAttackPower
        {
            get { return _rangedAttackPower; }
            set
            {
                _rangedAttackPower = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("RangedAttackPower");
            }
        }
        public bool Civilian
        {
            get { return _civilian; }
            set
            {
                _civilian = value;
                RaisePropertyChanged("Civilian");
            }
        }

        #endregion


        #region Logic
        private void UnitFlags_BmspChanged(object sender, EventArgs e)
        {
            BitmaskCheckBox bmcb = (BitmaskCheckBox)sender;
            if (bmcb.GetValue() == 536870912) // play dead sets feign dead
                if (bmcb.IsChecked == true)
                    UnitFlags2.SetValueIsChecked(1, true);
                else
                    UnitFlags2.SetValueIsChecked(1, false);
        }
        private void UnitFlags2_BmspChanged(object sender, EventArgs e)
        {
            BitmaskCheckBox bmcb = (BitmaskCheckBox)sender;
            if (bmcb.GetValue() == 1) // feign death sets play dead
                if (bmcb.IsChecked == true)
                    UnitFlags.SetValueIsChecked(536870912, true);
                else
                    UnitFlags.SetValueIsChecked(536870912, false);
        }
        private void DynamicFlags_BmspChanged(object sender, EventArgs e)
        {
            BitmaskCheckBox bmcb = (BitmaskCheckBox)sender;
            if (bmcb.GetValue() == 32 && bmcb.IsChecked == true) // Is dead checks sets tapped & grey name
            {
                DynamicFlags.SetValueIsChecked(8, true);
                DynamicFlags.SetValueIsChecked(4, true);
            }
        }
        #endregion

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));

            // Set modified
            var page = (CreatureCreatorPage)App._MainWindow.CreatureCreatorTab.Content;
            if (page != null && page.CanCheckForModified)
                page.IsCreatureModified = true;
        }
    }
}
