using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Resources.Controls;
using TraceSourceLogger;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.Dashboard.Services;
using TradeHubGui.StrategyRunner.Managers;
using TradeHubGui.Views;

namespace TradeHubGui.ViewModel
{
    /// <summary>
    /// Interaction logic for Genetic Optimization Window
    /// </summary>
    public class GeneticOptimizationViewModel : BaseViewModel
    {
        private Type _type = typeof(GeneticOptimizationViewModel);

        #region Fields

        /// <summary>
        /// Holds reference to UI dispatcher
        /// </summary>
        private readonly Dispatcher _currentDispatcher;

        /// <summary>
        /// No. of rounds Genetic Algorithm should execute
        /// </summary>
        private int _rounds;

        /// <summary>
        /// No. of rounds Genetic Algorithm should has executed
        /// </summary>
        private int _roundsCompleted;

        /// <summary>
        /// No. of iteration to be executed in each round
        /// </summary>
        private int _iterations;

        /// <summary>
        /// Size of the population while executing Genetic Algorithm
        /// </summary>
        private int _populationSize;

        /// <summary>
        /// Execution status of Genetic Algorithm Optimization
        /// </summary>
        private OptimizationStatus _status;

        /// <summary>
        /// Contains Parameter information to be used by the Strategy Instance to execute
        /// </summary>
        private Dictionary<string, ParameterDetail> _parameterDetails;
        
        /// <summary>
        /// Contains names for the available market data providers
        /// </summary>
        private ObservableCollection<string> _marketDataProviders;

        /// <summary>
        /// Contains names for the available order execution providers
        /// </summary>
        private ObservableCollection<string> _orderExecutionProviders;

        private RelayCommand _runGeneticOptimization;
        private RelayCommand _exportGeneticOptimization;
        private RelayCommand _closeGeneticOptimizationWindow;
        private RelayCommand _showEditPropertiesWindowCommand;
        private RelayCommand _editInstanceParametersCommand;

        private Strategy _selectedStrategy;

        /// <summary>
        /// Handles activities for Genetic Optimization
        /// </summary>
        private OptimizationManagerGeneticAlgorithm _managerGeneticAlgorithm;

        /// <summary>
        /// Collection of parameters to be used in Optimization process
        /// </summary>
        private ObservableCollection<OptimizationParameterDetail> _optimizationParameters;

        /// <summary>
        /// Holds Optimzation result with Optimized value for each parameter
        /// </summary>
        private ObservableCollection<OptimizationParameterDetail> _optimizedParameters;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public GeneticOptimizationViewModel(Strategy strategy)
        {
            // Save Dispatcher reference to be used for UI modifications
            _currentDispatcher = Dispatcher.CurrentDispatcher;

            _selectedStrategy = strategy;

            // Initial default values
            _rounds = 1;
            _iterations = 1;
            _populationSize = 45;
            _roundsCompleted = 0;

            _managerGeneticAlgorithm = new OptimizationManagerGeneticAlgorithm();
            _optimizationParameters = new ObservableCollection<OptimizationParameterDetail>();
            _optimizedParameters = new ObservableCollection<OptimizationParameterDetail>();
            _marketDataProviders = new ObservableCollection<string>();
            _orderExecutionProviders = new ObservableCollection<string>();
            
            // Populate Local maps for available Provider names
            PopulateMarketDataProviders();
            PopulateOrderExecutionProviders();

            // Make sure event is only subscribed once
            EventSystem.Unsubscribe<GeneticAlgorithmResult>(DisplayResult);

            // Subscribe event
            EventSystem.Subscribe<GeneticAlgorithmResult>(DisplayResult);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command used to open 'Edit Instance Window' from Strategy Instance Grid
        /// </summary>
        public ICommand ShowEditInstanceWindowCommand
        {
            get
            {
                return _showEditPropertiesWindowCommand ?? (_showEditPropertiesWindowCommand = new RelayCommand(
                    param => ShowEditPropertiesWindowExecute(param)));
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

        public ICommand RunGeneticOptimizationCommand
        {
            get
            {
                return _runGeneticOptimization ?? (_runGeneticOptimization = new RelayCommand(
                    param => RunGeneticOptimizationExecute(), param => RunGeneticOptimizationCanExecute()));
            }
        }

        public ICommand ExportGeneticOptimizationCommand
        {
            get
            {
                return _exportGeneticOptimization ?? (_exportGeneticOptimization = new RelayCommand(
                    param => ExportGeneticOptimizationExecute(), param => ExportGeneticOptimizationCanExecute()));
            }
        }

        public ICommand CloseGeneticOptimizationWindowCommand
        {
            get
            {
                return _closeGeneticOptimizationWindow ?? (_closeGeneticOptimizationWindow = new RelayCommand(
                    param => CloseGeneticOptimizationWindowExecute(), param => CloseGeneticOptimizationWindowCanExecute()));
            }
        }

        #endregion

        #region Properties

        public int Rounds
        {
            get { return _rounds; }
            set
            {
                if (_rounds != value)
                {
                    _rounds = value;
                    OnPropertyChanged("Rounds");
                }
            }
        }

        public int Iterations
        {
            get { return _iterations; }
            set
            {
                if (_iterations != value)
                {
                    _iterations = value;
                    OnPropertyChanged("Iterations");
                }
            }
        }

        public int PopulationSize
        {
            get { return _populationSize; }
            set
            {
                if (_populationSize != value)
                {
                    _populationSize = value;
                    OnPropertyChanged("PopulationSize");
                }
            }
        }

        /// <summary>
        /// Related Strategy for Genetic Optimization
        /// </summary>
        public Strategy SelectedStrategy
        {
            get { return _selectedStrategy; }
            set
            {
                if (_selectedStrategy != value)
                {
                    _selectedStrategy = value;
                    OnPropertyChanged("Strategy");
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
        /// Collection of parameters to be used in Optimization process
        /// </summary>
        public ObservableCollection<OptimizationParameterDetail> OptimizationParameters
        {
            get { return _optimizationParameters; }
            set
            {
                _optimizationParameters = value;
                OnPropertyChanged("OptimizationParameters");
            }
        }

        /// <summary>
        /// Holds Optimzation result with Optimized value for each parameter
        /// </summary>
        public ObservableCollection<OptimizationParameterDetail> OptimizedParameters
        {
            get { return _optimizedParameters; }
            set
            {
                _optimizedParameters = value;
                OnPropertyChanged("OptimizatedParameters");
            }
        }

        /// <summary>
        /// No. of rounds Genetic Algorithm should has executed
        /// </summary>
        public int RoundsCompleted
        {
            get { return _roundsCompleted; }
            set
            {
                _roundsCompleted = value;
                OnPropertyChanged("RoundsCompleted");
            }
        }

        /// <summary>
        /// Execution status of Genetic Algorithm Optimization
        /// </summary>
        public OptimizationStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
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

        #region Methods triggered on Commands

        /// <summary>
        /// Called when "Parameter Details" button is clicked
        /// </summary>
        private void ShowEditPropertiesWindowExecute(object param)
        {
            ParameterDetails = SelectedStrategy.ParameterDetails.ToDictionary(entry => entry.Key, entry => (ParameterDetail)entry.Value.Clone());
            
            EditInstanceWindow window = new EditInstanceWindow();
            window.DataContext = this;
            window.Title = "EDIT PARAMETERS";
            window.Tag = string.Format("Parameters for strategy {0}", _selectedStrategy.Key);
            window.Owner = (Window)param;
            window.ShowDialog();
        }

        /// <summary>
        /// Modifies selected Strategy Instance's Parameter Details
        /// </summary>
        private void EditInstanceParametersExecute(object param)
        {
            // Update Parameter Details
            SelectedStrategy.ParameterDetails = ParameterDetails.ToDictionary(entry => entry.Key, entry => (ParameterDetail)entry.Value.Clone());

            // Close "Edit Instance" window
            ((Window)param).Close();

            OptimizationParameterDetails();
        }

        private bool ExportGeneticOptimizationCanExecute()
        {
            return (RoundsCompleted == Rounds);
        }

        /// <summary>
        /// Called when 'Export' button is clicked
        /// </summary>
        private void ExportGeneticOptimizationExecute()
        {
            ExportGeneticAlgorithmResults();
        }

        private bool RunGeneticOptimizationCanExecute()
        {
            bool canExecute = CheckValidOptimizationParameterValues();

            if (Status.Equals(OptimizationStatus.Working))
            {
                canExecute = false;
            }
            
            return canExecute;
        }

        /// <summary>
        /// Called when 'Execute' button is clicked
        /// </summary>
        private void RunGeneticOptimizationExecute()
        {
            StartGeneticOptimization();
        }

        private bool CloseGeneticOptimizationWindowCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Called when window 'Close' is hit
        /// </summary>
        /// <returns></returns>
        private object CloseGeneticOptimizationWindowExecute()
        {
            _managerGeneticAlgorithm.Dispose();
            return null;
        }

        #endregion

        /// <summary>
        /// Request <seealso cref="OptimizationManagerGeneticAlgorithm"/> to start the Optimization Process
        /// </summary>
        private void StartGeneticOptimization()
        {
            // Reset Value
            RoundsCompleted = 0;

            // Array of object to be used as parameters for given strategy
            var parameterValues = GetParameterValues(SelectedStrategy.ParameterDetails).ToArray();

            var optimizationParametersDetail = new SortedDictionary<int, OptimizationParameterDetail>();

            foreach (var optimizationParameterDetail in OptimizationParameters)
            {
                optimizationParametersDetail.Add(optimizationParameterDetail.Index, optimizationParameterDetail);
            }

            var strategyDetails = new GeneticAlgorithmParameters(SelectedStrategy.StrategyType, parameterValues,
                optimizationParametersDetail, _iterations, _populationSize, _rounds);

            // Change Status
            Status = OptimizationStatus.Working;

            // Clear existing data
            OptimizedParameters.Clear();

            // Raise Event to notify listeners to start Optimization Process
            Task.Factory.StartNew(() => { EventSystem.Publish<GeneticAlgorithmParameters>(strategyDetails); });
        }

        /// <summary>
        /// Finds parameters to be used for optimization for the given Strategy and displays on UI
        /// </summary>
        private void OptimizationParameterDetails()
        {
            // Clear existing data
            OptimizationParameters.Clear();

            // Contains custom defined attributes in the given assembly
            Dictionary<int, Tuple<string, Type>> customAttributes = null;

            // Get Custom Attributes
            if (_selectedStrategy.StrategyType != null)
            {
                // Get custom attributes from the given assembly
                customAttributes = StrategyHelper.GetCustomAttributes(_selectedStrategy.StrategyType);

                foreach (KeyValuePair<int, Tuple<string, Type>> keyValuePair in customAttributes)
                {
                    var parameter = new OptimizationParameterDetail();

                    parameter.Index = keyValuePair.Key;
                    parameter.Description = keyValuePair.Value.Item1;
                    parameter.ParameterType = keyValuePair.Value.Item2;

                    // Add to observable collection to display on UI
                    OptimizationParameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// Displays GA Optimization Result on UI
        /// </summary>
        /// <param name="optimizationResult">Contains optimized parameters information</param>
        private void DisplayResult(GeneticAlgorithmResult optimizationResult)
        {
            _currentDispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                RoundsCompleted++;

                foreach (OptimizationParameterDetail optimizationParameterDetail in optimizationResult.ParameterList)
                {
                    OptimizedParameters.Add(optimizationParameterDetail);
                }

                if (RoundsCompleted == Rounds)
                {
                    Status = OptimizationStatus.Completed;
                }
            }));
        }

        /// <summary>
        /// Returns IList of actual parameter values from the Parameter Details object
        /// </summary>
        /// <returns></returns>
        public IList<object> GetParameterValues(Dictionary<string, ParameterDetail> parameterDetails)
        {
            IList<object> parameterValues = new List<object>();

            // Traverse all parameter
            foreach (KeyValuePair<string, ParameterDetail> keyValuePair in parameterDetails)
            {
                // Makes sure all parameters are in right format
                var input = StrategyHelper.GetParametereValue(keyValuePair.Value.ParameterValue.ToString(), keyValuePair.Value.ParameterType.Name);

                // Add actual parameter values to the new object list
                parameterValues.Add(input);
            }

            return parameterValues;
        }

        /// <summary>
        /// Dump the results to file
        /// </summary>
        private void ExportGeneticAlgorithmResults()
        {
            try
            {
                IList<string> lines = null;
                string folderPath = string.Empty;

                // Get Directory in which to save stats
                using (System.Windows.Forms.FolderBrowserDialog form = new System.Windows.Forms.FolderBrowserDialog())
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        folderPath = form.SelectedPath;
                    }
                }

                // Write Optimization Stats to the file
                if (folderPath != string.Empty)
                {
                    lines = new List<string>();

                    // Create header row
                    string header = "Round,";
                    foreach (var optimizationParameterDetail in _optimizationParameters)
                    {
                        header += optimizationParameterDetail.Description + ",";
                    }
                    header += "Risk";

                    // Add Header Row
                    lines.Add(header);

                    int tempRound = 1;
                    int tempItemCount = 0;
                    string row = tempRound.ToString();

                    // Create Individual Rows
                    foreach (var optimizationParameterDetail in _optimizedParameters)
                    {
                        row += "," + optimizationParameterDetail.OptimizedValue;

                        if (++tempItemCount == (_optimizationParameters.Count + 1))
                        {
                            // Add Row
                            lines.Add(row);

                            // Start a new row
                            row = (++tempRound).ToString();

                            // Reset variable
                            tempItemCount = 0;
                        }
                    }

                    // NOTE: Below line is wrong (wrong file name and path probably.)

                    // Create file path
                    string path = folderPath + "\\" + "GA-Results.csv";

                    // Write data
                    File.WriteAllLines(path, lines);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "ExportGeneticAlgoReuslts");
            }
        }

        /// <summary>
        /// Performs some basic checks to see if valid Optimization parameter values are provided
        /// </summary>
        /// <returns></returns>
        private bool CheckValidOptimizationParameterValues()
        {
            if (_optimizationParameters.Count==0)
                return false;

            foreach (var optimizationParameterDetail in _optimizationParameters)
            {
                if (optimizationParameterDetail.StartValue.Equals(default(double)))
                    return false;

                if (optimizationParameterDetail.EndValue.Equals(default(double)))
                    return false;

                if (optimizationParameterDetail.StartValue.Equals(optimizationParameterDetail.EndValue))
                    return false;

                if (optimizationParameterDetail.EndValue < optimizationParameterDetail.StartValue)
                    return false;
            }

            return true;
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
    }
}
