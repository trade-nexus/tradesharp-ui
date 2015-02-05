using System;
using System.ComponentModel;

namespace TradeHubGui.Common.Models
{
    public class OrderDetails : INotifyPropertyChanged
    {
        private string _id;
        private string _side;
        private string _type;
        private decimal _price;
        private int _quantity;
        private DateTime _time;
        private string _status;

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

        public string Type
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
