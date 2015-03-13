using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.Utility;

namespace TradeHubGui.ViewModel
{
    public class LimitOrderBookViewModel : BaseViewModel
    {
        #region Fields

        private SortedObservableCollection<LimitOrderBookRecord> _bidRecords;
        private SortedObservableCollection<LimitOrderBookRecord> _askRecords;
        
        #endregion

        #region Constructors

        public LimitOrderBookViewModel(MarketDataDetail marketDataDetail)
        {
            _bidRecords = marketDataDetail.BidRecordsCollection;
            _askRecords = marketDataDetail.AskRecordsCollection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Contains all BID values for LOB
        /// </summary>
        public SortedObservableCollection<LimitOrderBookRecord> BidRecords
        {
            get { return _bidRecords; }
            set
            {
                if (_bidRecords != value)
                {
                    _bidRecords = value;
                    OnPropertyChanged("BidRecords");
                }
            }
        }

        /// <summary>
        /// Contains all ASK values for LOB
        /// </summary>
        public SortedObservableCollection<LimitOrderBookRecord> AskRecords
        {
            get { return _askRecords; }
            set
            {
                if (_askRecords != value)
                {
                    _askRecords = value;
                    OnPropertyChanged("AskRecords");
                }
            }
        }

        #endregion
    }
}
