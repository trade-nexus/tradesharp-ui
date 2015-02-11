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

namespace TradeHubGui.ViewModel
{
    public class BruteOptimizationViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// Holds reference to UI dispatcher
        /// </summary>
        private readonly Dispatcher _currentDispatcher;

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
        }
        #endregion

        #region Properties

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

        #region Methods

        private bool RunBruteOptimizationCanExecute()
        {
            // TODO: make some condition here if necessary

            return false;
        }

        private void RunBruteOptimizationExecute()
        {
            throw new NotImplementedException();
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
    }
}
