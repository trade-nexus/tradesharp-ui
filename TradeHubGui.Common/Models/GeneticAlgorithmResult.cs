using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.ValueObjects;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains ouput/result from the execution of Genetic Algorithm Optmization
    /// </summary>
    public class GeneticAlgorithmResult
    {
        /// <summary>
        /// Contains all optimized parameter information
        /// </summary>
        private IList<OptimizationParameterDetail> _parameterList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public GeneticAlgorithmResult()
        {
            ParameterList = new List<OptimizationParameterDetail>();
        }

        /// <summary>
        /// Contains all optimized parameter information
        /// </summary>
        public IList<OptimizationParameterDetail> ParameterList
        {
            get { return _parameterList; }
            set { _parameterList = value; }
        }
    }
}
