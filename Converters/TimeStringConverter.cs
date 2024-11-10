using System.Windows.Data;

namespace Labb3_CsProg_ITHS.NET.Converters
{
	[ValueConversion(typeof(uint), typeof(string))]
	public class TimeStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is uint time)
			{
				return time.ToString();
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is string str)
			{
				if(uint.TryParse(str, out uint result))
				{
					return result;
				}
			}
			return 0;
		}
	}
}