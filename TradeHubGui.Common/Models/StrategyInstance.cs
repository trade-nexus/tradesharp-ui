using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common.ValueObjects;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains Individual Strategy Instance information
    /// </summary>
    public class StrategyInstance : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Unique Key to identify instance
        /// </summary>
        private string _instanceKey;

        /// <summary>
        /// Symbol of instrument
        /// </summary>
        private string _symbol;

        /// <summary>
        /// Brief Strategy Description
        /// </summary>
        private string _description;

        /// <summary>
        /// Contains Parameter details to be used by Strategy
        /// Key = Parameter Name
        /// Value = Parameter Type (e.g. Int32, String, Decimal, etc.) , Parameter Value if entered
        /// </summary>
        private Dictionary<string, ParameterDetail> _parameters; 

        /// <summary>
        /// Strategy Type containing TradeHubStrategy
        /// </summary>
        private Type _strategyType;

        /// <summary>
        /// Current Execution Status of Stratgey Instance i.e. 'None' | 'Executing' | 'Executed'
        /// </summary>
        private StrategyStatus _status = StrategyStatus.None;

        /// <summary>
        /// Holds basic execution information for the current instance to be used for UI
        /// </summary>
        private ExecutionDetails _executionDetails;

        #endregion

        #region Properties

        /// <summary>
        /// Unique Key to identify instance
        /// </summary>
        public string InstanceKey
        {
            get { return _instanceKey; }
            set
            {
                if (_instanceKey != value)
                {
                    _instanceKey = value;
                    OnPropertyChanged("InstanceKey");
                }
            }
        }

        /// <summary>
        /// Symbol of instrument
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged("Symbol");
                }
            }
        }

        /// <summary>
        /// Brief Strategy Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Contains Parameter details to be used by Strategy
        /// Key = Parameter Name
        /// Value = Parameter Type (e.g. Int32, String, Decimal, etc.) , Parameter Value if entered
        /// </summary>
        public Dictionary<string, ParameterDetail> Parameters
        {
            get { return _parameters; }
            set
            {
                if (_parameters != value)
                {
                    _parameters = value;
                    OnPropertyChanged("Parameters");
                }
            }
        }

        /// <summary>
        /// Strategy Type containing TradeHubStrategy
        /// </summary>
        public Type StrategyType
        {
            get { return _strategyType; }
            set
            {
                if (_strategyType != value)
                {
                    _strategyType = value;
                    OnPropertyChanged("StrategyType");
                }
            }
        }

        /// <summary>
        /// Current Execution Status of Stratgey Instance i.e. 'None' | 'Executing' | 'Executed'
        /// </summary>
        public StrategyStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        /// <summary>
        /// Holds basic execution information for the current instance to be used for UI
        /// </summary>
        public ExecutionDetails ExecutionDetails
        {
            get { return _executionDetails; }
            set
            {
                if (_executionDetails != value)
                {
                    _executionDetails = value;
                    OnPropertyChanged("ExecutionDetails");
                }
            }
        }

        #endregion

        public StrategyInstance()
        {
            
        }

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="instanceKey">Unique Key to identify instance</param>
        /// <param name="parameters">Contains Parameter details to be used by Strategy</param>
        /// <param name="strategyType">Strategy Type containing TradeHubStrategy</param>
        public StrategyInstance(string instanceKey, Dictionary<string, ParameterDetail> parameters, Type strategyType)
        {
            // Initialize
            _executionDetails = new ExecutionDetails();

            // Save information
            _instanceKey = instanceKey;
            _parameters = parameters;
            _strategyType = strategyType;

            // Use Instance Key to identify its execution information
            _executionDetails.Key = _instanceKey;
        }

        /// <summary>
        /// Adds new order details to the local map
        /// </summary>
        /// <param name="orderDetails"></param>
        public void AddOrderDetails(OrderDetails orderDetails)
        {
            _executionDetails.AddOrderDetails(orderDetails);
        }

        /// <summary>
        /// Returns IList of actual parameter values from the Parameter Details object
        /// </summary>
        /// <returns></returns>
        public  IList<object> GetParameterValues()
        {
            IList<object> parameterValues = new List<object>();

            // Traverse all parameter
            foreach (KeyValuePair<string, ParameterDetail> keyValuePair in Parameters)
            {
                // Add actual parameter values to the new object list
                parameterValues.Add(keyValuePair.Value.ParameterValue);
            }

            return parameterValues;
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
