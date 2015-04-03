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
    /// Contains details for the Historical Bars
    /// </summary>
    public class HistoricalBarParameters
    {
        /// <summary>
        /// Type of Historical Bar e.g. Tick, Trade, Daily, Intra Day, etc.
        /// </summary>
        private string _type;

        /// <summary>
        /// Starting date from which to fetch the historical bar data
        /// </summary>
        private DateTime _startDate;

        /// <summary>
        /// End date for the range of historical bar data
        /// </summary>
        private DateTime _endDate;

        /// <summary>
        /// Bar interval for historical data
        /// </summary>
        private uint _interval;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public HistoricalBarParameters()
        {
            _interval = 60;
            _type = BarType.DAILY;
            _startDate = DateTime.UtcNow;
            _endDate = DateTime.UtcNow;
        }


        /// <summary>
        /// Type of Historical Bar e.g. Tick, Trade, Daily, Intra Day, etc.
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Starting date from which to fetch the historical bar data
        /// </summary>
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        /// <summary>
        /// End date for the range of historical bar data
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        /// <summary>
        /// Bar interval for historical data
        /// </summary>
        public uint Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }
    }
}
