using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace TradeHubGui.Common.Constants
{
    public enum Services
    {
        [Description("TradeHub MarketDataEngine Service")] 
        MarketDataService = 0,

        [Description("TradeHub OrderExecutionEngine Service")] 
        OrderExecutionService = 1,

        [Description("TradeHub Trade Manager Service")] 
        TradeService = 2,

        [Description("TradeHub PositionEngine Service")] 
        PositionService = 2
    }
}
