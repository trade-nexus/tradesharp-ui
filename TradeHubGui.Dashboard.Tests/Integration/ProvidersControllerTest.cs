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
using NUnit.Framework;
using TradeHubGui.Common.Infrastructure;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.Dashboard.Tests.Integration
{
    [TestFixture]
    public class ProvidersControllerTest
    {
        private ProvidersController _providersController;

        [SetUp]
        public void SetUp()
        {
            _providersController = new ProvidersController();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        [Category("Integration")]
        public void LoadMarketDataProviders()
        {
            // Request Controller for infomation
            Task<IList<MarketDataProvider>> availableProviders = _providersController.GetAvailableMarketDataProviders();

            Assert.IsTrue(availableProviders.Result.Count.Equals(4));
        }

        [Test]
        [Category("Integration")]
        public void EditMarketDataProviderCredentials()
        {
            // Request Controller for infomation
            Task<IList<MarketDataProvider>> availableProviders = _providersController.GetAvailableMarketDataProviders();

            Assert.IsTrue(availableProviders.Result.Count.Equals(4));

            Provider interactiveBroker =
                availableProviders.Result.FirstOrDefault(
                    provider => provider.ProviderName.Equals("InteractiveBrokers"));

            // Will hold orignal values
            List<ProviderCredential> orignalCredentials = new List<ProviderCredential>();

            // Set dummy values
            foreach (ProviderCredential credential in interactiveBroker.ProviderCredentials)
            {
                // Save Orignal Values
                ProviderCredential providerCredential = new ProviderCredential();
                providerCredential.CredentialName = credential.CredentialName;
                providerCredential.CredentialValue = credential.CredentialValue;
                orignalCredentials.Add(providerCredential);

                credential.CredentialValue = "TestValue";
            }

            // Update values
            _providersController.EditProviderCredentials(interactiveBroker);

            Provider tempInteractiveBroker =
                availableProviders.Result.FirstOrDefault(
                    provider => provider.ProviderName.Equals("InteractiveBrokers"));

            Assert.IsTrue(tempInteractiveBroker.ProviderCredentials[0].CredentialValue.Equals("TestValue"));

            // Set Orignal Values
            tempInteractiveBroker.ProviderCredentials = orignalCredentials;

            // Update values
            _providersController.EditProviderCredentials(tempInteractiveBroker);

        }

        [Test]
        [Category("Integration")]
        public void LoadOrderExecutionProviders()
        {
            // Request Controller for infomation
            Task<List<OrderExecutionProvider>> availableProviders = _providersController.GetAvailableOrderExecutionProviders();

            Assert.IsTrue(availableProviders.Result.Count.Equals(3));
        }

        [Test]
        [Category("Integration")]
        public void EditOrderExecutionProviderCredentials()
        {
            // Request Controller for infomation
            Task<List<OrderExecutionProvider>> availableProviders = _providersController.GetAvailableOrderExecutionProviders();

            Assert.IsTrue(availableProviders.Result.Count.Equals(3));

            Provider blackwood =
                availableProviders.Result.FirstOrDefault(
                    provider => provider.ProviderName.Equals("Blackwood"));

            // Will hold orignal values
            List<ProviderCredential> orignalCredentials = new List<ProviderCredential>();

            // Set dummy values
            foreach (ProviderCredential credential in blackwood.ProviderCredentials)
            {
                // Save Orignal Values
                ProviderCredential providerCredential = new ProviderCredential();
                providerCredential.CredentialName = credential.CredentialName;
                providerCredential.CredentialValue = credential.CredentialValue;
                orignalCredentials.Add(providerCredential);

                credential.CredentialValue = "TestValue";
            }

            // Update values
            _providersController.EditProviderCredentials(blackwood);

            Provider tempBlackwood =
                availableProviders.Result.FirstOrDefault(
                    provider => provider.ProviderName.Equals("Blackwood"));

            Assert.IsTrue(tempBlackwood.ProviderCredentials[0].CredentialValue.Equals("TestValue"));

            // Set Orignal Values
            tempBlackwood.ProviderCredentials = orignalCredentials;

            // Update values
            _providersController.EditProviderCredentials(tempBlackwood);

        }

        [Test]
        [Category("Integration")]
        public void ModifyApplicationConfigFile()
        {
            bool result = XmlFileManager.ModifyAppConfigForSpringObject("SampleConfigFile.config", @"~/Config/Sample");

            Assert.IsTrue(result);
        }
    }
}
