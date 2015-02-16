using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Models
{
    public class ProviderCredential : INotifyPropertyChanged
    {
        #region Fields

        private string _credentialName;
        private string _credentialValue;

        #endregion

        #region Constructors

        public ProviderCredential()
        {

        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Holds credential name, for example 'Username', 'Password' etc.
        /// </summary>
        public string CredentialName
        {
            get { return _credentialName; }
            set
            {
                if (_credentialName != value)
                {
                    _credentialName = value;
                    OnPropertyChanged("CredentialName");
                }
            }
        }

        /// <summary>
        /// Holds credential value
        /// </summary>
        public string CredentialValue
        {
            get { return _credentialValue; }
            set
            {
                if (_credentialValue != value)
                {
                    _credentialValue = value;
                    OnPropertyChanged("CredentialValue");
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
