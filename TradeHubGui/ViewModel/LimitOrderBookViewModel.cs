using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class LimitOrderBookViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<LimitOrderBookRecord> _bidRecords;
        private ObservableCollection<LimitOrderBookRecord> _askRecords;
        
        #endregion

        #region Constructors

        public LimitOrderBookViewModel(MarketDataDetail marketDataDetail)
        {
            _bidRecords = marketDataDetail.BidRecordsCollection;
            _askRecords = marketDataDetail.AskRecordsCollection;
            
            //#region Dummy population of LOB

            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Ask) { AskPrice = 517.00m, AskQuantity = 1 });
            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Ask) { AskPrice = 513.00m, AskQuantity = 3 });
            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Ask) { AskPrice = 512.00m, AskQuantity = 13 });
            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Ask) { AskPrice = 510.90m, AskQuantity = 20 });
            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Ask) { AskPrice = 509.00m, AskQuantity = 23 });

            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Bid) { BidQuantity = 21, BidPrice = 507.20m });
            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Bid) { BidQuantity = 19, BidPrice = 507.00m });
            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Bid) { BidQuantity = 9, BidPrice = 504.00m });
            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Bid) { BidQuantity = 2, BidPrice = 501.00m });
            //_limitOrderBookRecords.Add(new LimitOrderBookRecord(LobRecordType.Bid) { BidQuantity = 1, BidPrice = 498.00m });
            
            //#endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Contains all BID values for LOB
        /// </summary>
        public ObservableCollection<LimitOrderBookRecord> BidRecords
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
        public ObservableCollection<LimitOrderBookRecord> AskRecords
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
