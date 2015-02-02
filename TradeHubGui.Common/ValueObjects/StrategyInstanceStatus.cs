using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.Common.ValueObjects
{
    /// <summary>
    /// Indicates State of Strategy
    /// </summary>
    public class StrategyInstanceStatus
    {
        private string _strategyKey = string.Empty;
        private string _instanceKey = string.Empty;
        private StrategyStatus _strategyStatus;

        /// <summary>
        /// Unique Key to identify Strategy
        /// </summary>
        public string StrategyKey
        {
            get { return _strategyKey; }
            set { _strategyKey = value; }
        }

        /// <summary>
        /// Unique Key to identify Strategy Instance
        /// </summary>
        public string InstanceKey
        {
            get { return _instanceKey; }
            set { _instanceKey = value; }
        }

        /// <summary>
        /// Current Execution Status of Stratgey Instance i.e. 'None' | 'Executing' | 'Executed'
        /// </summary>
        public StrategyStatus StrategyStatus
        {
            get { return _strategyStatus; }
            set { _strategyStatus = value; }
        }
    }
}
