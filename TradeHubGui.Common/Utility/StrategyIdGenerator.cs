using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Utility
{
    /// <summary>
    /// Creates Unique IDs for Strategy and Strategy Instances for a given session
    /// </summary>
    public static class StrategyIdGenerator
    {
        private const int MinStrategyValue = 0xAA;
        private const int MaxStrategyValue = 0xFF;
        private static int _strategyIdValue = MinStrategyValue - 1;

        private const int MinInstanceValue = 0xA000;
        private const int MaxInstanceValue = 0xF999;

        /// <summary>
        /// Contains mapping for Strategy ID to Instance IDs
        /// KEY = Strategy ID
        /// Value = Instance ID
        /// </summary>
        private static Dictionary<string, int> _strategyIdMap = new Dictionary<string, int>();

        /// <summary>
        /// Returns New Strategy ID
        /// </summary>
        /// <returns></returns>
        public static string GetStrategyKey()
        {
            // Create new Strategy ID
            if (_strategyIdValue < MaxStrategyValue)
            {
                _strategyIdValue++;
            }
            else
            {
                _strategyIdValue = MinStrategyValue;
            }

            // Convert to String representation
            string idGenerated = _strategyIdValue.ToString("X");

            // Add New Id to local Map
            _strategyIdMap.Add(idGenerated, MinInstanceValue);

            // Return new ID generated
            return idGenerated;
        }

        /// <summary>
        /// Returns New Strategy Instance ID for the given strategy
        /// </summary>
        /// <param name="strategyKey"></param>
        /// <returns></returns>
        public static string GetInstanceKey(string strategyKey)
        {
            int currentValue;

            // Get current value in use for the given strategy
            if (_strategyIdMap.TryGetValue(strategyKey, out currentValue))
            {
                // Create new Strategy Instance ID
                if (currentValue < MaxInstanceValue)
                {
                    currentValue++;
                }
                else
                {
                    currentValue = MinInstanceValue;
                }

                // Update Value in local Map
                _strategyIdMap[strategyKey] = currentValue;

                // Return new ID generated with the combination of Parent Strategy ID
                return strategyKey + "-" + currentValue.ToString("X");
            }

            // Empty String as ID should stop the further processes for Strategy Instance
            return string.Empty;
        }
    }
}
