using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TradeHubGui.Converters
{
    class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts incoming Boolean parameter to Visibility object
        /// </summary>
        /// <param name="value">boolean input</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>System.Windows.Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool input = System.Convert.ToBoolean(value);

            if (input != null && input)
            {
                return System.Windows.Visibility.Visible;
            }

            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
