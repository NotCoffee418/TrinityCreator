using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrinityCreator.Tools.ProfileCreator
{
    /// <summary>
    /// Interaction logic for ProfileCreatorSetting.xaml
    /// </summary>
    public partial class ProfileCreatorSetting : UserControl, INotifyPropertyChanged
    {
        public ProfileCreatorSetting(string settingKey, string tooltip = "")
        {
            InitializeComponent();
            DataContext = this;
            SettingKey = settingKey;
            Tooltip = tooltip;
        }

        private string _settingKey;
        private string _settingValue;
        private bool _isIncluded = true;

        public string SettingKey
        {
            get { return _settingKey; }
            set
            {
                _settingKey = value;
                RaisePropertyChanged("SettingKey");
            }
        }
        public string SettingValue
        {
            get { return _settingValue; }
            set
            {
                _settingValue = value;
                RaisePropertyChanged("SettingValue");
            }
        }
        public bool IsIncluded
        {
            get { return _isIncluded; }
            set
            {
                _isIncluded = value;
                RaisePropertyChanged("IsIncluded");
            }
        }
        public string Tooltip { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
