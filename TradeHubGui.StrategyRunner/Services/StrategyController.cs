/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Spring.Context.Support;
using TraceSourceLogger;
using TraceSourceLogger.ValueObjects;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.DomainModels.OrderDomain;
using TradeHub.Common.Core.Repositories;
using TradeHub.Common.Persistence;
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
        /// Holds UI thread reference
        /// </summary>
        private Dispatcher _currentDispatcher;

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
        /// Default Constructor
        /// </summary>
        public StrategyController()
        {
            IPersistRepository<object> persistRepository = ContextRegistry.GetContext()["PersistRepository"] as IPersistRepository<object>;

            // Save UI thread reference
            _currentDispatcher = Dispatcher.CurrentDispatcher;

            PersistencePublisher.InitializeDisruptor(persistRepository);
        }

        /// <summary>
        /// Sets database persistence status
        /// </summary>
        /// <param name="enablePersistence">indicates if the orders are to be persisted or not</param>
        public void AllowPersistence(bool enablePersistence)
        {
            PersistencePublisher.AllowPersistence(enablePersistence);
        }

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
            StrategyExecutor executor = new StrategyExecutor(strategyInstance, _currentDispatcher);

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
                Task.Factory.StartNew(()=>_strategiesCollection[instanceKey].ExecuteStrategy());

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
        /// Returns locally saved data from strategy instance
        /// </summary>
        /// <param name="instanceKey">Unique ID to identify strategy instance</param>
        /// <returns></returns>
        public IReadOnlyList<string> GetStrategyInstanceLocalData(string instanceKey)
        {
            // Get strategy executor for given instance key
            var strategyExecutor = _strategiesCollection[instanceKey];

            // Retrieve data
            return strategyExecutor.GetLocalData();
        }

        /// <summary>
        /// Updates the Notification parameters for the given strategy instance key
        /// </summary>
        /// <param name="instanceKey">Unique ID to identify strategy instance</param>
        /// <param name="newOrder">Indicates if new order notification is required</param>
        /// <param name="acceptedOrder">Indicates if accepted order notification is required</param>
        /// <param name="execution">Indicates if execution notification is required</param>
        /// <param name="rejection">Indicates if order rejection notification is required</param>
        public void UpdateNotificationProperties(string instanceKey, bool newOrder, bool acceptedOrder, bool execution, bool rejection)
        {
            // Get Strategy Executor object for given Instance Key
            if (_strategiesCollection.ContainsKey(instanceKey))
            {
                // Set individual notification properties
                _strategiesCollection[instanceKey].SetNewOrderNotification(newOrder);
                _strategiesCollection[instanceKey].SetAcceptedOrderNotification(acceptedOrder);
                _strategiesCollection[instanceKey].SetExecutionNotification(execution);
                _strategiesCollection[instanceKey].SetRejectionNotification(rejection);
            }
        }

        public Tuple<bool, bool, bool, bool> GetNotificationProperties(string instanceKey)
        {
            // Get Strategy Executor object for given Instance Key
            if (_strategiesCollection.ContainsKey(instanceKey))
            {
                // Get individual notification properties
                return new Tuple<bool, bool, bool, bool>(
                    _strategiesCollection[instanceKey].NewOrderNotification,
                    _strategiesCollection[instanceKey].AcceptedOrderNotification,
                    _strategiesCollection[instanceKey].ExecutionNotification,
                    _strategiesCollection[instanceKey].RejectionNotification
                    );
            }

            return null;
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

        /// <summary>
        /// Stops all strategy runner activities 
        /// </summary>
        public void Close()
        {
            foreach (var executor in _strategiesCollection.Values)
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
                Logger.Info("All Strategy instances removed", _type.FullName, "Stop");
            }
        }
    }
}
