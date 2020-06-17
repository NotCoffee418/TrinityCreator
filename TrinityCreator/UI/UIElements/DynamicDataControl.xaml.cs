using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TrinityCreator.Data;

namespace TrinityCreator.UI.UIElements
{
    /// <summary>
    ///     Interaction logic for DynamicDataControl.xaml
    /// </summary>
    public partial class DynamicDataControl : UserControl
    {
        private List<DockPanel> _lines = new List<DockPanel>();
        public event EventHandler RemoveRequestEvent;

        /// <summary>
        /// DDC With Combobox (key) & TextBox (value)
        /// </summary>
        /// <param name="keyOptions"></param>
        /// <param name="maxLines"></param>
        /// <param name="showAll"></param>
        /// <param name="header1"></param>
        /// <param name="header2"></param>
        /// <param name="defaultValue"></param>
        /// <param name="valueMySqlDt">MySql max value</param>
        public DynamicDataControl(object[] keyOptions, int maxLines, bool showAll = false, string header1 = "",
            string header2 = "", string defaultValue = "", string valueMySqlDt = "")
        {
            ValueMySqlDt = valueMySqlDt;
            Prepare(maxLines, header1, header2, defaultValue);
            KeyOptions = keyOptions;
            IsTextboxKey = false;

            if (showAll)
            {
                AddLineBtn.Visibility = Visibility.Collapsed;
                foreach (var key in KeyOptions)
                    AddLine(key, false);
            }
            else AddLine(null, true);
        }

        /// <summary>
        /// DDC with TextBox as key & value
        /// </summary>
        /// <param name="maxLines"></param>
        /// <param name="header1"></param>
        /// <param name="header2"></param>
        /// <param name="defaultValue"></param>
        /// <param name="keyMySqlDt">MySql max value</param>
        /// <param name="valueMySqlDt">MySql max value</param>
        public DynamicDataControl(int maxLines, string header1 = "", string header2 = "", string defaultValue = "0", string keyMySqlDt = "", string valueMySqlDt = "")
        {
            KeyMySqlDt = keyMySqlDt;
            ValueMySqlDt = valueMySqlDt;
            Prepare(maxLines, header1, header2, defaultValue);
            IsTextboxKey = true;
            AddLine();
        }

        private void Prepare(int maxLines, string header1, string header2, string defaultValue)
        {
            InitializeComponent();

            MaxLines = maxLines;
            DefaultValue = defaultValue;

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

        private string DefaultValue { get; set; }
        private bool IsTextboxKey { get; set; }
        public int MaxLines { get; set; }
        public object[] KeyOptions { get; set; }
        public string KeyMySqlDt { get; private set; }
        public string ValueMySqlDt { get; private set; }

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
                if (IsTextboxKey)
                {
                    var tbKey = (TextBox)line.Children[0];
                    var tbValue = (TextBox)line.Children[1];
                    if (tbKey.Text != "" && tbValue.Text != "")
                        d.Add(tbKey.Text, tbValue.Text);
                }
                else
                {
                    var cb = (ComboBox)line.Children[0];
                    var tb = (TextBox)line.Children[1];

                    if (tb.Text != "")
                        d.Add(cb.SelectedValue, tb.Text);
                }
            }
            return d;
        }

        /// <summary>
        /// Adds dictionary SQL values for incrementing SQL fields
        /// </summary>
        /// <param name="d">Query dictionary reference</param>
        /// <param name="keyPrefix">field name without number</param>
        /// <param name="valuePrefix">field name without number</param>
        /// <param name="valueMultiplier">multiplies value by this before adding to dictionary</param>
        /// <returns></returns>
        public void AddValues(Dictionary<string,string> d, string keyPrefix, string valuePrefix, int valueMultiplier = 1)
        {
            try
            {
                int i = 0;
                foreach (KeyValuePair<object, string> line in GetUserInput())
                {
                    int key = 0;
                    if (IsTextboxKey) key = int.Parse((string)line.Key); // parse to validate
                    else key = ((IKeyValue)line.Key).Id;

                    int value = int.Parse(line.Value) * valueMultiplier;
                    i++;

                    if (value != 0) // only add if socketColorX is set
                    {
                        d.Add(keyPrefix + i, key.ToString());
                        d.Add(valuePrefix + i, value.ToString());
                    }
                }
            }
            catch
            {
                throw new Exception("Invalid data in " + keyPrefix + " or " + valuePrefix);
            }
        }
        /// <summary>
        /// Single valued DDC
        /// </summary>
        /// <param name="d"></param>
        public void AddValues(Dictionary<string, string> d, string keyPrefix = "")
        {
            foreach (KeyValuePair<object, string> line in GetUserInput())
                try
                {
                    string key = "";
                    if (keyPrefix == "")
                        key = (string)line.Key.ToString();
                    else
                        key = keyPrefix + ((IKeyValue)line.Key).Id;
                    d.Add(key, int.Parse(line.Value).ToString());
                }
                catch
                { throw new Exception("Invalid data in " + (string)line.Key); }
        }

        /// <summary>
        /// Seperated list of values in 1 field
        /// </summary>
        /// <param name="d"></param>
        /// <param name="keyPrefix"></param>
        public void AddValues(Dictionary<string, string> d, string sqlKey, char seperator)
        {
            string result = "";
            try
            {
                foreach (KeyValuePair<object, string> line in GetUserInput())
                {
                    int val = int.Parse(line.Value);
                    if (val != 0)
                        result += val + seperator;
                }

                if (result.Length > 0)
                    result.Substring(0, result.Length - 1);
                d.Add(sqlKey, result);
            }
            catch
            { throw new Exception("Invalid data in " + sqlKey); }
        }
        
        public int GetFirstValue()
        {
            return int.Parse(GetUserInput().First().Value);
        }

        private void addLineBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_lines.Count() < MaxLines)
                AddLine();
        }

        private void AddLine(object key = null, bool showRemoveBtn = true)
        {
            var dp = new DockPanel();
            dp.Margin = new Thickness(0, 2, 0, 2);

            if (IsTextboxKey)
            {
                TextBox tbKey = new TextBox();
                tbKey.Width = 150;
                tbKey.Text = DefaultValue;
                tbKey.LostFocus += TbKey_LostFocus;
                tbKey.Margin = new Thickness(5, 0, 0, 0);
                dp.Children.Add(tbKey);
            }
            else // combobox key
            {
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
                cb.Margin = new Thickness(5, 0, 0, 0);
                dp.Children.Add(cb);
            }

            // Add value textbox
            var tbValue = new TextBox();
            tbValue.Margin = new Thickness(5, 0, 0, 0);
            tbValue.Text = DefaultValue;
            tbValue.LostFocus += TbValue_LostFocus;
            tbValue.MinWidth = 140;
            dp.Children.Add(tbValue);


            // Add remove button
            if (showRemoveBtn)
            {
                // Prepare img
                Image removeImg = new Image();
                removeImg.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(
                    "pack://application:,,,/TrinityCreator;component/Resources/remove-icon.png", UriKind.Absolute));

                // Prepare button
                Button removeBtn = new Button();
                removeBtn.Width = 24;
                removeBtn.Height = 24;
                removeBtn.Content = removeImg;
                removeBtn.Margin = new Thickness(5, 0, 0, 0);
                removeBtn.HorizontalAlignment = HorizontalAlignment.Left;
                removeBtn.Click += RemoveBtn_Click;
                
                dp.Children.Add(removeBtn);
            }

            _lines.Add(dp);
            DynamicSp.Children.Add(dp);

            TriggerChangedEvent(this, new EventArgs());
        }

        private void TbKey_LostFocus(object sender, RoutedEventArgs e)
        {
            if (KeyMySqlDt != "")
                LimitValue((TextBox)sender, KeyMySqlDt);
            TriggerChangedEvent(sender, e);
        }

        private void TbValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ValueMySqlDt != "")
                LimitValue((TextBox)sender, ValueMySqlDt);
            TriggerChangedEvent(sender, e);
        }

        /// <summary>
        /// Limit int value
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="MySqlDt"></param>
        private void LimitValue(TextBox tb, string MySqlDt)
        {
            string oldTxt = tb.Text;
            string newTxt = "0";
            try {
                newTxt = Database.DataType.LimitLength(int.Parse(oldTxt), MySqlDt).ToString();
            }
            catch {
                newTxt = Database.DataType.LimitLength(2147483647, MySqlDt).ToString();
            }

            if (oldTxt != newTxt)
                tb.Text = newTxt;
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_lines.Count > 1 && _lines.Count <= MaxLines)
            {
                // I don't know either. I regret everything that happens in this class. :(
                var target = (DockPanel)((Button)sender).Parent;
                _lines.Remove(target);
                DynamicSp.Children.Remove(target);
                TriggerChangedEvent(this, new EventArgs());
            }
        }

        private void TriggerChangedEvent(object sender, EventArgs e)
        {
            DynamicDataChanged(sender, e);
        }
    }
}