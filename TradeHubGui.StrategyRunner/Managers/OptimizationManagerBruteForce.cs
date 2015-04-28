using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TraceSourceLogger;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.Utility;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Executors;

namespace TradeHubGui.StrategyRunner.Managers
{
    public class OptimizationManagerBruteForce : IDisposable
    {
        private Type _type = typeof(OptimizationManagerBruteForce);

        private AsyncClassLogger _asyncClassLogger;

        /// <summary>
        /// Holds UI thread reference
        /// </summary>
        private Dispatcher _currentDispatcher;

        private bool _disposed = false;

        /// <summary>
        /// Contains ctor arguments to be used for multiple iteration
        /// </summary>
        private List<object[]> _ctorArguments;

        /// <summary>
        /// Holds reference to the Parameters information provided for Brute force optimization
        /// </summary>
        private BruteForceParameters _optimizationParameters;

        /// <summary>
        /// Keeps tracks of all the running strategies
        /// KEY = Unique string to identify each strategy instance
        /// Value = <see cref="StrategyExecutor"/>
        /// </summary>
        private ConcurrentDictionary<string, StrategyExecutor> _strategiesCollection;

        /// <summary>
        /// Save constuctor parameter info for the selected strategy
        /// </summary>
        private ObservableCollection<BruteForceParameterDetail> _parmatersDetails;

        /// <summary>
        /// Contains ctor arguments to be used for multiple iteration
        /// </summary>
        public List<object[]> CtorArguments
        {
            get { return _ctorArguments; }
            set { _ctorArguments = value; }
        }

        /// <summary>
        /// Default Argument
        /// </summary>
        public OptimizationManagerBruteForce()
        {
            _currentDispatcher = Dispatcher.CurrentDispatcher;

            //_asyncClassLogger = ContextRegistry.GetContext()["StrategyRunnerLogger"] as AsyncClassLogger;
            _asyncClassLogger = new AsyncClassLogger("OptimizationManagerBruteForce");

            // Initialize
            _ctorArguments = new List<object[]>();
            _strategiesCollection = new ConcurrentDictionary<string, StrategyExecutor>();

            // Subscribe to event aggregator
            EventSystem.Subscribe<BruteForceParameters>(StartOptimization);
        }

        /// <summary>
        /// Start Strategy Optimization
        /// </summary>
        /// <param name="optimizationParameters">Contains info for the parameters to be used for optimization</param>
        private void StartOptimization(BruteForceParameters optimizationParameters)
        {
            try
            {
                // Save instance 
                _optimizationParameters = optimizationParameters;

                if (_asyncClassLogger.IsInfoEnabled)
                {
                    _asyncClassLogger.Info("Getting argument combinations", _type.FullName, "StartOptimization");
                }
                
                // Change Status to indicate on UI
                _optimizationParameters.Status = OptimizationStatus.Working;

                // Clear all previous information
                _strategiesCollection.Clear();
                _ctorArguments.Clear();

                // Get Parameter values to be used in the Constructor
                object[] ctorArguments = optimizationParameters.GetParameterValues();

                // Get Conditional Parameter values to be used for creating Iterations
                Tuple<int, object, double>[] conditionalParameters = optimizationParameters.GetConditionalParameters();

                // Save Parameter Details
                _parmatersDetails = optimizationParameters.ParameterDetails;

                // Get all ctor arguments to be used for optimization
                CreateCtorCombinations(ctorArguments, conditionalParameters);

                // Initialize Stratgey for each set of arguments
                foreach (object[] ctorArgumentValues in _ctorArguments)
                {
                    // Get new Key.
                    string key = ApplicationIdGenerator.NextId();

                    var instanceParameterDetails = new Dictionary<string, ParameterDetail>();

                    for (int i = 0; i < ctorArgumentValues.Length; i++)
                    {
                        // Create new parameter details to be when creating Strategy Instance object
                        ParameterDetail tempParameterDetail = new ParameterDetail(_parmatersDetails[i].ParameterType, ctorArgumentValues[i]);

                        instanceParameterDetails.Add(_parmatersDetails[i].Description, tempParameterDetail);
                    }

                    // Create Strategy Instance object
                    var instance = new StrategyInstance(key, instanceParameterDetails, optimizationParameters.StrategyType);

                    // Save Strategy details in new Strategy Executor object
                    var strategyExecutor = new StrategyExecutor(instance, _currentDispatcher);

                    // Register Event
                    strategyExecutor.StatusChanged += OnStrategyExecutorStatusChanged;

                    // Add to local map
                    _strategiesCollection.AddOrUpdate(key, strategyExecutor, (ky, value) => strategyExecutor);

                    StringBuilder parametersInfo = new StringBuilder();
                    foreach (object ctorArgument in strategyExecutor.CtorArguments)
                    {
                        parametersInfo.Append(ctorArgument.ToString());
                        parametersInfo.Append(" | ");
                    }

                    // Create new object to be used with Event Aggregator
                    var optimizationStatistics = new OptimizationStatistics(strategyExecutor.StrategyKey);
                    optimizationStatistics.Description = parametersInfo.ToString();
                    optimizationStatistics.ExecutionDetails = instance.ExecutionDetails;

                    // Raise event to Bind statistics to UI and will be updated as each instance is executed
                    EventSystem.Publish<OptimizationStatistics>(optimizationStatistics);
                }

                // Save total number of iterations count
                _optimizationParameters.TotalIterations = _strategiesCollection.Count;

                // Start executing each instance
                StartStrategyExecution();
            }
            catch (Exception exception)
            {
                _asyncClassLogger.Error(exception, _type.FullName, "StartOptimization");
            }
        }

        /// <summary>
        /// Strats executing individual strategy instances created for each iteration
        /// </summary>
        private void StartStrategyExecution()
        {
            try
            {
                if (_asyncClassLogger.IsDebugEnabled)
                {
                    _asyncClassLogger.Debug("Starting strategy instance for optimization.", _type.FullName, "StratStrategyExecution");
                }

                if (_strategiesCollection.Count > 0)
                {
                    // Get the iteration to be executed;
                    var strategyExecutor = _strategiesCollection.ElementAt(0).Value;

                    // Execute strategy if its not already executing/executed
                    if (strategyExecutor.StrategyStatus.Equals(StrategyStatus.None))
                    {
                        strategyExecutor.ExecuteStrategy();
                    }
                }
                else
                {
                    // Change Status to indicate on UI
                    _optimizationParameters.Status = OptimizationStatus.Completed;
                    EventSystem.Publish<UiElement>();
                }
                // Execute each instance on a separate thread
                // Parallel.ForEach(_strategiesCollection.Values,
                //                strategyExecutor => Task.Factory.StartNew(strategyExecutor.ExecuteStrategy));
            }
            catch (Exception exception)
            {
                _asyncClassLogger.Error(exception, _type.FullName, "StratStrategyExecution");
            }
        }

        /// <summary>
        /// Creates all possible ctor combinations
        /// </summary>
        /// <param name="ctorArgs">ctor arguments to create combinations with</param>
        /// <param name="conditionalParameters">contains info for the conditional parameters</param>
        public void CreateCtorCombinations(object[] ctorArgs, Tuple<int, object, double>[] conditionalParameters)
        {
            try
            {
                var itemsCount = conditionalParameters.Length;
                // Get all posible optimizations
                GetAllIterations(ctorArgs.Clone() as object[], conditionalParameters, itemsCount - 1);
            }
            catch (Exception exception)
            {
                _asyncClassLogger.Error(exception, _type.FullName, "CreateCtorCombinations");
            }
        }

        /// <summary>
        /// Gets all possible combinations for the given parameters
        /// </summary>
        /// <param name="args">ctor arguments to create combinations with</param>
        /// <param name="conditionalParameters">contains info for the conditional parameters</param>
        /// <param name="conditionalIndex">index of conditional parameter to be used for iterations</param>
        private void GetAllIterations(object[] args, Tuple<int, object, double>[] conditionalParameters, int conditionalIndex)
        {
            try
            {
                // get index of parameter to be incremented
                int index = conditionalParameters[conditionalIndex].Item1;

                // Get end value for the parameter
                double endPoint;
                if (!double.TryParse(conditionalParameters[conditionalIndex].Item2.ToString(), out endPoint))
                {
                    return;
                }

                // Get increment value to be used 
                double increment = conditionalParameters[conditionalIndex].Item3;

                // Get Orignal Value
                double orignalValue = Convert.ToDouble(args[index]);

                // Iterate through all combinations
                for (double i = 0; ; i += increment)
                {
                    // Modify parameter value
                    var parameter = orignalValue + i;

                    if (parameter > endPoint) break;

                    // Convert string value to required format
                    var value = StrategyHelper.GetParametereValue(parameter.ToString(), _parmatersDetails[index].ParameterType.Name);

                    // Update arguments array
                    args[index] = value;

                    // Check if the combination is already present
                    if (!ValueAdded(args, _ctorArguments, index))
                    {
                        // Add the updated arguments to local map
                        _ctorArguments.Add(args.Clone() as object[]);

                        // Get further iterations if 
                        if (conditionalIndex > 0)
                        {
                            GetAllIterations(args.Clone() as object[], conditionalParameters, conditionalIndex - 1);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _asyncClassLogger.Error(exception, _type.FullName, "IterateParameters");
            }
        }

        /// <summary>
        /// Called when there is a change in strategy status
        /// </summary>
        /// <param name="key">Unique Key to identify the Strategy</param>
        /// <param name="status">indicates whether the strategy is running or stopped</param>
        private void OnStrategyExecutorStatusChanged(string key, StrategyStatus status)
        {
            try
            {
                if (_asyncClassLogger.IsInfoEnabled)
                {
                    _asyncClassLogger.Info("Strategy: " + key + " is running: " + status, _type.FullName, "OnStrategyExecutorStatusChanged");
                }

                // Get Statistics if the strategy is completed
                if (status==StrategyStatus.Executed)
                {
                    StrategyExecutor strategyExecutor;
                    if (_strategiesCollection.TryRemove(key, out strategyExecutor))
                    {
                        StringBuilder parametersInfo = new StringBuilder();

                        foreach (object ctorArgument in strategyExecutor.CtorArguments)
                        {
                            parametersInfo.Append(ctorArgument.ToString());
                            parametersInfo.Append(" | ");
                        }

                        // Unhook Event
                        strategyExecutor.StatusChanged -= OnStrategyExecutorStatusChanged;

                        // Stop Strategy
                        strategyExecutor.StopStrategy();

                        // Close all connections
                        strategyExecutor.Close();

                        // Update Iterations information
                        _optimizationParameters.CompletedIterations += 1;
                        _optimizationParameters.RemainingIterations = _optimizationParameters.TotalIterations -
                                                                      _optimizationParameters.CompletedIterations;

                        // Execute next iteration
                        StartStrategyExecution();
                    }
                }
            }
            catch (Exception exception)
            {
                _asyncClassLogger.Error(exception, _type.FullName, "OnStrategyExecutorStatusChanged");
            }
        }

        /// <summary>
        /// Checks if the value is already added in given list
        /// </summary>
        /// <param name="newValue">Value to verfiy</param>
        /// <param name="localMap">Local map to check for given value</param>
        /// <param name="index">Index on which to verify the value</param>
        private bool ValueAdded(object[] newValue, List<object[]> localMap, int index)
        {
            if (localMap.Count > 0)
            {
                var lastElement = localMap.Last();
                if (lastElement != null)
                {
                    if (lastElement[index].Equals(newValue[index]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Make sure event is only subscribed once
                    EventSystem.Unsubscribe<BruteForceParameters>(StartOptimization);

                    _ctorArguments.Clear();
                    _strategiesCollection.Clear();
                }
                // Release unmanaged resources.
                _disposed = true;
            }
        }
    }
}
