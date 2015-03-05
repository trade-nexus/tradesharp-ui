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
        public static List<Provider> MarketDataProviders = new List<Provider>();

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
        public async Task<IList<Provider>> GetAvailableMarketDataProviders()
        {
            var availableProvidersInformation = _dataProvidersManager.GetAvailableProviders();

            // Safety check incase information was not populated
            if (availableProvidersInformation == null)
                return null;

            // Populate Individual Market Data Provider details
            foreach (var keyValuePair in availableProvidersInformation)
            {
                Provider tempProvider = new Provider()
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
        public void EditProviderCredentials(Provider provider)
        {
            // Handle Market Data Provider
            if (provider.ProviderType.Equals(ProviderType.MarketData))
            {
                _dataProvidersManager.EditProviderCredentials(provider);
            }
            // Handle Order Execution Provider
            else if (provider.ProviderType.Equals(ProviderType.OrderExecution))
            {
                _executionProvidersManager.EditProviderCredentials(provider);
            }
        }
    }
}
