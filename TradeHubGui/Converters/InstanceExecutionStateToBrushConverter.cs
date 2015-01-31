using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TradeHubGui.Converters
{
    public class InstanceExecutionStateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                string state = (string)value;
                if (state.Equals("Running"))
                {
                    return Application.Current.Resources["GreenBrush"];
                }
                else if (state.Equals("Stopped"))
                {
                    return Application.Current.Resources["RedBrush"];
                }
            }

            return Brushes.Silver;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
