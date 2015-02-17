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
    public class DataGridIncrementValuesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IncrementIntegerTemplate { get; set; }
        public DataTemplate IncrementUnsignedIntegerTemplate { get; set; }
        public DataTemplate IncrementDecimalTemplate { get; set; }
        public DataTemplate IncrementStringTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // Used in Brute Force Optimization View
            var detailBruteForceParameter = item as BruteForceParameterDetail;

            if (detailBruteForceParameter != null)
            {
                if (detailBruteForceParameter.ParameterType == typeof(string))
                {
                    return IncrementStringTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(int))
                {
                    return IncrementIntegerTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(uint))
                {
                    return IncrementUnsignedIntegerTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(decimal) || detailBruteForceParameter.ParameterType == typeof(float) || detailBruteForceParameter.ParameterType == typeof(double))
                {
                    return IncrementDecimalTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
