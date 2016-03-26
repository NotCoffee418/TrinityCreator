using System.ComponentModel;

namespace TrinityCreator
{
    public class Damage : INotifyPropertyChanged
    {
        private int _maxDamage;
        private int _minDamage;
        private int _speed;
        private DamageType _type;

        public int MinDamage
        {
            get { return _minDamage; }
            set
            {
                _minDamage = value;
                RaisePropertyChanged("MinDamage");
                RaisePropertyChanged("Dps");
                RaisePropertyChanged("DpsInfo");
            }
        }

        public int MaxDamage
        {
            get { return _maxDamage; }
            set
            {
                _maxDamage = value;
                RaisePropertyChanged("MaxDamage");
                RaisePropertyChanged("Dps");
                RaisePropertyChanged("DpsInfo");
            }
        }

        public int Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                RaisePropertyChanged("Speed");
                RaisePropertyChanged("Dps");
                RaisePropertyChanged("DpsInfo");
            }
        }

        public DamageType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
                RaisePropertyChanged("Dps");
                RaisePropertyChanged("DpsInfo");
            }
        }

        public double Dps
        {
            get
            {
                var avgDmg = (MinDamage + MaxDamage)/2.0;
                return avgDmg/(Speed/1000.0);
            }
        }

        public string DpsInfo
        {
            get
            {
                try
                {
                    return "(" + Dps.ToString("0.00") + " damage per second)";
                }
                catch
                {
                    return "(INVALID damage per second)";
                }
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