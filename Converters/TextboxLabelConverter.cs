using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Labb3_CsProg_ITHS.NET.Converters
{
	[ValueConversion(typeof(string), typeof(Visibility))]
	public class TextboxLabelConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is string text)
			{
				if(text.Length == 0)
					return Visibility.Visible;
				else 
					return Visibility.Hidden;
			}
			else if(value is bool focused)
			{
				if (focused)
					return Visibility.Hidden;
				else if(parameter is TextBox box && box.Text.Length == 0 )return Visibility.Visible;
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return false;
		}
	}
}
