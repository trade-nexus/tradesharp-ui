using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.DomainModels.OrderDomain;

namespace TradeHubGui.StrategyRunner.Representations
{
    /// <summary>
    /// Representation for execution
    /// </summary>
    public class ExecutionRepresentation
    {
        /// <summary>
        /// Stratgey instance key
        /// </summary>
        public string InstanceKey { get; set; }

        /// <summary>
        /// Order execution
        /// </summary>
        public Execution Execution { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExecutionRepresentation()
        {
            
        }

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <param name="execution"></param>
        public ExecutionRepresentation(string instanceKey, Execution execution)
        {
            InstanceKey = instanceKey;
            Execution = execution;
        }
    }
}
