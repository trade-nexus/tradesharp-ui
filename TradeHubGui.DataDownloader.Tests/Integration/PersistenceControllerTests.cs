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
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Spring.Context.Support;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.Dashboard.Services;
using TradeHubGui.DataDownloader.Service;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;
using TradeHubConstants = TradeHub.Common.Core.Constants;

namespace TradeHubGui.DataDownloader.Tests.Integration
{
    [TestFixture]
    public class PersistenceControllerTests
    {
        private MarketDataController _marketDataController;
        private DataPersistenceController _persistenceController;

        [SetUp]
        public void SetUp()
        {
            _marketDataController = ContextRegistry.GetContext()["MarketDataController"] as MarketDataController;
            _persistenceController = new DataPersistenceController();
            _persistenceController.SavePersistInformation(false,true);
        }

        [TearDown]
        public void TearDown()
        {
            _marketDataController.Stop();
        }

        [Test]
        public void PersistTickDataTest()
        {
            Thread.Sleep(10000);
            MarketDataProvider provider = new MarketDataProvider();
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.MarketDataProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<MarketDataProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            // Create Security to use the Symbol information
            Security security = new Security() { Symbol = "TEST" };

            // Create a new subscription request for requesting market data
            var subscriptionRequest = new SubscriptionRequest(security, provider, MarketDataType.Tick,
                SubscriptionType.Subscribe);

            // Create Tick details to hold market data information
            MarketDataDetail dataDetail = new MarketDataDetail(security);

            dataDetail.PersistenceInformation.SaveTrades = true;

            // Add TickDetails object to 
            provider.AddMarketDetail(dataDetail);

            EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);

            Thread.Sleep(20000);
        }
    }
}
