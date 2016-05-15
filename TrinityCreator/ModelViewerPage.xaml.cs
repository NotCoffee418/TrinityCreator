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
using TrinityCreator.Database;

namespace TrinityCreator
{
    /// <summary>
    /// Interaction logic for ModelViewerPage.xaml
    /// </summary>
    public partial class ModelViewerPage : Page
    {
        public ModelViewerPage()
        {
            InitializeComponent();
            App.ModelViewer = this;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadModel();
        }

        public void LoadModel(int displayId = 0, string type = "")
        {
            if (displayId == 0)
            {
                try
                {
                    displayId = int.Parse(displayIdTxt.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid Display Id", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else displayIdTxt.Text = displayId.ToString();

            if (type == "")
            {
                if (displayTypeCb.SelectedIndex == -1)
                    displayTypeCb.SelectedIndex = 0;

                type = ((ComboBoxItem)displayTypeCb.SelectedValue).Content.ToString();
            }
            else displayTypeCb.SelectedValue = type;

            switch (type)
            {
                case "Item":
                    if (inputIdCb.SelectedIndex == 1)
                        displayId = SqlQuery.GetItemDisplayFromEntry(displayId);
                    mvBrowser.LoadUrl("http://www.wowhead.com/#modelviewer:4:13;" + displayId);
                    break;
                case "Creature": // input should be entry instead of display
                    if (Connection.IsConfigured())
                    {
                        if (inputIdCb.SelectedIndex == 0)
                            displayId = SqlQuery.GetCreatureIdFromDisplayId(displayId);
                        mvBrowser.LoadUrl("http://www.wowhead.com/npc=" + displayId + "/#modelviewer:10+0", true);
                    }
                    break;
            }
        }
    }
}
