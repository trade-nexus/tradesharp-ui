using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Converters
{
    public class ServiceStatusToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts service status to Visibility. Used for enabling/disabling of 'Start' and 'Stop' service buttons from Service Configuration.
        /// </summary>
        /// <param name="value">StrategyStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>bool</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                ServiceStatus status = (ServiceStatus)value;

                // If parameter is Invert, then use inverted logic for button visibility
                if (parameter != null && ((string) parameter).Equals("Invert"))
                {
                    if (status.Equals(ServiceStatus.Running))
                    {
                        return System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        return System.Windows.Visibility.Collapsed;
                    }
                }
                else
                {
                    if (status.Equals(ServiceStatus.Stopped))
                    {
                        return System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        return System.Windows.Visibility.Collapsed;
                    }
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
