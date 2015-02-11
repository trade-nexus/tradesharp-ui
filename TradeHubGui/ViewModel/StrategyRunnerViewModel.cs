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
using TradeHubGui.Common.Utility;
using DomainModels = TradeHub.Common.Core.DomainModels;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Services;
using TradeHubGui.Views;
using MessageBoxUtils;
using Forms = System.Windows.Forms;

namespace TradeHubGui.ViewModel
{
    public class StrategyRunnerViewModel : BaseViewModel
    {
        private Type _type = typeof(StrategyRunnerViewModel);

        private MetroDialogSettings _dialogSettings;
        private ObservableCollection<Strategy> _strategies;
        private ObservableCollection<StrategyInstance> _instances;
        private ObservableCollection<ExecutionDetails> _executionDetailsCollection;
        private ObservableCollection<OrderDetails> _orderDetailsCollection;
        private ExecutionDetails _selectedExecutionDetails;
        private bool _executionDetailsForAllInstances;
        private Strategy _selectedStrategy;
        private StrategyInstance _selectedInstance;
        private RelayCommand _showCreateInstanceWindowCommand;
        private RelayCommand _createInstanceCommand;
        private RelayCommand _runInstanceCommand;
        private RelayCommand _stopInstanceCommand;
        private RelayCommand _deleteInstanceCommand;
        private RelayCommand _editInstanceParametersCommand;
        private RelayCommand _showEditInstanceWindowCommand;
        private RelayCommand _showGeneticOptimizationWindowCommand;
        private RelayCommand _showBruteOptimizationWindowCommand;
        private RelayCommand _loadStrategyCommand;
        private RelayCommand _removeStrategyCommand;
        private RelayCommand _selectProviderCommand;
        private RelayCommand _importInstancesCommand;
        private string _strategyPath;
        private string _csvInstancesPath;
        private Dictionary<string, ParameterDetail> _parameterDetails;

        /// <summary>
        /// Provides functionality for all Strategy related operations
        /// </summary>
        private StrategyController _strategyController;

        /// <summary>
        /// Constructor
        /// </summary>
        public StrategyRunnerViewModel()
        {
            _strategyController = new StrategyController();
            _strategies = new ObservableCollection<Strategy>();

            // Get Existing Strategies in the system and populate on UI
            LoadExistingStrategies();

            // Select 1st strategy from ListBox if not empty
            if (Strategies.Count > 0)
            {
                SelectedStrategy = Strategies[0];
            }
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
        public ObservableCollection<ExecutionDetails> ExecutionDetailsCollection
        {
            get
            {
                _executionDetailsCollection = new ObservableCollection<ExecutionDetails>();

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
        public ExecutionDetails SelectedExecutionDetails
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
        /// Command used with Run button in the Strategy Instance grid
        /// </summary>
        public ICommand StopInstanceCommand
        {
            get
            {
                return _stopInstanceCommand ?? (_stopInstanceCommand = new RelayCommand(param => StopInstanceExecute()));
            }
        }
        
        /// <summary>
        /// Command used with 'Edit' button in Edit Instance Window
        /// </summary>
        public ICommand EditInstanceParametersCommand
        {
            get
            {
                return _editInstanceParametersCommand ?? (_editInstanceParametersCommand = new RelayCommand(param => EditInstanceParametersExecute(param)));
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
                return _showGeneticOptimizationWindowCommand ?? (_showGeneticOptimizationWindowCommand = new RelayCommand(
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
                return _loadStrategyCommand ?? (_loadStrategyCommand = new RelayCommand(param => LoadStrategyExecute()));
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
                       (_importInstancesCommand = new RelayCommand(param => ImportInstancesExecute()));
            }
        }

        #endregion

        /// <summary>
        /// Displays Strategy Instance Parameter's window to edit input values
        /// </summary>
        private void ShowEditInstanceWindowExecute()
        {
            EditInstanceWindow window = new EditInstanceWindow();
            window.DataContext = this;
            window.Tag = string.Format("Instance {0}", SelectedInstance.InstanceKey);
            window.Owner = MainWindow;

            ParameterDetails = SelectedInstance.Parameters.ToDictionary(entry => entry.Key, entry => (ParameterDetail)entry.Value.Clone());;

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
            var parameters = SelectedStrategy.ParameterDetails.ToDictionary(entry => entry.Key, entry => (ParameterDetail)entry.Value.Clone());

            // Create a new Strategy Instance with provided parameters
            var strategyInstance = SelectedStrategy.CreateInstance(parameters);

            // Select created instance in DataGrid
            SelectedInstance = strategyInstance;

            // Set Initial status to 'Stopped'
            SelectedInstance.Status = DomainModels.StrategyStatus.None;

            // Add Instance to Observable Collection for UI
            Instances.Add(strategyInstance);

            // Send instance to controller where its execution life cycle can be managed
            _strategyController.AddStrategyInstance(strategyInstance);

            // Close "Create Instance" window
            ((Window)param).Close();
        }

        /// <summary>
        /// Start Strategy Instance execution. Triggered with 'RunInstanceCommand'
        /// </summary>
        private void RunInstanceExecute()
        {
            SelectedInstance.Status = DomainModels.StrategyStatus.Initializing;
            // Request Strategy Controller to start selected Strategy Instance execution
            _strategyController.RunStrategy(SelectedInstance.InstanceKey);

            //SelectedInstance.Status = DomainModels.StrategyStatus.Executing;
        }

        /// <summary>
        /// Triggered with 'StopInstanceCommand'
        /// </summary>
        private void StopInstanceExecute()
        {
            // Request Strategy Controller to Stop execution for selected Strategy Instance
            _strategyController.StopStrategy(SelectedInstance.InstanceKey);

            //SelectedInstance.Status = DomainModels.StrategyStatus.None;
        }

        /// <summary>
        /// Removes selected intance from the UI and requests Strategy Controller to remove it from working session as well.
        /// </summary>
        private void DeleteInstanceExecute()
        {
            if ((Forms.DialogResult)WPFMessageBox.Show(MainWindow, string.Format("Delete strategy instance {0}?", SelectedInstance.InstanceKey), "Strategy Runner",
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
        /// Modifies selected Strategy Instance's Parameter Details
        /// </summary>
        private void EditInstanceParametersExecute(object param)
        {
            // Update Parameter Details
            SelectedInstance.Parameters = ParameterDetails.ToDictionary(entry => entry.Key, entry => (ParameterDetail)entry.Value.Clone());

            // Close "Edit Instance" window
            ((Window)param).Close();
        }

        private void ShowGeneticOptimizationWindowExecute()
        {
            if (TryActivateShownWindow(typeof(GeneticOptimizationWindow)))
            {
                return;
            }

            GeneticOptimizationWindow window = new GeneticOptimizationWindow();

            // Create ViewModel as DataContext and pass selected strategy for Genetic Optimization
            window.DataContext = new GeneticOptimizationViewModel(SelectedStrategy);
            window.Tag = string.Format("{0}", SelectedStrategy.Key);
            window.Show();
        }

        private void ShowBruteOptimizationWindowExecute()
        {
            if (TryActivateShownWindow(typeof(BruteOptimizationWindow)))
            {
                return;
            }

            BruteOptimizationWindow window = new BruteOptimizationWindow();
            window.DataContext = new BruteOptimizationViewModel(SelectedStrategy);
            window.Tag = string.Format("{0}", SelectedStrategy.Key);
            window.Show();
        }

        private void SelectProviderExecute()
        {
            SelectProviderWindow window = new SelectProviderWindow();
            window.Owner = MainWindow;
            window.ShowDialog();
        }

        /// <summary>
        /// Loads a New Strategy into the Application
        /// </summary>
        private void LoadStrategyExecute()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = "Load Strategy File";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Assembly Files (.dll)|*.dll|All Files (*.*)|*.*";
            System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
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
            if ((Forms.DialogResult)WPFMessageBox.Show(MainWindow, string.Format("Remove strategy {0}?", SelectedStrategy.Key), "Strategy Runner",
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

        private void ImportInstancesExecute()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = "Import Instances";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                CsvInstancesPath = openFileDialog.FileName;

                AddMultipleInstanceFromFile(CsvInstancesPath);
            }
        }

        private bool TryActivateShownWindow(Type typeOfWindow)
        {
            if (Application.Current != null)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeOfWindow)
                    {
                        window.Focus();
                        window.Activate();
                        return true;
                    }
                }
            }

            return false;
        }

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

            // Select the 1st instance from DataGrid
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
        /// Dummy method for filling example instances
        /// </summary>
        private void FillInstancesAA()
        {
            Instances = new ObservableCollection<StrategyInstance>();
            Instances.Add(new StrategyInstance() { InstanceKey = "AA001", Symbol = "GOOG", Description = "Dynamic trade", Status = DomainModels.StrategyStatus.None });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA002", Symbol = "GOOG", Description = "Test", Status = DomainModels.StrategyStatus.None });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA003", Symbol = "HP", Description = "Test", Status = DomainModels.StrategyStatus.Executing });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA004", Symbol = "AAPL", Description = "Test", Status = DomainModels.StrategyStatus.Executed });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA005", Symbol = "MSFT", Description = "Dynamic trade", Status = DomainModels.StrategyStatus.Executed });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA006", Symbol = "GOOG", Description = "Dynamic trade", Status = DomainModels.StrategyStatus.Executed });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA007", Symbol = "HP", Description = "Test", Status = DomainModels.StrategyStatus.Executed });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA008", Symbol = "HP", Description = "Test", Status = DomainModels.StrategyStatus.None });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA009", Symbol = "MSFT", Description = "Dynamic trade", Status = DomainModels.StrategyStatus.None });
            SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
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
            var instances = await Task.Factory.StartNew(() => CreateMultipleStrategyInstances(fileName));
            //var instances = CreateMultipleStrategyInstances(fileName);

            // Update UI
            LoadMultipleInstances(instances);
        }

        /// <summary>
        /// Creates Multiple Strategy Instance objects using the selected file
        /// </summary>
        /// <param name="fileName">.csv file complete name</param>
        /// <returns>List of newly created Strategy Instances</returns>
        private IList<StrategyInstance> CreateMultipleStrategyInstances(string fileName)
        {
            IList<StrategyInstance> instances = new List<StrategyInstance>();

            var parametersList = FileReader.ReadParameters(fileName);

            foreach (string[] parameters in parametersList)
            {
                if (parameters.Length != SelectedStrategy.ParameterDetails.Count)
                {
                    if (Logger.IsDebugEnabled)
                    {
                        Logger.Debug("Parameters count doesn't match the required number. " + parameters, _type.FullName, "CreateMultipleStrategyInstances");
                    }
                    continue;
                }

                int count = 0;

                var instanceParameters = SelectedStrategy.ParameterDetails.ToDictionary(entry => entry.Key, entry => (ParameterDetail)entry.Value.Clone());

                // Populate parameters to be used by Strategy Instance
                foreach (KeyValuePair<string, ParameterDetail> keyValuePair in instanceParameters)
                {
                    var input = StrategyHelper.GetParametereValue(parameters[count++], keyValuePair.Value.ParameterType.Name);
                    keyValuePair.Value.ParameterValue = input;
                }

                // Create a new Strategy Instance with provided parameters
                var strategyInstance = SelectedStrategy.CreateInstance(instanceParameters);

                // Add to local Map
                instances.Add(strategyInstance);
            }

            return instances;
        }

        /// <summary>
        /// Loads a list of newly created 'Strategy Instance' into UI and application session
        /// </summary>
        /// <param name="instances">List of Strategy Instance objects</param>
        private void LoadMultipleInstances(IList<StrategyInstance> instances)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            foreach (StrategyInstance strategyInstance in instances)
            {
                // Send instance to controller where its execution life cycle can be managed
                _strategyController.AddStrategyInstance(strategyInstance);

                // Display Strategy Instance on UI
                Instances.Add(strategyInstance);
            }

            stopWatch.Stop();
            Console.WriteLine("LoadMultipleInstances method runtime ---> " + stopWatch.Elapsed);

            // Select the 1st instance from DataGrid
            SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
        }

        /// <summary>
        /// Creates a new strategy object
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
    }
}
