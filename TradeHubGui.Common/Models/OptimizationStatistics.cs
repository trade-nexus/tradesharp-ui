using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Saves Strategy Statistics calculated using executions
    /// </summary>
    public class OptimizationStatistics : INotifyPropertyChanged
    {
        /// <summary>
        /// Strategy ID for which the stats are calculated
        /// </summary>
        private string _strategyId;

        /// <summary>
        /// Parameter Description
        /// </summary>
        private string _description;

        /// <summary>
        /// Contains executions information occured during optimization cycle
        /// </summary>
        private ExecutionDetails _executionDetails;

        #region Properties

        /// <summary>
        /// Strategy ID for which the stats are calculated
        /// </summary>
        public string StrategyId
        {
            get { return _strategyId; }
            set
            {
                _strategyId = value;
                OnPropertyChanged("StrategyId");
            }
        }
        
        /// <summary>
        /// Parameter Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Contains executions information occured during optimization cycle
        /// </summary>
        public ExecutionDetails ExecutionDetails
        {
            get { return _executionDetails; }
            set
            {
                _executionDetails = value;
                OnPropertyChanged("ExecutionDetails");
            }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="strategyId">Strategy id for which to calculate the statistics</param>
        public OptimizationStatistics(string strategyId)
        {
            _strategyId = strategyId;
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}