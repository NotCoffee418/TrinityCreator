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
    public class WeaponSpeedToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                double r = (int)value / 1000.0;
                return r.ToString("0.00");
            }
            else return "INVALID";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
