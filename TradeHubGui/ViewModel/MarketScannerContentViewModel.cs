using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class MarketScannerContentViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Instrument> _instruments;
        private Instrument selectedInstrument;
        private string _newSymbol;
        private RelayCommand _addNewSymbolCommand;
        private RelayCommand _showLimitOrderBookCommand;
        private RelayCommand _showChartCommand;
        private RelayCommand _sendOrderCommand;
        private RelayCommand _unsubscribeCommand;
        private RelayCommand _showPositionStatsCommand;
        #endregion

        #region Constructor
        public MarketScannerContentViewModel()
        {
            #region Temporary fill instruments (this will be removed)
            _instruments = new ObservableCollection<Instrument>();
            _instruments.Add(new Instrument("AAPL", 23, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("GOOG", 43, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("MSFT", 33, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("HP", 42, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("AOI", 34, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("WAS", 15, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("AAPL", 23, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("GOOG", 23, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("MSFT", 45, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("HP", 33, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("AOI", 24, 450.34f, 20, 456.00f, 445.34f, 23));
            _instruments.Add(new Instrument("WAS", 23, 450.34f, 20, 456.00f, 445.34f, 23));
            #endregion
        }
        #endregion

        #region Properties
        /// <summary>
        /// Collection of instruments for watching related to certain market data provider
        /// </summary>
        public ObservableCollection<Instrument> Instruments
        {
            get { return _instruments; }
            set
            {
                _instruments = value;
                OnPropertyChanged("Instruments");
            }
        }

        /// <summary>
        /// Selected istrument
        /// </summary>
        public Instrument SelectedInstrument
        {
            get { return selectedInstrument; }
            set
            {
                if (selectedInstrument != value)
                {
                    selectedInstrument = value;
                    OnPropertyChanged("SelectedInstrument");
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

        #region Methods
        private void AddNewSymbolExecute()
        {
            //TODO: subscribe for watching

            // Create new instrument
            Instrument instrument = new Instrument(NewSymbol.Trim(), 0, 0, 0, 0, 0, 0);

            // Add new instrument to the instruments collection
            Instruments.Add(instrument);

            // Select new instrument in DataGrid
            SelectedInstrument = instrument;

            // Clear NewSymbol string
            NewSymbol = string.Empty;
        }

        /// <summary>
        /// If no entered new symbol, return false
        /// </summary>
        /// <returns></returns>
        private bool AddNewSymbolCanExecute()
        {
            if (string.IsNullOrWhiteSpace(NewSymbol))
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
