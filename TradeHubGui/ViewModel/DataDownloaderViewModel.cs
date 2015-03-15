using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.ViewModel
{
    public class DataDownloaderViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<MarketDataProvider> _marketDataProviders;
        private MarketDataProvider _selectedMarketDataProvider;

        #endregion

        #region Constructor

        public DataDownloaderViewModel()
        {
            InitializeMarketDataProviders();
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

        #endregion
    }
}
