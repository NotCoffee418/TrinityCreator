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
using System.Windows.Shapes;

namespace TrinityCreator.Profiles
{
    /// <summary>
    /// Interaction logic for ProfileSelectionWindow.xaml
    /// </summary>
    public partial class ProfileSelectionWindow : Window
    {
        public ProfileSelectionWindow()
        {
            InitializeComponent();

            // Load & list profiles
            foreach (Profile p in Profile.ListProfiles())
                profileControlsSp.Children.Add(new ProfileSelectionControl(p));

            // ActiveProfile changed handler
            Profile.ActiveProfileChangedEvent += Profile_ActiveProfileChangedEvent;
        }

        private void Profile_ActiveProfileChangedEvent(object sender, EventArgs e)
        {
            // Store the local path of selected profile in Settings to remember between sessions
            Properties.Settings.Default.ActiveProfilePath = Profile.Active.LocalPath;
            Properties.Settings.Default.Save();

            // Close the window once a profile has been selected
            this.Close();
        }
    }
}
