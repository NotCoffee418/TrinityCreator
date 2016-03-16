using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TrinityCreator
{
    /// <summary>
    ///     Interaction logic for DynamicDataControl.xaml
    /// </summary>
    public partial class DynamicDataControl : UserControl
    {
        private readonly string _defaultValue;

        private readonly List<DockPanel> _lines = new List<DockPanel>();

        public DynamicDataControl(object[] keyOptions, int maxLines, bool showAll = false, string header1 = "",
            string header2 = "", string defaultValue = "")
        {
            InitializeComponent();
            MaxLines = maxLines;
            KeyOptions = keyOptions;
            AddHeaders(header1, header2);
            _defaultValue = defaultValue;

            if (showAll)
            {
                AddLineBtn.Visibility = Visibility.Collapsed;
                foreach (var key in KeyOptions)
                    AddLine(key);
            }
            else AddLine();
        }

        public int MaxLines { get; set; }

        public object[] KeyOptions { get; set; }

        public event EventHandler DynamicDataChanged = delegate { };

        /// <summary>
        ///     Returns the user input
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, string> GetUserInput()
        {
            var d = new Dictionary<object, string>();
            foreach (var line in _lines)
            {
                var cb = (ComboBox) line.Children[0];
                var tb = (TextBox) line.Children[1];

                if (tb.Text != "")
                    d.Add(cb.SelectedValue, tb.Text);
            }
            return d;
        }

        private void addLineBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_lines.Count() < MaxLines)
                AddLine();
        }

        private void AddHeaders(string header1, string header2)
        {
            if (header1 != "" && header2 != "")
            {
                var dp = new DockPanel();
                dp.Margin = new Thickness(0, 2, 0, 2);

                var l1 = new Label();
                l1.Content = header1;
                l1.Width = 150;
                HeaderDp.Children.Add(l1);

                var l2 = new Label();
                l2.Content = header2;
                HeaderDp.Children.Add(l2);
            }
        }

        private void AddLine(object key = null, string value = "")
        {
            var dp = new DockPanel();
            dp.Margin = new Thickness(0, 2, 0, 2);

            var cb = new ComboBox();
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

            var tb = new TextBox();
            tb.Margin = new Thickness(5, 0, 0, 0);
            tb.Text = _defaultValue;
            tb.TextChanged += TriggerChangedEvent;
            dp.Children.Add(tb);

            _lines.Add(dp);
            DynamicSp.Children.Add(dp);

            TriggerChangedEvent(this, new EventArgs());
        }


        private void removeLineBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_lines.Count > 1 && _lines.Count() <= MaxLines)
            {
                var lastindex = _lines.Count - 1;
                _lines.RemoveAt(lastindex);
                DynamicSp.Children.RemoveAt(lastindex);
                TriggerChangedEvent(this, new EventArgs());
            }
        }

        private void TriggerChangedEvent(object sender, EventArgs e)
        {
            DynamicDataChanged(sender, e);
        }
    }
}