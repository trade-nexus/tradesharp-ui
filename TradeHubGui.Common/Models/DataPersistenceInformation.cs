using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains information regarding data to be persisted
    /// </summary>
    public class DataPersistenceInformation : INotifyPropertyChanged
    {
        /// <summary>
        /// Indicates if given market data Trades need to be persisted
        /// </summary>
        private bool _saveTrades = false;

        /// <summary>
        /// Indicates if given market data Quotes need to be persisted
        /// </summary>
        private bool _saveQuotes = false;

        /// <summary>
        /// Indicates if given market data Bars need to be persisted
        /// </summary>
        private bool _saveBars = false;

        public DataPersistenceInformation()
        {
            
        }

        #region Properties

        /// <summary>
        /// Indicates if given market data Trades need to be persisted
        /// </summary>
        public bool SaveTrades
        {
            get { return _saveTrades; }
            set
            {
                _saveTrades = value;
                OnPropertyChanged("SaveTrades");
            }
        }

        /// <summary>
        /// Indicates if given market data Quotes need to be persisted
        /// </summary>
        public bool SaveQuotes
        {
            get { return _saveQuotes; }
            set
            {
                _saveQuotes = value;
                OnPropertyChanged("SaveQuotes");
            }
        }

        /// <summary>
        /// Indicates if given market data Bars need to be persisted
        /// </summary>
        public bool SaveBars
        {
            get { return _saveBars; }
            set
            {
                _saveBars = value;
                OnPropertyChanged("SaveBars");
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
