using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TradeHubGui.Common.Models;
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
        }
    }
}
