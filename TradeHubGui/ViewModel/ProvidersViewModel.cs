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
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class ProvidersViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Provider> _marketDataProviders;
        private ObservableCollection<Provider> _orderExecutionProviders;
        private Provider _selectedMarketDataProvider;
        private Provider _selectedOrderExecutionProvider;
        private RelayCommand _addProviderCommand;
        private RelayCommand _removeProviderCommand;
        private RelayCommand _connectProviderCommand;
        private RelayCommand _disconnectProviderCommand;
        #endregion

        #region Constructors
        public ProvidersViewModel()
        {
            InitMarketDataProviders();
            InitOrderExecutionProviders();
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
        /// Collection of order execution providers
        /// </summary>
        public ObservableCollection<Provider> OrderExecutionProviders
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
        /// Selected order execution provider
        /// </summary>
        public Provider SelectedOrderExecutionProvider
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

        #region Methods
        /// <summary>
        /// Initialization of market data providers
        /// </summary>
        private void InitMarketDataProviders()
        {
            _marketDataProviders = new ObservableCollection<Provider>();

            #region NOTE: this region is just dummy initialization for testing purpose and this will be replaced with real initialization

            List<ProviderCredential> credentials = new List<ProviderCredential>();
            credentials.Add(new ProviderCredential() { CredName = "Username", CredValue = string.Empty });
            credentials.Add(new ProviderCredential() { CredName = "Password", CredValue = string.Empty });
            credentials.Add(new ProviderCredential() { CredName = "IP address", CredValue = string.Empty });
            credentials.Add(new ProviderCredential() { CredName = "Port", CredValue = string.Empty });

            Provider provider = new Provider() { ProviderName = MarketDataProvider.Blackwood, ConnectionStatus = "Connected" };
            provider.ProviderCredentials = credentials;
            _marketDataProviders.Add(provider);

            credentials = new List<ProviderCredential>();
            credentials.Add(new ProviderCredential() { CredName = "Username", CredValue = string.Empty });
            credentials.Add(new ProviderCredential() { CredName = "Password", CredValue = string.Empty });

            provider = new Provider() { ProviderName = MarketDataProvider.InteractiveBrokers, ConnectionStatus = "Connected" };
            provider.ProviderCredentials = credentials;
            _marketDataProviders.Add(provider);

            credentials = new List<ProviderCredential>();
            credentials.Add(new ProviderCredential() { CredName = "Username", CredValue = string.Empty });
            credentials.Add(new ProviderCredential() { CredName = "Password", CredValue = string.Empty });
            credentials.Add(new ProviderCredential() { CredName = "IP address", CredValue = string.Empty });
            credentials.Add(new ProviderCredential() { CredName = "Port", CredValue = string.Empty });

            provider = new Provider() { ProviderName = MarketDataProvider.Simulated, ConnectionStatus = "Disconnected" };
            provider.ProviderCredentials = credentials;
            _marketDataProviders.Add(provider);

            credentials = new List<ProviderCredential>();
            credentials.Add(new ProviderCredential() { CredName = "Username", CredValue = string.Empty });
            credentials.Add(new ProviderCredential() { CredName = "Password", CredValue = string.Empty });

            provider = new Provider() { ProviderName = MarketDataProvider.SimulatedExchange, ConnectionStatus = "Connected" };
            provider.ProviderCredentials = credentials;
            _marketDataProviders.Add(provider);

            #endregion

            // Select initially 1st provider in ComboBox
            if (_marketDataProviders != null && _marketDataProviders.Count > 0)
                SelectedMarketDataProvider = _marketDataProviders[0];
        }

        /// <summary>
        /// Initialization of order execution providers
        /// </summary>
        private void InitOrderExecutionProviders()
        {
            _orderExecutionProviders = new ObservableCollection<Provider>();

            // TODO: populate collection

            // Select initially 1st provider in ComboBox
            if (_orderExecutionProviders != null && _orderExecutionProviders.Count > 0)
                SelectedOrderExecutionProvider = _orderExecutionProviders[0];
        }

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
            Provider selectedProvider = param.Equals("MarketDataProvider") ? SelectedMarketDataProvider : SelectedOrderExecutionProvider;

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

        private void ConnectProviderExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                //TODO:
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                //TODO:
            }
        }

        private bool ConnectProviderCanExecute(object param)
        {
            return false;
        }

        private void DisconnectProviderExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                //TODO:
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                //TODO:
            }
        }

        private bool DisconnectProviderCanExecute(object param)
        {
            return false;
        }

        #endregion
    }
}
