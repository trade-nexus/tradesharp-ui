using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class LimitOrderBookViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<LimitOrderBookRecord> _limitOrderBookRecords;
        
        #endregion

        #region Constructors

        public LimitOrderBookViewModel(MarketDataDetail marketDataDetail)
        {
            _limitOrderBookRecords = marketDataDetail.LimitOrderBookCollection;
            
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

        public ObservableCollection<LimitOrderBookRecord> LimitOrderBookRecords
        {
            get { return _limitOrderBookRecords; }
            set
            {
                if (_limitOrderBookRecords != value)
                {
                    _limitOrderBookRecords = value;
                    OnPropertyChanged("LimitOrderBookRecords");
                }
            }
        }

        #endregion
    }
}
