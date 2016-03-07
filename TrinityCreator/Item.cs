using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class Item
    {
        private string _name;
        private string _quote;
        private ItemClass _itemclass;
        private ItemSubClass _itemsubclass;
        private ItemQuality _quality;
        private int _displayid;
        private int _entryid;
        private ItemBonding _binds;
        private int _minlevel;
        private int _maxallowed;
        private BitmaskStackPanel _allowedclass;
        private BitmaskStackPanel _allowedrace;
        private int _valuebuy;
        private int _valuesell;
        private ItemInventoryType _inventoryType;
        private BitmaskStackPanel _flags;
        private BitmaskStackPanel _flagsExtra;
        private int _buyCount;
        private int _stackable;
        private int _containerSlots;
        private Damage _damageInfo;
        private DynamicDataControl _resistances;
        private int _ammoType;
        private int _durability;
        private DynamicDataControl _gemSockets;
        private SocketBonus _socketBonus;
        private DynamicDataControl _stat;
        private int _armor;
        private int _block;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Quote
        {
            get
            {
                return _quote;
            }
            set
            {
                _quote = value;
            }
        }

        public ItemClass Class
        {
            get
            {
                return _itemclass;
            }
            set
            {
                _itemclass = value;
            }
        }

        public ItemSubClass ItemSubClass
        {
            get
            {
                return _itemsubclass;
            }
            set
            {
                _itemsubclass = value;
            }
        }

        public ItemQuality Quality
        {
            get
            {
                return _quality;
            }
            set
            {
                _quality = value;
            }
        }

        public int DisplayId
        {
            get
            {
                return _displayid;
            }
            set
            {
                _displayid = value;
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
                _entryid = value;
            }
        }

        public ItemBonding Binds
        {
            get
            {
                return _binds;
            }
            set
            {
                _binds = value;
            }
        }

        public int MinLevel
        {
            get
            {
                return _minlevel;
            }
            set
            {
                _minlevel = value;
            }
        }

        public int MaxAllowed
        {
            get
            {
                return _maxallowed;
            }
            set
            {
                _maxallowed = value;
            }
        }

        public BitmaskStackPanel AllowedClass
        {
            get
            {
                return _allowedclass;
            }
            set
            {
                _allowedclass = value;
            }
        }

        public BitmaskStackPanel AllowedRace
        {
            get
            {
                return _allowedrace;
            }
            set
            {
                _allowedrace = value;
            }
        }

        public int ValueBuy
        {
            get
            {
                return _valuebuy;
            }
            set
            {
                _valuebuy = value;
            }
        }

        public int ValueSell
        {
            get
            {
                return _valuesell;
            }
            set
            {
                _valuesell = value;
            }
        }

        public ItemInventoryType InventoryType
        {
            get
            {
                return _inventoryType;
            }
            set
            {
                _inventoryType = value;
            }
        }

        public BitmaskStackPanel Flags
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
            }
        }

        public BitmaskStackPanel FlagsExtra
        {
            get
            {
                return _flagsExtra;
            }
            set
            {
                _flagsExtra = value;
            }
        }

        public int BuyCount
        {
            get
            {
                return _buyCount;
            }
            set
            {
                _buyCount = value;
            }
        }

        public int Stackable
        {
            get
            {
                return _stackable;
            }
            set
            {
                _stackable = value;
            }
        }

        public int ContainerSlots
        {
            get
            {
                return _containerSlots;
            }
            set
            {
                _containerSlots = value;
            }
        }

        public Damage DamageInfo
        {
            get
            {
                return _damageInfo;
            }
            set
            {
                _damageInfo = value;
            }
        }

        public DynamicDataControl Resistances
        {
            get
            {
                return _resistances;
            }
            set
            {
                _resistances = value;
            }
        }

        public int AmmoType
        {
            get
            {
                return _ammoType;
            }
            set
            {
                _ammoType = value;
            }
        }

        public int Durability
        {
            get
            {
                return _durability;
            }
            set
            {
                _durability = value;
            }
        }

        public DynamicDataControl GemSockets
        {
            get
            {
                return _gemSockets;
            }
            set
            {
                _gemSockets = value;
            }
        }

        public SocketBonus SocketBonus
        {
            get
            {
                return _socketBonus;
            }
            set
            {
                _socketBonus = value;
            }
        }

        public DynamicDataControl Stats
        {
            get
            {
                return _stat;
            }
            set
            {
                _stat = value;
            }
        }

        public int Armor
        {
            get
            {
                return _armor;
            }
            set
            {
                _armor = value;
            }
        }
        public int Block
        {
            get
            {
                return _block;
            }
            set
            {
                _block = value;
            }
        }

        public BitmaskStackPanel BagFamily { get; internal set; }


        /// <summary>
        /// Generates keyvaluepairs of the database table name and value to insert
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GenerateQueryValues()
        {
            var kvplist = new Dictionary<string, string>();
            kvplist.Add("entry", EntryId.ToString());
            kvplist.Add("description", "'" + Quote + "'");
            kvplist.Add("class", Class.Id.ToString());
            kvplist.Add("subclass", ItemSubClass.Id.ToString());
            kvplist.Add("name", "'" + Name + "'");
            kvplist.Add("displayid", DisplayId.ToString());
            kvplist.Add("Quality", Quality.Id.ToString());
            kvplist.Add("bonding", Binds.Id.ToString());
            kvplist.Add("RequiredLevel", MinLevel.ToString());
            kvplist.Add("maxcount", MaxAllowed.ToString());
            kvplist.Add("AllowableClass", AllowedClass.BitmaskValue.ToString());
            kvplist.Add("AllowableRace", AllowedRace.BitmaskValue.ToString());
            kvplist.Add("BuyPrice", ValueBuy.ToString());
            kvplist.Add("SellPrice", ValueSell.ToString());
            kvplist.Add("InventoryType", InventoryType.Id.ToString());
            kvplist.Add("Material", ItemSubClass.Material.Id.ToString());
            kvplist.Add("sheath", InventoryType.Sheath.ToString());
            kvplist.Add("Flags", Flags.BitmaskValue.ToString());
            kvplist.Add("FlagsExtra", FlagsExtra.BitmaskValue.ToString());
            kvplist.Add("BuyCount", BuyCount.ToString());
            kvplist.Add("stackable", Stackable.ToString());
            kvplist.Add("ContainerSlots", ContainerSlots.ToString());
            kvplist.Add("dmg_min1", DamageInfo.MinDamage.ToString());
            kvplist.Add("dmg_max1", DamageInfo.MaxDamage.ToString());
            kvplist.Add("dmg_type1", DamageInfo.Type.Id.ToString());
            kvplist.Add("delay", DamageInfo.Speed.ToString());
            kvplist.Add("MaxDurability", Durability.ToString());

            // Add gem sockets
            int socketId = 1;
            foreach (var gem in GemSockets.GetUserInput())
            {
                if (gem.Value != "0" && gem.Value != "")
                {
                    try
                    {
                        Socket s = (Socket)gem.Key;
                        int sCount = int.Parse(s.Description); // validate

                        kvplist.Add("socketColor_" + socketId, s.Id.ToString());
                        kvplist.Add("socketContent_" + socketId, sCount.ToString());
                        socketId++;
                    }
                    catch
                    {
                        throw new Exception("Invalid gem socket data.");
                    }
                }
            }
            // Socket bonus
            kvplist.Add("socketBonus", SocketBonus.Id.ToString());


            // resistances
            try
            {
                // loops unique keys
                foreach (var kvp in Resistances.GetUserInput())
                {
                    DamageType type = (DamageType)kvp.Key;
                    int value = int.Parse(kvp.Value); // validate int
                    kvplist.Add(type.Description + "_res", value.ToString());
                }
            }
            catch
            {
                throw new Exception("Invalid value in magic resistance.");
            }

            kvplist.Add("ammo_type", AmmoType.ToString());

            // Stats
            try
            {
                int count = 1;
                foreach (var kvp in Stats.GetUserInput())
                {
                    Stat stat = (Stat)kvp.Key;
                    int value = int.Parse(kvp.Value); // validate int
                    kvplist.Add("stat_type" + count, stat.Id.ToString());
                    kvplist.Add("stat_value" + count, value.ToString());
                    count++;
                }
            }
            catch
            {
                throw new Exception("Invalid value in magic resistance.");
            }

            
            kvplist.Add("armor", Armor.ToString());     // armor
            kvplist.Add("block", Block.ToString());     // block
            kvplist.Add("BagFamily", BagFamily.BitmaskValue.ToString()); // bag family

            return kvplist;
        }

        /// <summary>
        /// Generates SQL query for the item
        /// </summary>
        /// <returns></returns>
        public string GenerateSqlQuery(Dictionary<string, string> kvplist = null)
        {
            string query1 = "INSERT INTO item_template (";
            string query2 = ") VALUES (";

            // get correct kvplist
            if (kvplist == null)
                kvplist = GenerateQueryValues();

            // generate query
            query1 += string.Join(", ", kvplist.Keys);
            query2 += string.Join(", ", kvplist.Values) + ");" + Environment.NewLine;

            return query1 + query2;
        }
    }
}
