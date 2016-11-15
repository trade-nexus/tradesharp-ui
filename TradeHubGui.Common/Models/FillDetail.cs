using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains Order's Fill information
    /// </summary>
    public class FillDetail : INotifyPropertyChanged
    {
        /// <summary>
        /// Unique ID to identify Fill
        /// </summary>
        private string _fillId;

        /// <summary>
        /// Quantity filled in the current cycle
        /// </summary>
        private int _fillQuantity;

        /// <summary>
        /// Price for the filled quantity
        /// </summary>
        private decimal _fillPrice;

        /// <summary>
        /// Fill Time
        /// </summary>
        private DateTime _fillDatetime;

        /// <summary>
        /// Type of fill i.e. Fill, Partial
        /// </summary>
        private ExecutionType _fillType;

        #region Properties

        /// <summary>
        /// Unique ID to identify Fill
        /// </summary>
        public string FillId
        {
            get { return _fillId; }
            set
            {
                _fillId = value; 
                OnPropertyChanged("FillId");
            }
        }

        /// <summary>
        /// Quantity filled in the current cycle
        /// </summary>
        public int FillQuantity
        {
            get { return _fillQuantity; }
            set
            {
                _fillQuantity = value;
                OnPropertyChanged("FillQuantity");
            }
        }

        /// <summary>
        /// Price for the filled quantity
        /// </summary>
        public decimal FillPrice
        {
            get { return _fillPrice; }
            set
            {
                _fillPrice = value;
                OnPropertyChanged("FillPrice");
            }
        }

        /// <summary>
        /// Fill Time
        /// </summary>
        public DateTime FillDatetime
        {
            get { return _fillDatetime; }
            set
            {
                _fillDatetime = value; 
                OnPropertyChanged("FillDateTime");
            }
        }

        /// <summary>
        /// Type of fill i.e. Fill, Partial
        /// </summary>
        public ExecutionType FillType
        {
            get { return _fillType; }
            set
            {
                _fillType = value; 
                OnPropertyChanged("FillType");
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
