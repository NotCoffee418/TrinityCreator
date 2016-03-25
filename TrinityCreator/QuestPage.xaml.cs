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

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for QuestPage.xaml
    /// </summary>
    public partial class QuestPage : Page, INotifyPropertyChanged
    {
        public QuestPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private MainWindow _mainWindow;

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddQuestBtn_Click(object sender, RoutedEventArgs e)
        {
            // Tab item content
            TabItem ti = new TabItem();
            QuestControl qc = new QuestControl();
            ti.Content = qc;

            // header binding
            Binding b = new Binding();
            b.Source = qc.Quest;
            b.Path = new PropertyPath("LogTitle");
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(ti, TabItem.HeaderProperty, b);

            // Add
            QuestTabControl.Items.Add(ti);
            UpdateQuestChain();
        }

        private void RemoveQuestBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void UpdateQuestChain()
        {
            foreach (var item in QuestTabControl.Items)
            {
                
            }
        }

    }
}
