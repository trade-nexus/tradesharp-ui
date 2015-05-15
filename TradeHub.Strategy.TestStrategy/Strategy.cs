using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.CustomAttributes;
using TradeHub.Common.Core.DomainModels;
using TradeHub.StrategyEngine.TradeHub;

namespace TradeHub.Strategy.TestStrategy
{
    [TradeHubAttributes("Strategy", typeof(Strategy))]
    public class Strategy:TradeHubStrategy
    {
        private readonly uint _u;
        private readonly decimal _decimal;
        private readonly double _d;
        private readonly float _f;
        private readonly int _i;
        private readonly long _l;
        private readonly string _s;

        public Strategy(uint @uint, decimal @decimal, double @double, float @float, int @int, long @long, string @string)
            : base("SimulatedExchange", "SimulatedExchange", "SimulatedExchange")
        {
            _u = @uint;
            _decimal = @decimal;
            _d = @double;
            _f = @float;
            _i = @int;
            _l = @long;
            _s = @string;
        }

        protected override void OnRun()
        {
            DisplayMessage("Strategy Started @: " + DateTime.Now);
        }

        protected override void OnStop()
        {
            DisplayMessage("Strategy Stopped @: " + DateTime.Now);
        }

        public override void OnMarketDataServiceLogonArrived(string marketDataProvider)
        {
            DisplayMessage("Market data logon arrived @: " + DateTime.Now);
        }

        public override void OnHistoricalDataServiceLogonArrived(string historicalDataProvider)
        {
            DisplayMessage("Historical data logon arrived @: " + DateTime.Now);
        }

        public override void OnOrderExecutionServiceLogonArrived(string orderExecutionProvider)
        {
            DisplayMessage("Order Execution logon arrived @: " + DateTime.Now);
        }
    }
}
