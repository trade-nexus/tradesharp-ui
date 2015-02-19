using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TradeHubGui.Common.Models;

namespace TradeHubGui.Dashboard.Managers
{
    /// <summary>
    /// Handles Market Data Provider's related Admin functionality
    /// </summary>
    public class MarketDataProvidersManager
    {
        private Type _type = typeof(MarketDataProvidersManager);

        private string _marketDataProvidersFolderPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TradeHub\\MarketDataProviders\\";

        private string _marketDataProvidersFileName = "AvailableProviders.xml";

        public MarketDataProvidersManager()
        {
            
        }

        /// <summary>
        /// Returns a list of available market data providers
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<ProviderCredential>> GetAvailableProviders()
        {
            Dictionary<string, List<ProviderCredential>> availableProviders = new Dictionary<string, List<ProviderCredential>>();

            var availableProvidersDoc = new XmlDocument();

            // Read configuration file
            availableProvidersDoc.Load(_marketDataProvidersFolderPath + _marketDataProvidersFileName);

            // Read the all Node value
            XmlNodeList providersInfo = availableProvidersDoc.SelectNodes(xpath: "Providers/*");

            if (providersInfo != null)
            {
                // Extract individual attribute value
                foreach (XmlNode node in providersInfo)
                {
                    string credentialsFileName = node.Name + @"Params.xml";
                    var availableCredentialsDoc = new XmlDocument();
                    List<ProviderCredential> providerCredentialList = new List<ProviderCredential>();

                    if (File.Exists(_marketDataProvidersFolderPath + credentialsFileName))
                    {
                        // Read configuration file
                        availableCredentialsDoc.Load(_marketDataProvidersFolderPath + credentialsFileName);

                        // Read all the parametes defined in the configuration file
                        XmlNodeList configNodes = availableCredentialsDoc.SelectNodes(xpath: node.Name + "/*");
                        if (configNodes != null)
                        {
                            // Extract individual attribute value
                            foreach (XmlNode innerNode in configNodes)
                            {
                                ProviderCredential providerCredential = new ProviderCredential();

                                providerCredential.CredentialName = innerNode.Name;
                                providerCredential.CredentialValue = innerNode.InnerText;

                                // Add to Credentials list
                                providerCredentialList.Add(providerCredential);
                            }
                        }
                    }
                    // Add all details to providers info map
                    availableProviders.Add(node.Name, providerCredentialList);
                }
            }

            return availableProviders;
        }
    }
}
