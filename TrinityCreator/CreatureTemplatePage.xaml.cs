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
using TrinityCreator.CreatureTemplates;

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for CreatureTemplatePage.xaml
    /// </summary>
    public partial class CreatureTemplatePage : Page
    {
        public CreatureTemplatePage()
        {
            InitializeComponent();
            templateListBox.ItemsSource = TemplateHandler.ListTemplateDescriptions();
            templateListBox.MouseDoubleClick += TemplateListBox_MouseDoubleClick;
            templateListBox.SelectedIndex = 0;       
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
        }
    }
}
