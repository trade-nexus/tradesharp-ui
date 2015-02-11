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
    public class OptimizationExecutionStatusToBrushConverter : IValueConverter
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
                OptimizationStatus status = (OptimizationStatus)value;

                // Return certain brush depending on StrategyStatus
                if (status.Equals(OptimizationStatus.Working))
                {
                    return Application.Current.Resources["BlueBrush"];
                }
                else if (status.Equals(OptimizationStatus.Completed))
                {
                    return Application.Current.Resources["GreenBrush"];
                }
                else if (status.Equals(OptimizationStatus.Stopped))
                {
                    return Application.Current.Resources["RedBrush"];
                }
                else if (status.Equals(OptimizationStatus.None))
                {
                    return Application.Current.Resources["GrayBrush"];
                }
            }

            // Initial state is None, so the color is Gray
            return Application.Current.Resources["GrayBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
