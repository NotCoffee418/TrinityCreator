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
    /// Interaction logic for ProfileCreatorEntry.xaml
    /// </summary>
    public partial class ProfileCreatorEntry : UserControl, INotifyPropertyChanged
    {
        public ProfileCreatorEntry(string appKey, string tooltip = "")
        {
            InitializeComponent();
            DataContext = this;
            AppKey = appKey;
        }

        private string _appKey;
        private string _sqlKey;
        private string _tableName;
        private bool _isIncluded = true;

        public string AppKey 
        {
            get { return _appKey; }
            set
            {
                _appKey = value;
                RaisePropertyChanged("AppKey");
            }
        }
        public string SqlKey
        {
            get { return _sqlKey; }
            set
            {
                _sqlKey = value;
                RaisePropertyChanged("SqlKey");
            }
        }
        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                RaisePropertyChanged("TableName");
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
