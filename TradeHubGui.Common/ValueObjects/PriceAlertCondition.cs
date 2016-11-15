using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.ValueObjects
{
    /// <summary>
    /// Contains conditions which need to be met to generate price alerts
    /// </summary>
    public class PriceAlertCondition
    {
        /// <summary>
        /// Contains condition operator which needs to be applied in calculation
        /// </summary>
        private ConditionOperator _conditionOperator;

        /// <summary>
        /// Contains the price value at which the alert should be triggered
        /// </summary>
        private decimal _conditionPrice;

        /// <summary>
        /// Contains condition operator which needs to be applied in calculation
        /// </summary>
        public ConditionOperator ConditionOperator
        {
            get { return _conditionOperator; }
        }

        /// <summary>
        /// Contains the price value at which the alert should be triggered
        /// </summary>
        public decimal ConditionPrice
        {
            get { return _conditionPrice; }
        }

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="conditionOperator">Contains condition operator which needs to be applied in calculation</param>
        /// <param name="conditionPrice">Contains the price value at which the alert should be triggered</param>
        public PriceAlertCondition(ConditionOperator conditionOperator, decimal conditionPrice)
        {
            _conditionOperator = conditionOperator;
            _conditionPrice = conditionPrice;
        }

        /// <summary>
        /// Evaluates the specified condition
        /// </summary>
        /// <param name="currentValue">Current value of the property for which the condition is specified</param>
        /// <returns></returns>
        public bool Evaluate(decimal currentValue)
        {
            return ApplyCondition(currentValue);
        }

        /// <summary>
        /// Translates the specified condition and applies to the current value
        /// </summary>
        /// <param name="currentValue">Current value of the property for which the condition is specified</param>
        /// <returns></returns>
        private bool ApplyCondition(decimal currentValue)
        {
            switch (_conditionOperator)
            {
                case ConditionOperator.Equals:
                    if (currentValue.Equals(_conditionPrice))
                        return true;
                    return false;
                case ConditionOperator.Greater:
                    if (currentValue >_conditionPrice)
                        return true;
                    return false;
                case ConditionOperator.Less:
                    if (currentValue < _conditionPrice)
                        return true;
                    return false;
                default:
                    return false;
            }
        }
    }
}
