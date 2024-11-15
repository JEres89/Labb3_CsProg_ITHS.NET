using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Labb3_CsProg_ITHS.NET.ViewModels;

namespace Labb3_CsProg_ITHS.NET.Converters
{
	// TODO: rename to something like VisibilityConverter
	public class ImplicitConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
		{
			if(targetType != typeof(Visibility) || value is null) return null;

			//if(value is null)
			//{
			//	switch(parameter)
			//	{
			//		case Visibility v:
			//			return v;

			//		case Type t when t == typeof(ConfigurationViewModel):
			//			ConfigurationViewModel asd = null;
			//			return (Visibility)asd;

			//		default:
			//			break;
			//	}

			//	//if(parameter is Type t && t == typeof(ConfigurationViewModel))
			//	//{
			//	//	ConfigurationViewModel asd = null;
			//	//	return (Visibility)asd;
			//	//}
			//}
			switch(parameter)
			{
				case null:
					break;

				case Visibility v:
					return v;

				default: break;
			}
			switch(value)
			{
				case ConfigurationViewModel target:
					return (Visibility)target;

				default:
					return null!;
			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null!;
		}
	}
}
