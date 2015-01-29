using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.StrategyRunner.Representations
{
    /// <summary>
    /// Representation strategy status
    /// </summary>
    public class StrategyStatusRepresentation
    {
        public string InstanceKey { get; set; }
        public StrategyStatus StrategyStatus { get; set;}

        /// <summary>
        /// Defualt constructor
        /// </summary>
        public StrategyStatusRepresentation()
        {
            
        }

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <param name="strategyStatus"></param>
        public StrategyStatusRepresentation(string instanceKey, StrategyStatus strategyStatus)
        {
            InstanceKey = instanceKey;
            StrategyStatus = strategyStatus;
        }
    }
}
