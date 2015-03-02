using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.ViewModel
{
    public class SendOrderViewModel :  BaseViewModel
    {
        private Type _type = typeof (SendOrderViewModel);

        #region Fields

        /// <summary>
        /// Holds reference of currently selected order execution provider
        /// </summary>
        private Provider _selectedOrderExecutionProvider;

        /// <summary>
        /// Contains information for the order to be sent
        /// </summary>
        private SendOrderModel _orderModel;

        /// <summary>
        /// Holds selected order type
        /// </summary>
        private OrderType _selectedOrderType;

        /// <summary>
        /// Contains supported order types
        /// </summary>
        private ObservableCollection<OrderType> _orderTypesCollection; 

        /// <summary>
        /// Contains collection of available Order Execution Providers
        /// </summary>
        private ObservableCollection<Provider> _orderExecutionProviders;

        private RelayCommand _sendBuyOrderCommand;
        private RelayCommand _sendSellOrderCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Holds reference of currently selected order execution provider
        /// </summary>
        public Provider SelectedOrderExecutionProvider
        {
            get { return _selectedOrderExecutionProvider; }
            set
            {
                _selectedOrderExecutionProvider = value;
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
        /// Contains collection of available Order Execution Providers
        /// </summary>
        public ObservableCollection<Provider> OrderExecutionProviders
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
                return _sendBuyOrderCommand ?? (_sendBuyOrderCommand = new RelayCommand(param => SendBuyOrderExecute()));
            }
        }

        /// <summary>
        /// Used for 'Sell' button for sending a Sell order with the selected parameters
        /// </summary>
        public ICommand SendSellOrderCommand
        {
            get
            {
                return _sendSellOrderCommand ?? (_sendSellOrderCommand = new RelayCommand(param => SendSellOrderExecute()));
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

        /// <summary>
        /// Called when 'Buy' button is clicked
        /// </summary>
        private void SendBuyOrderExecute()
        {
            SendOrder(OrderSide.BUY);
        }

        /// <summary>
        /// Called when 'Sell' button is clicked
        /// </summary>
        private void SendSellOrderExecute()
        {
            SendOrder(OrderSide.SELL);
        }

        #endregion

        /// <summary>
        /// Initialization of order execution providers
        /// </summary>
        private void InitializeOrderExecutionProviders()
        {
            _orderExecutionProviders = new ObservableCollection<Provider>();

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
        private void SendOrder(string orderSide)
        {
            // Create a new Object which will be used across the application
            OrderDetails orderDetails = new OrderDetails();

            orderDetails.Price = OrderModel.BuyPrice;
            orderDetails.StopPrice = OrderModel.TriggerPrice;
            orderDetails.Quantity = OrderModel.Size;
            orderDetails.Side = orderSide;
            orderDetails.Type = SelectedOrderType;
            orderDetails.Security = OrderModel.Security;
            orderDetails.Provider = SelectedOrderExecutionProvider.ProviderName;

            // Add to selected provider collection for future reference and updates
            SelectedOrderExecutionProvider.OrdersCollection.Add(orderDetails);

            // Create new order request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

            // Raise event to notify listener
            EventSystem.Publish<OrderRequest>(orderRequest);
        }
    }
}
