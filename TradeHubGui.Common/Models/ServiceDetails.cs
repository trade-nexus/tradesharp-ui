using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains necessary information for the application services
    /// </summary>
    public class ServiceDetails : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Indicates if the service is avaiable (Enabled) or not (Disabled)
        /// </summary>
        private bool _enabled;

        /// <summary>
        /// Application Service name
        /// </summary>
        private string _serviceName;

        /// <summary>
        /// Application Service name to be displayed on UI
        /// </summary>
        private string _serviceDisplayName;

        /// <summary>
        /// Current Status of the Service
        /// </summary>
        private ServiceStatus _status;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if the service is avaiable (Enabled) or not (Disabled)
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value; 
                OnPropertyChanged("Enabled");
            }
        }

        /// <summary>
        /// Application Service name
        /// </summary>
        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                _serviceName = value; 
                OnPropertyChanged("ServiceName");
            }
        }

        /// <summary>
        /// Current Status of the Service
        /// </summary>
        public ServiceStatus Status
        {
            get { return _status; }
            set
            {
                _status = value; 
                OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Application Service name to be displayed on UI
        /// </summary>
        public string ServiceDisplayName
        {
            get { return _serviceDisplayName; }
            set
            {
                _serviceDisplayName = value;
                OnPropertyChanged("ServiceDisplayName");
            }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="serviceName">Application Service name</param>
        /// <param name="serviceStatus">Current Status of the Service</param>
        public ServiceDetails(string serviceName, ServiceStatus serviceStatus)
        {
            _serviceName = serviceName;
            _status = serviceStatus;
            _serviceDisplayName = serviceName;
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
