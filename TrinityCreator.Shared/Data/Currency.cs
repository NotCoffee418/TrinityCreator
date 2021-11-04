using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace TrinityCreator.Shared.Data
{
    public class Currency : INotifyPropertyChanged
    {
        private int _copper;
        private int _gold;
        private int _silver;
        private int _amount;

        public Currency(string g, string s, string c)
        {
            StringsToCurrency(g, s, c);
        }

        public Currency(int amount)
        {
            SetCurrencyFromAmount(amount);
        }

        public int Gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                UpdateAmount();
                RaisePropertyChanged("Gold");
                RaisePropertyChanged("Amount");
            }
        }

        public int Silver
        {
            get { return _silver; }
            set
            {
                _silver = value;
                UpdateAmount();
                RaisePropertyChanged("Silver");
                RaisePropertyChanged("Amount");
            }
        }

        public int Copper
        {
            get { return _copper; }
            set
            {
                _copper = value;
                UpdateAmount();
                RaisePropertyChanged("Copper");
                RaisePropertyChanged("Amount");
            }
        }

        /// <summary>
        ///     integer value as it should be placed in the database
        /// </summary>
        public int Amount {
            get { return _amount; }
            set {  
                _amount = value;
                SetCurrencyFromAmount(value);
                RaisePropertyChanged("Gold");
                RaisePropertyChanged("Silver");
                RaisePropertyChanged("Copper");
                RaisePropertyChanged("Amount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


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
                for (var i = s.Count(); s.Count() < 2; i++)
                    s = "0" + s;
                for (var i = c.Count(); c.Count() < 2; i++)
                    c = "0" + c;

                try
                {
                    _amount = int.Parse(g + s + c);
                }
                catch // too big, set max
                {
                    _gold = 214748;
                    _silver = 36;
                    _copper = 47;
                    _amount = int.MaxValue;
                }
            }
            catch
            {
                throw new Exception("Currency must be numeric.");
            }
        }

        private void SetCurrencyFromAmount(int amount)
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

        public override string ToString()
        {
            return Amount.ToString();
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}