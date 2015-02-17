using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains necessary Tick information to be displayed on the UI
    /// </summary>
    public class TickDetails : INotifyPropertyChanged
    {
        /// <summary>
        /// Contains Tick's Symbol information
        /// </summary>
        private Security _security;

        /// <summary>
        /// Best available Bid price
        /// </summary>
        private decimal _bidPrice;

        /// <summary>
        /// Best avaiable Ask Price
        /// </summary>
        private decimal _askPrice;

        /// <summary>
        /// Price for Last Traded 
        /// </summary>
        private decimal _lastPrice;

        /// <summary>
        /// Quantity corresponding to Best Bid
        /// </summary>
        private decimal _bidQuantity;

        /// <summary>
        /// Quantity corresponding to Best Ask
        /// </summary>
        private decimal _askQuantity;

        /// <summary>
        /// Last Traded quantity
        /// </summary>
        private decimal _lastQuantity;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="security">Contains symbol information</param>
        public TickDetails(Security security)
        {
            // Save reference
            _security = security;
        }

        #region Properties

        /// <summary>
        /// Contains Tick's Symbol information
        /// </summary>
        public Security Security
        {
            get { return _security; }
            set { _security = value; }
        }

        /// <summary>
        /// Best available Bid price
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
        /// Best avaiable Ask Price
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
        /// Price for Last Traded 
        /// </summary>
        public decimal LastPrice
        {
            get { return _lastPrice; }
            set
            {
                _lastPrice = value;
                OnPropertyChanged("LastPrice");
            }
        }

        /// <summary>
        /// Quantity corresponding to Best Bid
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
        /// Quantity corresponding to Best Ask
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
        /// Last Traded quantity
        /// </summary>
        public decimal LastQuantity
        {
            get { return _lastQuantity; }
            set
            {
                _lastQuantity = value;
                OnPropertyChanged("LastQuantity");
            }
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
