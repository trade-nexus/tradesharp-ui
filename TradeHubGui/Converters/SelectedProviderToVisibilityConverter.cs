using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TradeHubGui.Common.Models;

namespace TradeHubGui.Converters
{
    public class SelectedProviderToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts selected provider to visibility. If no selected provider, returns Visibility.Collapsed, otherwise returns Visibility.Visible
        /// </summary>
        /// <param name="value">SelectedProvider</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>System.Windows.Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Provider selectedProvider = value as Provider;

            if(selectedProvider != null)
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
