using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class Damage : INotifyPropertyChanged
    {
        private int _minDamage;
        private int _maxDamage;
        private int _speed;
        private DamageType _type;

        public event PropertyChangedEventHandler PropertyChanged;

        public int MinDamage
        {
            get
            {
                return _minDamage;
            }
            set
            {
                _minDamage = value;
                RaisePropertyChanged("MinDamage");
                RaisePropertyChanged("Dps");
            }
        }

        public int MaxDamage
        {
            get
            {
                return _maxDamage;
            }
            set
            {
                _maxDamage = value;
                RaisePropertyChanged("MaxDamage");
                RaisePropertyChanged("Dps");
            }
        }

        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                RaisePropertyChanged("Speed");
                RaisePropertyChanged("Dps");
            }
        }

        public DamageType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
                RaisePropertyChanged("Dps");
            }
        }
        
        public string Dps
        {
            get
            {
                try
                {
                    double avgDmg = (MinDamage + MaxDamage) / 2.0;
                    double dps = avgDmg / (Speed / 1000.0);

                    string typeText = " ";
                    if (Type.Id != 0)
                        typeText = " " + Type.Description.ToLower() + " ";
                    return string.Format("({0}{1}damage per second)", dps.ToString("0.00"), typeText);
                }
                catch
                {
                    return "(INVALID damage per second)";
                }
            }
        }
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
