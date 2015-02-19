using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Constants
{
    /// <summary>
    /// Contains possible status's for the Application Services
    /// </summary>
    public enum ServiceStatus
    {
        Disabled,
        Starting,
        Running,
        Stopping,
        Stopped
    }
}
