using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class MarketScannerContentViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Instrument> _instruments;
        #endregion

        #region Constructor
        public MarketScannerContentViewModel()
        {
            #region Temporary fill instruments (this will be removed)
            _instruments = new ObservableCollection<Instrument>();
            for (int i = 0; i < 10; i++)
            {
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
            }
            #endregion
        }
        #endregion

        #region Properties
        public ObservableCollection<Instrument> Instruments
        {
            get { return _instruments; }
            set
            {
                _instruments = value;
                OnPropertyChanged("Instruments");
            }
        }
        #endregion
    }
}
