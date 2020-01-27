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

namespace TrinityCreator.Tools.ModelViewer
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

        public void LoadModel(int displayId = 0, string type = "", string idType = "")
        {
            // Sync with UI
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
                type = ((ComboBoxItem)displayTypeCb.SelectedValue).Content.ToString();
            else
            {
                switch (type)
                {
                    case "Item":
                        displayTypeCb.SelectedIndex = 0;
                        break;
                    case "Creature":
                        displayTypeCb.SelectedIndex = 1;
                        break;
                }
            }

            if (idType == "")
                idType = ((ComboBoxItem)inputIdCb.SelectedValue).Content.ToString();
            else
            {
                switch (idType)
                {
                    case "Display ID":
                        inputIdCb.SelectedIndex = 0;
                        break;
                    case "Entry ID":
                        inputIdCb.SelectedIndex = 1;
                        break;
                }
            }


            // load model
            switch (type)
            {
                case "Item":
                    if (idType == "Entry ID")
                        displayId = LookupQuery.GetItemDisplayFromEntry(displayId);
                    mvBrowser.LoadUrl("http://www.wowhead.com/#modelviewer:4:13;" + displayId, blankFirst:true);
                    break;
                case "Creature": // input should be entry instead of display
                    if (Connection.IsConfigured())
                    {
                        if (idType == "Display ID")
                            displayId = LookupQuery.GetCreatureIdFromDisplayId(displayId);
                        mvBrowser.LoadUrl("http://www.wowhead.com/npc=" + displayId + "/#modelviewer:10+0");
                    }
                    break;
            }
        }
    }
}
