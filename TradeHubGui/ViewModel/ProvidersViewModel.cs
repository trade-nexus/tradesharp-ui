using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private RelayCommand _addMarketDataProviderCommand;
        private RelayCommand _addOrderExecutionProviderCommand;

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
        /// Command used for Add new provider button for adding new marked data provider
        /// </summary>
        public ICommand AddMarketDataProviderCommand
        {
            get
            {
                return _addMarketDataProviderCommand ?? (_addMarketDataProviderCommand = new RelayCommand(param => AddMarketDataProviderExecute()));
            }
        }

        /// <summary>
        /// Command used for Add new provider button for adding new order execution provider
        /// </summary>
        public ICommand AddOrderExecutionProviderCommand
        {
            get
            {
                return _addOrderExecutionProviderCommand ?? (_addOrderExecutionProviderCommand = new RelayCommand(param => AddOrderExecutionProviderExecute()));
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
            if(_marketDataProviders != null && _marketDataProviders.Count > 0)
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
        /// Add new market data provider to the MarketDataProviders collection
        /// </summary>
        private void AddMarketDataProviderExecute()
        {
            // TODO: add new market data provider to the MarketDataProviders collection
            // this should open dialog for adding provider, and after that add that provider to the MarketDataProviders collection
        }

        /// <summary>
        /// Add new market data provider to the OrderExecutionProviders collection
        /// </summary>
        private void AddOrderExecutionProviderExecute()
        {
            // TODO: add new market data provider to the OrderExecutionProviders collection
            // this should open dialog for adding provider, and after that add that provider to the OrderExecutionProviders collection
        }
        #endregion
    }
}
