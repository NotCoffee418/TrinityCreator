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
        private ItemQuality _quality;
        private int _displayid;
        private int _entryid;
        private int _binds;
        private int _minlevel;
        private int _maxallowed;
        private int _allowedclass;
        private int _allowedrace;
        private int _valuesell;
        private int _valuebuy;

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

        public int Binds
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
                return MaxAllowed;
            }
            set
            {
                _maxallowed = value;
            }
        }

        public int AllowedClass
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

        public int AllowedRace
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
    }
}
