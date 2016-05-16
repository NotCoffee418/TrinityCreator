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
        internal void PrepareCreaturePage()
        {
            
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
            Creature.CanCheckForModified = true;
        }
        #endregion
    }
}
