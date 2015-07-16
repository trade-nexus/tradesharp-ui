using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHubGui.Common.ValueObjects;

namespace TradeHubGui.ViewModel
{
    public class AlertWindowViewModel : BaseViewModel
    {
        private AlertMessage _alertMessage;

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="alertMessage">Contains alert details</param>
        public AlertWindowViewModel(AlertMessage alertMessage)
        {
            _alertMessage = alertMessage;
        }

        /// <summary>
        /// Contains alert details
        /// </summary>
        public AlertMessage AlertMessage
        {
            get { return _alertMessage; }
        }
    }
}
