using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Spring.Context.Support;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.Dashboard.Tests.Integration
{
    [TestFixture]
    public class OrderExecutionControllerTests
    {
        private OrderExecutionController _orderExecutionController;

        [SetUp]
        public void StartUp()
        {
            _orderExecutionController = ContextRegistry.GetContext()["OrderExecutionController"] as OrderExecutionController;
        }

        [TearDown]
        public void TearDown()
        {
            _orderExecutionController.Stop();
        }

        [Test]
        [Category("Integration")]
        public void RequestNewConnection_SendRequestToServer_ReceiveLogon()
        {
            //Thread.Sleep(9000);
            Provider provider = new Provider();
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = OrderExecutionProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<Provider>(provider);

            //Thread.Sleep(9000);

            //Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));
        }
    }
}
