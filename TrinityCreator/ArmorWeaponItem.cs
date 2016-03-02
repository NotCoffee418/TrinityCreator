using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class ArmorWeaponItem : Item
    {
        
        
        private int _weaponequip;
        private int _weapontype;
        private bool _repairable;
        private int _durability;
        private double _weaponspeed;
        private int _armorblock;
        private int _armorarmor;
        private int _itemlevel;
        private int _stackable;
        private int _mindamage;
        private int _maxdamage;
        private List<Stat> _stats;
        private BonusStat _bonusstats;
        private List<Socket> _sockets;
        private SocketBonus _socketbonus;
    
        public ArmorWeaponItem()
        {

        }

        

        public int WeaponEquip
        {
            get
            {
                return _weaponequip;
            }
            set
            {
                _weaponequip = value;
            }
        }

        public int WeaponType
        {
            get
            {
                return _weapontype;
            }
            set
            {
                _weapontype = value;
            }
        }

        public bool Repairable
        {
            get
            {
                return _repairable;
            }
            set
            {
                _repairable = value;
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
        
        public double WeaponSpeed
        {
            get
            {
                return _weaponspeed;
            }
            set
            {
                _weaponspeed = value;
            }
        }

        public int ArmorBlock
        {
            get
            {
                return _armorblock;
            }
            set
            {
                _armorblock = value;
            }
        }

        public int ArmorArmor
        {
            get
            {
                return ArmorArmor;
            }
            set
            {
                _armorarmor = value;
            }
        }        

        public int ItemLevel
        {
            get
            {
                return _itemlevel;
            }
            set
            {
                _itemlevel = value;
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

        public int MinDamage
        {
            get
            {
                return _mindamage;
            }
            set
            {
                _mindamage = value;
            }
        }

        public int MaxDamage
        {
            get
            {
                return _maxdamage;
            }
            set
            {
                _maxdamage = value;
            }
        }

        public List<Stat> Stats
        {
            get
            {
                return _stats;
            }
            set
            {
                _stats = value;
            }
        }

        public BonusStat BonusStats
        {
            get
            {
                return _bonusstats;
            }
            set
            {
                _bonusstats = value;
            }
        }

        public List<Socket> Sockets
        {
            get
            {
                return _sockets;
            }
            set
            {
                _sockets = value;
            }
        }

        public SocketBonus SocketBonus
        {
            get
            {
                return _socketbonus;
            }
            set
            {
                _socketbonus = value;
            }
        }

        /// <summary>
        /// Generates keyvaluepairs of the database table name and value to insert
        /// </summary>
        /// <returns></returns>
        protected override Dictionary<string, string> GenerateQueryValues()
        {
            var kvplist = base.GenerateQueryValues();
            // todo add armor & weapon values

            return kvplist;
        }

        /// <summary>
        /// Generates SQL query for the item
        /// </summary>
        /// <returns></returns>
        public override string GenerateSqlQuery(Dictionary<string, string> kvplist = null)
        {
            return base.GenerateSqlQuery(GenerateQueryValues());
        }
    }
}
