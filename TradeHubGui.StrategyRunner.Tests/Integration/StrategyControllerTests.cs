using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TradeHub.Common.Core.Constants;
using TradeHub.StrategyEngine.Utlility.Services;
using TradeHubGui.Common.Models;
using TradeHubGui.StrategyRunner.Representations;
using TradeHubGui.StrategyRunner.Services;

namespace TradeHubGui.StrategyRunner.Tests.Integration
{
    [TestFixture]
    public class StrategyControllerTests
    {
        [Test]
        [Category("Integration")]
        public void Test()
        {
            string assemblyPath =
                Path.GetFullPath(@"~\..\..\..\..\Lib\testing\TradeHub.StrategyEngine.Testing.SimpleStrategy.dll");
            Assert.True(File.Exists(assemblyPath));
            var classtype = StrategyHelper.GetStrategyClassType(assemblyPath);
            var parametersDetails = StrategyHelper.GetParameterDetails(classtype);
            object[] paramters =
            {
                (int)10, (int)15, (string)"LAST", (string)"ERX", (decimal)1000, BarFormat.TIME, BarPriceType.LAST,
                MarketDataProvider.SimulatedExchange, OrderExecutionProvider.SimulatedExchange
            };
            var instance = StrategyHelper.CreateStrategyInstance(classtype, paramters);
            StrategyInstance strategyInstance = new StrategyInstance();
            strategyInstance.InstanceKey = "A00";
            strategyInstance.Parameters = paramters;
            strategyInstance.StrategyType = classtype;
            strategyInstance.Symbol = "ERX";
            StrategyController controller = new StrategyController();
            controller.StrategyStatusChanged += delegate(StrategyStatusRepresentation representation)
            {

            };
            controller.AddStrategyInstance(strategyInstance);
            controller.RunStrategy(strategyInstance.InstanceKey);
            ManualResetEvent resetEvent=new ManualResetEvent(false);
            resetEvent.WaitOne(20000);
        }
    }
}

