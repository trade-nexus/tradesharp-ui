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

        private string credName;
        private string credValue;

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
        public string CredName
        {
            get { return credName; }
            set
            {
                if (credName != value)
                {
                    credName = value;
                    OnPropertyChanged("CredName");
                }
            }
        }

        /// <summary>
        /// Holds credential value
        /// </summary>
        public string CredValue
        {
            get { return credValue; }
            set
            {
                if (credValue != value)
                {
                    credValue = value;
                    OnPropertyChanged("CrValue");
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
