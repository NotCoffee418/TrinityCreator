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
using TrinityCreator.Database;

namespace TrinityCreator.Tools.LootCreator
{
    /// <summary>
    /// Interaction logic for LootRowControl.xaml
    /// </summary>
    public partial class LootRowControl : UserControl, INotifyPropertyChanged
    {
        public LootRowControl()
        {
            InitializeComponent();
            DataContext = this;
        }


        public event EventHandler RemoveRequestEvent;
        public event PropertyChangedEventHandler PropertyChanged;
        private int _item;
        private float _chance = 100;
        private bool _questRequired;
        private int _minCount = 1;
        private int _maxCount = 1;

        public int Item
        {
            get { return _item; }
            set
            {
                _item = DataType.LimitLength(value, "mediumint(8)");
                RaisePropertyChanged("Item");
            }
        }
        // Reference - reference to another ID + item
        public float Chance
        {
            get { return _chance; }
            set
            {
                _chance = value;
                RaisePropertyChanged("Chance");
            }
        }
        public bool QuestRequired
        {
            get { return _questRequired; }
            set
            {
                _questRequired = value;
                RaisePropertyChanged("QuestRequired");
            }
        }
        public int MinCount
        {
            get { return _minCount; }
            set
            {
                if (value == 0)
                    value = 1;
                _minCount = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("MinCount");
            }
        }
        public int MaxCount
        {
            get { return _maxCount; }
            set
            {
                if (value == 0)
                    value = 1;
                _maxCount = DataType.LimitLength(value, "tinyint(3)");
                RaisePropertyChanged("MaxCount");
            }
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private void removeMeBtn_Click(object sender, RoutedEventArgs e)
        {
            RemoveRequestEvent(this, e);
        }
    }
}
