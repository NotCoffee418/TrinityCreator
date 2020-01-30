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
    public partial class ProfileCreatorDefinition : UserControl, INotifyPropertyChanged
    {
        public ProfileCreatorDefinition(string settingKey, string tooltip = "")
        {
            InitializeComponent();
            DataContext = this;
            DefinitionKey = settingKey;
            Tooltip = tooltip;
        }

        private string _definitionKey;
        private string _definitionValue;
        private bool _isIncluded = true;

        public string DefinitionKey
        {
            get { return _definitionKey; }
            set
            {
                _definitionKey = value;
                RaisePropertyChanged("DefinitionKey");
            }
        }
        public string DefinitionValue
        {
            get { return _definitionValue; }
            set
            {
                _definitionValue = value;
                RaisePropertyChanged("DefinitionValue");
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
