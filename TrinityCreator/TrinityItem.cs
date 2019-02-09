using System;
using System.Collections.Generic;
using System.ComponentModel;
using TrinityCreator.Database;
using TrinityCreator.Properties;

namespace TrinityCreator
{
    public class TrinityItem : INotifyPropertyChanged
    {
        public TrinityItem()
        {
            var espvars = Emulator.EmulatorHandler.SelectedEmulator.GetIdColumnName("Item");
            EntryId = SqlQuery.GetNextId(espvars.Item1, espvars.Item2);
        }

        private BitmaskStackPanel _allowedclass;
        private BitmaskStackPanel _allowedrace;
        private int _ammoType;
        private int _armor;
        private BitmaskStackPanel _bagFamily;
        private XmlKeyValue _binds;
        private int _block;
        private int _buyCount;
        private int _containerSlots;
        private Damage _damageInfo;
        private int _displayid;
        private int _durability;
        private int _entryid;
        private BitmaskStackPanel _flags;
        private BitmaskStackPanel _flagsExtra;
        private DynamicDataControl _gemSockets;
        private ItemInventoryType _inventoryType;
        private ItemClass _itemclass;
        private ItemSubClass _itemsubclass;
        private int _maxallowed;
        private int _minlevel;
        private string _name;
        private ItemQuality _quality;
        private string _quote;
        private DynamicDataControl _resistances;
        private XmlKeyValue _socketBonus;
        private int _stackable = 1;
        private DynamicDataControl _stat;
        private Currency _valuebuy;
        private Currency _valuesell;
        private int _statsCount;
        private int _itemLevel;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = DataType.LimitLength(value, 255);
                RaisePropertyChanged("Name");
            }
        }

        public string Quote
        {
            get { return _quote; }
            set
            {
                _quote = DataType.LimitLength(value, 255);
                RaisePropertyChanged("Quote");
            }
        }

        public ItemClass Class
        {
            get { return _itemclass; }
            set
            {
                _itemclass = value;
                RaisePropertyChanged("Class");

                // itemlevel for wep/armor
                if ((_itemclass.Id == 2 || _itemclass.Id == 4) && ItemLevel == 0)
                    ItemLevel = 1;
                else ItemLevel = 0;
            }
        }

        public ItemSubClass ItemSubClass
        {
            get { return _itemsubclass; }
            set
            {
                _itemsubclass = value;
                RaisePropertyChanged("ItemSubClass");
            }
        }

        public ItemQuality Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                RaisePropertyChanged("Quality");
            }
        }

        public int DisplayId
        {
            get { return _displayid; }
            set
            {
                _displayid = DataType.LimitLength(value, "mediumint(8)"); ;
                RaisePropertyChanged("DisplayId");
            }
        }

        public int EntryId
        {
            get
            {
                return _entryid;
            }
            set
            {
                _entryid = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("EntryId");
            }
        }

        public XmlKeyValue Binds
        {
            get { return _binds; }
            set
            {
                _binds = value;
                RaisePropertyChanged("Binds");
            }
        }

        public int MinLevel
        {
            get { return _minlevel; }
            set
            {
                _minlevel = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("MinLevel");
            }
        }

        public int MaxAllowed
        {
            get { return _maxallowed; }
            set
            {
                _maxallowed = DataType.LimitLength(value, "int(11)");
                RaisePropertyChanged("MaxAllowed");
            }
        }

        public BitmaskStackPanel AllowedClass
        {
            get
            {
                if (_allowedclass == null)
                    _allowedclass = BitmaskStackPanel.GetClassFlags(-1);
                return _allowedclass;
            }
            set
            {
                _allowedclass = value;
                RaisePropertyChanged("AllowedClass");
            }
        }

        public BitmaskStackPanel AllowedRace
        {
            get
            {
                if (_allowedrace == null)
                    _allowedrace = BitmaskStackPanel.GetRaceFlags(-1);
                return _allowedrace;
            }
            set
            {
                _allowedrace = value;
                RaisePropertyChanged("AllowedRace");
            }
        }

        public Currency ValueBuy
        {
            get
            {
                if (_valuebuy == null)
                    _valuebuy = new Currency(0);
                return _valuebuy;
            }
            set
            {
                _valuebuy = new Currency(DataType.LimitLength(value.Amount, "int(10)"));
                RaisePropertyChanged("ValueBuy");
            }
        }

        public Currency ValueSell
        {
            get
            {
                if (_valuesell == null)
                    _valuesell = new Currency(0);
                return _valuesell;
            }
            set
            {
                _valuesell = new Currency(DataType.LimitLength(value.Amount, "int(10)"));
                RaisePropertyChanged("ValueSell");
            }
        }

        public ItemInventoryType InventoryType
        {
            get { return _inventoryType; }
            set
            {
                _inventoryType = value;
                RaisePropertyChanged("InventoryType");
            }
        }

        public BitmaskStackPanel Flags
        {
            get
            {
                if (_flags == null)
                    _flags = BitmaskStackPanel.GetItemFlags();
                return _flags;
            }
            set
            {
                _flags = value;
                RaisePropertyChanged("Flags");
            }
        }

        public BitmaskStackPanel FlagsExtra
        {
            get
            {
                if (_flagsExtra == null)
                    _flagsExtra = BitmaskStackPanel.GetItemFlagsExtra();
                return _flagsExtra;
            }
            set
            {
                _flagsExtra = value;
                RaisePropertyChanged("FlagsExtra");
            }
        }

        public int BuyCount
        {
            get { return _buyCount; }
            set
            {
                _buyCount = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("BuyCount");
            }
        }

        public int Stackable
        {
            get { return _stackable; }
            set
            {
                _stackable = DataType.LimitLength(value, "int(11)");
                RaisePropertyChanged("Stackable");
            }
        }

        public int ContainerSlots
        {
            get { return _containerSlots; }
            set
            {
                _containerSlots = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("ContainerSlots");
            }
        }

        public Damage DamageInfo
        {
            get
            {
                if (_damageInfo == null)
                    _damageInfo = new Damage();
                return _damageInfo;
            }
            set
            {
                _damageInfo = value;
                RaisePropertyChanged("DamageInfo");
            }
        }

        public DynamicDataControl Resistances
        {
            get
            {
                if (_resistances == null)
                    _resistances = new DynamicDataControl(DamageType.GetDamageTypes(true), 6, true, defaultValue: "0", valueMySqlDt:"tinyint(3)");
                return _resistances;
            }
            set
            {
                _resistances = value;
                RaisePropertyChanged("Resistances");
            }
        }

        public int AmmoType
        {
            get { return _ammoType; }
            set
            {
                _ammoType = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("AmmoType");
            }
        }

        public int Durability
        {
            get { return _durability; }
            set
            {
                _durability = DataType.LimitLength(value, "smallint(5)"); ;
                RaisePropertyChanged("Durability");
            }
        }

        public DynamicDataControl GemSockets
        {
            get
            {
                if (_gemSockets == null)
                    _gemSockets = new DynamicDataControl(Socket.GetSocketList(), 3, false, "Socket Type", "Amount", "0", valueMySqlDt:"tinyint(1)");
                return _gemSockets;
            }
            set
            {
                _gemSockets = value;
                RaisePropertyChanged("GemSockets");
            }
        }

        public XmlKeyValue SocketBonus
        {
            get { return _socketBonus; }
            set
            {
                _socketBonus = value;
                RaisePropertyChanged("SocketBonus");
            }
        }

        public DynamicDataControl Stats
        {
            get
            {
                if (_stat == null)
                    _stat = new DynamicDataControl(XmlKeyValue.FromXml("Stat"), 10, false, "Stat", "Value", "0", "smallint(6)");
                return _stat;
            }
            set
            {
                _stat = value;
                RaisePropertyChanged("Stats");
            }
        }

        public int StatsCount
        {
            get { return _statsCount; }
            set
            {
                _statsCount = value;
                RaisePropertyChanged("StatsCount");
            }
        }

        public int Armor
        {
            get { return _armor; }
            set
            {
                _armor = DataType.LimitLength(value, "smallint(5)");
                RaisePropertyChanged("Armor");
            }
        }

        public int Block
        {
            get { return _block; }
            set
            {
                _block = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Block");
            }
        }


        public BitmaskStackPanel BagFamily
        {
            get
            {
                if (_bagFamily == null)
                    _bagFamily = BitmaskStackPanel.GetBagFamilies();
                return _bagFamily;
            }
            set { _bagFamily = value; }
        }

        public int ItemLevel
        {
            get { return _itemLevel; }
            set
            {
                _itemLevel = DataType.LimitLength(value, "smallint(3)");
                RaisePropertyChanged("ItemLevel");
            }
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