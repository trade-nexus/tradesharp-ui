/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TraceSourceLogger;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.Utility;
using TradeHubGui.Common.ValueObjects;
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

            // Subscribe Events
            EventSystem.Subscribe<string>(OnApplicationClose);
            EventSystem.Subscribe<ServiceDetails>(ManageServiceRequest);
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
            var availableServices = _servicesController.GetAvailableServices();
            foreach (var availableService in availableServices)
            {
                Services.Add(availableService);

                // Test Code
                if (availableService.Status!=ServiceStatus.Disabled)
                {
                    availableService.Status= ServiceStatus.Stopped;
                }

                //NOTE: Test code to simulate Service Start
                // BEGIN:
                //availableService.Status = ServiceStatus.Starting;
                //availableService.Status = ServiceStatus.Running;
                // :END

                EventSystem.Publish<ServiceDetails>(availableService);
            }

            ////NOTE: To be disbaled for testing
            InitializeServices();
        }

        /// <summary>
        /// Sends request to Start the given service
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

            _servicesController.StartService(serviceDetails);
        }

        /// <summary>
        /// Sends request to stop the given service
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
        /// Called when application is closing
        /// </summary>
        /// <param name="message"></param>
        private void OnApplicationClose(string message)
        {
            if (message.Equals("Close"))
            {
                foreach (var service in _services)
                {
                    if (service.Status.Equals(ServiceStatus.Running))
                    {
                        _servicesController.StopService(service);
                    }
                }
            }
        }

        #region Handle Service Request

        /// <summary>
        /// Processes incoming service related request for appropriate action
        /// </summary>
        /// <param name="serviceRequestInfo"></param>
        private void ManageServiceRequest(ServiceDetails serviceRequestInfo)
        {
            ServiceDetails serviceDetails = null;

            // Find Actual Service object
            foreach (var service in _services)
            {
                if (service.ServiceName.Equals(serviceRequestInfo.ServiceName))
                {
                    serviceDetails = service;
                    break;
                }
            }

            if (serviceDetails == null) return;


            if (serviceDetails.Status.Equals(ServiceStatus.Starting))
            {
                _servicesController.StartService(serviceDetails);
            }
            else if (serviceDetails.Status.Equals(ServiceStatus.Stopping))
            {
                _servicesController.StopService(serviceDetails);
            }
        }

        #endregion
    }
}
