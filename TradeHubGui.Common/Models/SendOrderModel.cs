using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.Models
{
    public class SendOrderModel : INotifyPropertyChanged
    {
        private decimal _buyPrice;
        private decimal _sellPrice;
        private decimal _triggerPrice;

        private int _size;

        private OrderType _type;

        private Security _security;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SendOrderModel()
        {
            _buyPrice = default(decimal);
            _sellPrice = default(decimal);
            _triggerPrice = default(decimal);

            _size = default(int);

            _type = OrderType.Market;
        }

        #region Properties

        /// <summary>
        /// Order Buy Price
        /// </summary>
        public decimal BuyPrice
        {
            get { return _buyPrice; }
            set
            {
                _buyPrice = value; 
                OnPropertyChanged("BuyPrice");
            }
        }

        /// <summary>
        /// Order Sell Price
        /// </summary>
        public decimal SellPrice
        {
            get { return _sellPrice; }
            set
            {
                _sellPrice = value;
                OnPropertyChanged("SellPrice");
            }
        }

        /// <summary>
        /// Order Trigger Price
        /// </summary>
        public decimal TriggerPrice
        {
            get { return _triggerPrice; }
            set
            {
                _triggerPrice = value;
                OnPropertyChanged("TriggerPrice");
            }
        }

        /// <summary>
        /// Type of Order
        /// </summary>
        public OrderType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        /// <summary>
        /// Order Quantity
        /// </summary>
        public int Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged("Size");
            }
        }

        /// <summary>
        /// Contains symbol information
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
