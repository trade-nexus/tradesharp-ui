using System;
using System.Windows.Data;

namespace Resources.Converters
{
	public class DoubleFormatConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.Format("{0:0.000}", (double)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.Empty;
		}

		#endregion
	}
}
