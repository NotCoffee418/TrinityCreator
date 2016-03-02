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
        private ItemBonding _binds;
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

        public void SetBuyPrice(string g, string s, string c)
        {
            ValueBuy = StringToCurrency(g, s, c);
        }
        public void SetSellPrice(string g, string s, string c)
        {
            ValueSell = StringToCurrency(g, s, c);
        }
        private int StringToCurrency(string g, string s, string c)
        {
            if (s.Count() > 2 || c.Count() > 2)
                throw new Exception("Silver and copper can only contain two numbers characters each.");
            try
            {
                // add 0 for correct value on blank or single character values
                for (int i = s.Count(); s.Count() < 2; i++)
                    s = "0" + s;
                for (int i = c.Count(); c.Count() < 2; i++)
                    c = "0" + c;

                return int.Parse(g.ToString() + s.ToString() + c.ToString());
            }
            catch
            {
                throw new Exception("Currency must be numeric.");
            }
        }


        /// <summary>
        /// Generates keyvaluepairs of the database table name and value to insert
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, string> GenerateQueryValues()
        {
            var kvplist = new Dictionary<string, string>();
            kvplist.Add("entry", EntryId.ToString());
            kvplist.Add("name", "'" + Name + "'");
            kvplist.Add("description", "'" + Quote + "'");
            kvplist.Add("Quality", Quality.Id.ToString());
            kvplist.Add("displayid", DisplayId.ToString());
            kvplist.Add("bonding", Binds.Id.ToString());
            kvplist.Add("RequiredLevel", MinLevel.ToString());
            kvplist.Add("maxcount", MaxAllowed.ToString());
            kvplist.Add("AllowableClass", AllowedClass.ToString());
            kvplist.Add("AllowableRace", AllowedRace.ToString());
            kvplist.Add("BuyPrice", ValueBuy.ToString());
            kvplist.Add("SellPrice", ValueSell.ToString());

            return kvplist;
        }

        /// <summary>
        /// Generates SQL query for the item
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateSqlQuery(Dictionary<string, string> kvplist = null)
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
