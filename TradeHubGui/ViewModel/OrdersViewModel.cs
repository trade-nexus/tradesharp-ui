using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class OrdersViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<OrderDetails> _orders;
        private ICollectionView _orderCollection;
        private string _filterString;
        #endregion

        #region Constructor
        public OrdersViewModel()
        {
            _orderCollection = CollectionViewSource.GetDefaultView(_orders);
            if(_orderCollection != null && _orderCollection.CanSort == true)
            {
                _orderCollection.SortDescriptions.Clear();
                _orderCollection.SortDescriptions.Add(new SortDescription("Security.Symbol", ListSortDirection.Ascending));
                _orderCollection.Filter = OrderCollectionFilter;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Collection of OrderDetails
        /// </summary>
        public ObservableCollection<OrderDetails> Orders
        {
            get { return _orders; }
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged("Orders");
                }
            }
        }

        /// <summary>
        /// ICollectionView used as items source for DataGrid
        /// </summary>
        public ICollectionView OrderCollection
        {
            get { return _orderCollection; }
        }

        /// <summary>
        /// Filter string used to find OrderDetails by symbol
        /// </summary>
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                OnPropertyChanged("FilterString");
                _orderCollection.Refresh();
            }
        }
        #endregion

        #region Commands
        #endregion

        #region Methods
        private bool OrderCollectionFilter(object item)
        {
            OrderDetails orderDetails = item as OrderDetails;
            if (_filterString != null)
            {
                return orderDetails.Security.Symbol.ToLower().Contains(_filterString);
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Events
        #endregion
    }
}
