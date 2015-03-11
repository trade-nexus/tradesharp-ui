using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains information specific to Market Data Provider
    /// </summary>
    public class MarketDataProvider : Provider
    {
        /// <summary>
        /// Contains subscribed symbol's tick information (Valid if the provider is type 'Market Data')
        /// KEY = Symbol
        /// VALUE = <see cref="MarketDataDetail"/>
        /// </summary>
        private Dictionary<string, MarketDataDetail> _tickDetailsMap;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MarketDataProvider()
        {
            // Initialize Map
            _tickDetailsMap = new Dictionary<string, MarketDataDetail>();
        }


        /// <summary>
        /// Contains market information for each subscribed symbol
        /// KEY = Symbol
        /// VALUE = <see cref="MarketDataDetail"/>
        /// </summary>
        public Dictionary<string, MarketDataDetail> TickDetailsMap
        {
            get { return _tickDetailsMap; }
            set { _tickDetailsMap = value; }
        }

        /// <summary>
        /// Updates tick information for the given Symbol
        /// </summary>
        /// <param name="symbol">Symbol Name</param>
        /// <param name="tick">Contains market data information</param>
        public void UpdateTickDetail(string symbol, Tick tick)
        {
            MarketDataDetail tickDetails;

            // Get TickDetails object to update tick information
            if (_tickDetailsMap.TryGetValue(tick.Security.Symbol, out tickDetails))
            {
                // Update collections for Depth information
                tickDetails.Update(tick);
            }
        }
    }
}
