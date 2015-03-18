using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common.Constants;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;

namespace TradeHubGui.Common.ValueObjects
{
    /// <summary>
    /// Contains Market Data subscription details
    /// </summary>
    public class SubscriptionRequest
    {
        /// <summary>
        /// Subscription category e.g. Subscribe, Un-Subscribe
        /// </summary>
        private SubscriptionType _subscriptionType;

        /// <summary>
        /// Type of Market data to subscribe
        /// </summary>
        private MarketDataType _marketDataType;

        /// <summary>
        /// Contains Symbol information
        /// </summary>
        private Security _security;

        /// <summary>
        /// Market Data Provider details
        /// </summary>
        private MarketDataProvider _provider;

        /// <summary>
        /// Contains Bar details to be used for subscription
        /// </summary>
        private BarParameters _liveBarDetail;

        /// <summary>
        /// Contains details to be used for historical bar data subscription
        /// </summary>
        private HistoricalBarParameters _historicalBarDetail;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="security">Contains Symbol information</param>
        /// <param name="provider">Market Data Provider details</param>
        /// <param name="marketDataType">Type of Market Data to subscribe</param>
        /// <param name="subscriptionType">Subscription category e.g. Subscribe, Un-Subscribe</param>
        public SubscriptionRequest(Security security, MarketDataProvider provider, MarketDataType marketDataType, SubscriptionType subscriptionType)
        {
            _security = security;
            _provider = provider;
            _marketDataType = marketDataType;
            _subscriptionType = subscriptionType;
        }

        #region Properties

        /// <summary>
        /// Subscription category e.g. Subscribe, Un-Subscribe
        /// </summary>
        public SubscriptionType SubscriptionType
        {
            get { return _subscriptionType; }
            set { _subscriptionType = value; }
        }

        /// <summary>
        /// Contains Symbol information
        /// </summary>
        public Security Security
        {
            get { return _security; }
            set { _security = value; }
        }

        /// <summary>
        /// Market Data Provider details
        /// </summary>
        public MarketDataProvider Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        /// <summary>
        /// Type of Market data to subscribe
        /// </summary>
        public MarketDataType MarketDataType
        {
            get { return _marketDataType; }
            set { _marketDataType = value; }
        }

        /// <summary>
        /// Contains Bar details to be used for subscription
        /// </summary>
        public BarParameters LiveBarDetail
        {
            get { return _liveBarDetail; }
        }

        /// <summary>
        /// Contains details to be used for historical bar data subscription
        /// </summary>
        public HistoricalBarParameters HistoricalBarDetail
        {
            get { return _historicalBarDetail; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets information for Live Bar subscription
        /// </summary>
        /// <param name="barLength">Bar length i.e. duration</param>
        /// <param name="pipSize">Pip size</param>
        /// <param name="barFormat">Bar format</param>
        /// <param name="barPriceType">bar price type</param>
        public void SetLiveBarDetails(decimal barLength, decimal pipSize, string barFormat, string barPriceType)
        {
            // Initialize object
            _liveBarDetail = new BarParameters();

            // Set parameters
            _liveBarDetail.BarLength = barLength;
            _liveBarDetail.PipSize = pipSize;
            _liveBarDetail.Format = barFormat;
            _liveBarDetail.PriceType = barPriceType;
        }

        /// <summary>
        /// Sets information for Historical bar data subscription
        /// </summary>
        public void SetHistoricalBarDetails(string barType, DateTime startDate, DateTime endDate)
        {
            // Initialize object
            _historicalBarDetail = new HistoricalBarParameters();

            // Set parameters
            _historicalBarDetail.Type = barType;
            _historicalBarDetail.StartDate = startDate;
            _historicalBarDetail.EndDate = endDate;
        }

        #endregion
    }
}