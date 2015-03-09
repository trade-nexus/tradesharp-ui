using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Models
{
    public class LobRecord
    {
        #region Fields

        private int _bidSize;
        private decimal _bidPrice;
        private decimal _askPrice;
        private int _askSize;

        #endregion

        #region Constructors

        public LobRecord()
        {

        }

        #endregion

        #region Properties

        public int BidSize
        {
            get { return _bidSize; }
            set { _bidSize = value; }
        }

        public decimal BidPrice
        {
            get { return _bidPrice; }
            set { _bidPrice = value; }
        }

        public decimal AskPrice
        {
            get { return _askPrice; }
            set { _askPrice = value; }
        }

        public int AskSize
        {
            get { return _askSize; }
            set { _askSize = value; }
        }

        #endregion
    }
}
