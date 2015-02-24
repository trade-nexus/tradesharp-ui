using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Constants
{
    /// <summary>
    /// Possible type of requests user can make related to the orders
    /// </summary>
    public enum OrderRequestType
    {
        New,
        Cancel,
        Replace,
    }
}
