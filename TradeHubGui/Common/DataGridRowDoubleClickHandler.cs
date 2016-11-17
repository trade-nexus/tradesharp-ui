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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;

namespace TradeHubGui.Common
{
    public class DataGridRowDoubleClickHandler : FrameworkElement
    {
        public DataGridRowDoubleClickHandler(DataGrid dataGrid)
        {
            MouseButtonEventHandler handler = (sender, args) =>
            {
                var row = sender as DataGridRow;
                if (row != null && row.IsSelected)
                {
                    var handlerName = GetHandler(dataGrid);

                    var dataContextType = dataGrid.DataContext.GetType();
                    var method = dataContextType.GetMethod(handlerName);
                    if (method == null)
                    {
                        PropertyInfo commandPI = dataContextType.GetProperty(handlerName);
                        if (commandPI != null &&
                            (commandPI.PropertyType == typeof(ICommand)))
                        {

                            ICommand command = (ICommand)commandPI.GetValue(dataGrid.DataContext, null);
                            if (command != null && command.CanExecute(null))
                            {
                                command.Execute(null);
                            }
                        }
                        else
                        {
                            throw new MissingMethodException(handlerName);
                        }
                    }
                    else
                    {
                        method.Invoke(dataGrid.DataContext, null);
                    }
                }
            };

            dataGrid.LoadingRow += (s, e) =>
                {
                    e.Row.MouseDoubleClick += handler;
                };

            dataGrid.UnloadingRow += (s, e) =>
                {
                    e.Row.MouseDoubleClick -= handler;
                };
        }

        public static string GetHandler(DataGrid dataGrid)
        {
            return (string)dataGrid.GetValue(HandlerProperty);
        }

        public static void SetHandler(DataGrid dataGrid, string value)
        {
            dataGrid.SetValue(HandlerProperty, value);
        }

        public static readonly DependencyProperty HandlerProperty = DependencyProperty.RegisterAttached(
            "Handler",
            typeof(string),
            typeof(DataGridRowDoubleClickHandler),
            new PropertyMetadata((o, e) =>
            {
                var dataGrid = o as DataGrid;
                if (dataGrid != null)
                {
                    new DataGridRowDoubleClickHandler(dataGrid);
                }
            }));
    }
}
