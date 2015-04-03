using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.ViewModel
{
    public class ServicesViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// Provides TradeHub services related functionality
        /// </summary>
        private TradeHubServicesController _servicesController;

        /// <summary>
        /// Contains Service Details for the available application services
        /// </summary>
        private ObservableCollection<ServiceDetails> _services;

        private RelayCommand _startServiceCommand;
        private RelayCommand _stopServiceCommand;
        private RelayCommand _enableDisableServiceCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Contains Service Details for the available application services
        /// </summary>
        public ObservableCollection<ServiceDetails> Services
        {
            get { return _services; }
            set
            {
                _services = value; 
                OnPropertyChanged("Services");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Used for 'Start' button for starting the application service
        /// </summary>
        public ICommand StartServiceCommand
        {
            get
            {
                return _startServiceCommand ??
                       (_startServiceCommand =
                           new RelayCommand(param => StartServiceExecute(param)));
            }
        }

        /// <summary>
        /// Used for 'Stop' button for stopping the application service
        /// </summary>
        public ICommand StopServiceCommand
        {
            get
            {
                return _stopServiceCommand ??
                       (_stopServiceCommand =
                           new RelayCommand(param => StopServiceExecute(param)));
            }
        }

        /// <summary>
        /// Used for 'Enable/Disable' CheckBox for managing application service lifecycle
        /// </summary>
        public ICommand EnableDisableServiceCommand
        {
            get
            {
                return _enableDisableServiceCommand ??
                       (_enableDisableServiceCommand =
                           new RelayCommand(param => EnableDisableServiceExecute(param), param => EnableDisableServiceCanExecute(param)));
            }
        }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ServicesViewModel()
        {
            _services = new ObservableCollection<ServiceDetails>();
            _servicesController = new TradeHubServicesController();

            // Get Initial Services information
            PopulateServices();
        }

        #region Command Trigger Methods

        /// <summary>
        /// Called when 'Start' button is clicked
        /// </summary>
        private void StartServiceExecute(object parameter)
        {
            string serviceName = (string) parameter;

            StartService(serviceName);
        }

        /// <summary>
        /// Called when 'Stop' button is clicked
        /// </summary>
        private void StopServiceExecute(object parameter)
        {
            string serviceName = (string)parameter;

            StopService(serviceName);
        }

        /// <summary>
        /// Used to Enable/Disable 'CheckBox (Enable/Disable Service)'
        /// </summary>
        /// <param name="parameter"></param>
        private bool EnableDisableServiceCanExecute(object parameter)
        {
            // TODO: Add Logic
            return false;
        }

        /// <summary>
        /// Called when 'CheckBox (Enable/Disable Service)' changes state
        /// </summary>
        /// <param name="parameter"></param>
        private void EnableDisableServiceExecute(object parameter)
        {
            // TODO: Add Logic
        }

        #endregion

        /// <summary>
        /// Populated initial services information to be displayed on UI
        /// </summary>
        private void PopulateServices()
        {
            ////NOTE: Test code to simulate Services
            //// BEGIN:
            //TestCodeToGenerateDummayServicesData();
            //return;
            //// :END

            var availableServices = _servicesController.GetAvailableServices();
            foreach (var availableService in availableServices)
            {
                Services.Add(availableService);
            }

            InitializeServices();
        }

        /// <summary>
        /// Called when user hits the 'Start' button
        /// </summary>
        /// <param name="serviceName"></param>
        private void StartService(string serviceName)
        {
            ServiceDetails serviceDetails = null;

            foreach (var service in _services)
            {
                if (service.ServiceName.Equals(serviceName))
                {
                    serviceDetails = service;
                    break;
                }
            }

            if (serviceDetails == null) return;

            ////NOTE: Test code to simulate Service Start
            //// BEGIN:
            //serviceDetails.Status = ServiceStatus.Starting;
            //serviceDetails.Status = ServiceStatus.Running;
            //return;
            //// :END

            _servicesController.StartService(serviceDetails);
        }

        /// <summary>
        /// Called when user hits the 'Stop' Button
        /// </summary>
        /// <param name="serviceName"></param>
        private void StopService(string serviceName)
        {
            ServiceDetails serviceDetails = null;

            foreach (var service in _services)
            {
                if (service.ServiceName.Equals(serviceName))
                {
                    serviceDetails = service;
                    break;
                }
            }

            if (serviceDetails == null) return;

            ////NOTE: Test code to simulate Service Start
            //// BEGIN:
            //serviceDetails.Status = ServiceStatus.Stopping;
            //serviceDetails.Status = ServiceStatus.Stopped;
            //return;
            //// :END

            _servicesController.StopService(serviceDetails);
        }

        /// <summary>
        /// Intialize Application Services
        /// </summary>
        private async void InitializeServices()
        {
            await Task.Run(() => _servicesController.InitializeServices());
        }

        /// <summary>
        /// Dummy services information
        /// </summary>
        private void TestCodeToGenerateDummayServicesData()
        {
            {
                ServiceDetails serviceDetails = new ServiceDetails("Market Data Service", ServiceStatus.Running);

                // Add to Observable collection to display on UI
                Services.Add(serviceDetails);
            }

            {
                ServiceDetails serviceDetails = new ServiceDetails("Order Execution Service", ServiceStatus.Running);

                // Add to Observable collection to display on UI
                Services.Add(serviceDetails);
            }

            {
                ServiceDetails serviceDetails = new ServiceDetails("Position Service", ServiceStatus.Stopped);

                // Add to Observable collection to display on UI
                Services.Add(serviceDetails);
            }

            {
                ServiceDetails serviceDetails = new ServiceDetails("Trade Service", ServiceStatus.Disabled);

                // Add to Observable collection to display on UI
                Services.Add(serviceDetails);
            }
        }
    }
}
