using System;
using System.Windows.Data;

namespace Resources.Converters
{
	/// <summary>
	/// A odd value to visibility converter singleton
	/// </summary>
	public class OddToVisibilityConverter : IValueConverter
	{
		#region Singleton Implementation

		private static OddToVisibilityConverter instance = new OddToVisibilityConverter();
		private int i = 0;

		private OddToVisibilityConverter()
		{
		}

		/// <summary>
		/// The converter instance
		/// </summary>
		public static OddToVisibilityConverter Instance
		{
			get
			{
				return instance;
			}
		}

		#endregion

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (i == (int.MaxValue - 1))
			{
				i = 0;
			}

			i++;
			if (i % 2 == 0)
			{
				// it's even
				return System.Windows.Visibility.Collapsed;
			}

			// it's odd
			return System.Windows.Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
