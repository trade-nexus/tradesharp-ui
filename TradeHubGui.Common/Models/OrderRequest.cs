using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains information related to order request made by the user from UI
    /// </summary>
    public class OrderRequest
    {
        /// <summary>
        /// Contains order information
        /// </summary>
        private OrderDetails _orderDetails;

        /// <summary>
        /// Type of order request
        /// </summary>
        private OrderRequestType _requestType;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="orderDetails">Contains order information</param>
        /// <param name="requestType">Type of order request</param>
        public OrderRequest(OrderDetails orderDetails, OrderRequestType requestType)
        {
            _orderDetails = orderDetails;
            _requestType = requestType;
        }

        /// <summary>
        /// Contains order information
        /// </summary>
        public OrderDetails OrderDetails
        {
            get { return _orderDetails; }
        }

        /// <summary>
        /// Type of order request
        /// </summary>
        public OrderRequestType RequestType
        {
            get { return _requestType; }
        }
    }
}
