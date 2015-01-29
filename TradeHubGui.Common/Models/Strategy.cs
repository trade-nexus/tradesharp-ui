using System.Collections.Generic;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains Strategy basic information
    /// </summary>
	public class Strategy
    {
        /// <summary>
        /// Unique to distinguish Strategy 
        /// </summary>
        private string _key;

        /// <summary>
        /// Strategy Name to display
        /// </summary>
        private string _name;

        /// <summary>
        /// Contains all strategy instances for the current strategy
        /// </summary>
        private IDictionary<string, StrategyInstance> _strategyInstances;

        /// <summary>
        /// Unique to distinguish Strategy 
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Strategy Name to display
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Contains all strategy instances for the current strategy
        /// </summary>
        public IDictionary<string, StrategyInstance> StrategyInstances
        {
            get { return _strategyInstances; }
            set { _strategyInstances = value; }
        }
    }
}
