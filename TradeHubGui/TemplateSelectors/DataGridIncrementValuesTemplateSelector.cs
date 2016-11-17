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
    public class DataGridIncrementValuesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IncrementIntegerTemplate { get; set; }
        public DataTemplate IncrementUnsignedIntegerTemplate { get; set; }
        public DataTemplate IncrementDecimalTemplate { get; set; }
        public DataTemplate IncrementDoubleTemplate { get; set; }
        public DataTemplate IncrementSingleTemplate { get; set; }
        public DataTemplate IncrementLongTemplate { get; set; }
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
                    detailBruteForceParameter.Increment = Convert.ToInt32(detailBruteForceParameter.Increment);
                    return IncrementUnsignedIntegerTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(float))
                {
                    detailBruteForceParameter.Increment = Convert.ToSingle(detailBruteForceParameter.Increment);
                    return IncrementSingleTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(decimal))
                {
                    return IncrementDecimalTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(double))
                {
                    return IncrementDoubleTemplate;
                }
                else if (detailBruteForceParameter.ParameterType == typeof(long))
                {
                    return IncrementLongTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
