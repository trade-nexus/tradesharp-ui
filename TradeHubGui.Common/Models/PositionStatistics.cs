using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.DomainModels;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Basic Position information for a particular Security (Symbol)
    /// </summary>
    public class PositionStatistics : INotifyPropertyChanged
    {
        /// <summary>
        /// Contains Symbol information
        /// </summary>
        private Security _security;

        /// <summary>
        /// Position on given Security
        /// </summary>
        private int _position;

        /// <summary>
        /// PnL on given Security
        /// </summary>
        private decimal _pnl;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="security">Contains Symbol Information</param>
        public PositionStatistics(Security security)
        {
            _security = security;
            _position = default(int);
            _pnl = default(decimal);
        }

        #region Properties

        /// <summary>
        /// Contains Symbol information
        /// </summary>
        public Security Security
        {
            get { return _security; }
            set { _security = value; }
        }

        /// <summary>
        /// Position on given Security
        /// </summary>
        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        /// <summary>
        /// PnL on given Security
        /// </summary>
        public decimal Pnl
        {
            get { return _pnl; }
            set
            {
                _pnl = value;
                OnPropertyChanged("Pnl");
            }
        }

        #endregion

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
