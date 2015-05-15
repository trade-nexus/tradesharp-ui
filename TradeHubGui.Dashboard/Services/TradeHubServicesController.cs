using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraceSourceLogger;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Managers;

namespace TradeHubGui.Dashboard.Services
{
    /// <summary>
    /// Handles TradeHub Application services related calls
    /// </summary>
    public class TradeHubServicesController
    {
        private Type _type = typeof (TradeHubServicesController);

        /// <summary>
        /// Handles all service related functionality
        /// </summary>
        private TradeHubServicesManager _servicesManager;

        /// <summary>
        /// Defautl Constructor
        /// </summary>
        public TradeHubServicesController()
        {
            // Initialize Manager
            _servicesManager = new TradeHubServicesManager();
        }

        /// <summary>
        /// Returns available services information
        /// </summary>
        /// <returns></returns>
        public List<ServiceDetails> GetAvailableServices()
        {
            return _servicesManager.ServiceDetailsCollection;
        }

        /// <summary>
        /// Initialize all available services
        /// </summary>
        public void InitializeServices()
        {
            _servicesManager.InitializeServices();
        }

        /// <summary>
        /// Starts given service
        /// </summary>
        /// <param name="serviceDetails">Contains service information</param>
        public async void StartService(ServiceDetails serviceDetails)
        {
            if (serviceDetails.Status.Equals(ServiceStatus.Stopped))
            {
                await Task.Run(() =>
                {
                    _servicesManager.StartService(serviceDetails);

                });
            }

            // Notify listeners if the service is running
            if (serviceDetails.Status == ServiceStatus.Running)
            {
                EventSystem.Publish<ServiceDetails>(serviceDetails);
            }
        }

        /// <summary>
        /// Stop given service
        /// </summary>
        /// <param name="serviceDetails">Contains service information</param>
        public async void StopService(ServiceDetails serviceDetails)
        {
            if (serviceDetails.Status.Equals(ServiceStatus.Running))
            {
                await Task.Run(() =>
                {
                    _servicesManager.StopService(serviceDetails);

                });
            }

            // Notify listeners if the service is running
            if (serviceDetails.Status == ServiceStatus.Stopped)
            {
                EventSystem.Publish<ServiceDetails>(serviceDetails);
            }
        }
    }
}
