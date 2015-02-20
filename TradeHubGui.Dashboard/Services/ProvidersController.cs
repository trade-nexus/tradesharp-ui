using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<IDictionary<string, List<ProviderCredential>>> GetAvailableMarketDataProviders()
        {
            return _dataProvidersManager.GetAvailableProviders();
        }

        /// <summary>
        /// Returns available order executiom providers along with there configuration information
        /// </summary>
        /// <returns></returns>
        public async Task<IDictionary<string, List<ProviderCredential>>> GetAvailableOrderExecutionProviders()
        {
            return _executionProvidersManager.GetAvailableProviders();
        } 
    }
}
