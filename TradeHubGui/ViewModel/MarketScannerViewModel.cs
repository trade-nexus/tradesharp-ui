using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;
using TradeHubGui.Views;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace TradeHubGui.ViewModel
{
    public class MarketScannerViewModel : BaseViewModel
    {
        #region Fields
        private DockingManager _dockManager;
        private RelayCommand _showNewMarketScannerWindowCommand;
        private RelayCommand _createMarketScannerCommand;
        private NewMarketScannerWindow _newMarketScannerWindow;
        private ObservableCollection<Provider> _marketDataProviders;
        private ProvidersController _providersController;
        private Provider _selectedMarketDataProvider;
        #endregion

        #region Constructor
        public MarketScannerViewModel(DockingManager dockManager)
        {
            _providersController = new ProvidersController();
            _dockManager = dockManager;
            _dockManager.DocumentClosing += DockingManager_DocumentClosing;
            _dockManager.Layout.PropertyChanged += LayoutRoot_PropertyChanged;
        }
        #endregion

        #region Properties
        public DockingManager DockManager
        {
            get { return _dockManager; }
            set { _dockManager = value; }
        }

        /// <summary>
        /// Collection of market data providers
        /// </summary>
        public ObservableCollection<Provider> MarketDataProviders
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
        public Provider SelectedMarketDataProvider
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

        #region Commands
        /// <summary>
        /// Opens 'New Market Scanner' window for chosing market data provider for scanning
        /// </summary>
        public ICommand ShowNewMarketScannerWindowCommand
        {
            get
            {
                return _showNewMarketScannerWindowCommand ?? (_showNewMarketScannerWindowCommand = new RelayCommand(param => ShowNewMarketScannerWindowExecute()));
            }
        }

        /// <summary>
        /// Creating of new market scanner document window for chosen market data provider
        /// </summary>
        public ICommand CreateMarketScannerCommand
        {
            get
            {
                return _createMarketScannerCommand ?? (_createMarketScannerCommand = new RelayCommand(param => CreateMarketScannerExecute(), param => CreateMarketScannerCanExecute(param)));
            }
        }
        #endregion

        #region Methods
        private void ShowNewMarketScannerWindowExecute()
        {
            _newMarketScannerWindow = new NewMarketScannerWindow();
            _newMarketScannerWindow.Owner = MainWindow;
            _newMarketScannerWindow.DataContext = this;
            
            // Populate MarketDataProviders with actual list of market data providers
            InitializeMarketDataProviders();

            // Show 'New Market Scanner' window
            _newMarketScannerWindow.ShowDialog();
        }

        /// <summary>
        /// If selected market data provider is not null, return true
        /// </summary>
        private bool CreateMarketScannerCanExecute(object param)
        {
            if (param != null)
                return true;

            return false;
        }

        /// <summary>
        /// Create market scanner data grid for selected market data provider
        /// </summary>
        private void CreateMarketScannerExecute()
        {
            // Add new scanner document window for selected market data provider
            var documentPane = _dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (documentPane != null)
            {
                // Create content and set DataContext
                MarketScannerContentView view = new MarketScannerContentView();
                view.DataContext = new MarketScannerContentViewModel();
                
                // Create document and add content to it
                LayoutDocument doc = new LayoutDocument();
                doc.Content = view;
                doc.IsSelected = true;
                doc.Title = SelectedMarketDataProvider.ProviderName;
                doc.ContentId = SelectedMarketDataProvider.ProviderName.Replace(" ", "_");

                // Add document to the documentPane
                documentPane.Children.Add(doc);
            }

            // Detach DataContext and close 'New Market Scanner' window
            _newMarketScannerWindow.DataContext = null;
            _newMarketScannerWindow.Close();
        }


        /// <summary>
        /// Initialization of market data providers
        /// </summary>
        private async void InitializeMarketDataProviders()
        {
            _marketDataProviders = new ObservableCollection<Provider>();

            // Request Controller for infomation
            var availableProviders = await Task.Run(() => _providersController.GetAvailableMarketDataProviders());

            // Safety check incase information was not populated
            if (availableProviders == null)
                return;

            // Populate Individual Market Data Provider details
            foreach (var keyValuePair in availableProviders)
            {
                Provider tempProvider = new Provider() { ProviderName = keyValuePair.Key, ConnectionStatus = ConnectionStatus.Disconnected };
                tempProvider.ProviderCredentials = keyValuePair.Value;

                // Add to Collection
                _marketDataProviders.Add(tempProvider);
            }

            // Select initially 1st provider in ComboBox
            if (_marketDataProviders != null && _marketDataProviders.Count > 0)
                SelectedMarketDataProvider = _marketDataProviders[0];
        }
        #endregion

        #region Events
        /// <summary>
        /// Handles changing of Active content
        /// </summary>
        /// <param name="sender">LayoutRoot</param>
        /// <param name="e"></param>
        void LayoutRoot_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var activeContent = ((LayoutRoot)sender).ActiveContent;
            if (e.PropertyName == "ActiveContent")
            {
                Debug.WriteLine(string.Format("ActiveContent-> {0}", activeContent));
            }
        }

        /// <summary>
        /// Handles closing event if invoked for certain document
        /// </summary>
        /// <param name="sender">DockingManager</param>
        /// <param name="e"></param>
        void DockingManager_DocumentClosing(object sender, Xceed.Wpf.AvalonDock.DocumentClosingEventArgs e)
        {
            if (WPFMessageBox.Show(MainWindow, "Are you sure you want to close the document?", "Market Data Scanner", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                e.Cancel = true;
        }
        #endregion
    }
}
