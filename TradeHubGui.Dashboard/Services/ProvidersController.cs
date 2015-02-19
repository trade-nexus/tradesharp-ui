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
        private MarketDataProvidersManager _dataProvidersManager;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProvidersController()
        {
            _dataProvidersManager = new MarketDataProvidersManager();
        }

        /// <summary>
        /// Returns a list of available market data providers
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<ProviderCredential>> GetAvailableProviders()
        {
            return _dataProvidersManager.GetAvailableProviders();
        } 
    }
}
