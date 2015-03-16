using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Dashboard.Services;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;

namespace TradeHubGui.ViewModel
{
    public class DataDownloaderViewModel : BaseViewModel
    {
        #region Fields

        private List<string> _barTypes;
        private List<string> _barFormats;
        private List<string> _barPriceTypes; 
        private ObservableCollection<MarketDataProvider> _marketDataProviders;
        private MarketDataProvider _selectedMarketDataProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataDownloaderViewModel()
        {
            _barTypes = new List<string>();
            _barFormats = new List<string>();
            _barPriceTypes = new List<string>();

            InitializeMarketDataProviders();

            PopulateBarTypesList();
            PopulateBarFormatesList();
            PopulateBarPriceTypesList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Collection of market data providers
        /// </summary>
        public ObservableCollection<MarketDataProvider> MarketDataProviders
        {
            get { return _marketDataProviders; }
            set
            {
                if (_marketDataProviders != value)
                {
                    _marketDataProviders = value;
                    OnPropertyChanged("MarketDataProviders");
                }
            }
        }

        /// <summary>
        /// Selected market data provider
        /// </summary>
        public MarketDataProvider SelectedMarketDataProvider
        {
            get { return _selectedMarketDataProvider; }
            set
            {
                if (_selectedMarketDataProvider != value)
                {
                    _selectedMarketDataProvider = value;
                    OnPropertyChanged("SelectedMarketDataProvider");
                }
            }
        }

        /// <summary>
        /// Contains supported Bar types
        /// </summary>
        public List<string> BarTypes
        {
            get { return _barTypes; }
            set { _barTypes = value; }
        }

        /// <summary>
        /// Contains supported Bar formats
        /// </summary>
        public List<string> BarFormats
        {
            get { return _barFormats; }
            set { _barFormats = value; }
        }

        /// <summary>
        /// Contains supported Bar Price types
        /// </summary>
        public List<string> BarPriceTypes
        {
            get { return _barPriceTypes; }
            set { _barPriceTypes = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialization of market data providers
        /// </summary>
        public void InitializeMarketDataProviders()
        {
            _marketDataProviders = new ObservableCollection<MarketDataProvider>();

            // Populate Individual Market Data Provider details
            foreach (var provider in ProvidersController.MarketDataProviders)
            {
                // Add to Collection only connected market data providers
                if (provider.ConnectionStatus == TradeHub.Common.Core.Constants.ConnectionStatus.Connected)
                {
                    _marketDataProviders.Add(provider);
                }
            }

            OnPropertyChanged("MarketDataProviders");

            // Select initially 1st provider
            if (_marketDataProviders != null && _marketDataProviders.Count > 0)
                SelectedMarketDataProvider = _marketDataProviders[0];
        }

        /// <summary>
        /// Adds available values to Bar Types list to be displayed on UI
        /// </summary>
        private void PopulateBarTypesList()
        {
            _barTypes.Add(BarType.DAILY);
            _barTypes.Add(BarType.INTRADAY);
            _barTypes.Add(BarType.MIDPOINT);
            _barTypes.Add(BarType.MONTHLY);
            _barTypes.Add(BarType.TICK);
            _barTypes.Add(BarType.TRADE);
            _barTypes.Add(BarType.WEEKLY);
        }

        /// <summary>
        /// Adds available values to Bar Formates list to be displayed on UI
        /// </summary>
        private void PopulateBarFormatesList()
        {
            _barFormats.Add(BarFormat.TIME);
            _barFormats.Add(BarFormat.DISPLACEMENT);
            _barFormats.Add(BarFormat.EQUAL_ENGINEERED);
            _barFormats.Add(BarFormat.UNEQUAL_ENGINEERED);
        }

        /// <summary>
        /// Adds available values to Bar Price Types list to be displayed on UI
        /// </summary>
        private void PopulateBarPriceTypesList()
        {
            _barPriceTypes.Add(BarPriceType.ASK);
            _barPriceTypes.Add(BarPriceType.BID);
            _barPriceTypes.Add(BarPriceType.LAST);
            _barPriceTypes.Add(BarPriceType.MEAN);
        }

        #endregion
    }
}
