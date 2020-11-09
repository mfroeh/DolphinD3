using System;
using System.Windows.Data;

namespace Dolphin.Ui
{
    public class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value is bool boolean ? !boolean : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value is bool boolean ? !boolean : value;
        }
    }
}