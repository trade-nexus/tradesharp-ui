using System;
using System.Collections.Generic;
using System.Windows.Media;
using TradeHubGui.Common.ValueObjects;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains Individual Strategy Instance information
    /// </summary>
	public class StrategyInstance
    {
        /// <summary>
        /// Unique Key to identify instance
        /// </summary>
        private string _instanceKey;

        /// <summary>
        /// Brief Strategy Description
        /// </summary>
        private string _description;

        /// <summary>
        /// Parameter values to used
        /// </summary>
        private object[] _parameters;

        /// <summary>
        /// Strategy Type containing TradeHubStrategy
        /// </summary>
        private Type _strategyType;

        /// <summary>
        /// Holds basic execution information for the current instance to be used for UI
        /// </summary>
        private ExecutionDetails _executionDetails;

        #region Properties

        /// <summary>
        /// Unique Key to identify instance
        /// </summary>
        public string InstanceKey
        {
            get { return _instanceKey; }
            set { _instanceKey = value; }
        }

        /// <summary>
        /// Brief Strategy Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Parameter values to used
        /// </summary>
        public object[] Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        /// <summary>
        /// Strategy Type containing TradeHubStrategy
        /// </summary>
        public Type StrategyType
        {
            get { return _strategyType; }
            set { _strategyType = value; }
        }

        //NOTE: Might not be required
        public string Symbol { get; set; }

        /// <summary>
        /// TODO: this is temporary property for Execution State of instance
        /// NOTE: We can use 'StrategyStatus' from 'TradeHub.Common.Core', its an enum with values: None, Executing, Executed
        /// Currently it is string, but maybe some enum with all possible states for instance (running, stopped ... )
        /// </summary>
        public String ExecutionState { get; set; }

        /// <summary>
        /// Holds basic execution information for the current instance to be used for UI
        /// </summary>
        public ExecutionDetails ExecutionDetails
        {
            get { return _executionDetails; }
            set { _executionDetails = value; }
        }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public StrategyInstance()
        {
            // Initialize
            _executionDetails = new ExecutionDetails();

            // Use Instance Key to identify its execution information
            _executionDetails.Key = _instanceKey;
        }
        
        /// <summary>
        /// Adds new order details to the local map
        /// </summary>
        /// <param name="orderDetails"></param>
        public void AddOrderDetails(OrderDetails orderDetails)
        {
            _executionDetails.AddOrderDetails(orderDetails);
        }
    }
}
