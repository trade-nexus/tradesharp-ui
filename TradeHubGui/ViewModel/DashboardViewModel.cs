using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Spring.Context.Support;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private ObservableCollection<Instrument> _instruments;
        private RelayCommand _showDataApiConfigurationCommand;
        private RelayCommand _showOrderApiConfigurationCommand;
        private RelayCommand _showServicesConfigurationCommand;
        
        private ProvidersViewModel _providersViewModel;
        private ServicesViewModel _servicesViewModel;

        private MarketDataController _marketDataController;

        /// <summary>
        /// Constructors
        /// </summary>
        public DashboardViewModel()
        {
            _providersViewModel = new ProvidersViewModel();
            _servicesViewModel = new ServicesViewModel();

            _marketDataController = ContextRegistry.GetContext()["MarketDataController"] as MarketDataController;

            EventSystem.Subscribe<string>(OnApplicationClose);

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

        /// <summary>
        /// Collection of market data providers for displaying on Dashboard
        /// </summary>
        public ObservableCollection<Provider> MarketDataProviders
        {
            get { return _providersViewModel.MarketDataProviders; }
        }

        /// <summary>
        /// Collection of order execution providers for displaying on Dashboard
        /// </summary>
        public ObservableCollection<Provider> OrderExecutionProviders
        {
            get { return _providersViewModel.OrderExecutionProviders; }
        }

        /// <summary>
        /// Collection for Application Services to be displayed on Dashboard
        /// </summary>
        public ObservableCollection<ServiceDetails> Services
        {
            get { return _servicesViewModel.Services; }
        }

        #endregion

        #region Commands

        public ICommand ShowDataApiConfigurationCommand
        {
            get
            {
                return _showDataApiConfigurationCommand ?? (_showDataApiConfigurationCommand = new RelayCommand(param => ShowDataApiConfigurationExecute()));
            }
        }

        public ICommand ShowOrderApiConfigurationCommand
        {
            get
            {
                return _showOrderApiConfigurationCommand ?? (_showOrderApiConfigurationCommand = new RelayCommand(param => ShowOrderApiConfigurationExecute()));
            }
        }

        public ICommand ShowServicesConfigurationCommand
        {
            get
            {
                return _showServicesConfigurationCommand ?? (_showServicesConfigurationCommand = new RelayCommand(param => ShowServicesConfigurationExecute()));
            }
        }

        #endregion

        private void ShowDataApiConfigurationExecute()
        {
            if(ToggleFlyout(0))
            {
                (MainWindow.Flyouts.Items[0] as Flyout).DataContext = _providersViewModel;
            }
        }

        private void ShowOrderApiConfigurationExecute()
        {
            if (ToggleFlyout(1))
            {
                (MainWindow.Flyouts.Items[1] as Flyout).DataContext = _providersViewModel;
            }
        }

        private void ShowServicesConfigurationExecute()
        {
            if (ToggleFlyout(2))
            {
                (MainWindow.Flyouts.Items[2] as Flyout).DataContext = _servicesViewModel;
            }
        }

        private object GetMarketDataProviders()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when application is closing
        /// </summary>
        /// <param name="message"></param>
        private void OnApplicationClose(string message)
        {
            if (message.Equals("Close"))
            {
                _marketDataController.Stop();
            }
        }

    }
}
