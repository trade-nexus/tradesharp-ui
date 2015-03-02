using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Converters
{
    public class StrategyIsSelectedToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts Strategy IsSelected to SolidColorBrush
        /// </summary>
        /// <param name="value">IsSelected</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>System.Windows.Media.SolidColorBrush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                var isSelected = (bool)value;

                // Return certain brush
                if (isSelected)
                {
                    return Application.Current.Resources["BaseBrush3"];
                }
                else
                {
                    return Brushes.Transparent;
                }
            }

            // default color
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
