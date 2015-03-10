using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// VALUE = <see cref="TickDetail"/>
        /// </summary>
        private Dictionary<string, TickDetail> _tickDetailsMap;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MarketDataProvider()
        {
            // Initialize Map
            _tickDetailsMap = new Dictionary<string, TickDetail>();
        }


        /// <summary>
        /// Contains market information for each subscribed symbol
        /// KEY = Symbol
        /// VALUE = <see cref="TickDetail"/>
        /// </summary>
        public Dictionary<string, TickDetail> TickDetailsMap
        {
            get { return _tickDetailsMap; }
            set { _tickDetailsMap = value; }
        }

    }
}
