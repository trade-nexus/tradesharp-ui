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
using TradeHub.StrategyEngine.MarketData;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.Dashboard.Tests.Integration
{
    [TestFixture]
    public class MarketDataControllerTests
    {
        private MarketDataController _marketDataController;

        [SetUp]
        public void StartUp()
        {
             //var service = ContextRegistry.GetContext()["MarketDataService"] as MarketDataService;
            _marketDataController = ContextRegistry.GetContext()["MarketDataController"] as MarketDataController;

            //_marketDataController = new MarketDataController(service);
        }

        [TearDown]
        public void TearDown()
        {
            _marketDataController.Stop();
        }

        [Test]
        [Category("Integration")]
        public void RequestNewConnection_SendRequestToServer_ReceiveLogon()
        {
            Thread.Sleep(5000);
            Provider provider = new Provider();
            provider.ProviderType= ProviderType.MarketData;
            provider.ConnectionStatus= ConnectionStatus.Disconnected;
            provider.ProviderName = MarketDataProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<Provider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));
        }

        [Test]
        [Category("Integration")]
        public void RequestMarketData_SendRequestToServer_ReceiveMarketData()
        {
            Thread.Sleep(5000);
            Provider provider = new Provider();
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = MarketDataProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<Provider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            // Create Security to use the Symbol information
            Security security = new Security(){Symbol = "AAPL"};
            
            // Create a new subscription request for requesting market data
            var subscriptionRequest = new SubscriptionRequest(security,provider,SubscriptionType.Subscribe);
            
            // Create Tick details to hold market data information
            TickDetail tickDetails = new TickDetail(security);

            // Add TickDetails object to 
            provider.TickDetailsMap.Add(security.Symbol, tickDetails);
            
            EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);

            Thread.Sleep(5000);

            Assert.IsTrue(tickDetails.AskPrice.Equals(1.24M));
        }
    }
}
