using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.ValueObjects
{
    /// <summary>
    /// Contains Parameter Detail information
    /// </summary>
    public class ParameterDetail : ICloneable
    {
        /// <summary>
        /// Type of the parameter i.e. Int32, Decimal, String
        /// </summary>
        private Type _parameterType;

        /// <summary>
        /// Value for the given parameter value
        /// </summary>
        private object _parameterValue;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="parameterType">Type of the parameter i.e. Int32, Decimal, String</param>
        /// <param name="parameterValue">Value for the given parameter value</param>
        public ParameterDetail(Type parameterType, object parameterValue)
        {
            _parameterType = parameterType;
            _parameterValue = parameterValue;
        }

        /// <summary>
        /// Type of the parameter i.e. Int32, Decimal, String
        /// </summary>
        public Type ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        /// <summary>
        /// Value for the given parameter value
        /// </summary>
        public object ParameterValue
        {
            get { return _parameterValue; }
            set { _parameterValue = value; }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
