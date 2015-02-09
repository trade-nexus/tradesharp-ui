using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.ValueObjects
{
    /// <summary>
    /// Contains Parameter details to be used for Genetic Algorithm Optimization
    /// </summary>
    public class OptimizationParameterDetail : INotifyPropertyChanged
    {
        /// <summary>
        /// Index of the parameter in discussion
        /// </summary>
        private int _index;

        /// <summary>
        /// Description of the Parameter
        /// </summary>
        private string _description;

        /// <summary>
        /// Start point of parameter range
        /// </summary>
        private double _startValue;

        /// <summary>
        /// End point of parameter range
        /// </summary>
        private double _endValue;

        /// <summary>
        /// System Type of the parameter e.g. Int32, Double, etc
        /// </summary>
        private Type _parameterType;

        /// <summary>
        /// Optimized Parameter Value after Optimization Process
        /// </summary>
        private double _optimizedValue;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OptimizationParameterDetail()
        {
            _index = default(int);
            _description = string.Empty;
            _startValue = default(double);
            _endValue = default(double);
            _optimizedValue = default(double);
            _parameterType = null;
        }

        /// <summary>
        /// Index of the parameter in discussion
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
            }
        }

        /// <summary>
        /// Description of the Parameter
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
        /// Start point of parameter range
        /// </summary>
        public double StartValue
        {
            get { return _startValue; }
            set
            {
                _startValue = value;
                OnPropertyChanged("StartValue");
            }
        }

        /// <summary>
        /// End point of parameter range
        /// </summary>
        public double EndValue
        {
            get { return _endValue; }
            set
            {
                _endValue = value;
                OnPropertyChanged("EndValue");
            }
        }

        /// <summary>
        /// System Type of the parameter e.g. Int32, Double, etc
        /// </summary>
        public Type ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        /// <summary>
        /// Optimized Parameter Value after Optimization Process
        /// </summary>
        public double OptimizedValue
        {
            get { return _optimizedValue; }
            set
            {
                _optimizedValue = value;
                OnPropertyChanged("OptimizedValue");
            }
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
