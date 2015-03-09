using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceSourceLogger;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.ValueObjects.AdminMessages;
using TradeHub.StrategyEngine.MarketData;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Managers;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;

namespace TradeHubGui.Dashboard.Services
{
    /// <summary>
    /// Provides access for Market Data related queries and response
    /// </summary>
    public class MarketDataController
    {
        private Type _type = typeof (MarketDataController);

        /// <summary>
        /// Responsible for providing requested market data functionality
        /// </summary>
        private MarketDataManager _marketDataManager;

        /// <summary>
        /// Keeps tracks of all the Providers
        /// KEY = Provider Name
        /// Value = Provider details <see cref="Provider"/>
        /// </summary>
        private IDictionary<string, MarketDataProvider> _providersMap; 

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="marketDataService">Provides communication access with Market Data Server</param>
        public MarketDataController(MarketDataService marketDataService)
        {
            // Initialize Manager
            _marketDataManager = new MarketDataManager(marketDataService);

            // Intialize local maps
            _providersMap = new Dictionary<string, MarketDataProvider>();

            // Subscribe Application events
            SubscribeEvents();

            // Subscribe Market Data Manager events
            SubscribeManagerEvents();
        }

        /// <summary>
        /// Subscribe events to receive incoming market data requests
        /// </summary>
        private void SubscribeEvents()
        {
            // Register Event to receive connect/disconnect requests
            EventSystem.Subscribe<MarketDataProvider>(NewConnectionRequest);

            // Register Event to receive subscribe/unsubscribe requests
            EventSystem.Subscribe<SubscriptionRequest>(NewSubscriptionRequest);
        }

        /// <summary>
        /// Subscribe events to receive incoming data and responses from Market Data Manager
        /// </summary>
        private void SubscribeManagerEvents()
        {
            _marketDataManager.LogonArrivedEvent += OnLogonArrived;
            _marketDataManager.LogoutArrivedEvent += OnLogoutArrived;

            _marketDataManager.TickArrivedEvent += OnTickArrived;
        }

        #region Incoming Requests

        /// <summary>
        /// Called when new Connection request is made by the user
        /// </summary>
        /// <param name="marketDataProvider"></param>
        private void NewConnectionRequest(MarketDataProvider marketDataProvider)
        {
            // Only entertain 'Market Data Provider' related calls
            if (!marketDataProvider.ProviderType.Equals(ProviderType.MarketData))
                return;

            if (marketDataProvider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
            {
                // Open a new market data connection
                ConnectMarketDataProvider(marketDataProvider);
            }
            else if (marketDataProvider.ConnectionStatus.Equals(ConnectionStatus.Connected))
            {
                // Close existing connection
                DisconnectMarketDataProvider(marketDataProvider);
            }
        }

        /// <summary>
        /// Called when a new subscription request is made by the user
        /// </summary>
        /// <param name="subscriptionRequest"></param>
        private void NewSubscriptionRequest(SubscriptionRequest subscriptionRequest)
        {
            if (subscriptionRequest.SubscriptionType.Equals(SubscriptionType.Subscribe))
            {
                Subscribe(subscriptionRequest);
            }
            else
            {
                Unsubscribe(subscriptionRequest);
            }
        }

        /// <summary>
        /// Called when connection request is received for given Market Data Provider
        /// </summary>
        /// <param name="marketDataProvider">Contains provider details</param>
        private void ConnectMarketDataProvider(MarketDataProvider marketDataProvider)
        {
            // Check if the provider already exists in the local map
            if (!_providersMap.ContainsKey(marketDataProvider.ProviderName))
            {
                // Add incoming provider to local map
                _providersMap.Add(marketDataProvider.ProviderName, marketDataProvider);
            }

            // Check current provider status
            if (marketDataProvider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
            {
                // Forward connection request
                _marketDataManager.Connect(marketDataProvider.ProviderName);
            }
            else
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info(marketDataProvider.ProviderName + " connection status is already set to connected.",
                        _type.FullName, "ConnectMarketDataProvider");
                }
            }
        }

        /// <summary>
        /// Called when disconnect request is received for given Market Data Provider
        /// </summary>
        /// <param name="marketDataProvider">Contains provider details</param>
        private void DisconnectMarketDataProvider(MarketDataProvider marketDataProvider)
        {
            // Check current provider status
            if (marketDataProvider.ConnectionStatus.Equals(ConnectionStatus.Connected))
            {
                // Forward disconnect request
                _marketDataManager.Disconnect(marketDataProvider.ProviderName);
            }
            else
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info(marketDataProvider.ProviderName + " connection status is already set to dis-connected.",
                        _type.FullName, "DisconnectMarketDataProvider");
                }
            }
        }

        /// <summary>
        /// Called when your user requests new market data subscription
        /// </summary>
        /// <param name="subscriptionRequest">Contains subscription information</param>
        private void Subscribe(SubscriptionRequest subscriptionRequest)
        {
            _marketDataManager.Subscribe(subscriptionRequest.Security, subscriptionRequest.Provider.ProviderName);
        }

        /// <summary>
        /// Called when your user requests market data to be unsubscribed
        /// </summary>
        /// <param name="subscriptionRequest">Contains subscription information</param>
        private void Unsubscribe(SubscriptionRequest subscriptionRequest)
        {
            _marketDataManager.Unsubscribe(subscriptionRequest.Security, subscriptionRequest.Provider.ProviderName);
        }

        #endregion

        #region Market Data Manager Events

        /// <summary>
        /// Called when requested provider is successfully 'Logged ON'
        /// </summary>
        /// <param name="providerName"></param>
        private void OnLogonArrived(string providerName)
        {
            MarketDataProvider provider;
            if (_providersMap.TryGetValue(providerName, out provider))
            {
                provider.ConnectionStatus = ConnectionStatus.Connected;
            }
        }

        /// <summary>
        /// Called when requested market data provider is successfully 'Logged OUT'
        /// </summary>
        /// <param name="providerName"></param>
        private void OnLogoutArrived(string providerName)
        {
            MarketDataProvider provider;
            if (_providersMap.TryGetValue(providerName, out provider))
            {
                provider.ConnectionStatus = ConnectionStatus.Disconnected;
            }
        }

        /// <summary>
        /// Called when new Tick information is received from Market Data Sever
        /// </summary>
        /// <param name="tick">Contains market details</param>
        private void OnTickArrived(Tick tick)
        {
            MarketDataProvider provider;

            // Get Provider object
            if (_providersMap.TryGetValue(tick.MarketDataProvider, out provider))
            {
                TickDetail tickDetails;

                // Get TickDetails object to update tick information
                if (provider.TickDetailsMap.TryGetValue(tick.Security.Symbol, out tickDetails))
                {
                    // Update Bid
                    tickDetails.BidPrice = tick.BidPrice;
                    tickDetails.BidQuantity = tick.BidSize;

                    // Update Ask
                    tickDetails.AskPrice = tick.AskPrice;
                    tickDetails.AskQuantity = tick.AskSize;

                    // Update Last
                    tickDetails.LastPrice = tick.LastPrice;
                    tickDetails.LastQuantity = tick.LastSize;
                }
            }
        }

        #endregion

        /// <summary>
        /// Stops all market data related activities
        /// </summary>
        public void Stop()
        {
            // Send logout for each connected market data provider
            foreach (KeyValuePair<string, MarketDataProvider> keyValuePair in _providersMap)
            {
                if (keyValuePair.Value.ConnectionStatus.Equals(ConnectionStatus.Connected))
                {
                    _marketDataManager.Disconnect(keyValuePair.Key);
                }
            }

            _marketDataManager.Stop();
        }
    }
}
