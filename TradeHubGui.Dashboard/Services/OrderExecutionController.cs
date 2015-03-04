using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceSourceLogger;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels.OrderDomain;
using TradeHub.Common.Core.FactoryMethods;
using TradeHub.StrategyEngine.OrderExecution;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Managers;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;

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
        /// Value = Provider details <see cref="OrderExecutionProvider"/>
        /// </summary>
        private IDictionary<string, OrderExecutionProvider> _providersMap;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="orderExecutionService">Provides communication access with Order Execution Server</param>
        public OrderExecutionController(OrderExecutionService orderExecutionService)
        {
            // Initialize Manager
            _orderExecutionManager = new OrderExecutionManager(orderExecutionService);

            // Intialize local maps
            _providersMap = new Dictionary<string, OrderExecutionProvider>();

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
            EventSystem.Subscribe<OrderExecutionProvider>(NewConnectionRequest);
            EventSystem.Subscribe<OrderRequest>(NewOrderRequest);
        }

        /// <summary>
        /// Subscribe events to receive incoming data and responses from Order Execution Manager
        /// </summary>
        private void SubscribeManagerEvents()
        {
            _orderExecutionManager.LogonArrivedEvent += OnLogonArrived;
            _orderExecutionManager.LogoutArrivedEvent += OnLogoutArrived;

            _orderExecutionManager.OrderAcceptedEvent += OnOrderAccepted;
            _orderExecutionManager.ExecutionArrivedEvent += OnExecutionArrived;
            _orderExecutionManager.CancellationArrivedEvent += OnCancellationArrived;
            _orderExecutionManager.RejectionArrivedEvent += OnRejectionArrived;
        }

        #region Incoming Requests

        /// <summary>
        /// Called when new Connection request is made by the user
        /// </summary>
        /// <param name="orderExecutionProvider"></param>
        private void NewConnectionRequest(OrderExecutionProvider orderExecutionProvider)
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
        private void ConnectOrderExecutionProvider(OrderExecutionProvider orderExecutionProvider)
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
        private void DisconnectOrderExecutionProvider(OrderExecutionProvider orderExecutionProvider)
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

        /// <summary>
        /// Called when new order related request is made by the user
        /// </summary>
        /// <param name="orderRequest">Contains Order details</param>
        private void NewOrderRequest(OrderRequest orderRequest)
        {
            OrderExecutionProvider provider;
            //Find respective Provider
            if (_providersMap.TryGetValue(orderRequest.OrderDetails.Provider, out provider))
            {
                // Only entertain request if provider is connected
                if (provider.ConnectionStatus.Equals(ConnectionStatus.Connected))
                {
                    // Hanlde New Order Requests
                    if (orderRequest.RequestType.Equals(OrderRequestType.New))
                    {
                        // Handle Market Order Request
                        if (orderRequest.OrderDetails.Type.Equals(OrderType.Market))
                        {
                            MarketOrderRequest(orderRequest.OrderDetails);      
                        }
                        // Handle Limit Order Request
                        else if (orderRequest.OrderDetails.Type.Equals(OrderType.Limit))
                        {
                            LimitOrderRequest(orderRequest.OrderDetails);
                        }
                    }
                    // Hanlde Order Cancellation Requests
                    else if (orderRequest.RequestType.Equals(OrderRequestType.Cancel))
                    {
                        CancelOrderRequest(orderRequest.OrderDetails);
                    }
                }
                else
                {
                    if (Logger.IsInfoEnabled)
                    {
                        Logger.Info(orderRequest.OrderDetails.Provider + " provider not connected.",
                            _type.FullName, "NewOrderRequest");
                    }
                }

            }
            else
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info(orderRequest.OrderDetails.Provider + " provider not found.",
                        _type.FullName, "NewOrderRequest");
                }
            }
        }

        /// <summary>
        /// Called if the incoming order request is for Market Order
        /// </summary>
        /// <param name="orderDetails">Contains all order details</param>
        private void MarketOrderRequest(OrderDetails orderDetails)
        {
            // Forward market order request
            _orderExecutionManager.MarketOrderRequests(orderDetails);
        }

        /// <summary>
        /// Called if the incoming order request is for Limit Order
        /// </summary>
        /// <param name="orderDetails">Contains all order details</param>
        private void LimitOrderRequest(OrderDetails orderDetails)
        {
            // Forward limit order request
            _orderExecutionManager.LimitOrderRequest(orderDetails);
        }

        /// <summary>
        /// Called if the incoming request is to cancel an existing order
        /// </summary>
        /// <param name="orderDetails">Contains all order details</param>
        private void CancelOrderRequest(OrderDetails orderDetails)
        {
            // Forward Cancellation request
            _orderExecutionManager.CancelOrderRequest(orderDetails.ID);
        }

        #endregion

        #region Order Execution Manager Events

        /// <summary>
        /// Called when requested provider is successfully 'Logged ON'
        /// </summary>
        /// <param name="providerName"></param>
        private void OnLogonArrived(string providerName)
        {
            OrderExecutionProvider provider;
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
            OrderExecutionProvider provider;
            if (_providersMap.TryGetValue(providerName, out provider))
            {
                provider.ConnectionStatus = ConnectionStatus.Disconnected;
            }
        }

        /// <summary>
        /// Called when the requested order is accepted
        /// </summary>
        /// <param name="order">Contains accepted order details</param>
        private void OnOrderAccepted(Order order)
        {
            OrderExecutionProvider provider;

            // Get Order Execution Provider
            if (_providersMap.TryGetValue(order.OrderExecutionProvider, out provider))
            {
                OrderDetails orderDetails = provider.GetOrderDetail(order.OrderID, order.OrderStatus);

                // Update order status
                if (orderDetails != null) 
                    orderDetails.Status = order.OrderStatus;
            }
        }

        /// <summary>
        /// Called when order execution is receievd
        /// </summary>
        /// <param name="execution">Contains execution details</param>
        private void OnExecutionArrived(Execution execution)
        {
            OrderExecutionProvider provider;

            // Get Order Execution Provider
            if (_providersMap.TryGetValue(execution.OrderExecutionProvider, out provider))
            {
                OrderDetails orderDetails = null;

                // Find 'Order Details' object 
                foreach (OrderDetails tempOrderDetails in provider.OrdersCollection)
                {
                    if (tempOrderDetails.ID.Equals(execution.Order.OrderID))
                    {
                        orderDetails = tempOrderDetails;
                        break;
                    }
                }

                // Update order parameters
                if (orderDetails != null)
                {
                    orderDetails.Status = execution.Order.OrderStatus;

                    // Create fill information
                    var fillDetail = new FillDetail();

                    fillDetail.FillId = execution.Fill.ExecutionId;
                    fillDetail.FillPrice = execution.Fill.ExecutionPrice;
                    fillDetail.FillQuantity = execution.Fill.ExecutionSize;
                    fillDetail.FillDatetime = execution.Fill.ExecutionDateTime;
                    fillDetail.FillType = execution.Fill.ExecutionType;

                    // Add to order details object
                    orderDetails.FillDetails.Add(fillDetail);

                    // Use incoming information to update position statistics
                    provider.UpdatePosition(orderDetails);
                }
            }
        }

        /// <summary>
        /// Called when requested order is rejected
        /// </summary>
        /// <param name="rejection">Contains rejection details</param>
        private void OnRejectionArrived(Rejection rejection)
        {
            OrderExecutionProvider provider;

            // Get Order Execution Provider
            if (_providersMap.TryGetValue(rejection.OrderExecutionProvider, out provider))
            {
                OrderDetails orderDetails = provider.GetOrderDetail(rejection.OrderId, OrderStatus.REJECTED);

                // Update order parameters
                if (orderDetails != null)
                    orderDetails.Status = OrderStatus.REJECTED;
            }
        }

        /// <summary>
        /// Called when order cancellaiton request is successful
        /// </summary>
        /// <param name="order">Contains cancelled order details</param>
        private void OnCancellationArrived(Order order)
        {
            OrderExecutionProvider provider;

            // Get Order Execution Provider
            if (_providersMap.TryGetValue(order.OrderExecutionProvider, out provider))
            {
                OrderDetails orderDetails = provider.GetOrderDetail(order.OrderID, OrderStatus.CANCELLED);

                // Update order parameters
                if (orderDetails != null)
                    orderDetails.Status = OrderStatus.CANCELLED;
            }
        }

        #endregion

        /// <summary>
        /// Stops all order execution related activities
        /// </summary>
        public void Stop()
        {
            // Send logout for each connected order execution provider
            foreach (KeyValuePair<string, OrderExecutionProvider> keyValuePair in _providersMap)
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
