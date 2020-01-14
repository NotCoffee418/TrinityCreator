using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TrinityCreator.UI.Converters
{
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            if (value is bool)
            {
                flag = (bool) value;
            }
            else if (value is bool?)
            {
                var nullable = (bool?) value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            else if (value is string && (string) value != "")
                flag = true;
            else if (value is int && (int) value != 0)
                flag = true;
            else if (value is long && (long) value != 0)
                flag = true;
            else if (value is double && (double) value != 0 && !double.IsNaN((double) value) &&
                     !double.IsInfinity((double) value))
                flag = true;

            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}