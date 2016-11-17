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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.StrategyRunner.Representations;
using TradeHubGui.StrategyRunner.Services;
using TradeHubConstats = TradeHub.Common.Core.Constants;

namespace TradeHubGui.StrategyRunner.Tests.Integration
{
    [TestFixture]
    public class StrategyControllerTests
    {
        [Test]
        [Category("Integration")]
        public void TestThatStrategyIsExecutingAndChangingItsStatus()
        {
            string assemblyPath =
                Path.GetFullPath(@"~\..\..\..\..\Lib\testing\TradeHub.StrategyEngine.Testing.SimpleStrategy.dll");
            Assert.True(File.Exists(assemblyPath));
            var classtype = StrategyHelper.GetStrategyClassType(assemblyPath);
            var parametersDetails = StrategyHelper.GetParameterDetails(classtype);

            Dictionary<string, ParameterDetail> parameters = new Dictionary<string, ParameterDetail>();

            parameters.Add("1", new ParameterDetail(typeof(int),10));
            parameters.Add("2", new ParameterDetail(typeof(int),15));
            parameters.Add("3", new ParameterDetail(typeof(string), "LAST"));
            parameters.Add("4", new ParameterDetail(typeof(string), "ERX"));
            parameters.Add("5", new ParameterDetail(typeof(decimal), 1000));
            parameters.Add("6", new ParameterDetail(typeof(string), BarFormat.TIME));
            parameters.Add("7", new ParameterDetail(typeof(string), BarPriceType.LAST));
            parameters.Add("8", new ParameterDetail(typeof(string), TradeHubConstats.MarketDataProvider.SimulatedExchange));
            parameters.Add("9", new ParameterDetail(typeof(string), TradeHubConstats.OrderExecutionProvider.SimulatedExchange));

            object[] paramters =
            {
                (int)10, (int)15, (string)"LAST", (string)"ERX", (decimal)1000, BarFormat.TIME, BarPriceType.LAST,
                TradeHubConstats.MarketDataProvider.SimulatedExchange, TradeHubConstats.OrderExecutionProvider.SimulatedExchange
            };

            var instance = StrategyHelper.CreateStrategyInstance(classtype, paramters);
            StrategyInstance strategyInstance = new StrategyInstance();
            strategyInstance.InstanceKey = "A00";
            strategyInstance.Parameters = parameters;
            strategyInstance.StrategyType = classtype;
            strategyInstance.Symbol = "ERX";
            StrategyController controller = new StrategyController();
            StrategyStatusRepresentation statusRepresentationrepresentation = null;
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            controller.StrategyStatusChanged += delegate(StrategyStatusRepresentation representation)
            {
                statusRepresentationrepresentation = representation;
                resetEvent.Set();
            };
            controller.AddStrategyInstance(strategyInstance);
            resetEvent.WaitOne(5000);
            controller.RunStrategy(strategyInstance.InstanceKey);
            resetEvent.WaitOne(2000);
            Assert.NotNull(statusRepresentationrepresentation);
            Assert.AreEqual(StrategyStatus.Executing,statusRepresentationrepresentation.StrategyStatus);
        }
    }
}

