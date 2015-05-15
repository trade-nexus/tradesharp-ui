using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class StrategyInstanceSummaryViewModel : BaseViewModel
    {
        private StrategyInstance _strategyInstance;
        private RelayCommand _clearDataCommand;

        #region Properties

        /// <summary>
        /// Conatins instance details
        /// </summary>
        public StrategyInstance StrategyInstance
        {
            get { return _strategyInstance; }
            set
            {
                _strategyInstance = value;
                OnPropertyChanged("StrategyInstance");
            }
        }

        #endregion


        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="strategyInstance">Contains instance details</param>
        public StrategyInstanceSummaryViewModel(StrategyInstance strategyInstance)
        {
            _strategyInstance = strategyInstance;
        }

        #region Commands

        /// <summary>
        /// Used for Clear Button
        /// </summary>
        public ICommand ClearDataCommand
        {
            get
            {
                return _clearDataCommand ?? (_clearDataCommand = new RelayCommand(param => ClearDataExecute()));
            }
        }

        #endregion

        #region Command Trigger Methods

        /// <summary>
        /// Called when clear data button is clicked
        /// </summary>
        private void ClearDataExecute()
        {
            StrategyInstance.InstanceSummary.Clear();
        }

        #endregion
    }
}
