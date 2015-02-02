using System.Collections.Generic;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels.OrderDomain;
using OrderStatus = TradeHub.Common.Core.Constants.OrderStatus;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// 
    /// </summary>
	public class ExecutionDetails
    {
        /// <summary>
        /// List of all the Order detials during the current session
        /// </summary>
        private IList<OrderDetails> _orderDetailsList; 

		public string Key { get; set; }
		public int Executed { get; set; }
		public int BuyCount { get; set; }
		public int SellCount { get; set; }

        /// <summary>
        /// List of all the Order detials during the current session
        /// </summary>
        public IList<OrderDetails> OrderDetailsList
        {
            get { return _orderDetailsList; }
            set { _orderDetailsList = value; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExecutionDetails()
        {
            // Initialize Objects
            OrderDetailsList = new List<OrderDetails>();
        }

        /// <summary>
        /// Adds new order details to the local map
        /// </summary>
        /// <param name="orderDetails"></param>
        public void AddOrderDetails(OrderDetails orderDetails)
        {
            // Counts are to be incremented if incoming order detail is for execution
            if (orderDetails.Status.Equals(OrderStatus.EXECUTED))
            {
                Executed++;

                if (orderDetails.Side.Equals(OrderSide.BUY))
                {
                    BuyCount++;
                }
                else
                {
                    SellCount++;
                }
            }

            // Add to local Map
            _orderDetailsList.Add(orderDetails);
        }
	}
}
