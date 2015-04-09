using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.Utility;
using TradeHubGui.Dashboard.Services;
using Forms = System.Windows.Forms;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;

namespace TradeHubGui.ViewModel
{
    public class ProvidersViewModel : BaseViewModel
    {
        #region Fields

        private string _newDataProviderName;
        private string _newExecutionProviderName;
        private string  _dataProviderInfoMessage;
        private string  _executionProviderInfoMessage;
        private string _newMarketDataProviderPath;
        private string _newOrderExecutionProviderPath;

        private bool _newDataProviderParametersRequired;
        private bool _newExecutionProviderParametersRequired;
        private bool _dataProviderParametersToBeDisplayed;
        private bool _executionProviderParametersToBeDisplayed;

        private ObservableCollection<MarketDataProvider> _marketDataProviders;
        private ObservableCollection<OrderExecutionProvider> _orderExecutionProviders;

        private MarketDataProvider _selectedMarketDataProvider;
        private OrderExecutionProvider _selectedOrderExecutionProvider;

        private RelayCommand _addProviderCommand;
        private RelayCommand _removeProviderCommand;
        private RelayCommand _connectProviderCommand;
        private RelayCommand _disconnectProviderCommand;
        private RelayCommand _saveParametersCommand;
        private RelayCommand _saveNewProviderCommand;
        private RelayCommand _cancelCommand;

        private ProvidersController _providersController;

        #endregion

        #region Constructors
        
        public ProvidersViewModel()
        {
            _providersController = new ProvidersController();
            _marketDataProviders = new ObservableCollection<MarketDataProvider>();
            _orderExecutionProviders = new ObservableCollection<OrderExecutionProvider>();

            InitializeMarketDataProviders();
            InitializeOrderExecutionProviders();

            DataProviderParametersToBeDisplayed = true;
            ExecutionProviderParametersToBeDisplayed = true;
            NewDataProviderParametersRequired = false;
            NewExecutionProviderParametersRequired = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name to be used for the new Market Data Provider
        /// </summary>
        public string NewDataProviderName
        {
            get { return _newDataProviderName; }
            set
            {
                _newDataProviderName = value;
                OnPropertyChanged("NewDataProviderName");
            }
        }

        /// <summary>
        /// Name to be used for the new Order Execution Provider
        /// </summary>
        public string NewExecutionProviderName
        {
            get { return _newExecutionProviderName; }
            set
            {
                _newExecutionProviderName = value;
                OnPropertyChanged("NewExecutionProviderName");
            }
        }

        /// <summary>
        /// Displays Market Data Provider Info message on UI
        /// </summary>
        public string DataProviderInfoMessage
        {
            get { return _dataProviderInfoMessage; }
            set
            {
                _dataProviderInfoMessage = value;
                OnPropertyChanged("DataProviderInfoMessage");
            }
        }

        /// <summary>
        /// Displays Order Execution Provider Info message on UI
        /// </summary>
        public string ExecutionProviderInfoMessage
        {
            get { return _executionProviderInfoMessage; }
            set
            {
                _executionProviderInfoMessage = value;
                OnPropertyChanged("ExecutionProviderInfoMessage");
            }
        }

        /// <summary>
        /// Indicates if the Parameters for the Selected Data Provider are to be displayed or not
        /// </summary>
        public bool DataProviderParametersToBeDisplayed
        {
            get { return _dataProviderParametersToBeDisplayed; }
            set
            {
                _dataProviderParametersToBeDisplayed = value;
                OnPropertyChanged("DataProviderParametersToBeDisplayed");
            }
        }

        /// <summary>
        /// Indicates if the Parameters for the Selected Execution Provider are to be displayed or not
        /// </summary>
        public bool ExecutionProviderParametersToBeDisplayed
        {
            get { return _executionProviderParametersToBeDisplayed; }
            set
            {
                _executionProviderParametersToBeDisplayed = value;
                OnPropertyChanged("ExecutionProviderParametersToBeDisplayed");
            }
        }

        /// <summary>
        /// Indicates if Input parameters for adding new data provider is required or not
        /// </summary>
        public bool NewDataProviderParametersRequired
        {
            get { return _newDataProviderParametersRequired; }
            set
            {
                _newDataProviderParametersRequired = value;
                OnPropertyChanged("NewDataProviderParametersRequired");
            }
        }
        
        /// <summary>
        /// Indicates if the Input parameters for adding new execution provider is required or not
        /// </summary>
        public bool NewExecutionProviderParametersRequired
        {
            get { return _newExecutionProviderParametersRequired; }
            set
            {
                _newExecutionProviderParametersRequired = value;
                OnPropertyChanged("NewExecutionProviderParametersRequired");
            }
        }

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
        /// Collection of order execution providers
        /// </summary>
        public ObservableCollection<OrderExecutionProvider> OrderExecutionProviders
        {
            get { return _orderExecutionProviders; }
            set
            {
                if (_orderExecutionProviders != value)
                {
                    _orderExecutionProviders = value;
                    OnPropertyChanged("OrderExecutionProviders");
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
        /// Selected order execution provider
        /// </summary>
        public OrderExecutionProvider SelectedOrderExecutionProvider
        {
            get { return _selectedOrderExecutionProvider; }
            set
            {
                if (_selectedOrderExecutionProvider != value)
                {
                    _selectedOrderExecutionProvider = value;
                    OnPropertyChanged("SelectedOrderExecutionProvider");
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Used for 'Add new provider' button for adding new marked data provider or order execution provider depending on param
        /// </summary>
        public ICommand AddProviderCommand
        {
            get
            {
                return _addProviderCommand ?? (_addProviderCommand = new RelayCommand(param => AddProviderExecute(param)));
            }
        }

        /// <summary>
        /// Used for 'Remove provider' button for removing marked data provider or order execution provider depending on param
        /// </summary>
        public ICommand RemoveProviderCommand
        {
            get
            {
                return _removeProviderCommand ?? (_removeProviderCommand = new RelayCommand(param => RemoveProviderExecute(param), Param => RemoveProviderCanExecute()));
            }
        }

        /// <summary>
        /// Connect selected provider
        /// </summary>
        public ICommand ConnectProviderCommand
        {
            get
            {
                return _connectProviderCommand ?? (_connectProviderCommand = new RelayCommand(param => ConnectProviderExecute(param), param => ConnectProviderCanExecute(param)));
            }
        }

        /// <summary>
        /// Disconnect selected provider
        /// </summary>
        public ICommand DisconnectProviderCommand
        {
            get
            {
                return _disconnectProviderCommand ?? (_disconnectProviderCommand = new RelayCommand(param => DisconnectProviderExecute(param), param => DisconnectProviderCanExecute(param)));
            }
        }

        /// <summary>
        /// Disconnect selected provider
        /// </summary>
        public ICommand SaveParametersCommand
        {
            get
            {
                return _saveParametersCommand ?? (_saveParametersCommand = new RelayCommand(param => SaveParametersExecute(param), param => SaveParametersCanExecute(param)));
            }
        }

        /// <summary>
        /// Used for 'Save' button for adding new Provider
        /// </summary>
        public ICommand SaveNewProviderCommand
        {
            get
            {
                return _saveNewProviderCommand ?? (_saveNewProviderCommand = new RelayCommand(param => SaveNewProviderExecute(param), param => SaveNewProviderCanExecute()));
            }
        }

        /// <summary>
        /// Used for 'Cancel' button when adding new Provider
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(param => CancelCommandExecute(param), param => CancelCommandCanExecute()));
            }
        }

        #endregion

        #region Trigger Methods for Commands

        /// <summary>
        /// Add new provider to the MarketDataProviders or to the OrderExecutionProviders collection depending on param
        /// </summary>
        private void AddProviderExecute(object param)
        {
            Forms.OpenFileDialog openFileDialog = new Forms.OpenFileDialog();
            openFileDialog.Title = "Load Provider";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Assembly Files (.dll)|*.dll|All Files (*.*)|*.*";
            Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == Forms.DialogResult.OK)
            {
                if (param.Equals("MarketDataProvider"))
                {
                    SelectedMarketDataProvider = null;
                    DataProviderParametersToBeDisplayed = false;
                    NewDataProviderParametersRequired = true;

                    // Save selected '.dll' path
                    _newMarketDataProviderPath = openFileDialog.FileName;
                }
                else if (param.Equals("OrderExecutionProvider"))
                {
                    SelectedOrderExecutionProvider = null;
                    ExecutionProviderParametersToBeDisplayed = false;
                    NewExecutionProviderParametersRequired = true;

                    // Save selected '.dll' path
                    _newOrderExecutionProviderPath = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// Remove provider from the MarketDataProviders or from the OrderExecutionProviders collection depending on param
        /// </summary>
        private void RemoveProviderExecute(object param)
        {
            Provider selectedProvider = param.Equals("MarketDataProvider") ? (Provider) SelectedMarketDataProvider : SelectedOrderExecutionProvider;

            if ((System.Windows.Forms.DialogResult)WPFMessageBox.Show(MainWindow, string.Format("Remove provider {0}?", selectedProvider.ProviderName), "Question",
                 MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == System.Windows.Forms.DialogResult.Yes)
            {
                if (param.Equals("MarketDataProvider"))
                {
                    // Remove SelectedMarketDataProvider
                    MarketDataProviders.Remove(SelectedMarketDataProvider);

                    // Select 1st provider from collection if not empty
                    if (MarketDataProviders.Count > 0) 
                        SelectedMarketDataProvider = MarketDataProviders[0];
                }
                else if (param.Equals("OrderExecutionProvider"))
                {
                    // Remove SelectedOrderExecutionProvider
                    OrderExecutionProviders.Remove(SelectedOrderExecutionProvider);

                    // Select 1st provider from collection if not empty
                    if (OrderExecutionProviders.Count > 0)
                        SelectedOrderExecutionProvider = OrderExecutionProviders[0];
                }
            }
        }

        private bool RemoveProviderCanExecute()
        {
            if (SelectedMarketDataProvider != null || SelectedOrderExecutionProvider != null)
                return true;

            return false;
        }

        /// <summary>
        /// Called when 'Connect' button command is triggerd
        /// </summary>
        /// <param name="param"></param>
        private void ConnectProviderExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                ////NOTE: Test code to simulate Provider Connect
                //// BEGIN:
                //SelectedMarketDataProvider.ConnectionStatus = ConnectionStatus.Connected;
                //return;
                //// :END

                // Rasie event to request connection
                EventSystem.Publish<MarketDataProvider>(SelectedMarketDataProvider);
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                ////NOTE: Test code to simulate Provider Connect
                //// BEGIN:
                //SelectedOrderExecutionProvider.ConnectionStatus = ConnectionStatus.Connected;
                //return;
                //// :END

                // Rasie event to request connection
                EventSystem.Publish<OrderExecutionProvider>(SelectedOrderExecutionProvider);
            }
        }

        private bool ConnectProviderCanExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                if (SelectedMarketDataProvider != null && SelectedMarketDataProvider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
                {
                    return true;
                }
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                if (SelectedOrderExecutionProvider != null && SelectedOrderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Disconnected))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Called when 'Disconnect' button command is triggered
        /// </summary>
        /// <param name="param"></param>
        private void DisconnectProviderExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                ////NOTE: Test code to simulate Provider Dis-Connect
                //// BEGIN:
                //SelectedMarketDataProvider.ConnectionStatus = ConnectionStatus.Disconnected;
                //return;
                //// :END

                // Rasie event to request connection
                EventSystem.Publish<MarketDataProvider>(SelectedMarketDataProvider);
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                ////NOTE: Test code to simulate Provider Dis-Connect
                //// BEGIN:
                //SelectedOrderExecutionProvider.ConnectionStatus = ConnectionStatus.Disconnected;
                //return;
                //// :END

                // Rasie event to request connection
                EventSystem.Publish<OrderExecutionProvider>(SelectedOrderExecutionProvider);
            }
        }

        private bool DisconnectProviderCanExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                if (SelectedMarketDataProvider != null && SelectedMarketDataProvider.ConnectionStatus.Equals(ConnectionStatus.Connected))
                {
                    return true;
                }
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                if (SelectedOrderExecutionProvider != null && SelectedOrderExecutionProvider.ConnectionStatus.Equals(ConnectionStatus.Connected))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Called when 'Save Parameters' button is clicked
        /// </summary>
        /// <param name="param"></param>
        private void SaveParametersExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                if (_providersController.EditProviderCredentials(SelectedMarketDataProvider))
                    DataProviderInfoMessage = "Parameters Saved";
                else
                    DataProviderInfoMessage = "Parameters Not Saved";
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                if (_providersController.EditProviderCredentials(SelectedOrderExecutionProvider))
                    ExecutionProviderInfoMessage = "Parameters Saved";
                else
                    ExecutionProviderInfoMessage = "Parameters Not Saved";
            }
        }

        private bool SaveParametersCanExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                if (SelectedMarketDataProvider != null && SelectedMarketDataProvider.ProviderCredentials.Count>0)
                    return true;
                return false;
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                if (SelectedOrderExecutionProvider != null && SelectedOrderExecutionProvider.ProviderCredentials.Count > 0)
                    return true;
                return false;
            }

            return false;
        }

        /// <summary>
        /// Called when 'Save' button is clicked for adding new Provider
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private void SaveNewProviderExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                // Add Provider
                AddMarketDataProvider(_newMarketDataProviderPath, NewDataProviderName);

                // Get updated Data Providers list
                InitializeMarketDataProviders();

                NewDataProviderParametersRequired = false;
                DataProviderParametersToBeDisplayed = true;
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                // Add Provider
                AddOrderExecutionProvider(_newOrderExecutionProviderPath, NewExecutionProviderName);

                // Get updated Execution Providers list
                InitializeOrderExecutionProviders();

                NewExecutionProviderParametersRequired = false;
                ExecutionProviderParametersToBeDisplayed = true;
            }
        }

        private bool SaveNewProviderCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Called when 'Cancel' button is clicked for cancelling adding of new Provider
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private void CancelCommandExecute(object param)
        {
            if (param.Equals("MarketDataProvider"))
            {
                // Select initially 1st provider in ComboBox
                if (_marketDataProviders != null && _marketDataProviders.Count > 0)
                    SelectedMarketDataProvider = _marketDataProviders[0];

                NewDataProviderParametersRequired = false;
                DataProviderParametersToBeDisplayed = true;
            }
            else if (param.Equals("OrderExecutionProvider"))
            {
                if (_orderExecutionProviders != null && _orderExecutionProviders.Count > 0)
                    SelectedOrderExecutionProvider = _orderExecutionProviders[0];

                NewExecutionProviderParametersRequired = false;
                ExecutionProviderParametersToBeDisplayed = true;
            }
        }

        private bool CancelCommandCanExecute()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Initialization of market data providers
        /// </summary>
        private async void InitializeMarketDataProviders()
        {
            MarketDataProviders.Clear();

            // Request Controller for infomation
            var availableProviders = await Task.Run(() => _providersController.GetAvailableMarketDataProviders());

            // Safety check incase information was not populated
            if (availableProviders == null)
                return;

            // Populate Individual Market Data Provider details to show on UI
            foreach (var provider in availableProviders)
            {
                // Add to Collection
                MarketDataProviders.Add(provider);
            }

            // Select initially 1st provider in ComboBox
            if (_marketDataProviders != null && _marketDataProviders.Count > 0)
                SelectedMarketDataProvider = _marketDataProviders[0];
        }

        /// <summary>
        /// Initialization of order execution providers
        /// </summary>
        private async void InitializeOrderExecutionProviders()
        {
            OrderExecutionProviders.Clear();

            // Request Controller for infomation
            var availableProviders = await Task.Run(() => _providersController.GetAvailableOrderExecutionProviders());

            // Safety check incase information was not populated
            if (availableProviders == null)
                return;

            // Populate Individual Order Execution Provider details to show on UI
            foreach (var provider in availableProviders)
            {
                // Add to Collection
                OrderExecutionProviders.Add(provider);
            }

            // Select initially 1st provider in ComboBox
            if (_orderExecutionProviders != null && _orderExecutionProviders.Count > 0)
                SelectedOrderExecutionProvider = _orderExecutionProviders[0];
        }

        /// <summary>
        /// Adds new Market Data Provider to Market Data Engine - Server
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="providerName"></param>
        private void AddMarketDataProvider(string filePath, string providerName)
        {
            string serviceName = GetEnumDescription.GetValue(Services.MarketDataService);
            var service = new ServiceDetails(serviceName, ServiceStatus.Stopping);
            
            // Stop Market Data Service
            EventSystem.Publish<ServiceDetails>(service);

            var result = _providersController.AddMarketDataProvider(filePath, providerName);

            // Start Market Data Service
            service.Status= ServiceStatus.Starting;
            EventSystem.Publish<ServiceDetails>(service);

            // Show end result on UI
            DisplayInformationMessage(result, "Market Data Provider");
        }

        /// <summary>
        /// Adds new Order Execution Provider to Order Execution Engine - Server
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="providerName"></param>
        private void AddOrderExecutionProvider(string filePath, string providerName)
        {
            string serviceName = GetEnumDescription.GetValue(Services.OrderExecutionService);
            var service = new ServiceDetails(serviceName, ServiceStatus.Stopping);

            // Stop Order Execution Service
            EventSystem.Publish<ServiceDetails>(service);

            var result = _providersController.AddOrderExecutionProvider(filePath, providerName);

            // Start Order Execution Service
            service.Status = ServiceStatus.Starting;
            EventSystem.Publish<ServiceDetails>(service);

            // Show end result on UI
            DisplayInformationMessage(result, "Order Execution Provider");
        }

        /// <summary>
        /// Removes given Market Data Provider from Market Data Engine - Server
        /// </summary>
        /// <param name="providerName"></param>
        private void RemoveMarketDataProvider(string providerName)
        {
            
        }

        /// <summary>
        /// Shows information messages on UI related to Market Data Providers
        /// </summary>
        /// <param name="information"></param>
        /// <param name="messageTitle"></param>
        private void DisplayInformationMessage(Tuple<bool, string> information, string messageTitle)
        {
            // Display Error message
            WPFMessageBox.Show(MainWindow, information.Item2, messageTitle,
                MessageBoxButton.OK, (information.Item1) ? MessageBoxImage.Information : MessageBoxImage.Error);
        }
    }
}
