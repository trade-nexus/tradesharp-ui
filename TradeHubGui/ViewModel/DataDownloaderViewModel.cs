using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NHibernate.Hql.Ast.ANTLR;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.FactoryMethods;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.Dashboard.Services;
using TradeHubGui.DataDownloader.Service;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;

namespace TradeHubGui.ViewModel
{
    public class DataDownloaderViewModel : BaseViewModel
    {
        #region Fields

        private bool _writeBinary;
        private bool _writeCsv;

        private string _selectedBarType;
        private string _selectedBarFormat;
        private string _selectedBarPriceType;

        private uint _historicBarInterval;

        private decimal _barLength;
        private decimal _pipSize;

        private DateTime _startDate;
        private DateTime _endDate;

        private List<string> _barTypes;
        private List<string> _barFormats;
        private List<string> _barPriceTypes;

        private MarketDataProvider _selectedMarketDataProvider;
        private MarketDataDetail _selectedMarketDetail;

        private ObservableCollection<MarketDataDetail> _marketDetailCollection;
        private ObservableCollection<MarketDataProvider> _marketDataProviders;

        private RelayCommand _submitBarSettingsCommand;
        private RelayCommand _submitHistoricBarSettingsCommand;
        private RelayCommand _saveOptionsCommand;
        private RelayCommand _saveBarsCommand;

        private DataPersistenceController _persistenceController;
        private bool _hasOptionsChanges;
        private bool _hasBarSettingsChanges;
        private bool _hasHistoricBarSettingsChanges;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataDownloaderViewModel()
        {
            _historicBarInterval = 60;

            _barLength = default(decimal);
            _pipSize = default(decimal);

            _startDate = default(DateTime);
            _endDate = default(DateTime);

            _barTypes = new List<string>();
            _barFormats = new List<string>();
            _barPriceTypes = new List<string>();

            _persistenceController = new DataPersistenceController();

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
                    PopulateMarketDetailCollection();
                    OnPropertyChanged("SelectedMarketDataProvider");
                }
            }
        }

        /// <summary>
        /// Displays Selected Data Provider's Market Detail's Collection
        /// </summary>
        public ObservableCollection<MarketDataDetail> MarketDetailCollection
        {
            get { return _marketDetailCollection; }
            set
            {
                _marketDetailCollection = value;
                OnPropertyChanged("MarketDetailCollection");
            }
        }

        /// <summary>
        /// Currently selected market detail object in the data grid
        /// </summary>
        public MarketDataDetail SelectedMarketDetail
        {
            get { return _selectedMarketDetail; }
            set
            {
                _selectedMarketDetail = value;
                OnPropertyChanged("SelectedMarketDetail");
            }
        }

        /// <summary>
        /// Used for write binary option
        /// </summary>
        public bool WriteBinary
        {
            get { return _writeBinary; }
            set
            {
                if (_writeBinary != value)
                {
                    _writeBinary = value;
                    _hasOptionsChanges = true;
                    OnPropertyChanged("WriteBinary");
                }
            }
        }

        /// <summary>
        /// Used for write csv option
        /// </summary>
        public bool WriteCsv
        {
            get { return _writeCsv; }
            set
            {
                if (_writeCsv != value)
                {
                    _writeCsv = value;
                    _hasOptionsChanges = true;
                    OnPropertyChanged("WriteCsv");
                }
            }
        }

        /// <summary>
        /// Bar Format selected from the Combo Box
        /// </summary>
        public string SelectedBarFormat
        {
            get { return _selectedBarFormat; }
            set
            {
                _selectedBarFormat = value;
                _hasBarSettingsChanges = true;
                OnPropertyChanged("SelectedBarFormat");
            }
        }

        /// <summary>
        /// Selected Bar Price Type from the Combo Box
        /// </summary>
        public string SelectedBarPriceType
        {
            get { return _selectedBarPriceType; }
            set
            {
                _selectedBarPriceType = value;
                _hasBarSettingsChanges = true;
                OnPropertyChanged("SelectedBarPriceType");
            }
        }

        /// <summary>
        /// Bar interval to be used for Historical bar data request
        /// </summary>
        public uint HistoricBarInterval
        {
            get { return _historicBarInterval; }
            set
            {
                _historicBarInterval = value;
                _hasHistoricBarSettingsChanges = true;
                OnPropertyChanged("HistoricBarInterval");
            }
        }

        /// <summary>
        /// Pip size to be used for bar creation
        /// </summary>
        public decimal PipSize
        {
            get { return _pipSize; }
            set
            {
                _pipSize = value;
                _hasBarSettingsChanges = true;
                OnPropertyChanged("PipSize");
            }
        }

        /// <summary>
        /// Bar length which indicates duration
        /// </summary>
        public decimal BarLength
        {
            get { return _barLength; }
            set
            {
                _barLength = value;
                _hasBarSettingsChanges = true;
                OnPropertyChanged("BarLength");
            }
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

        /// <summary>
        /// Contains supported Bar types
        /// </summary>
        public List<string> BarTypes
        {
            get { return _barTypes; }
            set { _barTypes = value; }
        }

        /// <summary>
        /// Bar Type selected from the Combo Box
        /// </summary>
        public string SelectedBarType
        {
            get { return _selectedBarType; }
            set
            {
                _selectedBarType = value;
                _hasHistoricBarSettingsChanges = true;
                OnPropertyChanged("SelectedBarType");
            }
        }

        /// <summary>
        /// Historical Data start date
        /// </summary>
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                _hasHistoricBarSettingsChanges = true;
                OnPropertyChanged("StartDate");
            }
        }

        /// <summary>
        /// Historical Data end date
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                _hasHistoricBarSettingsChanges = true;
                OnPropertyChanged("EndDate");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command used to submit information for selected Bar Settings
        /// </summary>
        public ICommand SubmitBarSettingsCommand
        {
            get
            {
                return _submitBarSettingsCommand ?? (_submitBarSettingsCommand = new RelayCommand(param => SubmitBarSettingsExecute(), param => SubmitBarSettingsCanExecute()));
            }
        }

        /// <summary>
        /// Command used to submit information for selected Historical Bar Settings
        /// </summary>
        public ICommand SubmitHistoricBarSettingsCommand
        {
            get
            {
                return _submitHistoricBarSettingsCommand ??
                       (_submitHistoricBarSettingsCommand =
                           new RelayCommand(param => SubmitHistoricBarSettingsExecute(),
                               param => SubmitHistoricBarSettingsCanExecute()));
            }
        }

        /// <summary>
        /// Command used to submit information for selected Historical Bar Settings
        /// </summary>
        public ICommand SaveOptionsCommand
        {
            get
            {
                return _saveOptionsCommand ?? (_saveOptionsCommand = new RelayCommand(param => SaveOptionsExecute(), param => SaveOptionsCanExecute()));
            }
        }

        /// <summary>
        /// Command used for triggering bar settings
        /// </summary>
        public ICommand SaveBarsCommand
        {
            get
            {
                return _saveBarsCommand ?? (_saveBarsCommand = new RelayCommand(param => SaveBarsCommandExecute(param)));
            }
        }

        #endregion

        #region Commad Execute Methods

        /// <summary>
        /// Trigger bar settings to enable submit button
        /// </summary>
        private void SaveBarsCommandExecute(object param)
        {
            if ((bool)param)
            {
                _hasBarSettingsChanges = true;
                _hasHistoricBarSettingsChanges = true;
            }
        }

        /// <summary>
        /// Indicates if the Bar settings command can be executed or not
        /// </summary>
        /// <returns></returns>
        private bool SubmitBarSettingsCanExecute()
        {
            if (_selectedMarketDetail != null && _selectedMarketDetail.PersistenceInformation.SaveBars && _hasBarSettingsChanges
                && _selectedBarFormat != null && _selectedBarPriceType != null)
                return true;

            return false;
        }

        /// <summary>
        /// Called when 'Submit' button for 'Bar' settings is clicked
        /// </summary>
        private void SubmitBarSettingsExecute()
        {
            SubmitBarSettingsRequest();
            _hasBarSettingsChanges = false;
        }

        /// <summary>
        /// Indicates if the Historical Bar settings command can be executed or not
        /// </summary>
        /// <returns></returns>
        private bool SubmitHistoricBarSettingsCanExecute()
        {
            if (_selectedMarketDetail != null && _selectedMarketDetail.PersistenceInformation.SaveBars && _hasHistoricBarSettingsChanges
                && _selectedBarType != null)
                return true;

            return false;
        }

        /// <summary>
        /// Called when 'Submit' button for 'Historical Bar' settings is clicked
        /// </summary>
        private void SubmitHistoricBarSettingsExecute()
        {
            SubmitHistoricBarSettingsRequest();
            _hasHistoricBarSettingsChanges = false;
        }

        /// <summary>
        /// Called when 'Save' button for persistence options is clicked
        /// </summary>
        private void SaveOptionsExecute()
        {
            // Save persistence options
            _persistenceController.SavePersistInformation(_writeBinary, _writeCsv);
            _hasOptionsChanges = false;
        }

        private bool SaveOptionsCanExecute()
        {
            if (_hasOptionsChanges)
                return true;

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Requests Bar subscription for the selected Bar settings
        /// </summary>
        private void SubmitBarSettingsRequest()
        {
            // Forward request to persistence controller
            _persistenceController.SubmitBarRequest(SelectedMarketDetail.Security, _barLength, _pipSize,
                _selectedBarFormat, _selectedBarPriceType, SelectedMarketDataProvider);
        }

        /// <summary>
        /// Requests Historical Bar subscription for the selected Bar settings
        /// </summary>
        private void SubmitHistoricBarSettingsRequest()
        {
            // Forward request to persistence controller
            _persistenceController.SubmitHistoricDataRequest(
                SelectedMarketDetail.Security, _selectedBarType, _startDate,
                _endDate, _historicBarInterval, SelectedMarketDataProvider);
        }

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
                if (provider.ConnectionStatus == ConnectionStatus.Connected)
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
        /// Adds available values to Bar Formates list to be displayed on UI
        /// </summary>
        private void PopulateBarFormatesList()
        {
            _barFormats.Add(BarFormat.TIME);
            _barFormats.Add(BarFormat.DISPLACEMENT);
            _barFormats.Add(BarFormat.EQUAL_ENGINEERED);
            _barFormats.Add(BarFormat.UNEQUAL_ENGINEERED);

            // Initially select the first
            SelectedBarFormat = _barFormats[0];
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

            // Initially select the first
            SelectedBarPriceType = _barPriceTypes[0];
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

            // Initially select the first
            SelectedBarType = _barTypes[0];
        }

        /// <summary>
        /// Populates Market Data Details for the selected Market Data Provider
        /// </summary>
        private void PopulateMarketDetailCollection()
        {
            // Reset Collection
            MarketDetailCollection = new ObservableCollection<MarketDataDetail>();

            // Assign new values
            if (SelectedMarketDataProvider != null)
                MarketDetailCollection = SelectedMarketDataProvider.MarketDetailCollection;
        }

        #endregion
    }
}
