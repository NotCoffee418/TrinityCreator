using System;
using System.Collections.Generic;
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
using TrinityCreator.Emulator;

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for CreatureCreatorPage.xaml
    /// </summary>
    public partial class CreatureCreatorPage : Page
    {
        public CreatureCreatorPage()
        {
            InitializeComponent();
            DataContext = Creature;
            Loaded += CreatureCreatorPage_Loaded;
        }

        public CreatureCreatorPage(TrinityCreature creature) : this()
        {
            Creature = creature;
        }

        public bool CanCheckForModified = false; // protection for templates
        public bool IsCreatureModified = false;
        public bool FirstLoad = true;

        internal void PrepareCreaturePage()
        {
            PrepCb(rankCb, XmlKeyValue.FromXml("CreatureRank"));
            PrepCb(dmgSchoolCb, DamageType.GetDamageTypes());
            PrepCb(unitClassCb, XmlKeyValue.FromXml("UnitClass"));
            PrepCb(familyCb, CreatureFamily.GetCreatureFamilies());
            PrepCb(trainerCb, TrainerData.GetTrainerData());
            PrepCb(creatureTypeCb, XmlKeyValue.FromXml("CreatureType"));
            PrepCb(aiNameCb, XmlKeyValue.FromXml("AI"));
            PrepCb(movementCb, XmlKeyValue.FromXml("MovementType"));
        }

        private void PrepCb(ComboBox cb, IKeyValue[] src)
        {
            if (cb.ItemsSource == null)
                cb.ItemsSource = src;
            if (cb.SelectedIndex == -1)
                cb.SelectedIndex = 0;
        }

        private TrinityCreature _creature;
        public TrinityCreature Creature {
            get
            {
                if (_creature == null)
                    _creature = new TrinityCreature();
                return _creature;
            }
            set { _creature = value; }
        }



        #region Event Handlers
        private void CreatureCreatorPage_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareCreaturePage();
            CanCheckForModified = true;
            if (FirstLoad)
            {
                FirstLoad = false;
                ShowTemplateWindow();
            }

        }
        #endregion

        #region Button handlers
        private void exportDbBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = EmulatorHandler.GenerateQuery(Creature);
                ExportQuery.ToDatabase(query);
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
                string query = EmulatorHandler.GenerateQuery(Creature);
                ExportQuery.ToFile("Creature " + Creature.Entry, query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to generate query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowTemplateWindow();
        }
        private void findCreatureBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Creature;
        }

        private void findFactionBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Faction;
        }

        private void findEmoteBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Emotes;
        }

        private void createLootBtn_Click(object sender, RoutedEventArgs e)
        {
            App._MainWindow.LootCreatorTabItem.IsSelected = true;
        }
        private void findItemBtn_Click(object sender, RoutedEventArgs e)
        {
            App.LookupTool.SelectedTarget = LookupTool.Target.Item;
        }
        #endregion

        private void ShowTemplateWindow()
        {
            CreatureTemplateWindow ctw = new CreatureTemplateWindow();
            ctw.Show();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: Lazy fix, take this out when updating emulator system
            if (Properties.Settings.Default.emulator == 2) // Azerothcore
                MinMaxDmgDp.Visibility = Visibility.Visible;


            // Compatability
            if (!(Properties.Settings.Default.emulator == 0) && !(Properties.Settings.Default.emulator == 3)) // Azerothcore
                equipmentGb.Visibility = Visibility.Collapsed;


            // Emu support
            if (Properties.Settings.Default.emulator != 1) // not cmangos zero
                cMangosZeroDetailedStatsGb.Visibility = Visibility.Collapsed;
        }
    }
}
