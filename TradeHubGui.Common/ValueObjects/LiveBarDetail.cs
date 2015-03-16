using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;

namespace TradeHubGui.Common.ValueObjects
{
    /// <summary>
    /// Contains complete bar information
    /// </summary>
    public class LiveBarDetail
    {
        /// <summary>
        /// Bar format e.g. TIME
        /// </summary>
        private string _format;

        /// <summary>
        /// Bar price type e.g. ASK, LAST, BID
        /// </summary>
        private string _priceType;

        /// <summary>
        /// Pip size to be used in creating Bar entries
        /// </summary>
        private decimal _pipSize;

        /// <summary>
        /// Bar length e.g. 60 Seconds
        /// </summary>
        private decimal _barLength;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LiveBarDetail()
        {
            _barLength = 60;
            _pipSize = 0.0001M;
            _format = BarFormat.TIME;
            _priceType = BarPriceType.LAST;
        }

        #region Properties

        /// <summary>
        /// Bar format e.g. TIME
        /// </summary>
        public string Format
        {
            get { return _format; }
            set
            {
                _format = value; 
            }
        }

        /// <summary>
        /// Bar price type e.g. ASK, LAST, BID
        /// </summary>
        public string PriceType
        {
            get { return _priceType; }
            set
            {
                _priceType = value; 
            }
        }

        /// <summary>
        /// Pip size to be used in creating Bar entries
        /// </summary>
        public decimal PipSize
        {
            get { return _pipSize; }
            set
            {
                _pipSize = value; 
            }
        }

        /// <summary>
        /// Bar length e.g. 60 Seconds
        /// </summary>
        public decimal BarLength
        {
            get { return _barLength; }
            set
            {
                _barLength = value; 
            }
        }

        #endregion
    }
}
