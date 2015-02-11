using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Managers;

namespace TradeHubGui.ViewModel
{
    public class BruteOptimizationViewModel : BaseViewModel
    {
        #region Fields

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

            DisplayParameterDetails();
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
            // TODO: make some condition here if necessary

            return true;
        }

        private void RunBruteOptimizationExecute()
        {
            ExecuteBruteForceOptimization();
        }

        private bool ExportBruteOptimizationCanExecute()
        {
            // TODO: make some condition here if necessary

            return false;
        }

        private void ExportBruteOptimizationExecute()
        {
            throw new NotImplementedException();
        }

        private bool CloseBruteOptimizationWindowCanExecute()
        {
            // TODO: make some condition here if necessary

            return false;
        }

        private void CloseBruteOptimizationWindowExecute()
        {
            throw new NotImplementedException();
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
    }
}
