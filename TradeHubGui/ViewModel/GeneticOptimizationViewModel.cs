using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Resources.Controls;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Managers;
using TradeHubGui.Views;

namespace TradeHubGui.ViewModel
{
    /// <summary>
    /// Interaction logic for Genetic Optimization Window
    /// </summary>
    public class GeneticOptimizationViewModel : BaseViewModel
    {
        #region Fields

        private int _rounds;
        private int _iterations;
        private int _populationSize;
        private Dictionary<string, ParameterDetail> _parameterDetails;

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
            _selectedStrategy = strategy;

            // Initial default values
            _rounds = 1;
            _iterations = 1;
            _populationSize = 1;

            _managerGeneticAlgorithm = new OptimizationManagerGeneticAlgorithm();
            _optimizationParameters = new ObservableCollection<OptimizationParameterDetail>();
            _optimizedParameters = new ObservableCollection<OptimizationParameterDetail>();

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
                    param => ShowEditPropertiesWindowExecute()));
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

        #endregion

        #region Methods triggered on Commands

        /// <summary>
        /// Called when "Parameter Details" button is clicked
        /// </summary>
        private void ShowEditPropertiesWindowExecute()
        {
            EditInstanceWindow window = new EditInstanceWindow();
            window.DataContext = this;

            ParameterDetails = SelectedStrategy.ParameterDetails.ToDictionary(entry => entry.Key, entry => (ParameterDetail)entry.Value.Clone()); ;

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
            //TODO: make condition here (for example when export data is available)

            return false;
        }

        private void ExportGeneticOptimizationExecute()
        {
            //TODO: make implemetation
            throw new NotImplementedException();
        }

        private bool RunGeneticOptimizationCanExecute()
        {
            //TODO: make condition here if necessary (if no condition needed, just return true)

            return true;
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
            //TODO: make condition here (when the window can be closed, or return true if window can be closed anytime, but in that case just make sure to cancel execution if user click Close button)

            return false;
        }

        private object CloseGeneticOptimizationWindowExecute()
        {
            //TODO: make implementation (make sure to dispose this ViewModel on close, so this class should be disposable)
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Request <seealso cref="OptimizationManagerGeneticAlgorithm"/> to start the Optimization Process
        /// </summary>
        private void StartGeneticOptimization()
        {
            var parameterValues = GetParameterValues(SelectedStrategy.ParameterDetails).ToArray();

            SortedDictionary<int, OptimizationParameterDetail> sortedParameterDetails= new SortedDictionary<int, OptimizationParameterDetail>();

            foreach (var optimizationParameterDetail in _optimizationParameters)
            {
                sortedParameterDetails.Add(optimizationParameterDetail.Index, optimizationParameterDetail);
            }

            var strategyDetails = new GeneticAlgorithmParameters(SelectedStrategy.StrategyType, parameterValues, sortedParameterDetails, _iterations, _populationSize);

            // Raise Event to notify listeners to start Optimization Process
            EventSystem.Publish<GeneticAlgorithmParameters>(strategyDetails);
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
            foreach (OptimizationParameterDetail optimizationParameterDetail in optimizationResult.ParameterList)
            {
                OptimizedParameters.Add(optimizationParameterDetail);
            }
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
                // Add actual parameter values to the new object list
                parameterValues.Add(keyValuePair.Value.ParameterValue);
            }

            return parameterValues;
        }
    }
}
