using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace Resources.Converters
{
	public class FloatToGridLenghtConverter : IValueConverter
	{

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if ((float)value == 0)
			{
				return new GridLength(0, GridUnitType.Star);
			}
			else if (Math.Abs((float)value) / 100 < 0.1)
			{
				return new GridLength(0.1, GridUnitType.Star);
			}
			else
			{
				if (!float.IsNaN((float)value))
				{
					return new GridLength(Math.Abs((float)value) / 100, GridUnitType.Star);
				}
				else
				{
					return new GridLength(0, GridUnitType.Star);
				}
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
