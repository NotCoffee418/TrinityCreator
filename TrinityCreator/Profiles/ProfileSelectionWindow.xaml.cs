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
        }
    }
}
