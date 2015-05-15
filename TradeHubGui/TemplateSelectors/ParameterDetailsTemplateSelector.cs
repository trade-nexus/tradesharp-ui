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
                    if (pair.Key.Equals("marketdataprovider", StringComparison.InvariantCultureIgnoreCase) 
                        || pair.Key.Equals("dataprovider", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return element.FindResource("MarketDataTemplate") as DataTemplate;
                    }
                    else if (pair.Key.Equals("orderexecutionprovider", StringComparison.InvariantCultureIgnoreCase)
                        || pair.Key.Equals("executionprovider", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return element.FindResource("OrderExecutionTemplate") as DataTemplate;
                    }
                    else
                    {
                        return element.FindResource("StringDataTemplate") as DataTemplate;
                    }
                }
                else if (pair.Value.ParameterType == typeof(int))
                {
                    return element.FindResource("IntegerDataTemplate") as DataTemplate;
                }
                else if (pair.Value.ParameterType == typeof(uint))
                {
                    if (pair.Value.ParameterValue != null)
                    {
                        int convertedValue;

                        // BOX - Unbox
                        if (Int32.TryParse(pair.Value.ParameterValue.ToString(), out convertedValue))
                        {
                            // Assign converted value
                            pair.Value.ParameterValue = convertedValue;
                        }
                    }

                    return element.FindResource("UnsignedIntegerDataTemplate") as DataTemplate;
                }
                else if (pair.Value.ParameterType == typeof(float))
                {
                    if (pair.Value.ParameterValue != null)
                    {
                        Single convertedValue;

                        // BOX - Unbox
                        if (Single.TryParse(pair.Value.ParameterValue.ToString(), out convertedValue))
                        {
                            // Assign converted value
                            pair.Value.ParameterValue = convertedValue;
                        }
                    }

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
                else if (pair.Value.ParameterType == typeof(long))
                {
                    return element.FindResource("LongDataTemplate") as DataTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
