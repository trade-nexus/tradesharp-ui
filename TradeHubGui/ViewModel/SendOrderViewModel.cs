using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;

namespace TradeHubGui.ViewModel
{
    public class SendOrderViewModel :  BaseViewModel
    {
        private Type _type = typeof (SendOrderViewModel);

        #region Fields

        /// <summary>
        /// Holds reference of currently selected order execution provider
        /// </summary>
        private OrderExecutionProvider _selectedOrderExecutionProvider;

        /// <summary>
        /// Contains information for the order to be sent
        /// </summary>
        private SendOrderModel _orderModel;

        /// <summary>
        /// Holds selected order type
        /// </summary>
        private OrderType _selectedOrderType;

        /// <summary>
        /// Position statistics for the currently selected Symbol on given Order Execution Provider
        /// </summary>
        private PositionStatistics _positionStatistics;

        /// <summary>
        /// Contains supported order types
        /// </summary>
        private ObservableCollection<OrderType> _orderTypesCollection; 

        /// <summary>
        /// Contains collection of available Order Execution Providers
        /// </summary>
        private ObservableCollection<OrderExecutionProvider> _orderExecutionProviders;

        private RelayCommand _sendBuyOrderCommand;
        private RelayCommand _sendSellOrderCommand;
        private RelayCommand _closePositionCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Holds reference of currently selected order execution provider
        /// </summary>
        public OrderExecutionProvider SelectedOrderExecutionProvider
        {
            get { return _selectedOrderExecutionProvider; }
            set
            {
                _selectedOrderExecutionProvider = value;
                
                if (value != null && OrderModel.Security != null)
                {
                    UpdatePositionInformation(OrderModel.Security);
                }

                OnPropertyChanged("SelectedOrderExecutionProvider");
            }
        }

        /// <summary>
        /// Holds selected order type
        /// </summary>
        public OrderType SelectedOrderType
        {
            get { return _selectedOrderType; }
            set
            {
                _selectedOrderType = value;
                OnPropertyChanged("SelectedOrderType");
            }
        }

        /// <summary>
        /// Position statistics for the currently selected Symbol on given Order Execution Provider
        /// </summary>
        public PositionStatistics PositionStatistics
        {
            get { return _positionStatistics; }
            set
            {
                _positionStatistics = value;
                OnPropertyChanged("PositionStatistics");
            }
        }

        /// <summary>
        /// Contains collection of available Order Execution Providers
        /// </summary>
        public ObservableCollection<OrderExecutionProvider> OrderExecutionProviders
        {
            get { return _orderExecutionProviders; }
            set { _orderExecutionProviders = value; }
        }

        /// <summary>
        /// Contains information for the order to be sent
        /// </summary>
        public SendOrderModel OrderModel
        {
            get { return _orderModel; }
            set
            {
                _orderModel = value;
                OnPropertyChanged("OrderModel");
            }
        }

        /// <summary>
        /// Contains supported order types
        /// </summary>
        public ObservableCollection<OrderType> OrderTypesCollection
        {
            get { return _orderTypesCollection; }
            set
            {
                _orderTypesCollection = value;
                OnPropertyChanged("OrderTypesCollection");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Used for 'Buy' button for sending a Buy order with the selected parameters
        /// </summary>
        public ICommand SendBuyOrderCommand
        {
            get
            {
                return _sendBuyOrderCommand ??
                       (_sendBuyOrderCommand =
                           new RelayCommand(param => SendBuyOrderExecute(), param => SendBuyOrderCanExecute()));
            }
        }

        /// <summary>
        /// Used for 'Sell' button for sending a Sell order with the selected parameters
        /// </summary>
        public ICommand SendSellOrderCommand
        {
            get
            {
                return _sendSellOrderCommand ??
                       (_sendSellOrderCommand =
                           new RelayCommand(param => SendSellOrderExecute(), param => SendSellOrderCanExecute()));
            }
        }

        /// <summary>
        /// Used for 'Close' button for sending an order to close position on given symbol
        /// </summary>
        public ICommand ClosePositionCommand
        {
            get
            {
                return _closePositionCommand ??
                       (_closePositionCommand =
                           new RelayCommand(param => ClosePositionExecute(), param => ClosePositionCanExecute()));
            }
        }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SendOrderViewModel()
        {
            _orderModel = new SendOrderModel();

            InitializeOrderTypes();
            InitializeOrderExecutionProviders();
        }

        #region Command Trigger Methods

        private bool SendBuyOrderCanExecute()
        {
            return SelectedOrderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Connected);
        }

        /// <summary>
        /// Called when 'Buy' button is clicked
        /// </summary>
        private void SendBuyOrderExecute()
        {
            SendOrder(OrderSide.BUY, OrderModel.BuyPrice);
        }

        private bool SendSellOrderCanExecute()
        {
            return SelectedOrderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Connected);
        }

        /// <summary>
        /// Called when 'Sell' button is clicked
        /// </summary>
        private void SendSellOrderExecute()
        {
            SendOrder(OrderSide.SELL, OrderModel.SellPrice);
        }

        private bool ClosePositionCanExecute()
        {
            return SelectedOrderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Connected)
                   && Math.Abs(PositionStatistics.Position) > 0;
        }

        /// <summary>
        /// Called when 'Close' button is clicked
        /// </summary>
        private void ClosePositionExecute()
        {
            ClosePosition();
        }

        #endregion

        /// <summary>
        /// Initialization of order execution providers
        /// </summary>
        private void InitializeOrderExecutionProviders()
        {
            _orderExecutionProviders = new ObservableCollection<OrderExecutionProvider>();

            // Populate Individual Order Execution Provider details
            foreach (var provider in ProvidersController.OrderExecutionProviders)
            {
                // Add to Collection
                _orderExecutionProviders.Add(provider);
            }

            // Select initially 1st provider in ComboBox
            if (_orderExecutionProviders != null && _orderExecutionProviders.Count > 0)
                SelectedOrderExecutionProvider = _orderExecutionProviders[0];
        }

        /// <summary>
        /// Initializes supported order types
        /// </summary>
        private void InitializeOrderTypes()
        {
            _orderTypesCollection = new ObservableCollection<OrderType>();

            _orderTypesCollection.Add(OrderType.Market);
            _orderTypesCollection.Add(OrderType.Limit);

            // Select initially 1st order type in ComboBox
            if (_orderTypesCollection != null && _orderTypesCollection.Count > 0)
                SelectedOrderType = _orderTypesCollection[0];
        }

        /// <summary>
        /// Send a new Order Request
        /// </summary>
        /// <param name="orderSide">Order Side 'BUY/SELL'</param>
        /// <param name="orderPrice">limit price at which to send the order</param>
        private void SendOrder(string orderSide, decimal orderPrice)
        {
            // Create a new Object which will be used across the application
            OrderDetails orderDetails = new OrderDetails();

            orderDetails.Price = SelectedOrderType.Equals(OrderType.Market) ? 0 : orderPrice;
            orderDetails.StopPrice = OrderModel.TriggerPrice;
            orderDetails.Quantity = OrderModel.Size;
            orderDetails.Side = orderSide;
            orderDetails.Type = SelectedOrderType;
            orderDetails.Security = OrderModel.Security;
            orderDetails.Provider = SelectedOrderExecutionProvider.ProviderName;

            // Add to selected provider collection for future reference and updates
            SelectedOrderExecutionProvider.AddOrder(orderDetails);

            // Create new order request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

            // Raise event to notify listener
            EventSystem.Publish<OrderRequest>(orderRequest);
        }

        /// <summary>
        /// Closes current open position for the selected Symbol
        /// </summary>
        private void ClosePosition()
        {
            // Find Order Side
            string orderSide = PositionStatistics.Position > 0 ? OrderSide.SELL : OrderSide.BUY;

            // Find Order Quantity
            int orderQuantity = Math.Abs(PositionStatistics.Position);

            // Create a new Object which will be used across the application
            OrderDetails orderDetails = new OrderDetails();

            orderDetails.Price = 0 ;
            orderDetails.StopPrice = 0;
            orderDetails.Quantity = orderQuantity;
            orderDetails.Side = orderSide;
            orderDetails.Type = OrderType.Market;
            orderDetails.Security = OrderModel.Security;
            orderDetails.Provider = SelectedOrderExecutionProvider.ProviderName;

            // Add to selected provider collection for future reference and updates
            SelectedOrderExecutionProvider.AddOrder(orderDetails);

            // Create new order request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

            // Raise event to notify listener
            EventSystem.Publish<OrderRequest>(orderRequest);
        }

        /// <summary>
        /// Sets selected Order Execution Provider depending on the incoming provider name
        /// </summary>
        /// <param name="providerName">Order Execution Provider name</param>
        public bool SetOrderExecutionProvider(string providerName)
        {
            // Traverse all available providers
            foreach (OrderExecutionProvider orderExecutionProvider in _orderExecutionProviders)
            {
                if (orderExecutionProvider.ProviderName.EndsWith(providerName))
                {
                    SelectedOrderExecutionProvider = orderExecutionProvider;
                    return true;
                }
            }

            // Provider not found
            return false;
        }

        /// <summary>
        /// Sets requried security information to be displayed on UI and used while sending out order requests
        /// </summary>
        /// <param name="security">Contains Symbol information</param>
        /// <param name="bidPrice">Current Bid price</param>
        /// <param name="askPrice">Current Ask Price</param>
        public void SetSecurityInformation(Security security, decimal bidPrice, decimal askPrice)
        {
            OrderModel.Security = security;
            OrderModel.SellPrice = askPrice;
            OrderModel.BuyPrice = bidPrice;

            UpdatePositionInformation(security);
        }

        /// <summary>
        /// Update variable to display Position Statistics on UI
        /// </summary>
        /// <param name="security">Contains Symbol information</param>
        private void UpdatePositionInformation(Security security)
        {
            // Fetch Position Information
            var postionStatistics = SelectedOrderExecutionProvider.GetPositionStatistics(security.Symbol);

            // Update values to show on UI
            PositionStatistics = postionStatistics ?? new PositionStatistics(security);
        }
    }
}
