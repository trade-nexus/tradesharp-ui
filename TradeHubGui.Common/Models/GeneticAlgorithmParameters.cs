using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.ValueObjects;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains details for all the Parameters to be used for Genetic Optimization
    /// </summary>
    public class GeneticAlgorithmParameters
    {
        /// <summary>
        /// Constructor arguments to use
        /// </summary>
        private object[] _ctorArgs;

        /// <summary>
        /// Contains info for the parameters to be used for optimization
        /// </summary>
        private SortedDictionary<int,OptimizationParameterDetail> _optimzationParameters;
        
        /// <summary>
        /// Holds reference of user selected custom strategy
        /// </summary>
        private Type _strategyType;

        /// <summary>
        /// Iterations of the GA to be run
        /// </summary>
        private int _iterations;

        /// <summary>
        /// Population size to be used in Genetic Algorithm working
        /// </summary>
        private int _populationSize;

        /// <summary>
        /// No. of rounds Genetic Algorithm should run
        /// </summary>
        private int _rounds;

        /// <summary>
        /// Argument Constrcutor
        /// </summary>
        /// <param name="strategyType">Type of custom strategy used</param>
        /// <param name="ctorArgs">Constructor arguments to be used for given strategy</param>
        /// <param name="optimzationParameters">Parameters to be used for optimizing the strategy</param>
        /// <param name="iterations">No. of iterations to be executed</param>
        /// <param name="populationSize">Population size to be used in Genetic Algorithm working</param>
        /// <param name="rounds">No. of rounds Genetic Algorithm should run</param>
        public GeneticAlgorithmParameters(Type strategyType, object[] ctorArgs,
            SortedDictionary<int, OptimizationParameterDetail> optimzationParameters, int iterations,
            int populationSize, int rounds)
        {
            _strategyType = strategyType;
            _ctorArgs = ctorArgs;
            _optimzationParameters = optimzationParameters;
            _iterations = iterations;
            _populationSize = populationSize;
            _rounds = rounds;
        }

        /// <summary>
        /// Constructor arguments to use
        /// </summary>
        public object[] CtorArgs
        {
            get { return _ctorArgs; }
        }

        /// <summary>
        /// Contains info for the parameters to be used for optimization
        /// </summary>
        public SortedDictionary<int, OptimizationParameterDetail> OptimzationParameters
        {
            get { return _optimzationParameters; }
        }

        /// <summary>
        /// Holds reference of user selected custom strategy
        /// </summary>
        public Type StrategyType
        {
            get { return _strategyType; }
        }

        /// <summary>
        /// Population size to be used in Genetic Algorithm working
        /// </summary>
        public int PopulationSize
        {
            get { return _populationSize; }
            set { _populationSize = value; }
        }

        /// <summary>
        /// Iterations of the GA to be run
        /// </summary>
        public int Iterations
        {
            get { return _iterations; }
            set { _iterations = value; }
        }

        /// <summary>
        /// No. of rounds Genetic Algorithm should run
        /// </summary>
        public int Rounds
        {
            get { return _rounds; }
            set { _rounds = value; }
        }
    }
}
