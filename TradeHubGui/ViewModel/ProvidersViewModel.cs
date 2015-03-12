using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;

namespace TradeHubGui.ViewModel
{
    public class ProvidersViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<MarketDataProvider> _marketDataProviders;
        private ObservableCollection<OrderExecutionProvider> _orderExecutionProviders;

        private MarketDataProvider _selectedMarketDataProvider;
        private OrderExecutionProvider _selectedOrderExecutionProvider;

        private RelayCommand _addProviderCommand;
        private RelayCommand _removeProviderCommand;
        private RelayCommand _connectProviderCommand;
        private RelayCommand _disconnectProviderCommand;

        private ProvidersController _providersController;

        #endregion

        #region Constructors
        
        public ProvidersViewModel()
        {
            _providersController = new ProvidersController();

            InitializeMarketDataProviders();
            InitializeOrderExecutionProviders();
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
        /// Collection of order execution providers
        /// </summary>
        public ObservableCollection<OrderExecutionProvider> OrderExecutionProviders
        {
            get { return _orderExecutionProviders; }
            set
            {
                if (_orderExecutionProviders != value)
                {
                    _orderExecutionProviders = value;
                    OnPropertyChanged("OrderExecutionProviders");
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
        /// Selected order execution provider
        /// </summary>
        public OrderExecutionProvider SelectedOrderExecutionProvider
        {
            get { return _selectedOrderExecutionProvider; }
            set
            {
                if (_selectedOrderExecutionProvider != value)
                {
                    _selectedOrderExecutionProvider = value;
                    OnPropertyChanged("SelectedOrderExecutionProvider");
                }
            }
        }
        #endregion

        #region Commands

        /// <summary>
        /// Used for 'Add new provider' button for adding new marked data provider or order execution provider depending on param
        /// </summary>
        public ICommand AddProviderCommand
        {
            get
            {
                return _addProviderCommand ?? (_addProviderCommand = new RelayCommand(param => AddProviderExecute(param)));
            }
        }

        /// <summary>
        /// Used for 'Remove provider' button for removing marked data provider or order execution provider depending on param
        /// </summary>
        public ICommand RemoveProviderCommand
        {
            get
            {
                return _removeProviderCommand ?? (_removeProviderCommand = new RelayCommand(param => RemoveProviderExecute(param), Param => RemoveProviderCanExecute()));
            }
        }

        /// <summary>
        /// Connect selected provider
        /// </summary>
        public ICommand ConnectProviderCommand
        {
            get
            {
                return _connectProviderCommand ?? (_connectProviderCommand = new RelayCommand(param => ConnectProviderExecute(param), param => ConnectProviderCanExecute(param)));
            }
        }

        /// <summary>
        /// Disconnect selected provider
        /// </summary>
        public ICommand DisconnectProviderCommand
        {
            get
            {
                return _disconnectProviderCommand ?? (_disconnectProviderCommand = new RelayCommand(param => DisconnectProviderExecute(param), param => DisconnectProviderCanExecute(param)));
            }
        }

        #endregion

        #region Trigger Methods for Commands

        /// <summary>
        /// Add new provider to the MarketDataProviders or to the OrderExecutionProviders collection depending on param
        /// </summary>
        private void AddProviderExecute(object param)
        {
            // TODO: 
            // this should open dialog for adding provider, and after that add that provider to the certain providers collection


            if (param.Equals("MarketDataProvider"))
            {

            }
            else if (param.Equals("OrderExecutionProvider"))
            {

            }
        }

        /// <summary>
        /// Remove provider from the MarketDataProviders or from the OrderExecutionProviders collection depending on param
        /// </summary>
        private void RemoveProviderExecute(object param)
        {
            Provider selectedProvider = param.Equals("MarketDataProvider") ? (Provider) SelectedMarketDataProvider : SelectedOrderExecutionProvider;

            if ((System.Windows.Forms.DialogResult)WPFMessageBox.Show(MainWindow, string.Format("Remove provider {0}?", selectedProvider.ProviderName), "Question",
                 MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == System.Windows.Forms.DialogResult.Yes)
            {
                if (param.Equals("MarketDataProvider"))
                {
                    // Remove SelectedMarketDataProvider
                    MarketDataProviders.Remove(SelectedMarketDataProvider);

                    // Select 1st provider from collection if not empty
                    if (MarketDataProviders.Count > 0) 
                        SelectedMarketDataProvider = MarketDataProviders[0];
                }
                else if (param.Equals("OrderExecutionProvider"))
                {
                    // Remove SelectedOrderExecutionProvider
                    OrderExecutionProviders.Remove(SelectedOrderExecutionProvider);

                    // Select 1st provider from collection if not empty
                    if (OrderExecutionProviders.Count > 0)
                        SelectedOrderExecutionProvider = OrderExecutionProviders[0];
                }
            }
        }

        private bool RemoveProviderCanExecute()
        {
            if (SelectedMarketDataProvider != null || SelectedOrderExecutionProvider != null)
                return true;

            return false;
        }

        /// <summary>
        /// Called when 'Connect' button command is triggerd
        /// </summary>
        /// <param name="param"></param>
        private void ConnectProviderExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                //NOTE: Test code to simulate Provider Connect
                // BEGIN:
                SelectedMarketDataProvider.ConnectionStatus = ConnectionStatus.Connected;
                return;
                // :END

                // Rasie event to request connection
                EventSystem.Publish<MarketDataProvider>(SelectedMarketDataProvider);
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                //NOTE: Test code to simulate Provider Connect
                // BEGIN:
                SelectedOrderExecutionProvider.ConnectionStatus = ConnectionStatus.Connected;
                return;
                // :END

                // Rasie event to request connection
                EventSystem.Publish<OrderExecutionProvider>(SelectedOrderExecutionProvider);
            }
        }

        private bool ConnectProviderCanExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                if (SelectedMarketDataProvider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
                {
                    return true;
                }
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                if (SelectedOrderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Called when 'Disconnect' button command is triggered
        /// </summary>
        /// <param name="param"></param>
        private void DisconnectProviderExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                //NOTE: Test code to simulate Provider Dis-Connect
                // BEGIN:
                SelectedMarketDataProvider.ConnectionStatus = ConnectionStatus.Disconnected;
                return;
                // :END

                // Rasie event to request connection
                EventSystem.Publish<MarketDataProvider>(SelectedMarketDataProvider);
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                //NOTE: Test code to simulate Provider Dis-Connect
                // BEGIN:
                SelectedOrderExecutionProvider.ConnectionStatus = ConnectionStatus.Disconnected;
                return;
                // :END

                // Rasie event to request connection
                EventSystem.Publish<OrderExecutionProvider>(SelectedOrderExecutionProvider);
            }
        }

        private bool DisconnectProviderCanExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                if (SelectedMarketDataProvider.ConnectionStatus.Equals(ConnectionStatus.Connected))
                {
                    return true;
                }
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                if (SelectedOrderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Connected))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Initialization of market data providers
        /// </summary>
        private async void InitializeMarketDataProviders()
        {
            _marketDataProviders = new ObservableCollection<MarketDataProvider>();

            // Request Controller for infomation
            var availableProviders = await Task.Run(() => _providersController.GetAvailableMarketDataProviders());

            // Safety check incase information was not populated
            if (availableProviders == null)
                return;

            // Populate Individual Market Data Provider details to show on UI
            foreach (var provider in availableProviders)
            {
                // Add to Collection
                _marketDataProviders.Add(provider);
            }

            // Select initially 1st provider in ComboBox
            if (_marketDataProviders != null && _marketDataProviders.Count > 0)
                SelectedMarketDataProvider = _marketDataProviders[0];
        }

        /// <summary>
        /// Initialization of order execution providers
        /// </summary>
        private async void InitializeOrderExecutionProviders()
        {
            _orderExecutionProviders = new ObservableCollection<OrderExecutionProvider>();

            // Request Controller for infomation
            var availableProviders = await Task.Run(() => _providersController.GetAvailableOrderExecutionProviders());

            // Safety check incase information was not populated
            if (availableProviders == null)
                return;

            // Populate Individual Order Execution Provider details to show on UI
            foreach (var provider in availableProviders)
            {
                // Add to Collection
                _orderExecutionProviders.Add(provider);
            }

            // Select initially 1st provider in ComboBox
            if (_orderExecutionProviders != null && _orderExecutionProviders.Count > 0)
                SelectedOrderExecutionProvider = _orderExecutionProviders[0];
        }
    }
}
