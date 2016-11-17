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
using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.Dashboard.Services;
using TradeHubGui.Views;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;

namespace TradeHubGui.ViewModel
{
    public class MarketScannerViewModel : BaseViewModel
    {
        #region Fields
        private NewMarketScannerWindow _newMarketScannerWindow;
        private ObservableCollection<MarketDataProvider> _marketDataProviders;
        private MarketDataProvider _selectedMarketDataProvider;
        private ObservableCollection<MarketScannerWindowViewModel> _scannerWindowViewModels;

        private RelayCommand _showNewScannerWindowCommand;
        private RelayCommand _createScannerWindowCommand;
        private RelayCommand _closeScannerWindowCommand;
        private RelayCommand _focusScannerWindowCommand;
        private Dispatcher _currentDispatcher;

        #endregion

        #region Constructor
        public MarketScannerViewModel()
        {
            _currentDispatcher=Dispatcher.CurrentDispatcher;
            _scannerWindowViewModels = new ObservableCollection<MarketScannerWindowViewModel>();
            
            EventSystem.Subscribe<AlertMessage>(DisplayAlertMessage);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Collection of market data providers
        /// </summary>
        public ObservableCollection<MarketDataProvider> MarketDataProviders
        {
            get { return _marketDataProviders; }
            set
            {
                if (_marketDataProviders != value)
                {
                    _marketDataProviders = value;
                    OnPropertyChanged("MarketDataProviders");
                }
            }
        }

        /// <summary>
        /// Selected market data provider
        /// </summary>
        public MarketDataProvider SelectedMarketDataProvider
        {
            get { return _selectedMarketDataProvider; }
            set
            {
                if (_selectedMarketDataProvider != value)
                {
                    _selectedMarketDataProvider = value;
                    OnPropertyChanged("SelectedMarketDataProvider");
                }
            }
        }

        /// <summary>
        /// Collection of ViewModels for displaying infos about scanner windows
        /// </summary>
        public ObservableCollection<MarketScannerWindowViewModel> ScannerWindowViewModels
        {
            get { return _scannerWindowViewModels; }
            set
            {
                if (_scannerWindowViewModels != value)
                {
                    _scannerWindowViewModels = value;
                    OnPropertyChanged("ScannerWindowViewModels");
                }
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Opens 'New Market Scanner' window for chosing market data provider for scanning
        /// </summary>
        public ICommand ShowNewScannerWindowCommand
        {
            get
            {
                return _showNewScannerWindowCommand ?? (_showNewScannerWindowCommand = new RelayCommand(param => ShowNewScannerWindowExecute()));
            }
        }

        /// <summary>
        /// Creating of new market scanner window for chosen market data provider
        /// </summary>
        public ICommand CreateScannerWindowCommand
        {
            get
            {
                return _createScannerWindowCommand ?? (_createScannerWindowCommand = new RelayCommand(param => CreateScannerWindowExecute(), param => CreateScannerWindowCanExecute(param)));
            }
        }

        /// <summary>
        /// Focus certain market scanner window
        /// </summary>
        public ICommand FocusScannerWindowCommand
        {
            get
            {
                return _focusScannerWindowCommand ?? (_focusScannerWindowCommand = new RelayCommand(param => FocusScannerWindowExecute(param)));
            }
        }

        /// <summary>
        /// Close certain market scanner window
        /// </summary>
        public ICommand CloseScannerWindowCommand
        {
            get
            {
                return _closeScannerWindowCommand ?? (_closeScannerWindowCommand = new RelayCommand(param => CloseScannerWindowExecute(param)));
            }
        }

        #endregion

        #region Methods

        private void ShowNewScannerWindowExecute()
        {
            _newMarketScannerWindow = new NewMarketScannerWindow();
            _newMarketScannerWindow.Owner = MainWindow;
            _newMarketScannerWindow.DataContext = this;

            // Populate MarketDataProviders with actual list of market data providers
            InitializeMarketDataProviders();

            // Show 'New Market Scanner' window
            _newMarketScannerWindow.ShowDialog();
        }

        /// <summary>
        /// If selected market data provider is not null, return true
        /// </summary>
        private bool CreateScannerWindowCanExecute(object param)
        {
            if (param != null)
                return true;

            return false;
        }

        /// <summary>
        /// Create market scanner for selected market data provider
        /// </summary>
        private void CreateScannerWindowExecute()
        {
            // Try to find scanner window if already created for selected provider
            MarketScannerWindow scannerWindow = (MarketScannerWindow)FindWindowByTitle(SelectedMarketDataProvider.ProviderName);

            // if scanner is already created, just activate it, otherwise create new scanner window for slected data provider
            if (scannerWindow != null)
            {
                scannerWindow.WindowState = WindowState.Normal;
                scannerWindow.Activate();
            }
            else
            {
                scannerWindow = new MarketScannerWindow();
                MarketScannerWindowViewModel scannerWindowViewModel = new MarketScannerWindowViewModel(scannerWindow, SelectedMarketDataProvider, MarketDataProviders);

                // Add scanner window VeiwModel in collection for displaying on Market Scanner Dashboard
                ScannerWindowViewModels.Add(scannerWindowViewModel);

                scannerWindow.DataContext = scannerWindowViewModel;
                scannerWindow.Title = SelectedMarketDataProvider.ProviderName;
                scannerWindow.Closing += scannerWindow_Closing;
                scannerWindow.Show();
            }

            // Detach DataContext and close 'New Market Scanner' window
            _newMarketScannerWindow.DataContext = null;
            _newMarketScannerWindow.Close();
        }

        /// <summary>
        /// Focus certain market scanner window
        /// </summary>
        /// <param name="param">ProviderName</param>
        private void FocusScannerWindowExecute(object param)
        {
            MarketScannerWindow scannerWindow = (MarketScannerWindow)FindWindowByTitle((string)param);
            if (scannerWindow != null)
            {
                scannerWindow.WindowState = WindowState.Normal;
                scannerWindow.Activate();
            }
        }

        /// <summary>
        /// Close certain market scanner window
        /// </summary>
        /// <param name="param">ProviderName</param>
        private void CloseScannerWindowExecute(object param)
        {
            // Close scanner window
            MarketScannerWindow scannerWindow = (MarketScannerWindow)FindWindowByTitle((string)param);
            if (scannerWindow != null)
                scannerWindow.Close();
        }

        /// <summary>
        /// Initialization of market data providers
        /// </summary>
        private void InitializeMarketDataProviders()
        {
            _marketDataProviders = new ObservableCollection<MarketDataProvider>();

            // Populate Individual Market Data Provider details
            foreach (var provider in ProvidersController.MarketDataProviders)
            {
                // Add to Collection
                _marketDataProviders.Add(provider);
            }

            // Select initially 1st provider in ComboBox
            if (_marketDataProviders != null && _marketDataProviders.Count > 0)
                SelectedMarketDataProvider = _marketDataProviders[0];
        }

        /// <summary>
        /// Uses the incoming alert details to dispaly message
        /// </summary>
        private void DisplayAlertMessage(AlertMessage alertMessage)
        {
            _currentDispatcher.Invoke(DispatcherPriority.Background, (Action) (() =>
            {
                AlertWindow notificationWindow = new AlertWindow();
                notificationWindow.DataContext = new AlertWindowViewModel(alertMessage);

                notificationWindow.Show();
            }));
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles closing event if invoked for certain scanner window
        /// </summary>
        void scannerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MarketScannerWindow scannerWindow = (MarketScannerWindow)sender;

            if (WPFMessageBox.Show(scannerWindow, string.Format("Close scanner window {0}?", scannerWindow.Title), "Market Data Scanner",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                // If scanner window is closing...
                
                // close all LOB windows related to current scanner window
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is LimitOrderBookWindow && window.Title.Contains(scannerWindow.Title))
                    {
                        window.DataContext = null;
                        window.Close();
                    }
                }

                // remove that MarketScannerWindowViewModel from collection
                MarketScannerWindowViewModel scannerViewModel = ScannerWindowViewModels.First<MarketScannerWindowViewModel>(x => x.Provider.ProviderName == scannerWindow.Title);
                if (scannerViewModel != null)
                {
                    scannerViewModel.RemoveAllSymbols();
                    ScannerWindowViewModels.Remove(scannerViewModel);
                }

                // activate MainWindow
                MainWindow.Activate();
            }
        }

        #endregion
    }
}
