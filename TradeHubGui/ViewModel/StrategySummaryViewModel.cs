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
    public class StrategySummaryViewModel : BaseViewModel
    {
        private Strategy _strategy;
        private RelayCommand _clearDataCommand;

        #region Properties

        /// <summary>
        /// Conatins instance details
        /// </summary>
        public Strategy Strategy
        {
            get { return _strategy; }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="strategy">Contains instance details</param>
        public StrategySummaryViewModel(Strategy strategy)
        {
            _strategy = strategy;
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
            Strategy.ClearStatistics();
        }

        #endregion
    }
}
