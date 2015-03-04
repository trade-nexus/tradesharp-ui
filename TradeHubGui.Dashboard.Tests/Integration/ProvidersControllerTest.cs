using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
            Task<IList<Provider>> availableProviders = _providersController.GetAvailableMarketDataProviders();

            Assert.IsTrue(availableProviders.Result.Count.Equals(4));
        }

        [Test]
        [Category("Integration")]
        public void EditMarketDataProviderCredentials()
        {
            // Request Controller for infomation
            Task<IList<Provider>> availableProviders = _providersController.GetAvailableMarketDataProviders();

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

    }
}
