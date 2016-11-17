/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessageBoxUtils;
using TradeHub.NotificationEngine.Common.Utility;
using TradeHubGui.Common;
using TradeHubGui.Common.ApplicationSecurity;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.ViewModel
{
    public class AddLicenseViewModel : BaseViewModel
    {
        private static Type _type = typeof (AddLicenseViewModel);

        #region Fields
        
        private RelayCommand _addLicense;

        #endregion

        #region Commands
        /// <summary>
        /// Command used with 'Save' button
        /// </summary>
        public ICommand AddLicenseCommand
        {
            get
            {
                return _addLicense ??
                       (_addLicense = new RelayCommand(param => AddLicenseExecute()));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// License Expiration Date
        /// </summary>
        public string ExpirationDate
        {
            get { return TradeSharpLicenseManager.GetLicense().ExpirationDate.Date.ToString("D"); }
        }

        /// <summary>
        /// License Subscription Type
        /// </summary>
        public string LicenseType
        {
            get { return TradeSharpLicenseManager.GetLicense().LicenseSubscriptionType; }
        }

        /// <summary>
        /// Client Details
        /// </summary>
        public string ClientDetails
        {
            get { return TradeSharpLicenseManager.GetLicense().ClientDetails; }
        }

        #endregion
        
        #region Command Trigger Methods

        /// <summary>
        /// Called when 'Select License File' button is clicked
        /// </summary>
        private void AddLicenseExecute()
        {
            AddLicense();
        }

        #endregion

        /// <summary>
        /// Open dialog to select license and then add it to the application
        /// </summary>
        private void AddLicense()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = "Import License";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "OBJ Files (.obj)|*.obj|All Files (*.*)|*.*";
            System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                File.Copy(openFileDialog.FileName, "TradeSharpLicense.obj", true);
                var newLicense = TradeSharpLicenseManager.UpdateLicense();

                if (newLicense.LicenseSubscriptionType.Equals(Common.ApplicationSecurity.LicenseType.Invalid.ToString()))
                {
                    WPFMessageBox.Show(MainWindow, "The license file is invalid.\n\nPlease contact TradeSharp support at: tradesharp@aurorasolutions.io", "TRADESHARP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    WPFMessageBox.Show(MainWindow, "New License added.\n\nPlease restart the application", "TRADESHARP",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
