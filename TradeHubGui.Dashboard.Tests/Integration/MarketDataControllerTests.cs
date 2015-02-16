using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Spring.Context.Support;
using TradeHub.Common.Core.Constants;
using TradeHub.StrategyEngine.MarketData;
using TradeHubGui.Common;
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
             var service = ContextRegistry.GetContext()["MarketDataService"] as MarketDataService;

            _marketDataController= new MarketDataController(service);
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        [Category("Integration")]
        public void RequestNewConnection_SendRequestToServer_ReceiveLogon()
        {
            Provider provider = new Provider();
            provider.ConnectionStatus= ConnectionStatus.Disconnected;
            provider.ProviderName = MarketDataProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<Provider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));
        }
    }
}
