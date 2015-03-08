using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class LimitOrderBookViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<LobRecord> _limitOrderBookRecords;
        
        #endregion

        #region Constructors
        public LimitOrderBookViewModel(TickDetail tickDetail)
        {
            _limitOrderBookRecords = new ObservableCollection<LobRecord>();
            
            #region Dummy population of LOB
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = -1, BidPrice = -1, AskPrice = 517.00m, AskSize = 1 });
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = -1, BidPrice = -1, AskPrice = 513.00m, AskSize = 3 });
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = -1, BidPrice = -1, AskPrice = 512.00m, AskSize = 13 });
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = -1, BidPrice = -1, AskPrice = 510.90m, AskSize = 20 });
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = -1, BidPrice = -1, AskPrice = 509.00m, AskSize = 23 });

            _limitOrderBookRecords.Add(new LobRecord() { BidSize = 21, BidPrice = 507.20m, AskPrice = -1, AskSize = -1 });
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = 19, BidPrice = 507.00m, AskPrice = -1, AskSize = -1 });
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = 9, BidPrice = 504.00m, AskPrice = -1, AskSize = -1 });
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = 2, BidPrice = 501.00m, AskPrice = -1, AskSize = -1 });
            _limitOrderBookRecords.Add(new LobRecord() { BidSize = 1, BidPrice = 498.00m, AskPrice = -1, AskSize = -1 });
            #endregion
        }
        #endregion

        #region Properties

        public ObservableCollection<LobRecord> LimitOrderBookRecords
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
