using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceSourceLogger;
using TradeHub.Common.Core.Constants;
using TradeHub.StrategyEngine.OrderExecution;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Managers;

namespace TradeHubGui.Dashboard.Services
{
    /// <summary>
    /// Provides access for Order Execution queries and response
    /// </summary>
    public class OrderExecutionController
    {
        private Type _type = typeof(OrderExecutionController);

        /// <summary>
        /// Responsible for providing requested order execution functionality
        /// </summary>
        private OrderExecutionManager _orderExecutionManager;

        /// <summary>
        /// Keeps tracks of all the Providers
        /// KEY = Provider Name
        /// Value = Provider details <see cref="Provider"/>
        /// </summary>
        private IDictionary<string, Provider> _providersMap;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="orderExecutionService">Provides communication access with Order Execution Server</param>
        public OrderExecutionController(OrderExecutionService orderExecutionService)
        {
            // Initialize Manager
            _orderExecutionManager = new OrderExecutionManager(orderExecutionService);

            // Intialize local maps
            _providersMap = new Dictionary<string, Provider>();

            // Subscribe Application events
            SubscribeEvents();

            // Subscribe Order Execution Manager events
            SubscribeManagerEvents();
        }

        /// <summary>
        /// Subscribe events to receive incoming Order Execution requests
        /// </summary>
        private void SubscribeEvents()
        {
            // Register Event to receive connect/disconnect requests
            EventSystem.Subscribe<Provider>(NewConnectionRequest);
        }

        /// <summary>
        /// Subscribe events to receive incoming data and responses from Order Execution Manager
        /// </summary>
        private void SubscribeManagerEvents()
        {
            _orderExecutionManager.LogonArrivedEvent += OnLogonArrived;
            _orderExecutionManager.LogoutArrivedEvent += OnLogoutArrived;
        }

        #region Incoming Requests

        /// <summary>
        /// Called when new Connection request is made by the user
        /// </summary>
        /// <param name="orderExecutionProvider"></param>
        private void NewConnectionRequest(Provider orderExecutionProvider)
        {
            // Only entertain 'Order Execution Provider' related calls
            if (!orderExecutionProvider.ProviderType.Equals(ProviderType.OrderExecution))
                return;

            if (orderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
            {
                // Open a new order execution connection
                ConnectOrderExecutionProvider(orderExecutionProvider);
            }
            else if (orderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Connected))
            {
                // Close existing connection
                DisconnectOrderExecutionProvider(orderExecutionProvider);
            }
        }

        /// <summary>
        /// Called when connection request is received for given Order Execution Provider
        /// </summary>
        /// <param name="orderExecutionProvider">Contains provider details</param>
        private void ConnectOrderExecutionProvider(Provider orderExecutionProvider)
        {
            // Check if the provider already exists in the local map
            if (!_providersMap.ContainsKey(orderExecutionProvider.ProviderName))
            {
                // Add incoming provider to local map
                _providersMap.Add(orderExecutionProvider.ProviderName, orderExecutionProvider);
            }

            // Check current provider status
            if (orderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
            {
                // Forward connection request
                _orderExecutionManager.Connect(orderExecutionProvider.ProviderName);
            }
            else
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info(orderExecutionProvider.ProviderName + " connection status is already set to connected.",
                        _type.FullName, "ConnectOrderExecutionProvider");
                }
            }
        }

        /// <summary>
        /// Called when disconnect request is received for given Order Execution Provider
        /// </summary>
        /// <param name="orderExecutionProvider">Contains provider details</param>
        private void DisconnectOrderExecutionProvider(Provider orderExecutionProvider)
        {
            // Check current provider status
            if (orderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Connected))
            {
                // Forward disconnect request
                _orderExecutionManager.Disconnect(orderExecutionProvider.ProviderName);
            }
            else
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info(orderExecutionProvider.ProviderName + " connection status is already set to dis-connected.",
                        _type.FullName, "DisconnectOrderExecutionProvider");
                }
            }
        }

        #endregion

        #region Order Execution Manager Events

        /// <summary>
        /// Called when requested provider is successfully 'Logged ON'
        /// </summary>
        /// <param name="providerName"></param>
        private void OnLogonArrived(string providerName)
        {
            Provider provider;
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
            Provider provider;
            if (_providersMap.TryGetValue(providerName, out provider))
            {
                provider.ConnectionStatus = ConnectionStatus.Disconnected;
            }
        }

        #endregion

        /// <summary>
        /// Stops all order execution related activities
        /// </summary>
        public void Stop()
        {
            // Send logout for each connected order execution provider
            foreach (KeyValuePair<string, Provider> keyValuePair in _providersMap)
            {
                if (keyValuePair.Value.ConnectionStatus.Equals(ConnectionStatus.Connected))
                {
                    _orderExecutionManager.Disconnect(keyValuePair.Key);
                }
            }

            _orderExecutionManager.Stop();
        }
    }
}
