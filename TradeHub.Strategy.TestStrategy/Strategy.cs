using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.CustomAttributes;
using TradeHub.StrategyEngine.TradeHub;

namespace TradeHub.Strategy.TestStrategy
{
    [TradeHubAttributes("Strategy", typeof(Strategy))]
    public class Strategy:TradeHubStrategy
    {
        public Strategy(uint shares):base("","","")
        {
            
        }
    }
}
