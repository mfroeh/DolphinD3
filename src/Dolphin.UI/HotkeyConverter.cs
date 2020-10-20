using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Ui
{
	public class HotkeyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var hotkey = new Hotkey(value.ToString());

			return hotkey;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((Hotkey)value).ToString();
		}
	}
}