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
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Services;
using TradeHubGui.Views;

namespace TradeHubGui.ViewModel
{
    public class StrategyRunnerViewModel : BaseViewModel
    {
        private MetroDialogSettings _dialogSettings;
        private ObservableCollection<Strategy> _strategies;
        private ObservableCollection<StrategyInstance> _instances;
        private Strategy _selectedStrategy;
        private StrategyInstance _selectedInstance;
        private RelayCommand _showCreateInstanceWindowCommand;
        private RelayCommand _createInstanceCommand;
        private RelayCommand _runInstanceCommand;
        private RelayCommand _stopInstanceCommand;
        private RelayCommand _deleteInstanceCommand;
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
            _dialogSettings = new MetroDialogSettings() { AffirmativeButtonText = "Yes", NegativeButtonText = "No" };

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
        /// Collection of instances for selected strategy
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
                    PopulateStrategyInstanceDataGrid(value.Key);

                OnPropertyChanged("SelectedStrategy");
            }
        }

        /// <summary>
        /// Selected instance for selected strategy
        /// </summary>
        public StrategyInstance SelectedInstance
        {
            get { return _selectedInstance; }
            set
            {
                _selectedInstance = value;
                OnPropertyChanged("SelectedInstance");
            }
        }

        /// <summary>
        /// Parameter details for selected instance
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

        public ICommand CreateInstanceCommand
        {
            get
            {
                return _createInstanceCommand ?? (_createInstanceCommand = new RelayCommand(
                    param => CreateInstanceExecute(param), param => CreateInstanceCanExecute()));
            }
        }

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

        public ICommand ShowEditInstanceWindowCommand
        {
            get
            {
                return _showEditInstanceWindowCommand ?? (_showEditInstanceWindowCommand = new RelayCommand(
                    param => ShowEditInstanceWindowExecute(), param => ShowEditInstanceWindowCanExecute()));
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

        private bool ShowEditInstanceWindowCanExecute()
        {
            if (SelectedInstance == null) return false;
            return true;
        }

        private void ShowEditInstanceWindowExecute()
        {
            EditInstanceWindow window = new EditInstanceWindow();
            window.Tag = string.Format("INSTANCE {0}", SelectedInstance.InstanceKey);
            window.Owner = MainWindow;
            window.ShowDialog();
        }

        private bool ShowCreateInstanceWindowCanExecute()
        {
            if (SelectedStrategy == null) return false;
            return true;
        }

        private bool CreateInstanceCanExecute()
        {
            return true;
        }

        private void CreateInstanceExecute(object param)
        {
            IList<object> parameters = new List<object>();

            // Traverse all parameter
            foreach (KeyValuePair<string, ParameterDetail> keyValuePair in ParameterDetails)
            {
                // Add actual parameter values to the new object list
                parameters.Add(keyValuePair.Value.ParameterValue);
            }

            // Create a new Strategy Instance with provided parameters
            var strategyInstace = SelectedStrategy.CreateInstance(parameters.ToArray());

            // Set Initial status to 'Stopped'
            strategyInstace.ExecutionState = "Running";

            // Add Instance to Observable Collection for UI
            Instances.Add(strategyInstace);

            // Close "Create Instance" window
            ((Window)param).Close();
        }

        /// <summary>
        /// Triggered with 'RunInstanceCommand'
        /// </summary>
        private void RunInstanceExecute()
        {
            // Request Strategy Controller to start selected Strategy Instance execution
            //_strategyController.RunStrategy(SelectedInstance.InstanceKey);

            SelectedInstance.ExecutionState = "Running";
        }

        /// <summary>
        /// Triggered with 'StopInstanceCommand'
        /// </summary>
        private void StopInstanceExecute()
        {
            // Request Strategy Controller to Stop execution for selected Strategy Instance
            //_strategyController.StopStrategy(SelectedInstance.InstanceKey);

            SelectedInstance.ExecutionState = "Stopped";
        }

        /// <summary>
        /// Removes selected intance from the UI and requests Strategy Controller to remove it from working session as well.
        /// </summary>
        private async void DeleteInstanceExecute()
        {
            if (await
                MainWindow.ShowMessageAsync("Question",
                    string.Format("Delete strategy instance {0}?", SelectedInstance.InstanceKey),
                    MessageDialogStyle.AffirmativeAndNegative, _dialogSettings) == MessageDialogResult.Affirmative)
            {
                // Remove the instance from the UI
                Instances.Remove(SelectedInstance);

                // Remove the Instance from Strategy's Local Map
                SelectedStrategy.RemoveInstance(SelectedInstance.InstanceKey);

                // Request Strategy Controller to remove the instance from working session
                _strategyController.RemoveStrategyInstance(SelectedInstance.InstanceKey);
            }
        }

        private void ShowGeneticOptimizationWindowExecute()
        {
            if (TryActivateShownWindow(typeof(GeneticOptimizationWindow)))
            {
                return;
            }

            GeneticOptimizationWindow window = new GeneticOptimizationWindow();
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
            window.Tag = string.Format("{0}", SelectedStrategy.Key);
            window.Show();
        }

        private void ShowCreateInstanceWindowExecute()
        {
            CreateInstanceWindow window = new CreateInstanceWindow();
            window.DataContext = this;
            window.Tag = string.Format("STRATEGY {0}", SelectedStrategy.Key);

            // Traverse collection to find intended Strategy 
            foreach (var strategy in _strategies)
            {
                // Retrieve Parameters Information
                if (strategy.Key.Equals(SelectedStrategy.Key))
                {
                    ParameterDetails = strategy.ParameterDetails;
                }
            }

            window.Owner = MainWindow;
            window.ShowDialog();
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
        private async void RemoveStrategyExecute()
        {
            if (await MainWindow.ShowMessageAsync("Question", string.Format("Remove strategy {0}?", SelectedStrategy.Key),
               MessageDialogStyle.AffirmativeAndNegative, _dialogSettings) == MessageDialogResult.Affirmative)
            {
                Strategies.Remove(SelectedStrategy);
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
        /// <param name="strategyKey">Key to identify Strategy</param>
        private void PopulateStrategyInstanceDataGrid(string strategyKey)
        {
            // Find given Strategy
            foreach (Strategy strategy in _strategies.ToList())
            {
                // Find Strategy
                if (strategy.Key.Equals(strategyKey))
                {
                    // Clear current values
                    Instances = new ObservableCollection<StrategyInstance>();

                    // Populate Instances
                    foreach (var strategyInstance in strategy.StrategyInstances)
                    {
                        Instances.Add(strategyInstance.Value);
                    }

                    // Set the 1st instance as selected in UI
                    SelectedInstance = Instances.Count > 0 ? Instances[0] : null;

                    // Terminate Loop
                    break;
                }
            }
        }

        /// <summary>
        /// Dummy method for filling example instances
        /// </summary>
        private void FillInstancesAA()
        {
            Instances = new ObservableCollection<StrategyInstance>();
            Instances.Add(new StrategyInstance() { InstanceKey = "AA001", Symbol = "GOOG", Description = "Dynamic trade", ExecutionState = "Running" });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA002", Symbol = "GOOG", Description = "Test", ExecutionState = "Stopped" });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA003", Symbol = "HP", Description = "Test", ExecutionState = "Stopped" });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA004", Symbol = "AAPL", Description = "Test", ExecutionState = "Stopped" });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA005", Symbol = "MSFT", Description = "Dynamic trade", ExecutionState = "Stopped" });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA006", Symbol = "GOOG", Description = "Dynamic trade", ExecutionState = "Stopped" });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA007", Symbol = "HP", Description = "Test", ExecutionState = "Running" });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA008", Symbol = "HP", Description = "Test", ExecutionState = "Stopped" });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA009", Symbol = "MSFT", Description = "Dynamic trade", ExecutionState = "Running" });
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
                _strategies.Add(strategy);
            }
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
                _strategies.Add(strategy);
            }
        }

        /// <summary>
        /// Creates a new Strategy Object
        /// </summary>
        /// <param name="strategyPath">Path from which to load the strategy</param>
        private Strategy CreateStrategyObject(string strategyPath)
        {
            // Get Strategy Type from the selected Assembly
            var strategyType = StrategyHelper.GetStrategyClassType(strategyPath);

            // Get Strategy Parameter details
            var details = StrategyHelper.GetParameterDetails(strategyType);

            // Fetch Strategy Name from Assembly
            var strategyName = StrategyHelper.GetCustomClassSummary(strategyType);

            // Create Strategy Object
            Strategy strategy = new Strategy(strategyName, strategyType);

            Dictionary<string, ParameterDetail> tempDictionary = new Dictionary<string, ParameterDetail>();

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
