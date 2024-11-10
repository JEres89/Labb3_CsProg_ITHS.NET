using Labb3_CsProg_ITHS.NET.Models;
using System.Windows.Data;

namespace Labb3_CsProg_ITHS.NET.Converters
{
	[ValueConversion(typeof(Difficulty), typeof(double))]

	public class DifficultyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is Difficulty diff)
			{
				return (double)diff;
			}
			return 0.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is double dbl)
			{
				return (Difficulty)dbl;
			}
			return Difficulty.Easy;
		}
	}
}