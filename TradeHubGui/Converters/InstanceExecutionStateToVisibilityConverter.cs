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
    public class InstanceExecutionStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string state = (string)value;
                if (parameter != null && ((string)parameter).Equals("Invert"))
                {
                    if (state.Equals("Running"))
                    {
                        return System.Windows.Visibility.Visible;
                    }
                    else if (state.Equals("Stopped"))
                    {
                        return System.Windows.Visibility.Collapsed;
                    }
                }
                else
                {
                    if (state.Equals("Running"))
                    {
                        return System.Windows.Visibility.Collapsed;
                    }
                    else if (state.Equals("Stopped"))
                    {
                        return System.Windows.Visibility.Visible;
                    }
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
