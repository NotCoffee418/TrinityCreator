using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TrinityCreator.UI.Converters
{
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BoolInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
