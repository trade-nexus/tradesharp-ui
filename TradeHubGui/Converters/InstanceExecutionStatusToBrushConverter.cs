using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.Converters
{
    public class InstanceExecutionStatusToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts instance execution status to SolidColorBrush for indication of status
        /// </summary>
        /// <param name="value">StrategyStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>System.Windows.Media.SolidColorBrush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                StrategyStatus status = (StrategyStatus)value;

                // Return certain brush depending on StrategyStatus
                if (status.Equals(StrategyStatus.Executed))
                {
                    return Application.Current.Resources["GreenBrush"];
                }
                else if (status.Equals(StrategyStatus.Executing))
                {
                    return Application.Current.Resources["BlueBrush"];
                }
                else if (status.Equals(StrategyStatus.None))
                {
                    return Application.Current.Resources["RedBrush"];
                }
            }

            // Initial state is Stopped, so the color is Red
            return Application.Current.Resources["RedBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
