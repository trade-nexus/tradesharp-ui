using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TradeHubGui.Common.Models;

namespace TradeHubGui.TemplateSelectors
{
    public class CredentialsTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Template selector for market data provider and order execution provider credentials
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if(item != null)
            {
                ProviderCredential credential = (ProviderCredential)item;

                if(credential.CredentialName.Equals("Username"))
                {
                    return (DataTemplate)element.FindResource("UsernameDataTemplate");
                }
                else if(credential.CredentialName.Equals("Password"))
                {
                    return (DataTemplate)element.FindResource("PasswordDataTemplate");
                }
                else if(credential.CredentialName.Equals("IP address"))
                {
                    return (DataTemplate)element.FindResource("IpAddressDataTemplate");
                }
                else if (credential.CredentialName.Equals("Port"))
                {
                    return (DataTemplate)element.FindResource("PortDataTemplate");
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
