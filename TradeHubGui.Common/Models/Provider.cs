using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Generic Provider class used for Market Data Provider or Order Execution Provider
    /// </summary>
    public class Provider : INotifyPropertyChanged
    {
        #region Fields

        private string providerName;
        private string connectionStatus;
        private List<ProviderCredential> providerCredentials;

        #endregion

        #region Constructors

        public Provider()
        {
            providerCredentials = new List<ProviderCredential>(10);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Provider name
        /// </summary>
        public string ProviderName
        {
            get { return providerName; }
            set
            {
                if (providerName != value)
                {
                    providerName = value;
                    OnPropertyChanged("ProviderName");
                }
            }
        }

        /// <summary>
        /// Provider connection status
        /// </summary>
        public string ConnectionStatus
        {
            get { return connectionStatus; }
            set
            {
                if (connectionStatus != value)
                {
                    connectionStatus = value;
                    OnPropertyChanged("ConnectionStatus");
                }
            }
        }

        /// <summary>
        /// List of credentials for provider (i.e. Username, Password, IpAddress etc.)
        /// </summary>
        public List<ProviderCredential> ProviderCredentials
        {
            get { return providerCredentials; }
            set
            {
                if (providerCredentials != value)
                {
                    providerCredentials = value;
                    OnPropertyChanged("Credentials");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

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
