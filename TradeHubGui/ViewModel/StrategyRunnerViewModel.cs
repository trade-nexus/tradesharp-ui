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


﻿using System.Runtime.InteropServices;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TraceSourceLogger;
using TradeHubGui.Common.Infrastructure;
using TradeHubGui.Common.Utility;
using TradeHubGui.Dashboard.Services;
using DomainModels = TradeHub.Common.Core.DomainModels;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Services;
using TradeHubGui.Views;
using MessageBoxUtils;
using TradeHubGui.Common.ApplicationSecurity;
using Forms = System.Windows.Forms;

namespace TradeHubGui.ViewModel
{
    public class StrategyRunnerViewModel : BaseViewModel
    {
        private Type _type = typeof (StrategyRunnerViewModel);

        private ObservableCollection<Strategy> _strategies;
        private ObservableCollection<StrategyInstance> _instances;
        private ObservableCollection<StrategyExecutionDetails> _executionDetailsCollection;
        private ObservableCollection<OrderDetails> _orderDetailsCollection;

        private bool _enablePersistence;
        private bool _executionDetailsForAllInstances;

        private Strategy _selectedStrategy;
        private StrategyInstance _selectedInstance;
        private StrategyExecutionDetails _selectedExecutionDetails;

        private RelayCommand _showCreateInstanceWindowCommand;
        private RelayCommand _createInstanceCommand;
        private RelayCommand _runInstanceCommand;
        private RelayCommand _runMultipleInstanceCommand;
        private RelayCommand _stopInstanceCommand;
        private RelayCommand _stopMultipleInstanceCommand;
        private RelayCommand _deleteInstanceCommand;
        private RelayCommand _deleteMultipleInstanceCommand;
        private RelayCommand _editInstanceParametersCommand;
        private RelayCommand _showEditInstanceWindowCommand;
        private RelayCommand _showGeneticOptimizationWindowCommand;
        private RelayCommand _showBruteOptimizationWindowCommand;
        private RelayCommand _loadStrategyCommand;
        private RelayCommand _removeStrategyCommand;
        private RelayCommand _selectProviderCommand;
        private RelayCommand _importInstancesCommand;
        private RelayCommand _exportInstanceDataCommand;
        private RelayCommand _exportAllInstanceDataCommand;
        private RelayCommand _instanceSummaryCommand;
        private RelayCommand _strategySummaryCommand;
        private RelayCommand _notificationOptionsCommand;
        private RelayCommand _saveNotificationOptionsCommand;
        private RelayCommand _exportExecutionsCommand;
        private RelayCommand _exportAllExecutionsCommand;
        private RelayCommand _selectAllInstancesCommand;

        private string _strategyPath;
        private string _csvInstancesPath;
        private string _instanceDescription;

        /// <summary>
        /// Indicates if notification for new order is to be sent
        /// </summary>
        private bool _newOrderNotification = false;

        /// <summary>
        /// Indicates if notification on order acceptance is to be sent
        /// </summary>
        private bool _acceptedOrderNotification = false;

        /// <summary>
        /// Indicates if notification on order execution is to be sent
        /// </summary>
        private bool _executionNotification = false;

        /// <summary>
        /// Indicates if the notification on order rejection is to be sent
        /// </summary>
        private bool _rejectionNotification = false;

        private Dictionary<string, ParameterDetail> _parameterDetails;

        /// <summary>
        /// Provides functionality for all Strategy related operations
        /// </summary>
        private StrategyController _strategyController;

        /// <summary>
        /// Contains names for the available market data providers
        /// </summary>
        private ObservableCollection<string> _marketDataProviders;

        /// <summary>
        /// Contains names for the available order execution providers
        /// </summary>
        private ObservableCollection<string> _orderExecutionProviders;

        /// <summary>
        /// Constructor
        /// </summary>
        public StrategyRunnerViewModel()
        {
            // Initialize objects
            _strategyController = new StrategyController();
            _strategies = new ObservableCollection<Strategy>();
            _marketDataProviders = new ObservableCollection<string>();
            _orderExecutionProviders = new ObservableCollection<string>();

            EnablePersistence = false;

            // Get Existing Strategies in the system and populate on UI
            LoadExistingStrategies();

            // Select 1st strategy from ListBox if not empty
            if (Strategies.Count > 0)
            {
                SelectedStrategy = Strategies[0];
            }

            // Populate Local maps for available Provider names
            PopulateMarketDataProviders();
            PopulateOrderExecutionProviders();

            EventSystem.Subscribe<string>(OnApplicationClose);
        }

        #region Observable Collections

        /// <summary>
        /// Collection of loaded strategies
        /// </summary>
        public ObservableCollection<Strategy> Strategies
        {
            get { return _strategies; }
            set
            {
                _strategies = value;
                OnPropertyChanged("Strategies");
            }
        }

        /// <summary>
        /// Collection of instances for SelectedStrategy
        /// </summary>
        public ObservableCollection<StrategyInstance> Instances
        {
            get { return _instances; }
            set
            {
                _instances = value;
                OnPropertyChanged("Instances");
            }
        }

        /// <summary>
        /// Collection of execution details for SelectedInstance or for all instances of SelectedStrategy (depends on toggle bool)
        /// </summary>
        public ObservableCollection<StrategyExecutionDetails> ExecutionDetailsCollection
        {
            get
            {
                _executionDetailsCollection = new ObservableCollection<StrategyExecutionDetails>();

                //NOTE: This logic can stay here in getter, or can be moved to method for that purpose. 
                //NOTE: Refresh of this collection is neccessary when SelectedInstance is changed, or when ExecutionDetailsForAllInstances bool is changed
                if (ExecutionDetailsForAllInstances)
                {
                    if (SelectedStrategy != null)
                    {
                        // if ExecutionDetailsForAllInstances is true, traverse collection of all instances for selected strategy 
                        // and add execution details to _executionDetailsCollection
                        foreach (StrategyInstance instance in SelectedStrategy.StrategyInstances.Values)
                        {
                            _executionDetailsCollection.Add(instance.ExecutionDetails);
                        }
                    }
                }
                else
                {
                    if (SelectedInstance != null)
                    {
                        // if ExecutionDetailsForAllInstances is false, add execution details just for selected instance
                        _executionDetailsCollection.Add(SelectedInstance.ExecutionDetails);
                    }
                }

                return _executionDetailsCollection;
            }
            set
            {
                if (_executionDetailsCollection != value)
                {
                    _executionDetailsCollection = value;
                    OnPropertyChanged("ExecutionDetailsCollection");
                }
            }
        }

        /// <summary>
        /// Collection of all order details for SelectedExecutionDetails
        /// </summary>
        public ObservableCollection<OrderDetails> OrderDetailsCollection
        {
            get { return _orderDetailsCollection; }
            set
            {
                if (_orderDetailsCollection != value)
                {
                    _orderDetailsCollection = value;
                    OnPropertyChanged("OrderDetailsCollection");
                }
            }
        }

        /// <summary>
        /// Contains names for the available market data providers
        /// </summary>
        public ObservableCollection<string> MarketDataProviders
        {
            get { return _marketDataProviders; }
            set
            {
                _marketDataProviders = value;
                OnPropertyChanged("MarketDataProviders");
            }
        }

        /// <summary>
        /// Contains names for the available order execution providers
        /// </summary>
        public ObservableCollection<string> OrderExecutionProviders
        {
            get { return _orderExecutionProviders; }
            set
            {
                _orderExecutionProviders = value;
                OnPropertyChanged("OrderExecutionProviders");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Selected strategy from list of loaded strategies
        /// </summary>
        public Strategy SelectedStrategy
        {
            get { return _selectedStrategy; }
            set
            {
                _selectedStrategy = value;
                if (value != null)
                    PopulateStrategyInstanceDataGrid();

                OnPropertyChanged("SelectedStrategy");
            }
        }

        /// <summary>
        /// Selected instance for SelectedStrategy
        /// </summary>
        public StrategyInstance SelectedInstance
        {
            get { return _selectedInstance; }
            set
            {
                if (_selectedInstance != value)
                {
                    _selectedInstance = value;
                    OnPropertyChanged("SelectedInstance");
                    // if SelectedInstance is changed, refresh collection of ExecutionDetails (or call method for filling that collection)
                    OnPropertyChanged("ExecutionDetailsCollection");
                }
            }
        }

        /// <summary>
        /// Parameter details for SelectedInstance
        /// </summary>
        public Dictionary<string, ParameterDetail> ParameterDetails
        {
            get { return _parameterDetails; }
            set
            {
                _parameterDetails = value;
                OnPropertyChanged("ParameterDetails");
            }
        }

        /// <summary>
        /// Currently selected execution details in DataGrid
        /// </summary>
        public StrategyExecutionDetails SelectedExecutionDetails
        {
            get { return _selectedExecutionDetails; }
            set
            {
                if (_selectedExecutionDetails != value)
                {
                    _selectedExecutionDetails = value;
                    if (value != null)
                        PopulateOrderDetailsDataGrid();
                    else
                        OrderDetailsCollection = new ObservableCollection<OrderDetails>();
                    OnPropertyChanged("SelectedExecutionDetails");
                }
            }
        }

        /// <summary>
        /// Used to indicate if the order pesistence is required by the user
        /// </summary>
        public bool EnablePersistence
        {
            get { return _enablePersistence; }
            set
            {
                _enablePersistence = value;

                if (_strategyController != null)
                {
                    _strategyController.AllowPersistence(value);
                }

                OnPropertyChanged("EnablePersistence");
            }
        }

        /// <summary>
        /// Toggle bool property for indicating will be shown execution details only for selected instance or for all instances of selected strategy
        /// </summary>
        public bool ExecutionDetailsForAllInstances
        {
            get { return _executionDetailsForAllInstances; }
            set
            {
                if (_executionDetailsForAllInstances != value)
                {
                    _executionDetailsForAllInstances = value;
                    OnPropertyChanged("ExecutionDetailsForAllInstances");
                    // if bool is changed, refresh collection of ExecutionDetails (or call method for filling that collection)
                    OnPropertyChanged("ExecutionDetailsCollection");
                }
            }
        }

        /// <summary>
        /// Path of loaded instance
        /// </summary>
        public string StrategyPath
        {
            get { return _strategyPath; }
            set
            {
                _strategyPath = value;
                OnPropertyChanged("StrategyPath");
            }
        }

        /// <summary>
        /// Path of loaded csv file
        /// </summary>
        public string CsvInstancesPath
        {
            get { return _csvInstancesPath; }
            set
            {
                _csvInstancesPath = value;
                OnPropertyChanged("CsvInstancesPath");
            }
        }

        /// <summary>
        /// Brief description regarding each strategy instance
        /// </summary>
        public string InstanceDescription
        {
            get { return _instanceDescription; }
            set
            {
                _instanceDescription = value;
                OnPropertyChanged("InstanceDescription");
            }
        }

        /// <summary>
        /// Indicates if notification for new order is to be sent
        /// </summary>
        public bool NewOrderNotification
        {
            get { return _newOrderNotification; }
            set
            {
                _newOrderNotification = value;
                OnPropertyChanged("NewOrderNotification");
            }
        }

        /// <summary>
        /// Indicates if notification on order acceptance is to be sent
        /// </summary>
        public bool AcceptedOrderNotification
        {
            get { return _acceptedOrderNotification; }
            set
            {
                _acceptedOrderNotification = value;
                OnPropertyChanged("AcceptedOrderNotification");
            }
        }

        /// <summary>
        /// Indicates if notification on order execution is to be sent
        /// </summary>
        public bool ExecutionNotification
        {
            get { return _executionNotification; }
            set
            {
                _executionNotification = value;
                OnPropertyChanged("ExecutionNotification");
            }
        }

        /// <summary>
        /// Indicates if the notification on order rejection is to be sent
        /// </summary>
        public bool RejectionNotification
        {
            get { return _rejectionNotification; }
            set
            {
                _rejectionNotification = value;
                OnPropertyChanged("RejectionNotification");
            }
        }

        /// <summary>
        /// Indicates if multiple strategy instances are selected
        /// </summary>
        public bool IsMultipleSelected
        {
            get
            {
                if (SelectedStrategy != null)
                {
                    if (SelectedStrategy.StrategyInstances.Count > 1)
                    {
                        return SelectedStrategy.StrategyInstances.Values.ToList().Count(strategyInstance => strategyInstance.IsSelected == true) > 1;
                    }
                }
                return false;
            }
        }

        #endregion

        #region Commands

        public ICommand ShowCreateInstanceWindowCommand
        {
            get
            {
                return _showCreateInstanceWindowCommand ?? (_showCreateInstanceWindowCommand = new RelayCommand(
                    param => ShowCreateInstanceWindowExecute(), param => ShowCreateInstanceWindowCanExecute()));
            }
        }

        /// <summary>
        /// Command used with 'Create Instance' button
        /// </summary>
        public ICommand CreateInstanceCommand
        {
            get
            {
                return _createInstanceCommand ?? (_createInstanceCommand = new RelayCommand(
                    param => CreateInstanceExecute(param), param => CreateInstanceCanExecute()));
            }
        }

        /// <summary>
        /// Command used with 'Delete' button for each individual Instance in the Strategy Instance Grid
        /// </summary>
        public ICommand DeleteInstanceCommand
        {
            get
            {
                return _deleteInstanceCommand ?? (_deleteInstanceCommand = new RelayCommand(
                    param => DeleteInstanceExecute()));
            }
        }

        /// <summary>
        /// Command used with 'Delete Selected' button for each individual Instance in the Strategy Instance Grid
        /// </summary>
        public ICommand DeleteMultipleInstanceCommand
        {
            get
            {
                return _deleteMultipleInstanceCommand ?? (_deleteMultipleInstanceCommand = new RelayCommand(
                    param => DeleteMultipleInstanceExecute(), param => DeleteMultipleInstanceCanExecute()));
            }
        }

        /// <summary>
        /// Command used with Run button in the Strategy Instance grid
        /// </summary>
        public ICommand RunInstanceCommand
        {
            get
            {
                return _runInstanceCommand ?? (_runInstanceCommand = new RelayCommand(param => RunInstanceExecute()));
            }
        }

        /// <summary>
        /// Command used with Run Selected button in the Strategy Instance grid
        /// </summary>
        public ICommand RunMultipleInstanceCommand
        {
            get
            {
                return _runMultipleInstanceCommand ??
                       (_runMultipleInstanceCommand =
                           new RelayCommand(param => RunMultipleInstanceExecute(),
                               param => RunMultipleInstanceCanExecute()));
            }
        }

        /// <summary>
        /// Command used with Stop button in the Strategy Instance grid
        /// </summary>
        public ICommand StopInstanceCommand
        {
            get
            {
                return _stopInstanceCommand ?? (_stopInstanceCommand = new RelayCommand(param => StopInstanceExecute()));
            }
        }

        /// <summary>
        /// Command used with Stop Selected button in the Strategy Instance grid
        /// </summary>
        public ICommand StopMultipleInstanceCommand
        {
            get
            {
                return _stopMultipleInstanceCommand ??
                       (_stopMultipleInstanceCommand =
                           new RelayCommand(param => StopMultipleInstanceExecute(),
                               param => StopMultipleInstanceCanExecute()));
            }
        }

        /// <summary>
        /// Command used with 'Edit' button in Edit Instance Window
        /// </summary>
        public ICommand EditInstanceParametersCommand
        {
            get
            {
                return _editInstanceParametersCommand ??
                       (_editInstanceParametersCommand = new RelayCommand(param => EditInstanceParametersExecute(param)));
            }
        }

        /// <summary>
        /// Command used to open 'Edit Instance Window' from Strategy Instance Grid
        /// </summary>
        public ICommand ShowEditInstanceWindowCommand
        {
            get
            {
                return _showEditInstanceWindowCommand ?? (_showEditInstanceWindowCommand = new RelayCommand(
                    param => ShowEditInstanceWindowExecute()));
            }
        }

        public ICommand ShowGeneticOptimizationWindowCommand
        {
            get
            {
                return _showGeneticOptimizationWindowCommand ??
                       (_showGeneticOptimizationWindowCommand = new RelayCommand(
                           param => ShowGeneticOptimizationWindowExecute()));
            }
        }

        public ICommand ShowBruteOptimizationWindowCommand
        {
            get
            {
                return _showBruteOptimizationWindowCommand ?? (_showBruteOptimizationWindowCommand = new RelayCommand(
                    param => ShowBruteOptimizationWindowExecute()));
            }
        }

        public ICommand LoadStrategyCommand
        {
            get
            {
                return _loadStrategyCommand ??
                       (_loadStrategyCommand =
                           new RelayCommand(param => LoadStrategyExecute(), param => LoadStrategyCanExecute()));
            }
        }

        public ICommand RemoveStrategyCommand
        {
            get
            {
                return _removeStrategyCommand ?? (_removeStrategyCommand = new RelayCommand(
                    param => RemoveStrategyExecute()));
            }
        }

        public ICommand SelectProviderCommand
        {
            get
            {
                return _selectProviderCommand ??
                       (_selectProviderCommand = new RelayCommand(param => SelectProviderExecute()));
            }
        }

        public ICommand ImportInstancesCommand
        {
            get
            {
                return _importInstancesCommand ??
                       (_importInstancesCommand =
                           new RelayCommand(param => ImportInstancesExecute(), param => ImportInstancesCanExecute()));
            }
        }

        /// <summary>
        /// Command used for opening folder dialog and saving strategy instance data
        /// </summary>
        public ICommand ExportInstanceDataCommand
        {
            get
            {
                return _exportInstanceDataCommand ??
                       (_exportInstanceDataCommand =
                           new RelayCommand(param => ExportInstanceDataExecute(),
                               param => ExportInstanceDataCanExecute()));
            }
        }

        /// <summary>
        /// Command used for opening folder dialog and saving strategy's all instances data
        /// </summary>
        public ICommand ExportAllInstanceDataCommand
        {
            get
            {
                return _exportAllInstanceDataCommand ??
                       (_exportAllInstanceDataCommand =
                           new RelayCommand(param => ExportAllInstanceDataExecute(),
                               param => ExportAllInstanceDataCanExecute()));
            }
        }

        /// <summary>
        /// Command used for opening Strategy Instance summary window
        /// </summary>
        public ICommand InstanceSummaryCommand
        {
            get
            {
                return _instanceSummaryCommand ??
                       (_instanceSummaryCommand = new RelayCommand(param => InstanceSummaryExecute()));
            }
        }

        /// <summary>
        /// Command used for opening Strategy Summary window
        /// </summary>
        public ICommand StrategySummaryCommand
        {
            get
            {
                return _strategySummaryCommand ??
                       (_strategySummaryCommand = new RelayCommand(param => StrategySummaryExecute()));
            }
        }

        /// <summary>
        /// Command used for opening Notificaiton options window
        /// </summary>
        public ICommand NotificationOptionsCommand
        {
            get
            {
                return _notificationOptionsCommand ??
                       (_notificationOptionsCommand = new RelayCommand(param => NotificationOptionsExecute()));
            }
        }

        /// <summary>
        /// Command used for 'Save' button on Notificaiton options window
        /// </summary>
        public ICommand SaveNotificationOptionsCommand
        {
            get
            {
                return _saveNotificationOptionsCommand ??
                       (_saveNotificationOptionsCommand =
                           new RelayCommand(param => SaveNotificationOptionsExecute(param)));
            }
        }

        /// <summary>
        /// Command used for 'Export' button for strategy instance executions
        /// </summary>
        public ICommand ExportExecutionsCommand
        {
            get
            {
                return _exportExecutionsCommand ??
                       (_exportExecutionsCommand =
                           new RelayCommand(param => ExportExecutionsExecute(), param => ExportExecutionsCanExecute()));
            }
        }

        /// <summary>
        /// Command used for 'Export All instances' button for strategy instance executions
        /// </summary>
        public ICommand ExportAllExecutionsCommand
        {
            get
            {
                return _exportAllExecutionsCommand ??
                       (_exportAllExecutionsCommand =
                           new RelayCommand(param => ExportAllExecutionsExecute(), param => ExportAllExecutionsCanExecute()));
            }
        }

        /// <summary>
        /// Command used for 'Select All instances' button
        /// </summary>
        public ICommand SelectAllInstancesCommand
        {
            get
            {
                return _selectAllInstancesCommand ??
                       (_selectAllInstancesCommand =
                           new RelayCommand(param => SelectAllInstancesExecute(), param => SelectAllInstancesCanExecute()));
            }
        }

        #endregion

        #region Command Trigger Methods

        /// <summary>
        /// Displays Strategy Instance Parameter's window to edit input values
        /// </summary>
        private void ShowEditInstanceWindowExecute()
        {
            EditInstanceWindow window = new EditInstanceWindow();
            window.DataContext = this;
            window.Tag = string.Format("Instance {0}", SelectedInstance.InstanceKey);
            window.Owner = MainWindow;

            // Set Description field
            InstanceDescription = SelectedInstance.Description;

            // Set individual Instance parameters
            ParameterDetails = SelectedInstance.Parameters.ToDictionary(entry => entry.Key,
                entry => (ParameterDetail) entry.Value.Clone());

            window.ShowDialog();
        }

        private bool ShowCreateInstanceWindowCanExecute()
        {
            if (SelectedStrategy == null) return false;
            return true;
        }

        /// <summary>
        /// Displays Strategy Parameter's window to input values
        /// </summary>
        private void ShowCreateInstanceWindowExecute()
        {
            CreateInstanceWindow window = new CreateInstanceWindow();
            window.DataContext = this;
            window.Tag = string.Format("Instance for Strategy {0}", SelectedStrategy.Key);
            window.Owner = MainWindow;

            ParameterDetails = SelectedStrategy.ParameterDetails;

            window.ShowDialog();
        }

        private bool CreateInstanceCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Creates a new Strategy Instance and displays on UI
        /// </summary>
        /// <param name="param"></param>
        private void CreateInstanceExecute(object param)
        {
            var parameters = SelectedStrategy.ParameterDetails.ToDictionary(entry => entry.Key,
                entry => (ParameterDetail) entry.Value.Clone());

            // Create a new Strategy Instance with provided parameters
            var strategyInstance = SelectedStrategy.CreateInstance(parameters, InstanceDescription);

            // Select created instance in DataGrid
            SelectedInstance = strategyInstance;

            // Set Initial status to 'Stopped'
            SelectedInstance.Status = DomainModels.StrategyStatus.None;

            // Add Instance to Observable Collection for UI
            Instances.Add(strategyInstance);

            // Send instance to controller where its execution life cycle can be managed
            Task.Run(() => _strategyController.AddStrategyInstance(strategyInstance));

            // Close "Create Instance" window
            ((Window) param).Close();
        }

        /// <summary>
        /// Start Strategy Instance execution. Triggered with 'RunInstanceCommand'
        /// </summary>
        private void RunInstanceExecute()
        {
            SelectedInstance.Status = DomainModels.StrategyStatus.Initializing;

            // Request Strategy Controller to start selected Strategy Instance execution
            _strategyController.RunStrategy(SelectedInstance.InstanceKey);
        }

        /// <summary>
        /// Start Strategy Instance execution. Triggered with 'RunMultipleInstanceCommand'
        /// </summary>
        private void RunMultipleInstanceExecute()
        {
            foreach (var strategyInstance in SelectedStrategy.StrategyInstances.Values)
            {
                if (strategyInstance.IsSelected && 
                    (!strategyInstance.Status.Equals(DomainModels.StrategyStatus.Initializing) 
                        || !strategyInstance.Status.Equals(DomainModels.StrategyStatus.Executing)))
                {
                    // Change Instance status
                    strategyInstance.Status = DomainModels.StrategyStatus.Initializing;

                    // Request Strategy Controller to start selected Strategy Instance execution
                    _strategyController.RunStrategy(strategyInstance.InstanceKey);
                }
            }
        }

        private bool RunMultipleInstanceCanExecute()
        {
            return IsMultipleSelected;
        }

        /// <summary>
        /// Triggered with 'StopInstanceCommand'
        /// </summary>
        private void StopInstanceExecute()
        {
            // Request Strategy Controller to Stop execution for selected Strategy Instance
            _strategyController.StopStrategy(SelectedInstance.InstanceKey);
        }

        /// <summary>
        /// Triggered with 'StopMultipleInstanceCommand'
        /// </summary>
        private void StopMultipleInstanceExecute()
        {
            foreach (var strategyInstance in SelectedStrategy.StrategyInstances.Values)
            {
                if (strategyInstance.IsSelected 
                    && (strategyInstance.Status.Equals(DomainModels.StrategyStatus.Executing) 
                        || strategyInstance.Status.Equals(DomainModels.StrategyStatus.Executed)))
                {
                    // Request Strategy Controller to Stop execution for selected Strategy Instance
                    _strategyController.StopStrategy(strategyInstance.InstanceKey);
                }
            }
        }

        private bool StopMultipleInstanceCanExecute()
        {
            return IsMultipleSelected;
        }

        /// <summary>
        /// Removes selected intance from the UI and requests Strategy Controller to remove it from working session as well.
        /// </summary>
        private void DeleteInstanceExecute()
        {
            if ((Forms.DialogResult)
                WPFMessageBox.Show(MainWindow,
                    string.Format("Delete strategy instance {0}?", SelectedInstance.InstanceKey), "Strategy Runner",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == Forms.DialogResult.Yes)
            {
                // Request Strategy Controller to remove the instance from working session
                _strategyController.RemoveStrategyInstance(SelectedInstance.InstanceKey);

                // Remove the Instance from Strategy's Local Map
                SelectedStrategy.RemoveInstance(SelectedInstance.InstanceKey);

                // Remove the instance from the UI
                Instances.Remove(SelectedInstance);
            }
        }

        /// <summary>
        /// Removes selected intances from the UI and requests Strategy Controller to remove it from working session as well.
        /// </summary>
        private void DeleteMultipleInstanceExecute()
        {
            if ((Forms.DialogResult)
                WPFMessageBox.Show(MainWindow,
                    string.Format("Delete selected instances of {0}?", SelectedStrategy.Name), "Strategy Runner",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == Forms.DialogResult.Yes)
            {
                var selectedInstances = SelectedStrategy.StrategyInstances.Values.Where(strategyInstance => strategyInstance.IsSelected == true);

                foreach (var strategyInstance in selectedInstances.ToList())
                {
                    // Request Strategy Controller to remove the instance from working session
                    _strategyController.RemoveStrategyInstance(strategyInstance.InstanceKey);

                    // Remove the Instance from Strategy's Local Map
                    SelectedStrategy.RemoveInstance(strategyInstance.InstanceKey);

                    // Remove the instance from the UI
                    Instances.Remove(strategyInstance);
                }
            }
        }

        private bool DeleteMultipleInstanceCanExecute()
        {
            return IsMultipleSelected;
        }

        /// <summary>
        /// Modifies selected Strategy Instance's Parameter Details
        /// </summary>
        private void EditInstanceParametersExecute(object param)
        {
            // Update Description
            SelectedInstance.Description = InstanceDescription;

            // Update Parameter Details
            SelectedInstance.Parameters = ParameterDetails.ToDictionary(entry => entry.Key,
                entry => (ParameterDetail) entry.Value.Clone());

            // Close "Edit Instance" window
            ((Window) param).Close();
        }

        /// <summary>
        /// Displays Genetic Optimization Window
        /// </summary>
        private void ShowGeneticOptimizationWindowExecute()
        {
            if (TryActivateShownWindow(typeof (GeneticOptimizationWindow)))
            {
                return;
            }

            GeneticOptimizationWindow window = new GeneticOptimizationWindow();

            // Create ViewModel as DataContext and pass selected strategy for Genetic Optimization
            window.DataContext = new GeneticOptimizationViewModel(SelectedStrategy);
            window.Tag = string.Format("{0}", SelectedStrategy.Name);
            window.Show();
        }

        /// <summary>
        /// Displays Brute Force Optimization Window
        /// </summary>
        private void ShowBruteOptimizationWindowExecute()
        {
            if (TryActivateShownWindow(typeof (BruteOptimizationWindow)))
            {
                return;
            }

            BruteOptimizationWindow window = new BruteOptimizationWindow();
            window.DataContext = new BruteOptimizationViewModel(SelectedStrategy);
            window.Tag = string.Format("{0}", SelectedStrategy.Name);
            window.Show();
        }

        private void SelectProviderExecute()
        {
            SelectProviderWindow window = new SelectProviderWindow();
            window.Owner = MainWindow;
            window.ShowDialog();
        }

        private bool LoadStrategyCanExecute()
        {
            if (!TradeSharpLicenseManager.GetLicense().IsDemo)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads a New Strategy into the Application
        /// </summary>
        private void LoadStrategyExecute()
        {
            Forms.OpenFileDialog openFileDialog = new Forms.OpenFileDialog();
            openFileDialog.Title = "Load Strategy File";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Assembly Files (.dll)|*.dll|All Files (*.*)|*.*";
            Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == Forms.DialogResult.OK)
            {
                // Save selected '.dll' path
                StrategyPath = openFileDialog.FileName;

                // Create New Strategy Object
                AddStrategy(StrategyPath);
            }
        }

        /// <summary>
        // Remove Selected Strategy
        /// </summary>
        private void RemoveStrategyExecute()
        {
            if ((Forms.DialogResult)
                WPFMessageBox.Show(MainWindow, string.Format("Remove strategy {0}?", SelectedStrategy.Key),
                    "Strategy Runner",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == Forms.DialogResult.Yes)
            {
                // Stop/Remove all Strategy Instances
                foreach (string instanceKey in SelectedStrategy.StrategyInstances.Keys)
                {
                    // Stop executions
                    _strategyController.RemoveStrategyInstance(instanceKey);

                    // Remove from Strategy's internal map
                    SelectedStrategy.RemoveInstance(instanceKey);
                }

                //SelectedStrategy.StrategyType = null;
                var fileName = SelectedStrategy.FileName;

                // Remove Strategy from UI
                Strategies.Remove(SelectedStrategy);

                // Remove Strategy assembly from directory
                StrategyHelper.RemoveAssembly(fileName);
            }
        }

        private bool ImportInstancesCanExecute()
        {
            if (SelectedStrategy == null || TradeSharpLicenseManager.GetLicense().IsDemo) return false;
            return true;
        }

        /// <summary>
        /// Opens File Dialog to use '.csv' file for loading strategy instances
        /// </summary>
        private void ImportInstancesExecute()
        {
            Forms.OpenFileDialog openFileDialog = new Forms.OpenFileDialog();
            openFileDialog.Title = "Import Instances";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == Forms.DialogResult.OK)
            {
                CsvInstancesPath = openFileDialog.FileName;

                AddMultipleInstanceFromFile(CsvInstancesPath);
            }
        }

        private bool ExportInstanceDataCanExecute()
        {
            if (SelectedStrategy == null || TradeSharpLicenseManager.GetLicense().IsDemo) return false;
            return true;
        }

        /// <summary>
        /// Triggered when 'Export Instance Data' button is clicked
        /// </summary>
        private void ExportInstanceDataExecute()
        {
            string folderPath = string.Empty;

            // Get Directory in which to save stats
            using (System.Windows.Forms.FolderBrowserDialog form = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = form.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.Yes || dialogResult == Forms.DialogResult.OK)
                {
                    folderPath = form.SelectedPath;
                }
            }

            // Save data
            Task.Run(
                () => PersistCsv.SaveData(folderPath,
                    _strategyController.GetStrategyInstanceLocalData(SelectedInstance.InstanceKey),
                    SelectedInstance.InstanceKey));
        }

        private bool ExportAllInstanceDataCanExecute()
        {
            if (!IsMultipleSelected || TradeSharpLicenseManager.GetLicense().IsDemo) return false;
            return true;
        }

        /// <summary>
        /// Triggered when 'Export all Instances Data' button is clicked
        /// </summary>
        private void ExportAllInstanceDataExecute()
        {
            string folderPath = string.Empty;

            // Get Directory in which to save stats
            using (System.Windows.Forms.FolderBrowserDialog form = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = form.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.Yes || dialogResult == Forms.DialogResult.OK)
                {
                    folderPath = form.SelectedPath;
                }
                else
                {
                    return;
                }
            }


            // Save data
            Task.Run(() =>
                    Parallel.ForEach(SelectedStrategy.StrategyInstances, strategyInstance =>
                    {
                        if (strategyInstance.Value != null && strategyInstance.Value.IsSelected && !strategyInstance.Value.Status.Equals(DomainModels.StrategyStatus.None))
                        {
                            PersistCsv.SaveData(folderPath,
                                _strategyController.GetStrategyInstanceLocalData(strategyInstance.Value.InstanceKey),
                                strategyInstance.Value.InstanceKey);
                        }
                    }));
        }

        /// <summary>
        /// Opens a new Strategy Summary window
        /// </summary>
        private void StrategySummaryExecute()
        {
            if (SelectedStrategy == null) return;

            string title = string.Format("{0}", SelectedStrategy.Name);

            // if Summary window is already shown, just activate it
            StrategySummary strategySummaryWindow = (StrategySummary)FindWindowByTitle(title);
            if (strategySummaryWindow != null)
            {
                strategySummaryWindow.WindowState = WindowState.Normal;
                strategySummaryWindow.Activate();
                return;
            }

            // if Summary window is not already shown, create the new one and show it
            strategySummaryWindow = new StrategySummary();
            strategySummaryWindow.DataContext = new StrategySummaryViewModel(SelectedStrategy);
            strategySummaryWindow.Title = title;
            strategySummaryWindow.Show();
        }

        /// <summary>
        /// Opens a new Instance Summary window
        /// </summary>
        private void InstanceSummaryExecute()
        {
            if (SelectedInstance == null) return;

            string title = string.Format("{0} ({1})", SelectedInstance.InstanceKey, SelectedStrategy.Name);

            // if Summary window is already shown, just activate it
            StrategyInstanceSummary instanceSummaryWindow = (StrategyInstanceSummary) FindWindowByTitle(title);
            if (instanceSummaryWindow != null)
            {
                instanceSummaryWindow.WindowState = WindowState.Normal;
                instanceSummaryWindow.Activate();
                return;
            }

            // if Summary window is not already shown, create the new one and show it
            instanceSummaryWindow = new StrategyInstanceSummary();
            instanceSummaryWindow.DataContext = new StrategyInstanceSummaryViewModel(SelectedInstance);
            instanceSummaryWindow.Title = title;
            instanceSummaryWindow.Show();
        }

        /// <summary>
        /// Displays window to select Notificaiton options for the selected instance
        /// </summary>
        private void NotificationOptionsExecute()
        {
            // Get current notification values for selected instance
            var notificationOptions = _strategyController.GetNotificationProperties(SelectedInstance.InstanceKey);

            if (notificationOptions != null)
            {
                NewOrderNotification = notificationOptions.Item1;
                AcceptedOrderNotification = notificationOptions.Item2;
                ExecutionNotification = notificationOptions.Item3;
                RejectionNotification = notificationOptions.Item4;
            }

            StrategyNotificaitonParameterWindow window = new StrategyNotificaitonParameterWindow();
            window.DataContext = this;
            window.Owner = MainWindow;

            window.ShowDialog();
        }

        /// <summary>
        /// Triggered when 'Save' button on Notification Options Window is clicked
        /// </summary>
        private void SaveNotificationOptionsExecute(object param)
        {
            // Save New Order notification options
            _strategyController.UpdateNotificationProperties(SelectedInstance.InstanceKey, _newOrderNotification,
                _acceptedOrderNotification, _executionNotification, _rejectionNotification);

            // Close "Edit Instance" window
            ((Window) param).Close();
        }

        /// <summary>
        /// Triggered when 'Export' button on strategy instance order executions is clicked
        /// </summary>
        private void ExportExecutionsExecute()
        {
            if (SelectedInstance != null && SelectedInstance.ExecutionDetails.OrderDetailsList.Count <= 0)
                return;

            string folderPath = string.Empty;

            // Get Directory in which to save stats
            using (System.Windows.Forms.FolderBrowserDialog form = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = form.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.Yes || dialogResult == Forms.DialogResult.OK)
                {
                    folderPath = form.SelectedPath;
                }
                else
                {
                    return;
                }
            }

            // Save data
            Task.Run(() =>
                    PersistCsv.SaveData(folderPath, SelectedInstance.ExecutionDetails.OrderDetailsList,
                        SelectedInstance.InstanceKey));
        }

        private bool ExportExecutionsCanExecute()
        {
            if (TradeSharpLicenseManager.GetLicense().IsDemo) return false;
            return true;
        }

        /// <summary>
        /// Triggered when 'Export all Instances' button on strategy instance order executions is clicked
        /// </summary>
        private void ExportAllExecutionsExecute()
        {
            if (SelectedStrategy == null || SelectedStrategy.StrategyInstances.Count <= 0)
                return;

            string folderPath = string.Empty;

            // Get Directory in which to save stats
            using (System.Windows.Forms.FolderBrowserDialog form = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = form.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.Yes || dialogResult == Forms.DialogResult.OK)
                {
                    folderPath = form.SelectedPath;
                }
                else
                {
                    return;
                }
            }

            // Save data
            Task.Run(() =>
                    Parallel.ForEach(SelectedStrategy.StrategyInstances, strategyInstance =>
                    {
                        if (strategyInstance.Value.IsSelected && strategyInstance.Value.ExecutionDetails.OrderDetailsList.Count > 0)
                        {
                            PersistCsv.SaveData(folderPath, strategyInstance.Value.ExecutionDetails.OrderDetailsList,
                                strategyInstance.Value.InstanceKey);
                        }
                    }));
        }

        private bool ExportAllExecutionsCanExecute()
        {
            if (TradeSharpLicenseManager.GetLicense().IsDemo) 
                return false;
            return IsMultipleSelected;
        }

        /// <summary>
        /// Triggered when 'Select all Instances' button is clicked
        /// </summary>
        private void SelectAllInstancesExecute()
        {
            foreach (var strategyInstance in SelectedStrategy.StrategyInstances.Values)
            {
                strategyInstance.IsSelected = true;
            }
        }

        private bool SelectAllInstancesCanExecute()
        {
            if (SelectedStrategy != null)
            {
                return SelectedStrategy.StrategyInstances.Count > 0;
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Displays Strategy Instances created against selected Strategy
        /// </summary>
        private void PopulateStrategyInstanceDataGrid()
        {
            // Clear current values
            Instances = new ObservableCollection<StrategyInstance>();

            // Populate Instances
            foreach (var strategyInstance in SelectedStrategy.StrategyInstances)
            {
                Instances.Add(strategyInstance.Value);
            }

            // Select the 1st instance initially
            SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
        }

        /// <summary>
        /// Displays 'Order Details' against selected Strategy Instance's 'Execution Details' item
        /// </summary>
        private void PopulateOrderDetailsDataGrid()
        {
            // Clear current values
            OrderDetailsCollection = new ObservableCollection<OrderDetails>();

            OrderDetailsCollection = SelectedExecutionDetails.OrderDetailsList;
        }

        /// <summary>
        /// Populates existing strategies 
        /// </summary>
        private void LoadExistingStrategies()
        {
            // Request Assembly Paths
            var pathsCollection = StrategyHelper.GetAllStrategiesPath();

            // Traverse List
            foreach (string path in pathsCollection)
            {
                // Create a new Strategy Object from the given strategy path
                var strategy = CreateStrategyObject(path);

                // Update Observable Collection
                Strategies.Add(strategy);
            }
        }

        /// <summary>
        /// Uses '.csv' file with input parameters to Create Instances
        /// </summary>
        /// <param name="fileName">.csv file complete name</param>
        private async void AddMultipleInstanceFromFile(string fileName)
        {
            var instances = await Task.Run(() => CreateMultipleStrategyInstances(fileName));

            // Update UI
            LoadMultipleInstances(instances);
        }

        /// <summary>
        /// Creates Multiple Strategy Instance objects using the selected file
        /// </summary>
        /// <param name="fileName">.csv file complete name</param>
        /// <returns>List of newly created Strategy Instances</returns>
        private async Task<IList<StrategyInstance>> CreateMultipleStrategyInstances(string fileName)
        {
            IList<StrategyInstance> instances = new List<StrategyInstance>();

            var parametersList = FileReader.ReadParameters(fileName);

            foreach (string[] parameters in parametersList)
            {
                if (parameters.Length != SelectedStrategy.ParameterDetails.Count)
                {
                    if (Logger.IsDebugEnabled)
                    {
                        Logger.Debug("Parameters count doesn't match the required number. " + parameters, _type.FullName,
                            "CreateMultipleStrategyInstances");
                    }
                    continue;
                }

                int count = 0;

                var instanceParameters = SelectedStrategy.ParameterDetails.ToDictionary(entry => entry.Key,
                    entry => (ParameterDetail) entry.Value.Clone());

                // Populate parameters to be used by Strategy Instance
                foreach (KeyValuePair<string, ParameterDetail> keyValuePair in instanceParameters)
                {
                    var input = StrategyHelper.GetParametereValue(parameters[count++],
                        keyValuePair.Value.ParameterType.Name);
                    keyValuePair.Value.ParameterValue = input;
                }

                // Create a new Strategy Instance with provided parameters
                var strategyInstance = SelectedStrategy.CreateInstance(instanceParameters, "");

                // Add to local Map
                instances.Add(strategyInstance);

                // Send instance to controller where its execution life cycle can be managed
                _strategyController.AddStrategyInstance(strategyInstance);
            }

            return instances;
        }

        /// <summary>
        /// Loads a list of newly created 'Strategy Instance' into UI and application session
        /// </summary>
        /// <param name="instances">List of Strategy Instance objects</param>
        private void LoadMultipleInstances(IList<StrategyInstance> instances)
        {
            foreach (StrategyInstance strategyInstance in instances)
            {
                // Display Strategy Instance on UI
                Instances.Add(strategyInstance);
            }

            // Select the 1st instance from DataGrid
            SelectedInstance = Instances.Count > 0 ? Instances[0] : null;

            // Set initial parameters for creating a new instance
            if (SelectedInstance != null)
                SelectedStrategy.ParameterDetails = SelectedInstance.Parameters.ToDictionary(entry => entry.Key,
                    entry => (ParameterDetail) entry.Value.Clone());
        }

        /// <summary>
        /// Adds a new Strategy object to UI collection
        /// </summary>
        /// <param name="strategyPath">Path from which to load the strategy</param>
        private void AddStrategy(string strategyPath)
        {
            // Verfiy Strategy Library
            if (_strategyController.VerifyAndAddStrategy(strategyPath))
            {
                // Create a new Strategy Object from the given strategy path
                var strategy = CreateStrategyObject(strategyPath);

                // Update Observable Collection
                Strategies.Add(strategy);
            }
            else
            {
                // Display Error message
                WPFMessageBox.Show(MainWindow, "Selected library is not a valid TradeHub Strategy.", "Strategy Runner",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Creates a new Strategy Object
        /// </summary>
        /// <param name="strategyPath">Path from which to load the strategy</param>
        private Strategy CreateStrategyObject(string strategyPath)
        {
            Dictionary<string, ParameterDetail> tempDictionary = new Dictionary<string, ParameterDetail>();

            // Get Strategy Type from the selected Assembly
            var strategyType = StrategyHelper.GetStrategyClassType(strategyPath);

            // Get Strategy Parameter details
            var details = StrategyHelper.GetParameterDetails(strategyType);

            // Fetch Strategy Name from Assembly
            var strategyName = StrategyHelper.GetCustomClassSummary(strategyType);

            // Retrieve Name of the '.dll' file
            var fileName = StrategyHelper.GetStrategyFileName(strategyPath);

            // Create Strategy Object
            Strategy strategy = new Strategy(strategyName, strategyType, fileName);

            foreach (KeyValuePair<string, Type> keyValuePair in details)
            {
                tempDictionary.Add(keyValuePair.Key, new ParameterDetail(keyValuePair.Value, ""));
            }

            // Set Strategy Parameter Details
            strategy.ParameterDetails = tempDictionary;

            // Return created Strategy
            return strategy;
        }

        /// <summary>
        /// Populate market data provider names
        /// </summary>
        private void PopulateMarketDataProviders()
        {
            // Clear any existing values
            MarketDataProviders.Clear();

            // Populate Individual Market Data Provider Details
            foreach (var provider in ProvidersController.MarketDataProviders)
            {
                // Add to Collection
                MarketDataProviders.Add(provider.ProviderName);
            }
        }

        /// <summary>
        /// Populate order execution provider names
        /// </summary>
        private void PopulateOrderExecutionProviders()
        {
            // Clear any existing values
            OrderExecutionProviders.Clear();

            // Travers Individual Order Execution Provider Details
            foreach (var provider in ProvidersController.OrderExecutionProviders)
            {
                // Add to Collection
                OrderExecutionProviders.Add(provider.ProviderName);
            }
        }

        /// <summary>
        /// Used for activiting windows opened from strategy runner view
        /// </summary>
        /// <param name="typeOfWindow"></param>
        /// <returns></returns>
        private bool TryActivateShownWindow(Type typeOfWindow)
        {
            if (Application.Current != null)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeOfWindow)
                    {
                        window.WindowState = WindowState.Normal;
                        window.Activate();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Called when application is closing
        /// </summary>
        /// <param name="message"></param>
        private void OnApplicationClose(string message)
        {
            if (message.Equals("Close"))
            {
                _strategyController.Close();
            }
        }
    }
}
