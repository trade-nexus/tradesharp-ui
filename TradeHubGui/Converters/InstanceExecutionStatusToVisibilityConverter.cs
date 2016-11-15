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
    public class InstanceExecutionStatusToVisibilityConverter : IValueConverter
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
                StrategyStatus status = (StrategyStatus)value;

                // If parameter is Invert, then use inverted logic for button visibility
                if (parameter != null && ((string)parameter).Equals("Invert"))
                {
                    // If inverted logic and if strategy status is Executing or Executed, Start button is visible, Stop button is collapsed
                    if (status.Equals(StrategyStatus.Executing) || status.Equals(StrategyStatus.Executed))
                    {
                        return System.Windows.Visibility.Visible;
                    }
                    else if (status.Equals(StrategyStatus.None) || status.Equals(StrategyStatus.Stopped))
                    {
                        return System.Windows.Visibility.Collapsed;
                    }
                    else if (status.Equals(StrategyStatus.Initializing))
                    {
                        return System.Windows.Visibility.Visible;
                    }
                }
                else
                {
                    // If not inverted logic and if strategy status is Executing or Executed, Start button is collapsed, Stop button is visible
                    if (status.Equals(StrategyStatus.Executing) || status.Equals(StrategyStatus.Executed))
                    {
                        return System.Windows.Visibility.Collapsed;
                    }
                    else if (status.Equals(StrategyStatus.None) || status.Equals(StrategyStatus.Stopped))
                    {
                        return System.Windows.Visibility.Visible;
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
