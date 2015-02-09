using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels.OrderDomain;
using OrderStatus = TradeHub.Common.Core.Constants.OrderStatus;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecutionDetails : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// List of all the Order detials during the current session
        /// </summary>
        private ObservableCollection<OrderDetails> _orderDetailsList;

        public string _key;
        public int _executed;
        public int _buyCount;
        public int _sellCount;

        #endregion

        #region Properties

        public string Key
        {
            get { return _key; }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    OnPropertyChanged("Key");
                }
            }
        }

        public int Executed
        {
            get { return _executed; }
            set
            {
                if (_executed != value)
                {
                    _executed = value;
                    OnPropertyChanged("Executed");
                }
            }
        }

        public int BuyCount
        {
            get { return _buyCount; }
            set
            {
                if (_buyCount != value)
                {
                    _buyCount = value;
                    OnPropertyChanged("BuyCount");
                }
            }
        }

        public int SellCount
        {
            get { return _sellCount; }
            set
            {
                if (_sellCount != value)
                {
                    _sellCount = value;
                    OnPropertyChanged("SellCount");
                }
            }
        }

        /// <summary>
        /// List of all the Order detials during the current session
        /// </summary>
        public ObservableCollection<OrderDetails> OrderDetailsList
        {
            get { return _orderDetailsList; }
            set
            {
                _orderDetailsList = value;
                OnPropertyChanged("OrderDetailsList");
            }
        }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExecutionDetails()
        {
            // Initialize Objects
            OrderDetailsList = new ObservableCollection<OrderDetails>();
        }

        /// <summary>
        /// Adds new order details to the local map
        /// </summary>
        /// <param name="orderDetails"></param>
        public void AddOrderDetails(OrderDetails orderDetails)
        {
            // Counts are to be incremented if incoming order detail is for execution
            if (orderDetails.Status.Equals(OrderStatus.EXECUTED) || orderDetails.Status.Equals(OrderStatus.PARTIALLY_EXECUTED))
            {
                Executed++;

                if (orderDetails.Side.Equals(OrderSide.COVER))
                {
                    if (BuyCount < SellCount)
                    {
                        BuyCount += orderDetails.Quantity;
                    }
                    else
                    {
                        SellCount += orderDetails.Quantity;
                    }
                }
                else if (orderDetails.Side.Equals(OrderSide.BUY))
                {
                    BuyCount += orderDetails.Quantity;
                }
                else
                {
                    SellCount += orderDetails.Quantity;
                }
            }

            // Add to local Map
            _orderDetailsList.Add(orderDetails);
        }

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
