using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;

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

        private Strategy _selectedStrategy;

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
        }
        #endregion

        #region Commands
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

        #endregion

        #region Private methods
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

            return false;
        }

        private void RunGeneticOptimizationExecute()
        {
            //TODO: make implementation
            throw new NotImplementedException();
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
    }
}
