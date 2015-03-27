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
        public DataTemplate EndValueDoubleTemplate { get; set; }
        public DataTemplate EndValueSingleTemplate { get; set; }
        public DataTemplate EndValueLongTemplate { get; set; }
        public DataTemplate EndValueStringTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // Used in Genetic Optimization View
            var detailOptimizationParameter = item as OptimizationParameterDetail;

            if (detailOptimizationParameter != null)
            {
                if (detailOptimizationParameter.ParameterType == typeof(int))
                {
                    return EndValueIntegerTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(uint))
                {
                    detailOptimizationParameter.EndValue = Convert.ToInt32(detailOptimizationParameter.EndValue);
                    return EndValueUnsignedIntegerTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(float))
                {
                    detailOptimizationParameter.EndValue = Convert.ToSingle(detailOptimizationParameter.EndValue);
                    return EndValueSingleTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(decimal))
                {
                    return EndValueDecimalTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(double))
                {
                    return EndValueDoubleTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(long))
                {
                    return EndValueLongTemplate;
                }
            }

            // Used in Brute Force Optimization View
            var detailBruteForceParameter = item as BruteForceParameterDetail;

            if (detailBruteForceParameter != null)
            {
                if (detailBruteForceParameter.ParameterType == typeof(string))
                {
                    return EndValueStringTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(int))
                {
                    return EndValueIntegerTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(uint))
                {
                    detailBruteForceParameter.EndValue = Convert.ToInt32(detailBruteForceParameter.EndValue);
                    return EndValueUnsignedIntegerTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(float))
                {
                    detailBruteForceParameter.EndValue = Convert.ToSingle(detailBruteForceParameter.EndValue);
                    return EndValueSingleTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(decimal))
                {
                    return EndValueDecimalTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(double))
                {
                    return EndValueDoubleTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(long))
                {
                    return EndValueLongTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
