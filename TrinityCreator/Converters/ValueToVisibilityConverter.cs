using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TrinityCreator
{
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            else if (value is string && (string)value != "")
                flag = true;
            else if (value is int && (int)value != 0)
                flag = true;
            else if (value is long && (long)value != 0)
                flag = true;
            else if (value is double && (double)value != 0 && !double.IsNaN((double)value) && !double.IsInfinity((double)value))
                flag = true;

            return (flag ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
