using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class MarketScannerWindowViewModel : BaseViewModel
    {
        #region Fields

        private string _newSymbol;
        private Provider _provider;
        private TickDetail _selectedTickDetail;
        private ObservableCollection<TickDetail> _tickDetailsCollection;

        private RelayCommand _addNewSymbolCommand;
        private RelayCommand _showLimitOrderBookCommand;
        private RelayCommand _showChartCommand;
        private RelayCommand _sendOrderCommand;
        private RelayCommand _unsubscribeCommand;
        private RelayCommand _showPositionStatsCommand;

        #endregion

        #region Constructor

        public MarketScannerWindowViewModel()
        {
            #region Temporary fill instruments (this will be removed)
            _tickDetailsCollection = new ObservableCollection<TickDetail>();
            _tickDetailsCollection.Add(new TickDetail(new Security() { Symbol = "AAPL" })
            {
                BidQuantity = 23,
                BidPrice = 450.34M,
                AskQuantity = 20,
                AskPrice = 456.00M,
                LastPrice = 445.34M,
                LastQuantity = 23
            });
            _tickDetailsCollection.Add(new TickDetail(new Security() { Symbol = "GOOG" })
            {
                BidQuantity = 23,
                BidPrice = 450.34M,
                AskQuantity = 20,
                AskPrice = 456.00M,
                LastPrice = 445.34M,
                LastQuantity = 23
            });
            _tickDetailsCollection.Add(new TickDetail(new Security() { Symbol = "MSFT" })
            {
                BidQuantity = 23,
                BidPrice = 450.34M,
                AskQuantity = 20,
                AskPrice = 456.00M,
                LastPrice = 445.34M,
                LastQuantity = 23
            });
            _tickDetailsCollection.Add(new TickDetail(new Security() { Symbol = "HP" })
            {
                BidQuantity = 23,
                BidPrice = 450.34M,
                AskQuantity = 20,
                AskPrice = 456.00M,
                LastPrice = 445.34M,
                LastQuantity = 23
            });
            _tickDetailsCollection.Add(new TickDetail(new Security() { Symbol = "AOI" })
            {
                BidQuantity = 23,
                BidPrice = 450.34M,
                AskQuantity = 20,
                AskPrice = 456.00M,
                LastPrice = 445.34M,
                LastQuantity = 23
            });
            _tickDetailsCollection.Add(new TickDetail(new Security() { Symbol = "WAS" })
            {
                BidQuantity = 23,
                BidPrice = 450.34M,
                AskQuantity = 20,
                AskPrice = 456.00M,
                LastPrice = 445.34M,
                LastQuantity = 23
            });
            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Collection of Tick Details for watching related to certain market data provider
        /// </summary>
        public ObservableCollection<TickDetail> TickDetailsCollection
        {
            get { return _tickDetailsCollection; }
            set
            {
                _tickDetailsCollection = value;
                OnPropertyChanged("TickDetailsCollection");
            }
        }

        /// <summary>
        /// Selected Tick Detail
        /// </summary>
        public TickDetail SelectedTickDetail
        {
            get { return _selectedTickDetail; }
            set
            {
                if (_selectedTickDetail != value)
                {
                    _selectedTickDetail = value;
                    OnPropertyChanged("SelectedTickDetail");
                }
            }
        }

        /// <summary>
        /// New symbol for adding to the scanner
        /// </summary>
        public string NewSymbol
        {
            get { return _newSymbol; }
            set
            {
                if (_newSymbol != value)
                {
                    _newSymbol = value;
                    OnPropertyChanged("NewSymbol");
                }
            }
        }

        /// <summary>
        /// Contains Market Data Provider Details
        /// </summary>
        public Provider Provider
        {
            get { return _provider; }
            set 
            {
                if (_provider != value)
                {
                    _provider = value;
                    OnPropertyChanged("Provider");
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command used for adding new symbol to the scanner
        /// </summary>
        public ICommand AddNewSymbolCommand
        {
            get
            {
                return _addNewSymbolCommand ?? (_addNewSymbolCommand = new RelayCommand(param => AddNewSymbolExecute(), param => AddNewSymbolCanExecute()));
            }
        }

        /// <summary>
        /// Command used for showing LOB
        /// </summary>
        public ICommand ShowLimitOrderBookCommand
        {
            get
            {
                return _showLimitOrderBookCommand ?? (_showLimitOrderBookCommand = new RelayCommand(param => ShowLimitOrderBookExecute(param)));
            }
        }

        /// <summary>
        /// Command used for showing chart
        /// </summary>
        public ICommand ShowChartCommand
        {
            get
            {
                return _showChartCommand ?? (_showChartCommand = new RelayCommand(param => ShowChartExecute(param)));
            }
        }

        /// <summary>
        /// Command used for sending order
        /// </summary>
        public ICommand SendOrderCommand
        {
            get
            {
                return _sendOrderCommand ?? (_sendOrderCommand = new RelayCommand(param => SendOrderExecute(param)));
            }
        }

        /// <summary>
        /// Command used for unsubscribe
        /// </summary>
        public ICommand UnsubsribeCommand
        {
            get
            {
                return _unsubscribeCommand ?? (_unsubscribeCommand = new RelayCommand(param => UnsubsribeExecute(param)));
            }
        }

        /// <summary>
        /// Command used for showing position stats
        /// </summary>
        public ICommand ShowPositionStatsCommand
        {
            get
            {
                return _showPositionStatsCommand ?? (_showPositionStatsCommand = new RelayCommand(param => ShowPositionStatsExecute(param)));
            }
        }

        #endregion

        #region Trigger Methods for Commands

        private void AddNewSymbolExecute()
        {
            // Create new tick detail's object
            TickDetail tickDetail = new TickDetail(new Security() { Symbol = NewSymbol.Trim() });

            // Add Tick Detail object to Provider's local map
            _provider.TickDetailsMap.Add(tickDetail.Security.Symbol, tickDetail);

            // Add new tick detail to the Tick Detail's Map to show on UI
            TickDetailsCollection.Add(tickDetail);

            // Select new tick detail in DataGrid
            SelectedTickDetail = tickDetail;

            // Create a new subscription request for requesting market data
            var subscriptionRequest = new SubscriptionRequest(tickDetail.Security, _provider, SubscriptionType.Subscribe);

            // Raise Event to notify listeners
            EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);

            // Clear NewSymbol string
            NewSymbol = string.Empty;
        }

        /// <summary>
        /// If new symbol is not entered or if market provider is disconneted, return false, otherwise return true
        /// </summary>
        private bool AddNewSymbolCanExecute()
        {
            if (string.IsNullOrWhiteSpace(NewSymbol) || _provider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
                return false;

            return true;
        }

        /// <summary>
        /// Show LOB for current instrument
        /// </summary>
        /// <param name="param">current instrument</param>
        private void ShowLimitOrderBookExecute(object param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Show Chart for current instrument
        /// </summary>
        /// <param name="param">current instrument</param>
        private void ShowChartExecute(object param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send Order for current instrument
        /// </summary>
        /// <param name="param">current instrument</param>
        private void SendOrderExecute(object param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unsubsribe from current instrument
        /// </summary>
        /// <param name="param">current instrument</param>
        private void UnsubsribeExecute(object param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Show Position Stats for current instrument
        /// </summary>
        /// <param name="param">current instrument</param>
        private void ShowPositionStatsExecute(object param)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
