using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Constants
{
    /// <summary>
    /// Possible Order Types in the system
    /// </summary>
    public enum OrderType
    {
        Market,
        Limit,
        Stop,
        StopLimit
    }
}
