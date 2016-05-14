using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;

namespace TrinityCreator
{
    class TrinityCreature : INotifyPropertyChanged
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



        #endregion

        #region Generate Query
        public string GenerateSqlQuery()
        {
            return SqlQuery.GenerateInsert("creature_template", GenerateTemplateValues()) +
                SqlQuery.GenerateInsert("creature_template_addon", GenerateAddonValues()) +
                SqlQuery.GenerateInsert("creature_equip_template", GenerateEquipTemplateValues());
        }
        
        private Dictionary<string, string> GenerateTemplateValues()
        {
            throw new NotImplementedException();

            // BaseVariance & RangeVariance always 1
        }
        private Dictionary<string, string> GenerateAddonValues()
        {
            throw new NotImplementedException();
        }
        private Dictionary<string, string> GenerateEquipTemplateValues()
        {
            throw new NotImplementedException();
        }
        #endregion

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
