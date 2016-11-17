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


﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessageBoxUtils;
using Spring.Context.Support;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.ApplicationSecurity;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;

namespace TradeHubGui.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private RelayCommand _showDataApiConfigurationCommand;
        private RelayCommand _showOrderApiConfigurationCommand;
        private RelayCommand _showServicesConfigurationCommand;
        
        private ProvidersViewModel _providersViewModel;
        private ServicesViewModel _servicesViewModel;

        private MarketDataController _marketDataController;
        private OrderExecutionController _orderExecutionController;

        /// <summary>
        /// Constructors
        /// </summary>
        public DashboardViewModel()
        {
            _marketDataController = ContextRegistry.GetContext()["MarketDataController"] as MarketDataController;
            _orderExecutionController = ContextRegistry.GetContext()["OrderExecutionController"] as OrderExecutionController;

            _providersViewModel = new ProvidersViewModel();
            _servicesViewModel = new ServicesViewModel();

            EventSystem.Subscribe<string>(OnApplicationClose);
        }

        #region Properties

        /// <summary>
        /// Collection of market data providers for displaying on Dashboard
        /// </summary>
        public ObservableCollection<MarketDataProvider> MarketDataProviders
        {
            get { return _providersViewModel.MarketDataProviders; }
        }

        /// <summary>
        /// Collection of order execution providers for displaying on Dashboard
        /// </summary>
        public ObservableCollection<OrderExecutionProvider> OrderExecutionProviders
        {
            get { return _providersViewModel.OrderExecutionProviders; }
        }

        /// <summary>
        /// Collection for Application Services to be displayed on Dashboard
        /// </summary>
        public ObservableCollection<ServiceDetails> Services
        {
            get { return _servicesViewModel.Services; }
        }

        #endregion

        #region Commands

        public ICommand ShowDataApiConfigurationCommand
        {
            get
            {
                return _showDataApiConfigurationCommand ?? (_showDataApiConfigurationCommand = new RelayCommand(param => ShowDataApiConfigurationExecute()));
            }
        }

        public ICommand ShowOrderApiConfigurationCommand
        {
            get
            {
                return _showOrderApiConfigurationCommand ?? (_showOrderApiConfigurationCommand = new RelayCommand(param => ShowOrderApiConfigurationExecute()));
            }
        }

        public ICommand ShowServicesConfigurationCommand
        {
            get
            {
                return _showServicesConfigurationCommand ?? (_showServicesConfigurationCommand = new RelayCommand(param => ShowServicesConfigurationExecute()));
            }
        }

        #endregion

        private void ShowDataApiConfigurationExecute()
        {
            if(ToggleFlyout(0))
            {
                (MainWindow.Flyouts.Items[0] as Flyout).DataContext = _providersViewModel;
            }
        }

        private void ShowOrderApiConfigurationExecute()
        {
            if (ToggleFlyout(1))
            {
                (MainWindow.Flyouts.Items[1] as Flyout).DataContext = _providersViewModel;
            }
        }

        private void ShowServicesConfigurationExecute()
        {
            if (ToggleFlyout(2))
            {
                (MainWindow.Flyouts.Items[2] as Flyout).DataContext = _servicesViewModel;
            }
        }

        private object GetMarketDataProviders()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when application is closing
        /// </summary>
        /// <param name="message"></param>
        private void OnApplicationClose(string message)
        {
            if (message.Equals("Close"))
            {
                _marketDataController.Stop();
                _orderExecutionController.Stop();
            }
        }

    }
}
