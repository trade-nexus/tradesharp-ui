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
                        || pair.Key.Equals("dataprovider", StringComparison.InvariantCultureIgnoreCase)
                        || pair.Key.ToLowerInvariant().Contains("dataprovider"))
                    {
                        return element.FindResource("MarketDataTemplate") as DataTemplate;
                    }
                    else if (pair.Key.Equals("orderexecutionprovider", StringComparison.InvariantCultureIgnoreCase)
                        || pair.Key.Equals("executionprovider", StringComparison.InvariantCultureIgnoreCase)
                        || pair.Key.ToLowerInvariant().Contains("executionprovider"))
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
