using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains information about individual item in the limit order book collection
    /// </summary>
    public class LimitOrderBookRecord : INotifyPropertyChanged, IComparable<LimitOrderBookRecord>
    {
        private int _depth;
        
        private decimal _bidPrice;
        private decimal _askPrice;

        private decimal _bidQuantity;
        private decimal _askQuantity;

        private LobRecordType _recordType;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="recordType">Type of record</param>
        public LimitOrderBookRecord(LobRecordType recordType)
        {
            _depth = default(int);
            _bidPrice = -1;
            _askPrice = -1;
            _bidQuantity = -1;
            _askQuantity = -1;

            _recordType = recordType;
        }

        #region Properties

        /// <summary>
        /// Market Depth for the current record
        /// </summary>
        public int Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                OnPropertyChanged("Depth");
            }
        }

        /// <summary>
        /// Bid price on the current level
        /// </summary>
        public decimal BidPrice
        {
            get { return _bidPrice; }
            set
            {
                _bidPrice = value;
                OnPropertyChanged("BidPrice");
            }
        }

        /// <summary>
        /// Number of orders on the current bid level
        /// </summary>
        public decimal BidQuantity
        {
            get { return _bidQuantity; }
            set
            {
                _bidQuantity = value;
                OnPropertyChanged("BidQuantity");
            }
        }

        /// <summary>
        /// Ask price on current level
        /// </summary>
        public decimal AskPrice
        {
            get { return _askPrice; }
            set
            {
                _askPrice = value;
                OnPropertyChanged("AskPrice");
            }
        }

        /// <summary>
        /// Number of orders on the current ask level
        /// </summary>
        public decimal AskQuantity
        {
            get { return _askQuantity; }
            set
            {
                _askQuantity = value;
                OnPropertyChanged("AskQuantity");
            }
        }

        /// <summary>
        /// Type of Record in the Limit Order Book
        /// </summary>
        public LobRecordType RecordType
        {
            get { return _recordType; }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public int CompareTo(LimitOrderBookRecord record)
        {
            //LimitOrderBookRecord record = (LimitOrderBookRecord)obj;

            if (this.RecordType == LobRecordType.Ask)
            {
                // Sort based on AskPrice 
                if (this.AskPrice < record.AskPrice)
                {
                    return 1;
                }
                else if (record.AskPrice < this.AskPrice)
                    return -1;
                else
                    return 0;
            }
            else if (this.RecordType == LobRecordType.Bid)
            {
                // Sort based on BidPrice 
                if (this.BidPrice < record.BidPrice)
                {
                    return 1;
                }
                else if (record.BidPrice < this.BidPrice)
                    return -1;
                else
                    return 0;
            }

            return 0;
        }
    }
}
