using System;
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
