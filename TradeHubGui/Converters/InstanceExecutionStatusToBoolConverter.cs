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
    public class InstanceExecutionStatusToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts instance execution status to bool. Used for enabling/disabling of 'Edit' and 'Delete' instance buttons from DataGrid.
        /// </summary>
        /// <param name="value">StrategyStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>bool</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                StrategyStatus status = (StrategyStatus)value;

                // Return true only if status is Stopped or None
                if (status.Equals(StrategyStatus.Stopped) || status.Equals(StrategyStatus.None))
                {
                    return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
