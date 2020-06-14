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
using TrinityCreator.Profiles;
using TrinityCreator.Tools.LookupTool;
using TrinityCreator.UI.UIElements;

namespace TrinityCreator.Tools.QuestCreator
{
    /// <summary>
    /// Interaction logic for QuestControl.xaml
    /// </summary>
    public partial class QuestPage : UserControl, INotifyPropertyChanged
    {
        public QuestPage()
        {
            InitializeComponent();
            DataContext = Quest;
        }


        private void PrepareQuestControl()
        {
            // QuestInfo
            questInfoCb.ItemsSource = XmlKeyValue.FromXml("QuestInfo");
            questInfoCb.SelectedIndex = 0;

            // RewardXpDifficulty
            rewardXpCb.ItemsSource = XmlKeyValue.FromXml("QuestXp");
            rewardXpCb.SelectedIndex = 0;

            // QuestStarter & Ender types
            string[] starterEnderTypes = new string[]
            {
                "creature",
                "gameobject",
            };
            questStarterTypeCb.ItemsSource = starterEnderTypes;
            questStarterTypeCb.SelectedIndex = 0;
            questEnderTypeCb.ItemsSource = starterEnderTypes;
            questEnderTypeCb.SelectedIndex = 0;

            // Enable profile-specific values
            SetProfileValues();
            Profile.ActiveProfileChangedEvent += Profile_ActiveProfileChangedEvent;
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


        bool firstLoad = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (firstLoad)
            {
                PrepareQuestControl();
                firstLoad = false;
            }
        }

        #region ChangedEvents


        private void questInfoCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XmlKeyValue qi = (XmlKeyValue)questInfoCb.SelectedValue;
            BitmaskStackPanel bmsp = (BitmaskStackPanel)questFlagsGb.Content;

            if (qi != null && (qi.Id == 88 || qi.Id == 89))
                bmsp.SetValueIsChecked(64, true);
            else
                bmsp.SetValueIsChecked(64, false);
        }
        
        private void questStarterTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Quest.QuestgiverType = (string)questStarterTypeCb.SelectedValue;
        }

        private void questEnderTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Quest.QuestCompleterType = (string)questEnderTypeCb.SelectedValue;
        }

        #endregion

        #region Click events
        private void exportDbBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SaveQuery.CheckDuplicateHandleOverride(Export.C.Quest, Quest.EntryId))
                {
                    string query = Export.Quest(this.Quest);
                    SaveQuery.ToDatabase(query);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void exportSqlBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = Export.Quest(this.Quest);
                SaveQuery.ToFile("Quest " + Quest.EntryId, query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void newQuestBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to discard this quest and clear the form?",
                "Discard quest", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Quest = new TrinityQuest();
                DataContext = null;
                DataContext = Quest;
                PrepareQuestControl();
            }
        }

        private void findSortBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.QuestSort;
        }

        private void findQuestBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Quest;
        }

        private void findCreatureBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Creature;
        }

        private void findGoBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.GameObject;
        }

        private void findItemBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Item;
        }

        private void findSpellBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Spell;
        }

        private void findMapBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Map;
        }

        private void findFactionBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Faction;
        }

        private void findTitleBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Title;
        }

        #endregion


        private void Profile_ActiveProfileChangedEvent(object sender, EventArgs e)
        {
            SetProfileValues();
        }

        private void SetProfileValues()
        {
            var c = Export.C.Quest; // to shorten if statements

            // Quest XP handling (modifier or raw)
            RewardXpDifficultyDp.Visibility = Profile.Active.VisibileIfKeyDefined(c, "RewardXpDifficulty");
            RewardXpRawDp.Visibility = Profile.Active.VisibileIfKeyDefined(c, "RewardXpRaw");
        }

    }
}
