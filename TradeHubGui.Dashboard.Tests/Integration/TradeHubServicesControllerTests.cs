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
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.Dashboard.Tests.Integration
{
    [TestFixture]
    public class TradeHubServicesControllerTests
    {
        private TradeHubServicesController _servicesController;

        [SetUp]
        public void SetUp()
        {
            Task.Run(()=>_servicesController = new TradeHubServicesController());
            Thread.Sleep(1000);
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        [Category("Integration")]
        public void StartMarketDataService_Success()
        {
            var availableService = _servicesController.GetAvailableServices();

            var marketService = availableService.FirstOrDefault(
                provider => provider.ServiceName.Equals("MarketDataService"));

            _servicesController.StartService(marketService);

            Thread.Sleep(61000);

            Assert.IsTrue(marketService.Status.Equals(Common.Constants.ServiceStatus.Running));
        }

        [Test]
        [Category("Integration")]
        public void StartOrderExecutionService_Fail()
        {
            var availableService = _servicesController.GetAvailableServices();

            var orderService = availableService.FirstOrDefault(
                provider => provider.ServiceName.Equals("OrderExecutionService"));

            _servicesController.StartService(orderService);

            Assert.IsTrue(!orderService.Status.Equals(Common.Constants.ServiceStatus.Running));
        }

        public string ServiceStatus(string serviceName)
        {
            var controller = new ServiceController(serviceName);
            return controller.Status.ToString();
        }
    }
}
