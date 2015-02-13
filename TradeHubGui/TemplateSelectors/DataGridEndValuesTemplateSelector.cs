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
    public class DataGridEndValuesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EndValueIntegerTemplate { get; set; }
        public DataTemplate EndValueUnsignedIntegerTemplate { get; set; }
        public DataTemplate EndValueDecimalTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            OptimizationParameterDetail detail = item as OptimizationParameterDetail;

            if (detail != null)
            {
                if (detail.StartValue.GetType() == typeof(int))
                {
                    return EndValueIntegerTemplate;
                }
                else if (detail.StartValue.GetType() == typeof(uint))
                {
                    return EndValueUnsignedIntegerTemplate;
                }
                else if (detail.StartValue.GetType() == typeof(decimal) || detail.StartValue.GetType() == typeof(float) || detail.StartValue.GetType() == typeof(double))
                {
                    return EndValueDecimalTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
