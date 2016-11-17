/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
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
