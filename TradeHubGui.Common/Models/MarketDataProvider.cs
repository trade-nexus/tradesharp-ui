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
        private Dictionary<string, MarketDataDetail> _marketDetailsMap;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MarketDataProvider()
        {
            // Initialize Map
            _marketDetailsMap = new Dictionary<string, MarketDataDetail>();
        }


        /// <summary>
        /// Contains market information for each subscribed symbol
        /// KEY = Symbol
        /// VALUE = <see cref="MarketDataDetail"/>
        /// </summary>
        public Dictionary<string, MarketDataDetail> TickDetailsMap
        {
            get { return _marketDetailsMap; }
            set { _marketDetailsMap = value; }
        }

        /// <summary>
        /// Updates tick information for the given Symbol
        /// </summary>
        /// <param name="symbol">Symbol Name</param>
        /// <param name="tick">Contains market data information</param>
        public void UpdateMarketDetail(string symbol, Tick tick)
        {
            MarketDataDetail tickDetails;

            // Get TickDetails object to update tick information
            if (_marketDetailsMap.TryGetValue(tick.Security.Symbol, out tickDetails))
            {
                // Update collections for Depth information
                tickDetails.Update(tick);
            }
        }

        /// <summary>
        /// Removes tick information for the given Symbol from local maps
        /// </summary>
        /// <param name="symbol">Symbol Name</param>
        public void RemoveMarketInformation(string symbol)
        {
            MarketDataDetail marketDataDetail;

            // Get MarketDataDetail object which is to be removed
            if (_marketDetailsMap.TryGetValue(symbol, out marketDataDetail))
            {
                marketDataDetail.AskRecordsCollection.Clear();
                marketDataDetail.BidRecordsCollection.Clear();

                // remove from local map
                _marketDetailsMap.Remove(symbol);
            }
        }
    }
}
