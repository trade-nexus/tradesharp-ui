using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace Resources.Converters
{
	public class ToolBarTypeToVisibilityConverter : IValueConverter
	{

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{

			if (value == null)
			{
				return Visibility.Collapsed;
			}
			if (value.Equals(parameter))
			{
				return Visibility.Visible;
			}
			else
			{
				return Visibility.Collapsed;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
