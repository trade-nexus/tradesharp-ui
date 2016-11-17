/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;
using TradeHubConstants = TradeHub.Common.Core.Constants;

namespace TradeHubGui.ViewModel
{
    public class OrdersViewModel : BaseViewModel
    {
        #region Fields

        private string _filterString;

        private OrderDetails _selectedOrderDetail;
        private OrderExecutionProvider _selectedProvider;
        private PositionStatistics _selectedPosition;

        private ICollectionView _orderCollection;

        private ObservableCollection<OrderDetails> _orders;
        private ObservableCollection<FillDetail> _fillDetailCollection;
        private ObservableCollection<PositionStatistics> _positionStatisticsCollection; 
        private ObservableCollection<OrderExecutionProvider> _executionProviders;

        private RelayCommand _closePositionCommand;
        private RelayCommand _cancelOrderCommand;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrdersViewModel()
        {
            _orders = new ObservableCollection<OrderDetails>();
            _fillDetailCollection = new ObservableCollection<FillDetail>();
            _executionProviders = new ObservableCollection<OrderExecutionProvider>();
            _positionStatisticsCollection = new ObservableCollection<PositionStatistics>();

            _orderCollection = CollectionViewSource.GetDefaultView(_orders);
            if(_orderCollection != null && _orderCollection.CanSort == true)
            {
                _orderCollection.SortDescriptions.Clear();
                _orderCollection.SortDescriptions.Add(new SortDescription("Security.Symbol", ListSortDirection.Ascending));
                _orderCollection.Filter = OrderCollectionFilter;
            }

            PopulateExecutionProviders();
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Collection of OrderDetails
        /// </summary>
        public ObservableCollection<OrderDetails> Orders
        {
            get { return _orders; }
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged("Orders");
                }
            }
        }

        /// <summary>
        /// ICollectionView used as items source for DataGrid
        /// </summary>
        public ICollectionView OrderCollection
        {
            get { return _orderCollection; }
        }

        /// <summary>
        /// Filter string used to find OrderDetails by symbol
        /// </summary>
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                OnPropertyChanged("FilterString");
                _orderCollection.Refresh();
            }
        }

        /// <summary>
        /// Contains all available providers
        /// </summary>
        public ObservableCollection<OrderExecutionProvider> ExecutionProviders
        {
            get { return _executionProviders; }
            set
            {
                _executionProviders = value;
                OnPropertyChanged("ExecutionProviders");
            }
        }

        /// <summary>
        /// Contains Position Stats for all traded symbols for the selected Execution Provider
        /// </summary>
        public ObservableCollection<PositionStatistics> PositionStatisticsCollection
        {
            get { return _positionStatisticsCollection; }
            set
            {
                _positionStatisticsCollection = value;
                OnPropertyChanged("PositionStatisticsCollection");
            }
        }

        /// <summary>
        /// Currently User selected Order Execution Provider
        /// </summary>
        public OrderExecutionProvider SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                _selectedProvider = value;
                if (value != null)
                    PopulateOrderDetails();

                OnPropertyChanged("SelectedProvider");
            }
        }

        /// <summary>
        /// Contains all fills for the selected order
        /// </summary>
        public ObservableCollection<FillDetail> FillDetailCollection
        {
            get { return _fillDetailCollection; }
            set
            {
                _fillDetailCollection = value;
                OnPropertyChanged("FillDetailCollection");
            }
        }

        /// <summary>
        /// Currently selected order in the UI
        /// </summary>
        public OrderDetails SelectedOrderDetail
        {
            get { return _selectedOrderDetail; }
            set
            {
                _selectedOrderDetail = value;
                if (value != null)
                    PopulateFillDetail();

                OnPropertyChanged("SelectedOrderDetail");
            }
        }

        /// <summary>
        /// Currently Selected Position Statistics object from the grid view
        /// </summary>
        public PositionStatistics SelectedPosition
        {
            get { return _selectedPosition; }
            set
            {
                _selectedPosition = value;
                OnPropertyChanged("SelectedPosition");
            }
        }

        #endregion

        #region Commands
        
        /// <summary>
        /// Used for 'Close' button for sending an order to close position on given symbol
        /// </summary>
        public ICommand ClosePositionCommand
        {
            get
            {
                return _closePositionCommand ??
                       (_closePositionCommand =
                           new RelayCommand(param => ClosePositionExecute()));
            }
        }

        /// <summary>
        /// Used for 'Cancel' button for cancelling an existing order
        /// </summary>
        public ICommand CancelOrderCommand
        {
            get
            {
                return _cancelOrderCommand ??
                       (_cancelOrderCommand =
                           new RelayCommand(param => CancelOrderExecute()));
            }
        }

        #endregion

        #region Command Trigger Methods

        /// <summary>
        /// Called when 'Close' button is clicked
        /// </summary>
        private void ClosePositionExecute()
        {
            ClosePosition();
        }

        /// <summary>
        /// Called when 'Cancel' button is clicked
        /// </summary>
        private void CancelOrderExecute()
        {
            CancelOrder();
        }

        #endregion

        #region Methods

        private bool OrderCollectionFilter(object item)
        {
            OrderDetails orderDetails = item as OrderDetails;
            if (_filterString != null)
            {
                return orderDetails.Security.Symbol.ToLower().Contains(_filterString);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Populates values in Execution Providers Collection
        /// </summary>
        private void PopulateExecutionProviders()
        {
            foreach (var provider in ProvidersController.OrderExecutionProviders)
            {
                _executionProviders.Add(provider);
            }

            // Select the 1st provider initially
            SelectedProvider = _executionProviders.Count > 0 ? _executionProviders[0] : null;
        }

        /// <summary>
        /// Displays Order Details for the selected Execution Provider
        /// </summary>
        private void PopulateOrderDetails()
        {
            // Clear current values
            Orders = new ObservableCollection<OrderDetails>();
            PositionStatisticsCollection = new ObservableCollection<PositionStatistics>();

            // Set New Values
            Orders = SelectedProvider.OrdersCollection;
            PositionStatisticsCollection = SelectedProvider.PositionStatisticsCollection;
        }

        /// <summary>
        /// Displays Fill Details for the selected Order
        /// </summary>
        private void PopulateFillDetail()
        {
            // Clear current values
            FillDetailCollection = new ObservableCollection<FillDetail>();

            // Set New Values
            FillDetailCollection = SelectedOrderDetail.FillDetails;
        }
        
        /// <summary>
        /// Closes current open position for the selected Position 
        /// </summary>
        private void ClosePosition()
        {
            // Find Order Side
            string orderSide = SelectedPosition.Position > 0 ? OrderSide.SELL : OrderSide.BUY;

            // Find Order Quantity
            int orderQuantity = Math.Abs(SelectedPosition.Position);

            // Create a new Object which will be used across the application
            OrderDetails orderDetails = new OrderDetails(SelectedProvider.ProviderName);

            orderDetails.Price = 0;
            orderDetails.StopPrice = 0;
            orderDetails.Quantity = orderQuantity;
            orderDetails.Side = orderSide;
            orderDetails.Type = OrderType.Market;
            orderDetails.Security = SelectedPosition.Security;

            // Add to selected provider collection for future reference and updates
            SelectedProvider.AddOrder(orderDetails);

            // Create new order request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.New);

            // Raise event to notify listener
            EventSystem.Publish<OrderRequest>(orderRequest);
        }

        /// <summary>
        /// Sends order cancellation request for the selected symbol
        /// </summary>
        private void CancelOrder()
        {
            // Create a new Object which will be used to make a cancellation request
            OrderDetails orderDetails = new OrderDetails(SelectedOrderDetail.Provider);

            // Use existing orders ID to identity the order during cancellation
            orderDetails.ID = SelectedOrderDetail.ID;

            // Create cancel order request
            OrderRequest orderRequest = new OrderRequest(orderDetails, OrderRequestType.Cancel);

            // Raise event to notify listener
            EventSystem.Publish<OrderRequest>(orderRequest);
        }

        #endregion

        #region Events
        #endregion
    }
}
