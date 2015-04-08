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
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="parentNodeName"></param>
        /// <param name="childNodeName"></param>
        /// <returns></returns>
        public static bool AddChildNode(string filePath, string parentNodeName, string childNodeName)
        {
            try
            {
                bool valueSaved = false;

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNode root = doc.DocumentElement;

                if (root != null)
                {
                    //Create a new node.
                    XmlElement resourceNodeElement = doc.CreateElement(childNodeName);

                    // Add child node
                    root.AppendChild(resourceNodeElement);

                    valueSaved = true;
                    doc.Save(filePath);
                }

                return valueSaved;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "AddChildNode");
                return false;
            }
        }

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

        /// <summary>
        /// Modifies given App.Config file to add a new Spring object 
        /// </summary>
        public static bool ModifyAppConfigForSpringObject(string appConfigPath, string springObject)
        {
            try
            {
                bool valueSaved = false;

                XmlDocument doc = new XmlDocument();
                doc.Load(appConfigPath);

                XmlNode root = doc.DocumentElement;

                if (root != null)
                {
                    // Get Context Node
                    XmlNode contextNode = root.SelectSingleNode("descendant::context");

                    if (contextNode != null)
                    {
                        // Create a new node content
                        XmlElement resourceNodeElement = doc.CreateElement("resource");

                        // Add newly created node
                        contextNode.InsertBefore(resourceNodeElement, contextNode.FirstChild);

                        // Set node attributes
                        XmlAttribute newAttribute = doc.CreateAttribute("uri");
                        newAttribute.Value = springObject;
                        resourceNodeElement.Attributes.Append(newAttribute);

                        valueSaved = true;
                    }

                    doc.Save(appConfigPath);
                }

                return valueSaved;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "ModifyAppConfigForSpringObject");
                return false;
            }
        }
    }
}
