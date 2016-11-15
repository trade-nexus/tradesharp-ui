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
    public class ProviderConnectionStatusToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts provider connection status to bool
        /// </summary>
        /// <param name="value">ConnectionStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>true if connected, false if disconneted</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                var status = (ConnectionStatus)value;

                // Return certain brush depending on Connection Status
                if (status.Equals(ConnectionStatus.Connected))
                {
                    return true;
                }
                else if (status.Equals(ConnectionStatus.Disconnected))
                {
                    return false;
                }
            }

            // default value
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
