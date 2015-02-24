using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Managers;

namespace TradeHubGui.Dashboard.Services
{
    /// <summary>
    /// Responsible for Provider's (Market Data / Order Execution) related queries and request
    /// </summary>
    public class ProvidersController
    {
        private Type _type = typeof (ProvidersController);

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
        public static List<Provider> OrderExecutionProviders = new List<Provider>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProvidersController()
        {
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
        public async Task<List<Provider>> GetAvailableOrderExecutionProviders()
        {
            var availableProvidersInformation = _executionProvidersManager.GetAvailableProviders();

            // Safety check incase information was not populated
            if (availableProvidersInformation == null)
                return null;

            // Populate Individual Order Execution Provider details
            foreach (var keyValuePair in availableProvidersInformation)
            {
                Provider tempProvider = new Provider()
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
    }
}
