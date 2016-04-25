using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Interaction logic for QuestControl.xaml
    /// </summary>
    public partial class QuestControl : UserControl, INotifyPropertyChanged
    {
        public QuestControl()
        {
            InitializeComponent();
            DataContext = Quest;

            // Prepare controls
        }


        private void PrepareQuestControl()
        {
            // QuestInfo
            questInfoCb.ItemsSource = QuestInfo.ListQuestInfo();
            questInfoCb.SelectedIndex = 0;

            // QUestSort
            questsortCb.ItemsSource = QuestSort.ListQuestSort();
            questsortCb.SelectedIndex = 0;


        }

        TrinityQuest _quest;

        public event PropertyChangedEventHandler PropertyChanged;

        public TrinityQuest Quest
        {
            get
            {
                if (_quest == null)
                    _quest = new TrinityQuest();
                return _quest;
            }
            set { _quest = value; }
        }

        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareQuestControl();
        }

        #region ChangedEvents


        private void questInfoCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                QuestInfo qi = (QuestInfo)questInfoCb.SelectedValue;
                BitmaskStackPanel bmsp = (BitmaskStackPanel)questFlagsGb.Content;

                if (qi.Id == 88 || qi.Id == 89)
                    bmsp.SetValueIsChecked(64, true);
                else
                    bmsp.SetValueIsChecked(64, false);
            }
            catch { /*fail on load*/ }
        }


        private void questsortCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                QuestSort sort = (QuestSort)questsortCb.SelectedValue;
                if (sort.Id == 0)
                    sortZoneDock.Visibility = Visibility.Visible;
                else
                    sortZoneDock.Visibility = Visibility.Collapsed;
            }
            catch { }
        }

        #endregion

        #region Click events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            string query = Quest.GenerateSqlQuery();
            var sfd = new SaveFileDialog();
            sfd.DefaultExt = ".sql";
            sfd.FileName = "Item " + Quest.EntryId;
            sfd.Filter = "SQL File (.sql)|*.sql";
            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, query);

                // Increase next item's entry id
                Properties.Settings.Default.nextid_item = Quest.EntryId + 1;
                Properties.Settings.Default.Save();

                MessageBox.Show("Your item has been saved.", "Complete", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            /*}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }

        private void findSortBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.Target = "Find quest sort";
            App._MainWindow.ShowLookupTool();
        }
        #endregion
    }
}
