using System;
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
