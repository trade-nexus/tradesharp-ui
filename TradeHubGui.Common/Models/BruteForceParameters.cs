using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common.ValueObjects;

namespace TradeHubGui.Common.Models
{
    public class BruteForceParameters : INotifyPropertyChanged
    {
        /// <summary>
        /// Strategy Type containing TradeHubStrategy
        /// </summary>
        private Type _strategyType;

        /// <summary>
        /// Contains detailed information to be used while running Brute Force optimization
        /// </summary>
        private ObservableCollection<BruteForceParameterDetail> _parameterDetails;

        #region Properties

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
        /// Contains detailed information to be used while running Brute Force optimization
        /// </summary>
        public ObservableCollection<BruteForceParameterDetail> ParameterDetails
        {
            get { return _parameterDetails; }
            set
            {
                _parameterDetails = value;
                OnPropertyChanged("ParameterDetails");
            }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="strategyType">Strategy Type containing TradeHubStrategy</param>
        public BruteForceParameters(Type strategyType)
        {
            _strategyType = strategyType;
            _parameterDetails = new ObservableCollection<BruteForceParameterDetail>();
        }

        /// <summary>
        /// Returns Initial Parameter values
        /// </summary>
        /// <returns></returns>
        public object[] GetParameterValues()
        {
            int entryCount = 0;

            object[] parameterValues = new object[ParameterDetails.Count];

            // Traverse all parameter
            foreach (BruteForceParameterDetail iteratorVariable in ParameterDetails)
            {
                // Makes sure all parameters are in right format
                var input = StrategyHelper.GetParametereValue(iteratorVariable.ParameterValue.ToString(), iteratorVariable.ParameterType.Name);

                // Add actual parameter values to the new object list
                parameterValues[entryCount++] = input;
            }

            return parameterValues;
        }

        /// <summary>
        /// Returns array of parameter which will be used to make different iterations
        /// </summary>
        /// <returns></returns>
        public Tuple<int, object, double>[] GetConditionalParameters()
        {
            int index = 0;

            // Create a list to hold all optimization parameters
            var optimizationParameters = new List<Tuple<int, object, double>>();

            // Read info from all parameters
            foreach (var parameterDetail in ParameterDetails)
            {
                // Check if both End Point and Increment values are added
                if (!(parameterDetail.ParameterValue.ToString().Equals(parameterDetail.EndValue.ToString())))
                {
                    if (parameterDetail.Increment > 0)
                    {
                        // Add parameter info
                        optimizationParameters.Add(new Tuple<int, object, double>(index,
                                                                                  parameterDetail.EndValue,
                                                                                  parameterDetail.Increment));
                    }
                }

                index++;
            }

            return optimizationParameters.ToArray();
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
