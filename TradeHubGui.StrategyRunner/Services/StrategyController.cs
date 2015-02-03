using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceSourceLogger;
using TraceSourceLogger.ValueObjects;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.DomainModels.OrderDomain;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Executors;
using TradeHubGui.StrategyRunner.Representations;
using Strategy = TradeHubGui.Common.Models.Strategy;

namespace TradeHubGui.StrategyRunner.Services
{
    /// <summary>
    /// Provides access to strategy related tasks
    /// </summary>
    public class StrategyController
    {
        private Type _type = typeof(StrategyController);

        /// <summary>
        /// Records of current startegies instances
        /// KEY = Strategy Instance Unique Key
        /// VALUE = <seealso cref="StrategyExecutor"/>
        /// </summary>
        private ConcurrentDictionary<string, StrategyExecutor> _strategiesCollection =
            new ConcurrentDictionary<string, StrategyExecutor>();

        #region Events

        private event Action<StrategyStatusRepresentation> _strategyStatusChanged;
        private event Action<ExecutionRepresentation> _executionRecevied;

        /// <summary>
        /// Notify listeneres that on strategy status changed
        /// </summary>
        public event Action<StrategyStatusRepresentation> StrategyStatusChanged
        {
            add { if (_strategyStatusChanged == null) _strategyStatusChanged += value; }
            remove { _strategyStatusChanged -= value; }
        }

        /// <summary>
        /// Notify listeneres that on strategy status changed
        /// </summary>
        public event Action<ExecutionRepresentation> ExecutionReceived
        {
            add { if (_executionRecevied == null) _executionRecevied += value; }
            remove { _executionRecevied -= value; }
        }

        #endregion

        /// <summary>
        /// Verify and add strategy to TradeHub
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public bool VerifyAndAddStrategy(string assemblyPath)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Received call for adding strategy, path=" + assemblyPath, _type.FullName, "VerifyAndAddStrategy");
            }
            
            if (StrategyHelper.ValidateStrategy(assemblyPath))
            {
                StrategyHelper.CopyAssembly(assemblyPath);
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Strategy Added", _type.FullName, "VerifyAndAddStrategy");
                }
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Removes the Strategy from application and cleans directory aswell
        /// </summary>
        /// <param name="assemblyPath"></param>
        public void RemoveStrategy(string assemblyPath)
        {
            
        }

        /// <summary>
        /// Get all saved strategies
        /// </summary>
        /// <returns></returns>
        public List<Strategy> GetAllStrategies()
        {
            List<Strategy> savedStrategies=new List<Strategy>();

            //get all saved strategies
            var strategies=StrategyHelper.GetAllStrategiesName();
            
            //cast all strategies names to its object
            foreach (var strategy in strategies)
            {
                //savedStrategies.Add(new Strategy(){Key = strategy});
            }
            
            return savedStrategies;
        }

        /// <summary>
        /// Add new strategy instance
        /// </summary>
        /// <param name="strategyInstance"></param>
        public void AddStrategyInstance(StrategyInstance strategyInstance)
        {
            //create strategy executor instance responsible for entire Strategy Instance life cycle
            StrategyExecutor executor = new StrategyExecutor(strategyInstance);

            //subscribe to strategy event changed event
            executor.StatusChanged += OnStrategyStatusChanged;
            executor.ExecutionReceived += OnExecutionReceived;

            //map the executor instance in dictionary
            _strategiesCollection.TryAdd(strategyInstance.InstanceKey,executor);
        }

        /// <summary>
        /// Run the specific strategy instance
        /// </summary>
        /// <param name="instanceKey">Unique identity of Strategy Instance</param>
        public void RunStrategy(string instanceKey)
        {
            if (_strategiesCollection.ContainsKey(instanceKey))
            {
                _strategiesCollection[instanceKey].ExecuteStrategy();

                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Strategy started, instance key=" + instanceKey, _type.FullName, "RunStrategy");
                }
            }
        }

        /// <summary>
        /// Stop specific strategy instance
        /// </summary>
        /// <param name="instanceKey">Unique identity of Strategy Instance</param>
        public void StopStrategy(string instanceKey)
        {
            if (_strategiesCollection.ContainsKey(instanceKey))
            {
                _strategiesCollection[instanceKey].StopStrategy();

                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Strategy stopped, instance key=" + instanceKey, _type.FullName, "StopStrategy");
                }
            }
        }

        /// <summary>
        /// Remove existing strategy instance
        /// </summary>
        /// <param name="instanceKey">Unique identity of Strategy Instance</param>
        public void RemoveStrategyInstance(string instanceKey)
        {
            if (_strategiesCollection.ContainsKey(instanceKey))
            {
                StrategyExecutor executor;
                if (_strategiesCollection.TryRemove(instanceKey, out executor))
                {
                    // Stop strategy if its running
                    if (executor.StrategyStatus.Equals(StrategyStatus.Executing))
                    {
                        executor.StopStrategy();
                    }

                    //Unsubscribe event
                    executor.StatusChanged -= OnStrategyStatusChanged;
                    executor.ExecutionReceived -= OnExecutionReceived;

                    //dispose any resources used
                    executor.Close();
                }

                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Removed strategy, instance key=" + instanceKey, _type.FullName, "RemoveStrategyInstance");
                }
            }
        }

        /// <summary>
        /// On execution received from strategy executor
        /// </summary>
        /// <param name="executionRepresentation"></param>
        private void OnExecutionReceived(ExecutionRepresentation executionRepresentation)
        {
            if (_executionRecevied != null)
            {
                //notify listeners about new execution
                _executionRecevied(executionRepresentation);
            }
        }

        /// <summary>
        /// On strategy status changed
        /// </summary>
        /// <param name="instanceKey"></param>
        /// <param name="strategyStatus"></param>
        private void OnStrategyStatusChanged(string instanceKey, StrategyStatus strategyStatus)
        {
            // Create object to indicate status change to be used for Publishing
            StrategyInstanceStatus strategyInstanceStatus = new StrategyInstanceStatus();

            strategyInstanceStatus.StrategyKey = instanceKey.Split('-')[0];
            strategyInstanceStatus.InstanceKey = instanceKey;
            strategyInstanceStatus.StrategyStatus = strategyStatus;

            // Publish event to notify listeners
            EventSystem.Publish<StrategyInstanceStatus>(strategyInstanceStatus);
        }
    }
}
