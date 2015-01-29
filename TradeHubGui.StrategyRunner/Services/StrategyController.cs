using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceSourceLogger;
using TraceSourceLogger.ValueObjects;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common.Models;

namespace TradeHubGui.StrategyRunner.Services
{
    /// <summary>
    /// Provides access to strategy related tasks
    /// </summary>
    public class StrategyController
    {
        /// <summary>
        /// Records of current startegies instances
        /// </summary>
        private ConcurrentDictionary<string, StrategyExecutor> _strategiesCollection=new ConcurrentDictionary<string, StrategyExecutor>();

        private Type _type = typeof (StrategyController);
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
                savedStrategies.Add(new Strategy(){Key = strategy});
            }
            return savedStrategies;
        }

        /// <summary>
        /// Add new strategy instance
        /// </summary>
        /// <param name="strategyInstance"></param>
        public void AddStrategyInstance(StrategyInstance strategyInstance)
        {
            _strategiesCollection.TryAdd(strategyInstance.InstanceKey,
                new StrategyExecutor(strategyInstance.InstanceKey, strategyInstance.StrategyType,
                    strategyInstance.Parameters));
        }

        /// <summary>
        /// Add new strategy instance
        /// </summary>
        /// <param name="instanceKey"></param>
        public void RemoveStrategyInstance(string instanceKey)
        {
            if (_strategiesCollection.ContainsKey(instanceKey))
            {
                StrategyExecutor executor;
                if (_strategiesCollection.TryRemove(instanceKey, out executor))
                {
                    //TODO: If strategy is running stop it
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
        /// Run the specific strategy instance
        /// </summary>
        /// <param name="instanceKey"></param>
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
        /// <param name="instanceKey"></param>
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
    }
}
