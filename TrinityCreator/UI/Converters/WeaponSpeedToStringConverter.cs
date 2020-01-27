using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TrinityCreator.UI.Converters
{
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class WeaponSpeedToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var r = (int) value/1000.0;
                return r.ToString("0.00");
            }
            return "INVALID";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}