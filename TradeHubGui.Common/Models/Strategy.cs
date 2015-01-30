using System;
using System.Collections.Generic;
using TradeHubGui.Common.Utility;
using TradeHubGui.Common.ValueObjects;

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
        /// Strategy Type extracted from Assembly
        /// </summary>
        private Type _strategyType;

        /// <summary>
        /// Contains Parameter details to be used by Strategy
        /// Key = Parameter Name
        /// Value = Parameter Type (e.g. Int32, String, Decimal, etc.) , Parameter Value if entered
        /// </summary>
        private Dictionary<string, ParameterDetail> _parameterDetails; 

        /// <summary>
        /// Contains all strategy instances for the current strategy
        /// </summary>
        private IDictionary<string, StrategyInstance> _strategyInstances;

        #region Properties

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

        /// <summary>
        /// Strategy Type extracted from Assembly
        /// </summary>
        public Type StrategyType
        {
            get { return _strategyType; }
            set { _strategyType = value; }
        }

        /// <summary>
        /// Contains Parameter details to be used by Strategy
        /// Key = Parameter Name
        /// Value = Parameter Type (e.g. Int32, String, Decimal, etc.)
        /// </summary>
        public Dictionary<string, ParameterDetail> ParameterDetails
        {
            get { return _parameterDetails; }
            set { _parameterDetails = value; }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="name">Strategy Name</param>
        /// <param name="strategyType">Strategy Assembly Type</param>
        public Strategy(string name, Type strategyType)
        {
            // Get new strategy ID
            _key = StrategyIdGenerator.GetStrategyKey();

            // Save information
            _name = name;
            _strategyType = strategyType;

            // Initialize fields
            _parameterDetails = new Dictionary<string, ParameterDetail>();
            _strategyInstances= new Dictionary<string, StrategyInstance>();
        }

        /// <summary>
        /// Creates a new Strategy Instance object
        /// </summary>
        /// <param name="parameters">Parameter list to be used by the instance for execution</param>
        public StrategyInstance CreateInstance(object[] parameters)
        {
            // Get new Instance Key
            string instanceKey = StrategyIdGenerator.GetInstanceKey(_key);

            // Create new Strategy Instance Object
            var strategyInstance = new StrategyInstance()
            {
                InstanceKey = instanceKey,
                Parameters = parameters,
                StrategyType = _strategyType
            };

            // Add to local MAP
            _strategyInstances.Add(instanceKey, strategyInstance);

            // Return Instance
            return strategyInstance;
        }
    }
}
