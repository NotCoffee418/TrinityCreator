using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class Currency : INotifyPropertyChanged
    {
        public Currency(string g, string s, string c)
        {
            StringsToCurrency(g, s, c);
        }
        public Currency(int amount)
        {
            string g = "", s = "", c = "", amountstr = amount.ToString();
            if (amountstr.Length > 4)
                g = amountstr.Substring(0, amountstr.Length - 4);
            if (amountstr.Length > 3)
                s += amountstr[amountstr.Length - 4];
            if (amountstr.Length > 2)
                s += amountstr[amountstr.Length - 3];
            if (amountstr.Length > 1)
                c += amountstr[amountstr.Length - 2];
            c += amountstr[amountstr.Length - 1];

            // Set values
            StringsToCurrency(g, s, c);
        }

        private int _gold;
        private int _silver;
        private int _copper;
        private int _amount;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Gold
        {
            get
            {
                return _gold;
            }
            set
            {
                _gold = value;
                RaisePropertyChanged("Gold");
                RaisePropertyChanged("Amount");
                UpdateAmount();
            }
        }

        public int Silver
        {
            get
            {
                return _silver;
            }
            set
            {
                _silver = value;
                RaisePropertyChanged("Silver");
                RaisePropertyChanged("Amount");
                UpdateAmount();
            }
        }
        public int Copper
        {
            get
            {
                return _copper;
            }
            set
            {
                _copper = value;
                RaisePropertyChanged("Copper");
                RaisePropertyChanged("Amount");
                UpdateAmount();
            }
        }

        /// <summary>
        /// integer value as it should be placed in the database
        /// </summary>
        public int Amount
        {
            get
            {
                return _amount;
            }
        }


        private void UpdateAmount()
        {
            StringsToCurrency(Gold.ToString(), Silver.ToString(), Copper.ToString());
        }

        private void StringsToCurrency(string g, string s, string c)
        {
            if (s.Count() > 2 || c.Count() > 2)
                throw new Exception("Silver and copper can only contain two numbers characters each.");
            if (g.Length == 0) g = "00";
            if (s.Length == 0) s = "00";
            if (c.Length == 0) c = "00";
            try
            {
                _gold = int.Parse(g);
                _silver = int.Parse(s);
                _copper = int.Parse(c);

                // add 0 for correct value on blank or single character values
                for (int i = s.Count(); s.Count() < 2; i++)
                    s = "0" + s;
                for (int i = c.Count(); c.Count() < 2; i++)
                    c = "0" + c;

                _amount = int.Parse(g.ToString() + s.ToString() + c.ToString());
            }
            catch
            {
                throw new Exception("Currency must be numeric.");
            }
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
