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
using TrinityCreator.Tools.CreatureCreator.Templates;

namespace TrinityCreator.Tools.CreatureCreator
{
    /// <summary>
    /// Interaction logic for CreatureTemplatePage.xaml
    /// </summary>
    public partial class CreatureTemplateWindow : Window
    {
        public CreatureTemplateWindow()
        {
            InitializeComponent();
            if (openWindow != null)
                openWindow.Close();
            openWindow = this;
            Closing += CreatureTemplateWindow_Closing;

            templateListBox.ItemsSource = TemplateHandler.ListTemplateDescriptions();
            templateListBox.MouseDoubleClick += TemplateListBox_MouseDoubleClick;
            templateListBox.SelectedIndex = 0;       
        }

        public static CreatureTemplateWindow openWindow = null;

        private void CreatureTemplateWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            openWindow = null;
        }

        private void TemplateListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadSelectedTemplate();
        }

        private void loadTemplateBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadSelectedTemplate();
        }

        private void LoadSelectedTemplate()
        {
            string description = (string)templateListBox.SelectedValue;
            TemplateHandler.LoadTemplateByDescription(description);
            Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
