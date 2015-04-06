using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TraceSourceLogger;
using TradeHubGui.Common.Models;

namespace TradeHubGui.Dashboard.Managers
{
    /// <summary>
    /// Handles Order Execution Provider's related Admin functionality
    /// </summary>
    internal class OrderExecutionProvidersManager
    {
        private Type _type = typeof(OrderExecutionProvidersManager);

        /// <summary>
        /// Directory path at which Order Execution Provider's files are located
        /// </summary>
        private readonly string _orderExecutionProvidersFolderPath;

        /// <summary>
        /// File name which holds the name of all available order execution providers
        /// </summary>
        private readonly string _orderExecutionProvidersFileName;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public OrderExecutionProvidersManager()
        {
            //_orderExecutionProvidersFolderPath =
            //    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            //    "\\TradeHub\\OrderExecutionProviders\\";

            _orderExecutionProvidersFolderPath = Path.GetFullPath(@"~\..\..\Order Execution Engine\\Config\");

            _orderExecutionProvidersFileName = "AvailableOEProviders.xml";
        }

        /// <summary>
        /// Returns a list of available market data providers
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, List<ProviderCredential>> GetAvailableProviders()
        {
            // File Saftey Check
            if (!File.Exists(_orderExecutionProvidersFolderPath + _orderExecutionProvidersFileName)) 
                return null;

            // Will hold credential information against each availale provider
            IDictionary<string, List<ProviderCredential>> availableProviders = new Dictionary<string, List<ProviderCredential>>();

            // XML Document to read file
            var availableProvidersDocument = new XmlDocument();

            // Read file to get available provider's names.
            availableProvidersDocument.Load(_orderExecutionProvidersFolderPath + _orderExecutionProvidersFileName);

            // Read the all Node value
            XmlNodeList providersInfo = availableProvidersDocument.SelectNodes(xpath: "Providers/*");

            if (providersInfo != null)
            {
                // Extract individual attribute value
                foreach (XmlNode node in providersInfo)
                {
                    // Create file name from which to read Provider Credentials
                    string credentialsFileName = node.Name + @"OrderParams.xml";

                    // XML Document to read provider specific xml file
                    var availableCredentialsDoc = new XmlDocument();

                    // Holds extracted credentials from the xml file
                    var providerCredentialList = new List<ProviderCredential>();

                    if (File.Exists(_orderExecutionProvidersFolderPath + credentialsFileName))
                    {
                        // Read configuration file
                        availableCredentialsDoc.Load(_orderExecutionProvidersFolderPath + credentialsFileName);

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

        /// <summary>
        /// Edits given Order Execution provider credentails with the new values
        /// </summary>
        /// <param name="provider">Contains provider details</param>
        public void EditProviderCredentials(Provider provider)
        {
            try
            {
                // Create file path
                string filePath = _orderExecutionProvidersFolderPath + provider.ProviderName + @"OrderParams.xml";

                // Create XML Document Object to read credentials file
                XmlDocument document = new XmlDocument();

                // Load credentials file
                document.Load(filePath);

                XmlNode root = document.DocumentElement;

                // Travers all credential values
                foreach (ProviderCredential providerCredential in provider.ProviderCredentials)
                {
                    XmlNode xmlNode = root.SelectSingleNode("descendant::" + providerCredential.CredentialName);

                    if (xmlNode != null)
                    {
                        xmlNode.InnerText = providerCredential.CredentialValue;
                    }
                }

                // Save document
                document.Save(filePath);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "EditProviderCredentials");
            }
        }
    }
}
