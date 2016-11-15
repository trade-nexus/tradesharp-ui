using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.IqFeedExtenstion
{
    public class IqFeedConnector
    {
        private ConnectionForm _connectionForm;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public IqFeedConnector()
        {
            _connectionForm = new ConnectionForm();
        }

        public bool OpenConnectionForm(string loginId, string password, string productId, string productVersion)
        {
            return _connectionForm.Connect(loginId, password, productId, productVersion);
        }

        public void CloseConnectionForm()
        {
            if (_connectionForm.Connected)
            {
                _connectionForm.Stop();
            }
        }
    }
}
