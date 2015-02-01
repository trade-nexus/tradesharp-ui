using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Models
{
    public class Instrument
    {
        /// <summary>
        /// Instrument Constructor with Arguments
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="bidQty">Bid Quantity</param>
        /// <param name="bidPrice">Bid Price</param>
        /// <param name="askQty">Ask Quantity</param>
        /// <param name="askPrice">Ask Price</param>
        /// <param name="last">Last</param>
        /// <param name="volume">Volume</param>
        public Instrument(string symbol, float bidQty, float bidPrice, float askQty, float askPrice, float last, float volume)
        {
            Symbol = symbol;
            BidQty = bidQty;
            BidPrice = bidPrice;
            AskQty = askQty;
            AskPrice = askPrice;
            Last = last;
            Volume = volume;
        }

        #region Properties
        public string Symbol { get; set; }
        public float BidQty { get; set; }
        public float BidPrice { get; set; }
        public float AskQty { get; set; }
        public float AskPrice { get; set; }
        public float Last { get; set; }
        public float Volume { get; set; }
        #endregion
    }
}
