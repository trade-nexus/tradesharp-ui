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
        public DataTemplate StartValueDoubleTemplate { get; set; }
        public DataTemplate StartValueSingleTemplate { get; set; }
        public DataTemplate StartValueLongTemplate { get; set; }
        public DataTemplate StartValueStringTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // Used in Genetic Optimization View
            var detailOptimizationParameter = item as OptimizationParameterDetail;

            if (detailOptimizationParameter != null)
            {
                if (detailOptimizationParameter.ParameterType == typeof(int))
                {
                    return StartValueIntegerTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(uint))
                {
                    detailOptimizationParameter.StartValue = Convert.ToInt32(detailOptimizationParameter.StartValue);
                    return StartValueUnsignedIntegerTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(float))
                {
                    detailOptimizationParameter.StartValue = Convert.ToSingle(detailOptimizationParameter.StartValue);
                    return StartValueSingleTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(decimal))
                {
                    return StartValueDecimalTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(double))
                {
                    return StartValueDoubleTemplate;
                }
                else if (detailOptimizationParameter.ParameterType == typeof(long))
                {
                    return StartValueLongTemplate;
                }
            }

            // Used in Brute Force Optimization View
            var detailBruteForceParameter = item as BruteForceParameterDetail;

            if (detailBruteForceParameter != null)
            {
                if (detailBruteForceParameter.ParameterType == typeof(string))
                {
                    return StartValueStringTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(int))
                {
                    return StartValueIntegerTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(uint))
                {
                    detailBruteForceParameter.ParameterValue = Convert.ToInt32(detailBruteForceParameter.ParameterValue);
                    return StartValueUnsignedIntegerTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(float))
                {
                    detailBruteForceParameter.ParameterValue = Convert.ToSingle(detailBruteForceParameter.ParameterValue);
                    return StartValueSingleTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(decimal))
                {
                    return StartValueDecimalTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(double))
                {
                    return StartValueDoubleTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(long))
                {
                    return StartValueLongTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
