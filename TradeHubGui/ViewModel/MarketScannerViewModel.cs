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
        private NewMarketScannerWindow _newMarketScannerWindow;
        private ObservableCollection<Provider> _marketDataProviders;
        private Provider _selectedMarketDataProvider;
        private ObservableCollection<MarketScannerWindowViewModel> _scannerWindowViewModels;
        
        private RelayCommand _showNewScannerWindowCommand;
        private RelayCommand _createScannerWindowCommand;
        private RelayCommand _closeScannerWindowCommand;
        private RelayCommand _focusScannerWindowCommand;
        #endregion

        #region Constructor
        public MarketScannerViewModel()
        {
            _scannerWindowViewModels = new ObservableCollection<MarketScannerWindowViewModel>();
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
            MarketScannerWindow scannerWindow = FindScannerWindowByTitle(SelectedMarketDataProvider.ProviderName);

            // if scanner is already created, just activate it, otherwise create new scanner window for slected data provider
            if (scannerWindow != null)
            {
                scannerWindow.WindowState = WindowState.Normal;
                scannerWindow.Activate();
            }
            else
            {
                scannerWindow = new MarketScannerWindow();
                MarketScannerWindowViewModel scannerWindowViewModel = new MarketScannerWindowViewModel(scannerWindow, SelectedMarketDataProvider);
                
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
            MarketScannerWindow scannerWindow = FindScannerWindowByTitle((string)param);
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
            MarketScannerWindow scannerWindow = FindScannerWindowByTitle((string)param);
            if (scannerWindow != null)
                scannerWindow.Close();
        }

        /// <summary>
        /// Traverse through all windows of application and trying to find scanner window by title
        /// </summary>
        /// <param name="title">scanner window title</param>
        /// <returns></returns>
        private MarketScannerWindow FindScannerWindowByTitle(string title)
        {
            MarketScannerWindow scannerWindow = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MarketScannerWindow && window.Title == title)
                {
                    scannerWindow = (MarketScannerWindow)window;
                    break;
                }
            }

            return scannerWindow;
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
            MarketScannerWindow scannerWindow = (MarketScannerWindow)sender;

            if (WPFMessageBox.Show(scannerWindow, string.Format("Close scanner window {0}?", scannerWindow.Title), "Market Data Scanner", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                // If scanner window is closing, remove that MarketScannerWindowViewModel from collection
                MarketScannerWindowViewModel scannerViewModel = ScannerWindowViewModels.First<MarketScannerWindowViewModel>(x => x.Provider.ProviderName == scannerWindow.Title);
                if (scannerViewModel != null)
                    ScannerWindowViewModels.Remove(scannerViewModel);

                // if scanner window is successfully closed, activate MainWindow
                MainWindow.Activate();
            }
        }
        #endregion
    }
}
