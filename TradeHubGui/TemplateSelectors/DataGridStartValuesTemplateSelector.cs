using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TradeHubGui.Common.ValueObjects;

namespace TradeHubGui.TemplateSelectors
{
    public class DataGridStartValuesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StartValueIntegerTemplate { get; set; }
        public DataTemplate StartValueUnsignedIntegerTemplate { get; set; }
        public DataTemplate StartValueDecimalTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            OptimizationParameterDetail detail = item as OptimizationParameterDetail;

            if (detail != null)
            {
                if (detail.ParameterType == typeof(int))
                {
                    return StartValueIntegerTemplate;
                }
                else if (detail.ParameterType == typeof(uint))
                {
                    return StartValueUnsignedIntegerTemplate;
                }
                else if (detail.ParameterType == typeof(decimal) || detail.ParameterType == typeof(float) || detail.ParameterType == typeof(double))
                {
                    return StartValueDecimalTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
