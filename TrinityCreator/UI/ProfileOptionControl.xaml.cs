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
using TrinityCreator.Profiles;

namespace TrinityCreator.UI
{
    /// <summary>
    /// Interaction logic for ProfileSelectionControl.xaml
    /// </summary>
    public partial class ProfileSelectionControl : UserControl
    {
        public ProfileSelectionControl(Profile profile)
        {
            InitializeComponent();
            _Profile = profile;
            DataContext = profile;
            HighlightOnActive();

            // Hide link if not available
            if (profile.UpdateUrl == null || profile.UpdateUrl == string.Empty)
                linkSp.Visibility = Visibility.Collapsed;

            Profile.ActiveProfileChangedEvent += Profile_ActiveProfileChangedEvent;
        }

        public Profile _Profile { get; private set; }


        /// <summary>
        /// Open url if it's valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Uri uriResult;
            bool valid = Uri.TryCreate(e.Uri.ToString(), UriKind.Absolute, out uriResult)
                && uriResult.Scheme == Uri.UriSchemeHttp;
            if (valid) 
                System.Diagnostics.Process.Start(uriResult.ToString());
        }

        private void activateBrn_Click(object sender, RoutedEventArgs e)
        {
            Profile.Active = _Profile;
        }
        
        private void Profile_ActiveProfileChangedEvent(object sender, EventArgs e)
        {
            HighlightOnActive();
        }

        private void HighlightOnActive()
        {
            if (_Profile.Equals(Profile.Active))
            {
                // todo: Change background color if this is the active profile, have changed eventhandler on Profile.Active to run this
                this.Background = Brushes.LightBlue;
            }
            else this.Background = Brushes.White;
        }
    }
}
