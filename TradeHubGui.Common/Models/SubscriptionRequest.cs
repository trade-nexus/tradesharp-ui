using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.Models
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
        /// Contains Symbol information
        /// </summary>
        private Security _security;

        /// <summary>
        /// Market Data Provider details
        /// </summary>
        private MarketDataProvider _provider;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="security">Contains Symbol information</param>
        /// <param name="provider">Market Data Provider details</param>
        /// <param name="subscriptionType">Subscription category e.g. Subscribe, Un-Subscribe</param>
        public SubscriptionRequest(Security security, MarketDataProvider provider, SubscriptionType subscriptionType)
        {
            _security = security;
            _provider = provider;
            _subscriptionType = subscriptionType;
        }

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
    }
}
