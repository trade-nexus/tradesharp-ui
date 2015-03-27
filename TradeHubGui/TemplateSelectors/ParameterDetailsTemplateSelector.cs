using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TradeHubGui.Common;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.ViewModel;

namespace TradeHubGui.TemplateSelectors
{
    public class ParameterDetailsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            KeyValuePair<string, ParameterDetail> pair = (KeyValuePair<string, ParameterDetail>)item;
            FrameworkElement element = container as FrameworkElement;
            
            if (element != null && item != null)
            {
                if(pair.Value.ParameterType == typeof(string))
                {
                    return element.FindResource("StringDataTemplate") as DataTemplate;
                }
                else if (pair.Value.ParameterType == typeof(int))
                {
                    return element.FindResource("IntegerDataTemplate") as DataTemplate;
                }
                else if (pair.Value.ParameterType == typeof(uint))
                {
                    pair.Value.ParameterValue = Convert.ToInt32(pair.Value.ParameterValue);
                    return element.FindResource("UnsignedIntegerDataTemplate") as DataTemplate;
                }
                else if (pair.Value.ParameterType == typeof(float))
                {
                    pair.Value.ParameterValue = Convert.ToSingle(pair.Value.ParameterValue);
                    return element.FindResource("SingleDataTemplate") as DataTemplate;
                }
                else if (pair.Value.ParameterType == typeof(decimal))
                {
                    return element.FindResource("DecimalDataTemplate") as DataTemplate;
                }
                else if (pair.Value.ParameterType == typeof(double))
                {
                    return element.FindResource("DoubleDataTemplate") as DataTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
