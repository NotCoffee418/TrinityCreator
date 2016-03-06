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
    /// Interaction logic for DynamicDataControl.xaml
    /// </summary>
    public partial class DynamicDataControl : UserControl
    {
        public DynamicDataControl(object[] keyOptions, int maxLines, bool unique = false, string header1="", string header2 = "")
        {
            InitializeComponent();
            MaxLines = maxLines;
            KeyOptions = keyOptions;
            AddHeaders(header1, header2);

            if (unique)
            {
                addLineBtn.Visibility = Visibility.Collapsed;
                foreach (object key in KeyOptions)
                    AddLine(key);
            }
            else AddLine();
        }

        public event EventHandler Changed;

        private int _maxLines;
        private object[] _keyOptions;
        private List<DockPanel> lines = new List<DockPanel>();
            
        public int MaxLines
        {
            get
            {
                return _maxLines;
            }
            set
            {
                _maxLines = value;
            }
        }
        public object[] KeyOptions
        {
            get
            {
                return _keyOptions;
            }
            set
            {
                _keyOptions = value;
            }
        }

        /// <summary>
        /// Returns the user input
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, string> GetUserInput()
        {
            var d = new Dictionary<object, string>();
            foreach (var line in lines)
            {
                ComboBox cb = (ComboBox)line.Children[0];
                TextBox tb = (TextBox)line.Children[1];

                if (tb.Text != "")
                    d.Add(cb.SelectedValue, tb.Text);
            }
            return d;
        }

        private void addLineBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lines.Count() < MaxLines)
                AddLine();
        }
        
        private void AddHeaders(string header1, string header2)
        {
            if (header1 != "" && header2 != "")
            {
                DockPanel dp = new DockPanel();
                dp.Margin = new Thickness(0, 2, 0, 2);

                Label l1 = new Label();
                l1.Content = header1;
                l1.Width = 150;
                dp.Children.Add(l1);

                Label l2 = new Label();
                l2.Content = header2;
                dp.Children.Add(l2);

                dynamicSp.Children.Add(dp);
            }
        }

        private void AddLine(object key = null)
        {
            DockPanel dp = new DockPanel();
            dp.Margin = new Thickness(0, 2, 0, 2);

            ComboBox cb = new ComboBox();
            if (key == null)
                cb.ItemsSource = KeyOptions;
            else
            {
                cb.Items.Add(key);
                cb.IsEnabled = false;
            }
            cb.SelectedIndex = 0;
            cb.Width = 150;
            cb.SelectionChanged += TriggerChangedEvent;
            dp.Children.Add(cb);

            TextBox tb = new TextBox();
            tb.Margin = new Thickness(5, 0, 0, 0);
            tb.TextChanged += TriggerChangedEvent;
            dp.Children.Add(tb);

            lines.Add(dp);
            dynamicSp.Children.Add(dp);
        }

        private void TriggerChangedEvent(object sender, EventArgs e)
        {
            Changed(sender, e);
        }
    }
}
