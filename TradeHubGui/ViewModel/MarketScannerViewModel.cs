using MahApps.Metro.Controls;
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
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;
using TradeHubGui.Views;

namespace TradeHubGui.ViewModel
{
    public class MarketScannerViewModel : BaseViewModel
    {
        #region Fields
        private RelayCommand _showNewMarketScannerWindowCommand;
        private RelayCommand _createMarketScannerCommand;
        private NewMarketScannerWindow _newMarketScannerWindow;
        private ObservableCollection<Provider> _marketDataProviders;
        private Provider _selectedMarketDataProvider;
        #endregion

        #region Constructor
        public MarketScannerViewModel()
        {

        }
        #endregion

        #region Properties

        /// <summary>
        /// Collection of market data providers
        /// </summary>
        public ObservableCollection<Provider> MarketDataProviders
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
        public Provider SelectedMarketDataProvider
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
        #endregion

        #region Commands
        /// <summary>
        /// Opens 'New Market Scanner' window for chosing market data provider for scanning
        /// </summary>
        public ICommand ShowNewMarketScannerWindowCommand
        {
            get
            {
                return _showNewMarketScannerWindowCommand ?? (_showNewMarketScannerWindowCommand = new RelayCommand(param => ShowNewMarketScannerWindowExecute()));
            }
        }

        /// <summary>
        /// Creating of new market scanner window for chosen market data provider
        /// </summary>
        public ICommand CreateMarketScannerCommand
        {
            get
            {
                return _createMarketScannerCommand ?? (_createMarketScannerCommand = new RelayCommand(param => CreateMarketScannerExecute(), param => CreateMarketScannerCanExecute(param)));
            }
        }
        #endregion

        #region Methods
        private void ShowNewMarketScannerWindowExecute()
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
        private bool CreateMarketScannerCanExecute(object param)
        {
            if (param != null)
                return true;

            return false;
        }

        /// <summary>
        /// Create market scanner for selected market data provider
        /// </summary>
        private void CreateMarketScannerExecute()
        {
            // Try to find scanner window if already created for selected provider
            MarketScannerWindow scannerWindow = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MarketScannerWindow && window.Title == SelectedMarketDataProvider.ProviderName)
                {
                    scannerWindow = (MarketScannerWindow)window;
                    break;
                }
            }

            // if scanner is already created, just activate it, otherwise create new scanner window for slected data provider
            if (scannerWindow != null)
            {
                scannerWindow.WindowState = WindowState.Normal;
                scannerWindow.Activate();
            }
            else
            {
                scannerWindow = new MarketScannerWindow();
                scannerWindow.DataContext = new MarketScannerContentViewModel() { Provider = SelectedMarketDataProvider };
                scannerWindow.Title = SelectedMarketDataProvider.ProviderName;
                scannerWindow.Closing += scannerWindow_Closing;
                scannerWindow.Show();

                // Add scanner info to the Market Data Scanner dashboard
                //TODO:
            }

            // Detach DataContext and close 'New Market Scanner' window
            _newMarketScannerWindow.DataContext = null;
            _newMarketScannerWindow.Close();
        }

        /// <summary>
        /// Initialization of market data providers
        /// </summary>
        private void InitializeMarketDataProviders()
        {
            _marketDataProviders = new ObservableCollection<Provider>();

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
        #endregion

        #region Events

        /// <summary>
        /// Handles closing event if invoked for certain scanner window
        /// </summary>
        void scannerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WPFMessageBox.Show((MetroWindow)sender, "Are you sure you want to close the scanner window?", "Market Data Scanner", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                // if scanner window is successfully closed, activate MainWindow
                MainWindow.Activate();
            }
        }
        #endregion
    }
}
