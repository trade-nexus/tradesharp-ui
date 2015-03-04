using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels.OrderDomain;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Generic Provider class used for Order Execution Provider
    /// </summary>
    public class OrderExecutionProvider : Provider
    {
        #region Fields

        /// <summary>
        /// Contains current active orders for the given provider
        /// </summary>
        private List<OrderDetails> _activeOrdersList;

        /// <summary>
        /// Contains all orders information during the current application session
        /// </summary>
        private ObservableCollection<OrderDetails> _ordersCollection;

        /// <summary>
        /// Contains positions stats for all traded securities
        /// KEY = Symbol
        /// VALUE = Position information <see cref="PositionStatistics"/>
        /// </summary>
        private Dictionary<string, PositionStatistics> _positionStatisticsCollection;

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrderExecutionProvider()
        {
            _activeOrdersList = new List<OrderDetails>();
            _ordersCollection = new ObservableCollection<OrderDetails>();
            _positionStatisticsCollection = new Dictionary<string, PositionStatistics>();
        }

        #region Properties

        /// <summary>
        /// Contains all orders information during the current application session
        /// </summary>
        public ObservableCollection<OrderDetails> OrdersCollection
        {
            get { return _ordersCollection; }
            set { _ordersCollection = value; }
        }

        /// <summary>
        /// Contains current active orders for the given provider
        /// </summary>
        public List<OrderDetails> ActiveOrdersList
        {
            get { return _activeOrdersList; }
            set { _activeOrdersList = value; }
        }

        /// <summary>
        /// Contains positions stats for all traded securities
        /// KEY = Symbol
        /// VALUE = Position information <see cref="PositionStatistics"/>
        /// </summary>
        public Dictionary<string, PositionStatistics> PositionStatisticsCollection
        {
            get { return _positionStatisticsCollection; }
            set { _positionStatisticsCollection = value; }
        }

        #endregion

        /// <summary>
        /// Adds a new order to existing orders list/collection
        /// </summary>
        /// <param name="orderDetails">Contains order information</param>
        public void AddOrder(OrderDetails orderDetails)
        {
            // Add to active orders list
            ActiveOrdersList.Add(orderDetails);

            // Add to global orders collection
            OrdersCollection.Add(orderDetails);
        }

        /// <summary>
        /// Returns Order Details object for the given 'Order-ID'
        /// </summary>
        /// <param name="orderId">Unique ID to identity order</param>
        /// <param name="orderStatus">Order Status</param>
        /// <returns>Order Details object</returns>
        public OrderDetails GetOrderDetail(string orderId, string orderStatus)
        {
            OrderDetails orderDetails = null;

            // Find 'Order Details' object 
            foreach (OrderDetails tempOrderDetails in _activeOrdersList)
            {
                if (tempOrderDetails.ID.Equals(orderId))
                {
                    orderDetails = tempOrderDetails;

                    // Remove from active Orders list if no more updates are expected
                    if (orderStatus.Equals(OrderStatus.EXECUTED) || orderStatus.Equals(OrderStatus.CANCELLED) || orderStatus.Equals(OrderStatus.REJECTED))
                    {
                        _activeOrdersList.Remove(tempOrderDetails);
                    }

                    // terminate loop
                    break;
                }
            }

            return orderDetails;
        }

        /// <summary>
        /// Update position statistics for a given Security (Symbol)
        /// </summary>
        /// <param name="orderDetails">Order details</param>
        public void UpdatePosition(OrderDetails orderDetails)
        {
            PositionStatistics statistics;
            // Find Existing Statistics for the incoming order security
            if (!PositionStatisticsCollection.TryGetValue(orderDetails.Security.Symbol, out statistics))
            {
                // Create a new object 
                statistics = new PositionStatistics(orderDetails.Security);

                // Add to collection
                PositionStatisticsCollection.Add(orderDetails.Security.Symbol, statistics);
            }

            // Handle BUY Order
            if (orderDetails.Side.Equals(OrderSide.BUY))
            {
                statistics.Position += orderDetails.Quantity;
                statistics.Pnl -= orderDetails.Quantity*orderDetails.FillDetails.Last().FillPrice;
            }
            else
            {
                statistics.Position -= orderDetails.Quantity;
                statistics.Pnl += orderDetails.Quantity * orderDetails.FillDetails.Last().FillPrice;
            }
        }
    }
}
