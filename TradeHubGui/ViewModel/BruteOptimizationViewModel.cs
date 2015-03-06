using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TraceSourceLogger;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Managers;

namespace TradeHubGui.ViewModel
{
    public class BruteOptimizationViewModel : BaseViewModel
    {
        #region Fields

        private Type _type = typeof (BruteOptimizationViewModel);

        /// <summary>
        /// Holds reference to UI dispatcher
        /// </summary>
        private readonly Dispatcher _currentDispatcher;

        /// <summary>
        /// Handles activities for Brute Force Optimization
        /// </summary>
        private OptimizationManagerBruteForce _managerBruteForce; 

        /// <summary>
        /// Contains detailed information for all the parameters
        /// </summary>
        private BruteForceParameters _bruteForceParameters;

        /// <summary>
        /// Optimization Statistics for each iteration execution during Brute Force
        /// </summary>
        private ObservableCollection<OptimizationStatistics> _optimizationStatisticsCollection; 

        private RelayCommand _runBruteOptimization;
        private RelayCommand _exportBruteOptimization;
        private RelayCommand _closeBruteOptimizationWindow;
        
        private Strategy _selectedStrategy;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public BruteOptimizationViewModel(Strategy strategy)
        {
            // Save Dispatcher reference to be used for UI modifications
            _currentDispatcher = Dispatcher.CurrentDispatcher;
            
            _selectedStrategy = strategy;
            _bruteForceParameters = new BruteForceParameters(_selectedStrategy.StrategyType);
            _managerBruteForce = new OptimizationManagerBruteForce();
            OptimizationStatisticsCollection = new ObservableCollection<OptimizationStatistics>();

            DisplayParameterDetails();

            // Subscribe Event to Update UI for results
            EventSystem.Subscribe<OptimizationStatistics>(DisplayOptimizationStatistics);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Contains detailed information for all the parameters
        /// </summary>
        public BruteForceParameters BruteForceParameters
        {
            get { return _bruteForceParameters; }
            set
            {
                _bruteForceParameters = value;
                OnPropertyChanged("BruteForceParameters");
            }
        }

        /// <summary>
        /// Optimization Statistics for each iteration execution during Brute Force
        /// </summary>
        public ObservableCollection<OptimizationStatistics> OptimizationStatisticsCollection
        {
            get { return _optimizationStatisticsCollection; }
            set
            {
                _optimizationStatisticsCollection = value;
                OnPropertyChanged("OptimizationStatisticsCollection");
            }
        }

        #endregion

        #region Commands

        public ICommand RunBruteOptimizationCommand
        {
            get
            {
                return _runBruteOptimization ?? (_runBruteOptimization = new RelayCommand(
                    param => RunBruteOptimizationExecute(), param => RunBruteOptimizationCanExecute()));
            }
        }

        public ICommand ExportBruteOptimizationCommand
        {
            get
            {
                return _exportBruteOptimization ?? (_exportBruteOptimization = new RelayCommand(
                    param => ExportBruteOptimizationExecute(), param => ExportBruteOptimizationCanExecute()));
            }
        }

        public ICommand CloseBruteOptimizationWindowCommand
        {
            get
            {
                return _closeBruteOptimizationWindow ?? (_closeBruteOptimizationWindow = new RelayCommand(
                    param => CloseBruteOptimizationWindowExecute(), param => CloseBruteOptimizationWindowCanExecute()));
            }
        }

        #endregion

        #region Methods triggered on Commands

        private bool RunBruteOptimizationCanExecute()
        {
            var items = _bruteForceParameters.GetConditionalParameters();
            return (items.Count() > 0);
        }

        private void RunBruteOptimizationExecute()
        {
            ExecuteBruteForceOptimization();
        }

        private bool ExportBruteOptimizationCanExecute()
        {
            return (BruteForceParameters.Status == OptimizationStatus.Completed);
        }

        private void ExportBruteOptimizationExecute()
        {
            ExportBruteForceResults();
        }

        private bool CloseBruteOptimizationWindowCanExecute()
        {
            return true;
        }

        private void CloseBruteOptimizationWindowExecute()
        {
            _managerBruteForce.Dispose();
        }

        #endregion

        /// <summary>
        /// Displays Strategy parameters on the UI
        /// </summary>
        private void DisplayParameterDetails()
        {
            foreach (KeyValuePair<string, ParameterDetail> keyValuePair in _selectedStrategy.ParameterDetails)
            {
                var bruteForceParameterDetail = new BruteForceParameterDetail(keyValuePair.Key,
                    keyValuePair.Value.ParameterType, keyValuePair.Value.ParameterValue, keyValuePair.Value.ParameterValue, default(double));

                // Add information
                BruteForceParameters.ParameterDetails.Add(bruteForceParameterDetail);
            }
        }

        /// <summary>
        /// Starts the Optimization process
        /// </summary>
        private void ExecuteBruteForceOptimization()
        {
            EventSystem.Publish<BruteForceParameters>(BruteForceParameters);
        }

        /// <summary>
        /// Displays results obtained from Brute Force optimization
        /// </summary>
        /// <param name="optimizationStatistics">Inidividual Brute Force iteration results</param>
        private void DisplayOptimizationStatistics(OptimizationStatistics optimizationStatistics)
        {
            _currentDispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => OptimizationStatisticsCollection.Add(optimizationStatistics)));
        }

        /// <summary>
        /// Dump the results to file
        /// </summary>
        private void ExportBruteForceResults()
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
                    string header = "Property Info,Bought,Sold,Avg Buy Price,Avg Sell Price,Profit";

                    // Add Header Row
                    lines.Add(header);

                    StringBuilder row = new StringBuilder();

                    // Create Individual Rows
                    foreach (var optimizationStatistics in _optimizationStatisticsCollection)
                    {
                        // Clear any existing values
                        row.Clear();

                        row.Append(optimizationStatistics.Description);
                        row.Append(",");
                        row.Append(optimizationStatistics.ExecutionDetails.BuyCount);
                        row.Append(",");
                        row.Append(optimizationStatistics.ExecutionDetails.SellCount);
                        row.Append(",");
                        row.Append(optimizationStatistics.ExecutionDetails.AvgBuyPrice);
                        row.Append(",");
                        row.Append(optimizationStatistics.ExecutionDetails.AvgSellPrice);
                        row.Append(",");
                        row.Append(optimizationStatistics.ExecutionDetails.Profit);

                        // Add Row
                        lines.Add(row.ToString());
                    }

                    // Create file path
                    string path = folderPath + "\\" + "BruteForce-Results.csv";

                    // Write data
                    File.WriteAllLines(path, lines);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "ExportBruteForceResults");
            }
        }
    }
}
