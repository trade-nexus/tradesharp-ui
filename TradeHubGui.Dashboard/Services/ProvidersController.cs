using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Managers;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;

namespace TradeHubGui.Dashboard.Services
{
    /// <summary>
    /// Responsible for Provider's (Market Data / Order Execution) related queries and request
    /// </summary>
    public class ProvidersController
    {
        private Type _type = typeof (ProvidersController);

        /// <summary>
        /// Holds UI thread reference
        /// </summary>
        private Dispatcher _currentDispatcher;

        /// <summary>
        /// Handle Market Data Provider related functionaltiy
        /// </summary>
        private readonly MarketDataProvidersManager _dataProvidersManager;

        /// <summary>
        /// Handle Order Execution Provider related functionaltiy
        /// </summary>
        private readonly OrderExecutionProvidersManager _executionProvidersManager;

        /// <summary>
        /// Holds Available Market Data Provider objects
        /// </summary>
        public static List<MarketDataProvider> MarketDataProviders = new List<MarketDataProvider>();

        /// <summary>
        /// Holds Available Order Execution Provider Objects
        /// </summary>
        public static List<OrderExecutionProvider> OrderExecutionProviders = new List<OrderExecutionProvider>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProvidersController()
        {
            _currentDispatcher = Dispatcher.CurrentDispatcher;

            _dataProvidersManager = new MarketDataProvidersManager();
            _executionProvidersManager = new OrderExecutionProvidersManager();
        }

        /// <summary>
        /// Returns available market data providers along with there configuration information
        /// </summary>
        /// <returns></returns>
        public async Task<IList<MarketDataProvider>> GetAvailableMarketDataProviders()
        {
            var availableProvidersInformation = _dataProvidersManager.GetAvailableProviders();

            // Safety check incase information was not populated
            if (availableProvidersInformation == null)
                return null;

            MarketDataProviders.Clear();

            // Populate Individual Market Data Provider details
            foreach (var keyValuePair in availableProvidersInformation)
            {
                MarketDataProvider tempProvider = new MarketDataProvider()
                {
                    ProviderType = ProviderType.MarketData,
                    ProviderName = keyValuePair.Key,
                    ConnectionStatus = ConnectionStatus.Disconnected
                };
                tempProvider.ProviderCredentials = keyValuePair.Value;

                // Add to Collection
                MarketDataProviders.Add(tempProvider);
            }

            return MarketDataProviders;
        }

        /// <summary>
        /// Returns available order executiom providers along with there configuration information
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderExecutionProvider>> GetAvailableOrderExecutionProviders()
        {
            var availableProvidersInformation = _executionProvidersManager.GetAvailableProviders();

            // Safety check incase information was not populated
            if (availableProvidersInformation == null)
                return null;

            OrderExecutionProviders.Clear();

            // Populate Individual Order Execution Provider details
            foreach (var keyValuePair in availableProvidersInformation)
            {
                OrderExecutionProvider tempProvider = new OrderExecutionProvider(_currentDispatcher)
                {
                    ProviderType = ProviderType.OrderExecution,
                    ProviderName = keyValuePair.Key,
                    ConnectionStatus = ConnectionStatus.Disconnected
                };
                tempProvider.ProviderCredentials = keyValuePair.Value;

                // Add to Collection
                OrderExecutionProviders.Add(tempProvider);
            }

            return OrderExecutionProviders;
        }

        /// <summary>
        /// Modifies respective provider credentails as per given details
        /// </summary>
        /// <param name="provider">Provider who's credentials are to be modified</param>
        public bool EditProviderCredentials(Provider provider)
        {
            // Handle Market Data Provider
            if (provider.ProviderType.Equals(ProviderType.MarketData))
            {
                return _dataProvidersManager.EditProviderCredentials(provider);
            }
            // Handle Order Execution Provider
            else if (provider.ProviderType.Equals(ProviderType.OrderExecution))
            {
                return _executionProvidersManager.EditProviderCredentials(provider);
            }

            return false;
        }

        /// <summary>
        /// Adds given connector library as Market Data Provider in the Server
        /// </summary>
        /// <param name="connectorPath">Connector library path</param>
        /// <param name="providerName">Name to be used for the given connector</param>
        public Tuple<bool, string> AddMarketDataProvider(string connectorPath, string providerName)
        {
            return _dataProvidersManager.AddProvider(connectorPath, providerName);
        }

        /// <summary>
        /// Adds given connector library as Order Execution Provider in the Server
        /// </summary>
        /// <param name="connectorPath">Connector library path</param>
        /// <param name="providerName">Name to be used for the given connector</param>
        /// <returns></returns>
        public Tuple<bool, string> AddOrderExecutionProvider(string connectorPath, string providerName)
        {
            return _executionProvidersManager.AddProvider(connectorPath, providerName);
        }

        /// <summary>
        /// Removes given Data provider Order Execution Provider from the Server
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Tuple<bool, string> RemoveMarketDataProvider(Provider provider)
        {
            return _dataProvidersManager.RemoveProvider(provider);
        }

        /// <summary>
        /// Removes given Execution provider Order Execution Provider from the Server
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Tuple<bool, string> RemoveOrderExecutionProvider(Provider provider)
        {
            return _executionProvidersManager.RemoveProvider(provider);
        }
    }
}
