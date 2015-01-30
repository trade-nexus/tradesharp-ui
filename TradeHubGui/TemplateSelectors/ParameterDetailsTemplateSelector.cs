using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TradeHubGui.Common;
using TradeHubGui.ViewModel;

namespace TradeHubGui.TemplateSelectors
{
    public class ParameterDetailsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            KeyValuePair<string, Type> pair = (KeyValuePair<string, Type>)item;
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                if(pair.Value == typeof(string))
                {
                    return element.FindResource("StringDataTemplate") as DataTemplate;
                }
                else if (pair.Value == typeof(decimal))
                {
                    return element.FindResource("DecimalDataTemplate") as DataTemplate;
                }
                else if(pair.Value == typeof(int))
                {
                    return element.FindResource("IntegerDataTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
