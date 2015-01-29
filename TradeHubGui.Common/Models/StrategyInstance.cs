using System;
using System.Windows.Media;

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

        public SolidColorBrush StateBrush { get; set; }
    }
}
