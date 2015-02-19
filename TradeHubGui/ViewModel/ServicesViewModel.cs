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

namespace TradeHubGui.ViewModel
{
    public class ServicesViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// Holds refernce of Selected Service Details from the UI
        /// </summary>
        private ServiceDetails _selectedService;

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

        /// <summary>
        /// Holds refernce of Selected Service Details from the UI
        /// </summary>
        public ServiceDetails SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value; 
                OnPropertyChanged("SelectedService");
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
                           new RelayCommand(param => StartServiceExecute(), param => StartServiceCanExecute()));
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
                           new RelayCommand(param => StopServiceExecute(), param => StopServiceCanExecute()));
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

            // Get Initial Services information
            PopulateServices();

            if (Services.Count > 0) SelectedService = Services[0];
        }

        #region Command Trigger Methods

        /// <summary>
        /// Used to Enable/Disable 'Start' button
        /// </summary>
        /// <returns></returns>
        private bool StartServiceCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Called when 'Start' button is clicked
        /// </summary>
        private void StartServiceExecute()
        {
            StartService();
        }

        /// <summary>
        /// Used to Enable/Disable 'Stop' button
        /// </summary>
        /// <returns></returns>
        private bool StopServiceCanExecute()
        {
            return true;
        }

        /// <summary>
        /// Called when 'Stop' button is clicked
        /// </summary>
        private void StopServiceExecute()
        {
            StopService();
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
            //NOTE: Test code to simulate Services
            // BEGIN:
            TestCodeToGenerateDummayServicesData();
            return;
            // :END
        }

        /// <summary>
        /// Called when user hits the 'Start' button
        /// </summary>
        private void StartService()
        {
            //NOTE: Test code to simulate Service Start
            // BEGIN:
            SelectedService.Status=ServiceStatus.Starting;
            SelectedService.Status=ServiceStatus.Running;
            return;
            // :END
        }

        /// <summary>
        /// Called when user hits the 'Stop' Button
        /// </summary>
        private void StopService()
        {
            //NOTE: Test code to simulate Service Start
            // BEGIN:
            SelectedService.Status = ServiceStatus.Stopping;
            SelectedService.Status = ServiceStatus.Stopped;
            return;
            // :END
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
