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

        public bool CanCheckForModified = false; // protection for templates
        public bool IsCreatureModified = false;

        internal void PrepareCreaturePage()
        {
            PrepCb(rankCb, CreatureRank.GetCreatureRanks());
            PrepCb(dmgSchoolCb, DamageType.GetDamageTypes());
            PrepCb(unitClassCb, UnitClass.GetUnitClasses());
            PrepCb(familyCb, CreatureFamily.GetCreatureFamilies());
            PrepCb(trainerCb, TrainerData.GetTrainerData());
            PrepCb(creatureTypeCb, CreatureType.GetCreatureTypes());
            PrepCb(aiNameCb, AI.GetCreatureAI());
            PrepCb(movementCb, MovementType.GetMovementTypes());
            PrepCb(inhabitCb, InhabitType.GetInhabitTypes());
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
            App._MainWindow.CreatureTemplate.IsSelected = true;
        }
        #endregion
    }
}
