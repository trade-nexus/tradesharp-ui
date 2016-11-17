/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
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
                    if (detailBruteForceParameter.ParameterValue != null)
                    {
                        int convertedValue;

                        // BOX - Unbox
                        if (Int32.TryParse(detailBruteForceParameter.ParameterValue.ToString(), out convertedValue))
                        {
                            // Assign converted value
                            detailBruteForceParameter.ParameterValue = convertedValue;
                        }
                    }

                    return StartValueUnsignedIntegerTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(float))
                {
                    if (detailBruteForceParameter.ParameterValue != null)
                    {
                        Single convertedValue;

                        // BOX - Unbox
                        if (Single.TryParse(detailBruteForceParameter.ParameterValue.ToString(), out convertedValue))
                        {
                            // Assign converted value
                            detailBruteForceParameter.ParameterValue = convertedValue;
                        }
                    }

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
