using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TraceSourceLogger;

namespace TradeHubGui.Common.Infrastructure
{
    /// <summary>
    /// Provides functionality to Read/Modify given XML files
    /// </summary>
    public static class XmlFileManager
    {
        private static Type _type = typeof(XmlFileManager);

        /// <summary>
        /// Returns required values from the given file
        /// </summary>
        /// <param name="path">.csv File Path</param>
        /// <returns></returns>
        public static Tuple<string, string, string> GetHistoricalParameters(string path)
        {
            try
            {
                string startDate = "";
                string endDate = "";
                string providerName = "";

                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNode root = doc.DocumentElement;

                if (root != null)
                {
                    // Read Start Date value
                    XmlNode startNode = root.SelectSingleNode("descendant::StartDate");

                    if (startNode != null)
                    {
                        startDate = startNode.InnerText;
                    }

                    // Read End Date value
                    XmlNode endNode = root.SelectSingleNode("descendant::EndDate");

                    if (endNode != null)
                    {
                        endDate = endNode.InnerText;
                    }

                    // Read Provider Name
                    XmlNode providerNode = root.SelectSingleNode("descendant::Provider");

                    if (providerNode != null)
                    {
                        providerName = providerNode.InnerText;
                    }

                    // Return values
                    return new Tuple<string, string, string>(startDate, endDate, providerName);
                }

                return null;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "GetHistoricalParameters");
                return null;
            }
        }

        /// <summary>
        /// Saves values in the given file
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="providerName">Name of the Provider</param>
        /// <param name="path">.csv File Path</param>
        public static void SaveHistoricalParameters(string startDate, string endDate, string providerName, string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNode root = doc.DocumentElement;

                if (root != null)
                {
                    // Save Start Date
                    XmlNode startNode = root.SelectSingleNode("descendant::StartDate");
                    if (startNode != null)
                    {
                        startNode.InnerText = startDate;
                    }

                    // Save End Date
                    XmlNode endNode = root.SelectSingleNode("descendant::EndDate");
                    if (endNode != null)
                    {
                        endNode.InnerText = endDate;
                    }

                    // Save Provider Name
                    XmlNode providerNode = root.SelectSingleNode("descendant::Provider");
                    if (providerNode != null)
                    {
                        providerNode.InnerText = providerName;
                    }

                    doc.Save(path);
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "SaveHistoricalParameters");
            }
        }
    }
}
