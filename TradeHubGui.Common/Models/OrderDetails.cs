using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.DomainModels.OrderDomain;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.Models
{
    public class OrderDetails : INotifyPropertyChanged
    {
        private string _id;
        private Security _security;
        private string _side;
        private OrderType _type;
        private decimal _price;
        private decimal _stopPrice;
        private int _quantity;
        private DateTime _time;
        private string _status;
        private string _provider;

        /// <summary>
        /// Holds all related fills for the order
        /// </summary>
        private ObservableCollection<FillDetail> _fillDetails;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrderDetails()
        {
            _status = OrderStatus.OPEN;
            _fillDetails = new ObservableCollection<FillDetail>();
        }

        #region Properties

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string Side
        {
            get { return _side; }
            set
            {
                _side = value;
                OnPropertyChanged("Side");
            }
        }

        public OrderType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        /// <summary>
        /// Order Status represented as const static strings from TradeHub.Common.Core.Constants.OrderStatus class
        /// possible values are: CANCELLED, EXECUTED, OPEN, PARTIALLY_EXECUTED, REJECTED, SUBMITTED
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Stop price to be used with Stop/Stop Limit orders
        /// </summary>
        public decimal StopPrice
        {
            get { return _stopPrice; }
            set { _stopPrice = value; }
        }

        /// <summary>
        /// Holds all related fills for the order
        /// </summary>
        public ObservableCollection<FillDetail> FillDetails
        {
            get { return _fillDetails; }
            set { _fillDetails = value; }
        }

        /// <summary>
        /// Order execution provider name
        /// </summary>
        public string Provider
        {
            get { return _provider; }
            set
            {
                _provider = value; 
                OnPropertyChanged("Provider");
            }
        }

        /// <summary>
        /// Contains Symbol information
        /// </summary>
        public Security Security
        {
            get { return _security; }
            set
            {
                _security = value;
                OnPropertyChanged("Security");
            }
        }

        #endregion

        #region INotifyPropertyChanged members
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
