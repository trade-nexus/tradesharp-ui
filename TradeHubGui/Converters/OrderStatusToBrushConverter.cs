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
    public class OrderStatusToBrushConverter : IValueConverter
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

                // Return certain brush depending on OrderStatus
                if (status.Equals(OrderStatus.CANCELLED))
                {
                    return Application.Current.Resources["GrayBrush"];
                }
                else if (status.Equals(OrderStatus.SUBMITTED))
                {
                    return Application.Current.Resources["BlueBrush"];
                }
                else if (status.Equals(OrderStatus.EXECUTED))
                {
                    return Application.Current.Resources["GreenBrush"];
                }
                else if (status.Equals(OrderStatus.PARTIALLY_EXECUTED))
                {
                    return Application.Current.Resources["OrangeBrush"];
                }
                else if (status.Equals(OrderStatus.OPEN))
                {
                    return Application.Current.Resources["BlueBrush"];
                }
                else if (status.Equals(OrderStatus.REJECTED))
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
