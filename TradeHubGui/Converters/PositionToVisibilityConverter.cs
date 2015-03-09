using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TradeHubGui.Converters
{
    public class PositionToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts instance execution status to visibility of buttons Start and Stop
        /// </summary>
        /// <param name="value">StrategyStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">String as inversion indicator</param>
        /// <param name="culture">not used</param>
        /// <returns>System.Windows.Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                decimal position = Math.Abs(System.Convert.ToDecimal(value));

                // If Position is open 'Close' button is visible.
                if (position > 0)
                {
                    return System.Windows.Visibility.Visible;
                }
                // If Position is closed/not available 'Close' button is hidden.
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }

            // Default visibility for Start button is visible
            return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
