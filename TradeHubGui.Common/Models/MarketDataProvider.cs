using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        #region Fields

        /// <summary>
        /// Contains subscribed symbol's tick information (Valid if the provider is type 'Market Data')
        /// KEY = Symbol
        /// VALUE = <see cref="MarketDataDetail"/>
        /// </summary>
        private Dictionary<string, MarketDataDetail> _marketDetailsMap;

        /// <summary>
        /// Contains all Market Detail objects
        /// </summary>
        private ObservableCollection<MarketDataDetail> _marketDetailCollection;

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MarketDataProvider()
        {
            // Initialize Map
            _marketDetailsMap = new Dictionary<string, MarketDataDetail>();
            _marketDetailCollection = new ObservableCollection<MarketDataDetail>();
        }

        #region Properties

        /// <summary>
        /// Contains all Market Detail objects
        /// </summary>
        public ObservableCollection<MarketDataDetail> MarketDetailCollection
        {
            get { return _marketDetailCollection; }
            set
            {
                _marketDetailCollection = value;
                OnPropertyChanged("MarketDetailCollection");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds Market Detail object to Map/Collection
        /// </summary>
        /// <param name="marketDataDetail">Holds market data information</param>
        public void AddMarketDetail(MarketDataDetail marketDataDetail)
        {
            // Check if the object already exists for the given symbol
            if (!_marketDetailsMap.ContainsKey(marketDataDetail.Security.Symbol))
            {
                // Add object to MAP
                _marketDetailsMap.Add(marketDataDetail.Security.Symbol, marketDataDetail);

                // Add object to collection
                MarketDetailCollection.Add(marketDataDetail);
            }
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
                // Clear depth information
                marketDataDetail.AskRecordsCollection.Clear();
                marketDataDetail.BidRecordsCollection.Clear();

                // remove from local map
                _marketDetailsMap.Remove(symbol);

                // Remove from collection
                MarketDetailCollection.Remove(marketDataDetail);
            }
        }

        /// <summary>
        /// Checks if the given symbol is already loading into application from UI
        /// </summary>
        /// <param name="symbol">Symbol name</param>
        public bool IsSymbolLoaded(string symbol)
        {
            return _marketDetailsMap.ContainsKey(symbol);
        }

        #endregion
    }
}
