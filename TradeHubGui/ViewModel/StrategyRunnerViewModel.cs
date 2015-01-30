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
using TradeHubGui.StrategyRunner.Services;
using TradeHubGui.Views;

namespace TradeHubGui.ViewModel
{
    public class StrategyRunnerViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<Strategy> _strategies;
        private ObservableCollection<StrategyInstance> _instances;
        private Strategy _selectedStrategy;
        private StrategyInstance _selectedInstance;
        private RelayCommand _showCreateInstanceWindowCommand;
        private RelayCommand _showEditInstanceWindowCommand;
        private RelayCommand _showGeneticOptimizationWindowCommand;
        private RelayCommand _showBruteOptimizationWindowCommand;
        private RelayCommand _loadStrategyCommand;
        private RelayCommand _selectProviderCommand;
        private RelayCommand _importInstancesCommand;
        private string _strategyPath;
        private string _csvInstancesPath;
        private Dictionary<string, Type> _parameterDetails;

        /// <summary>
        /// Provides functionality for all Strategy related operations
        /// </summary>
        private StrategyController _strategyController;

        public StrategyRunnerViewModel()
        {
            _strategyController = new StrategyController();
            _strategies = new ObservableCollection<Strategy>();

            // Get Existing Strategies in the system and populate on UI
            LoadExistingStrategies();
        }

        #region Observable Collections

        public ObservableCollection<Strategy> Strategies
        {
            get { return _strategies; }
            set
            {
                _strategies = value;
                OnPropertyChanged("Strategies");
            }
        }

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

        public Strategy SelectedStrategy
        {
            get { return _selectedStrategy; }
            set
            {
                _selectedStrategy = value;
                PopulateStrategyInstanceDataGrid(value.Key);
                OnPropertyChanged("SelectedStrategy");
            }
        }

        public StrategyInstance SelectedInstance
        {
            get { return _selectedInstance; }
            set
            {
                _selectedInstance = value;
                OnPropertyChanged("SelectedInstance");
            }
        }

        public Dictionary<string, Type> ParameterDetails
        {
            get { return _parameterDetails; }
            set
            {
                _parameterDetails = value;
                OnPropertyChanged("ParameterDetails");
            }
        }

        public string StrategyPath
        {
            get { return _strategyPath; }
            set
            {
                _strategyPath = value;
                OnPropertyChanged("StrategyPath");
            }
        }

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
                    param => ShowGeneticOptimizationExecute()));
            }
        }

        public ICommand ShowBruteOptimizationWindowCommand
        {
            get
            {
                return _showBruteOptimizationWindowCommand ?? (_showBruteOptimizationWindowCommand = new RelayCommand(
                    param => ShowBruteOptimizationExecute()));
            }
        }

        public ICommand LoadStrategyCommand
        {
            get
            {
                return _loadStrategyCommand ?? (_loadStrategyCommand = new RelayCommand(param => LoadStrategyExecute()));
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

        private void ShowGeneticOptimizationExecute()
        {
            if (TryActivateShownWindow(typeof(GeneticOptimizationWindow)))
            {
                return;
            }

            GeneticOptimizationWindow window = new GeneticOptimizationWindow();
            window.Tag = string.Format("{0}", SelectedStrategy.Key);
            window.Show();
        }

        private void ShowBruteOptimizationExecute()
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
                    
                    //TODO: Where are the values of parameters? ParameterDetails holds just Key as string and Value as Type of Parameter, but no ParamValue.
                    // Instead of Type for Value, maybe better to hold some object for Value (i.e. ParamDetail with two properties, ParamType and ParamValue). 
                    // We need to discuss this.
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

        private void LoadStrategyExecute()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = "Load Strategy File";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Assembly Files (.dll)|*.dll|All Files (*.*)|*.*";
            System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                StrategyPath = openFileDialog.FileName;

                // Create New Strategy Object
                AddStrategy(StrategyPath);
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

        private bool TryActivateShownWindow(Type TypeOfWindow)
        {
            if (Application.Current != null)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == TypeOfWindow)
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

                    SelectedInstance = Instances.Count > 0 ? Instances[0] : null;

                    break;
                }
            }
        }

        private void FillInstancesAA()
        {
            Instances = new ObservableCollection<StrategyInstance>();
            Instances.Add(new StrategyInstance() { InstanceKey = "AA001", Symbol = "GOOG", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA002", Symbol = "GOOG", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA003", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA004", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA005", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA006", Symbol = "GOOG", Description = "Dynamic trade", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA007", Symbol = "HP", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA008", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AA009", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
        }

        private void FillInstancesAB()
        {
            Instances = new ObservableCollection<StrategyInstance>();
            Instances.Add(new StrategyInstance() { InstanceKey = "AB001", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB002", Symbol = "HP", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB003", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB004", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB005", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB006", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB007", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB008", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB009", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB011", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB012", Symbol = "HP", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB013", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB014", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB015", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB016", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB017", Symbol = "HP", Description = "Test", StateBrush = Brushes.Goldenrod });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB018", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
            Instances.Add(new StrategyInstance() { InstanceKey = "AB019", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
        }

        private void FillInstancesAC()
        {
            Instances = new ObservableCollection<StrategyInstance>();
            Instances.Add(new StrategyInstance() { InstanceKey = "AC001", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AC002", Symbol = "HP", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
            Instances.Add(new StrategyInstance() { InstanceKey = "AC003", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.Crimson });

            for (int i = 4; i < 121; i++)
            {
                Instances.Add(new StrategyInstance() { InstanceKey = "AC00" + i, Symbol = "BMW", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
            }

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

            // Set Strategy Parameter Details
            strategy.ParameterDetails = details;

            // Return created Strategy
            return strategy;
        }
    }
}
