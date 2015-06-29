using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.Converters
{
    public class OrderStatusToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts order status to SolidColorBrush
        /// </summary>
        /// <param name="value">OrderStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>System.Windows.Media.SolidColorBrush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                string status = (string)value;

                // If status is Executed/Partially Executed return visible.
                if (status.Equals(OrderStatus.EXECUTED) || status.Equals(OrderStatus.PARTIALLY_EXECUTED))
                {
                    return System.Windows.Visibility.Visible;
                }
                // Return Hidden for all other options
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }

            // Default visibility is Hidden
            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
