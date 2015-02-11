using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.ValueObjects
{
    public class BruteForceParameterDetail : ParameterDetail
    {
        /// <summary>
        /// Parameter descirption
        /// </summary>
        private string _description;

        /// <summary>
        /// Range end point to corresponde with the parameter value
        /// </summary>
        private object _endValue;

        /// <summary>
        /// Increment to be used to move from parameter value to end value
        /// </summary>
        private double _increment;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="description">Parameter descirption</param>
        /// <param name="parameterType">Type of the parameter i.e. Int32, Decimal, String</param>
        /// <param name="parameterValue">Value for the given parameter value</param>
        /// <param name="endValue">Range end point to corresponde with the parameter value</param>
        /// <param name="increment">Increment to be used to move from parameter value to end value</param>
        public BruteForceParameterDetail(string description, Type parameterType, object parameterValue, object endValue, double increment)
            : base(parameterType, parameterValue)
        {
            _description = description;
            _endValue = endValue;
            _increment = increment;
        }

        /// <summary>
        /// Range end point to corresponde with the parameter value
        /// </summary>
        public object EndValue
        {
            get { return _endValue; }
            set { _endValue = value; }
        }

        /// <summary>
        /// Increment to be used to move from parameter value to end value
        /// </summary>
        public double Increment
        {
            get { return _increment; }
            set { _increment = value; }
        }

        /// <summary>
        /// Parameter descirption
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
