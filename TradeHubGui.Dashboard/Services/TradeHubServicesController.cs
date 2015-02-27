using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void StartService(ServiceDetails serviceDetails)
        {
            if (serviceDetails.Status.Equals(ServiceStatus.Stopped))
            {
                _servicesManager.StartService(serviceDetails);
            }
        }

        /// <summary>
        /// Stop given service
        /// </summary>
        /// <param name="serviceDetails">Contains service information</param>
        public void StopService(ServiceDetails serviceDetails)
        {
            if (serviceDetails.Status.Equals(ServiceStatus.Running))
            {
                _servicesManager.StopService(serviceDetails);
            }
        }
    }
}
