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
    public class LimitOrderBookRecord : INotifyPropertyChanged
    {
        private int _depth;
        private decimal _quantity;
        private decimal _price;
        private LobRecordType _recordType;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="recordType">Type of record</param>
        public LimitOrderBookRecord(LobRecordType recordType)
        {
            _depth = default(int);
            _quantity = default(decimal);
            _price = default(decimal);

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
        /// Number of orders on the current level
        /// </summary>
        public decimal Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        /// <summary>
        /// Price on the current level
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
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
    }
}
