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
using System.Windows.Data;
using System.Windows.Media;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.Converters
{
    public class InstanceExecutionStatusToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts instance execution status to bool. Used for enabling/disabling of 'Edit' and 'Delete' instance buttons from DataGrid.
        /// </summary>
        /// <param name="value">StrategyStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>bool</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                // If parameter is Invert, then use inverted logic for button visibility
                if (parameter != null && ((string) parameter).Equals("Invert"))
                {
                    StrategyStatus status = (StrategyStatus) value;

                    // Return 'False' only if status is 'None' or 'Initializing' indicating it's not yet executed.
                    if (status.Equals(StrategyStatus.None) || status.Equals(StrategyStatus.Initializing))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (value != DependencyProperty.UnsetValue)
                {
                    StrategyStatus status = (StrategyStatus) value;

                    // Return true only if status is Stopped or None
                    if (status.Equals(StrategyStatus.Stopped) || status.Equals(StrategyStatus.None))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
