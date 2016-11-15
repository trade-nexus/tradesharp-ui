using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Models
{
    public class StrategyStatistics : INotifyPropertyChanged
    {
        /// <summary>
        /// Indentifies strategy instance
        /// </summary>
        private string _instanceId;

        /// <summary>
        /// Time of generation
        /// </summary>
        private DateTime _time;

        /// <summary>
        /// Inforamtion to be displayed
        /// </summary>
        private string _information;

        public StrategyStatistics(string instanceId, DateTime time, string information)
        {
            _instanceId = instanceId;
            _time = time;
            _information = information;
        }

        #region Properties

        /// <summary>
        /// Indentifies strategy instance
        /// </summary>
        public string InstanceId
        {
            get { return _instanceId; }
            set { _instanceId = value; }
        }

        /// <summary>
        /// Time of generation
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// Inforamtion to be displayed
        /// </summary>
        public string Information
        {
            get { return _information; }
            set { _information = value; }
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
