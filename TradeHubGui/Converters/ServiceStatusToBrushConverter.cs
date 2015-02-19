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
    public class ServiceStatusToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts order status to SolidColorBrush
        /// </summary>
        /// <param name="value">ServiceStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>System.Windows.Media.SolidColorBrush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                var status = (ServiceStatus)value;

                // Return certain brush depending on OrderStatus
                if (status.Equals(ServiceStatus.Disabled))
                {
                    return Application.Current.Resources["GrayBrush"];
                }
                else if (status.Equals(ServiceStatus.Starting))
                {
                    return Application.Current.Resources["MediumTurquoiseBrush"];
                }
                else if (status.Equals(ServiceStatus.Running))
                {
                    return Application.Current.Resources["GreenBrush"];
                }
                else if (status.Equals(ServiceStatus.Stopping))
                {
                    return Application.Current.Resources["OrangeBrush"];
                }
                else if (status.Equals(ServiceStatus.Stopped))
                {
                    return Application.Current.Resources["RedBrush"];
                }
            }

            // default color is Gray
            return Application.Current.Resources["GrayBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
