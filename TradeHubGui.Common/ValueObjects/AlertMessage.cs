using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.ValueObjects
{
    public class AlertMessage
    {
        /// <summary>
        /// Title/Heading for alert message
        /// </summary>
        private string _title;

        /// <summary>
        /// Message summary to display
        /// </summary>
        private string _summary;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="title">Title/Heading for alert message</param>
        /// <param name="summary">Message summary to display</param>
        public AlertMessage(string title, string summary)
        {
            _title = title;
            _summary = summary;
        }

        /// <summary>
        /// Title/Heading for alert message
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Message summary to display
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }
    }
}
