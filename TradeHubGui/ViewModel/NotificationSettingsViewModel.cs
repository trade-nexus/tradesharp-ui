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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHub.NotificationEngine.Common.Utility;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.ViewModel
{
    public class NotificationSettingsViewModel : BaseViewModel
    {
        private static Type _type = typeof (NotificationSettingsViewModel);

        #region Fields

        /// <summary>
        /// Username for Email Sender
        /// </summary>
        private string _senderUsername = String.Empty;

        /// <summary>
        /// Password for Email Sender
        /// </summary>
        private string _senderPassword = String.Empty;

        /// <summary>
        /// Username for Email Receiver
        /// </summary>
        private string _receiverUsername = String.Empty;

        /// <summary>
        /// Config file path for Email Sender parameters
        /// </summary>
        private string _emailSenderFilePath;

        /// <summary>
        /// Config file path for Email Receiver parameters
        /// </summary>
        private string _emailReceiverFilePath;

        /// <summary>
        /// Indicates if the Notification Parameters on UI have changed or not
        /// </summary>
        private bool _hasSettingsChanged;

        private RelayCommand _saveParameters;

        #endregion

        #region Commands
        /// <summary>
        /// Command used with 'Save' button
        /// </summary>
        public ICommand SaveParametersCommand
        {
            get
            {
                return _saveParameters ??
                       (_saveParameters = new RelayCommand(param => SaveParametersExecute(), param => SaveParametersCanExecute()));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Username for Email Sender
        /// </summary>
        public string SenderUsername
        {
            get { return _senderUsername; }
            set
            {
                _senderUsername = value;
                _hasSettingsChanged = true;
                OnPropertyChanged("SenderUsername");
            }
        }

        /// <summary>
        /// Password for Email Sender
        /// </summary>
        public string SenderPassword
        {
            get { return _senderPassword; }
            set
            {
                _senderPassword = value;
                _hasSettingsChanged = true;
                OnPropertyChanged("SenderPassword");
            }
        }

        /// <summary>
        /// Username for Email Receiver
        /// </summary>
        public string ReceiverUsername
        {
            get { return _receiverUsername; }
            set
            {
                _receiverUsername = value;
                _hasSettingsChanged = true;
                OnPropertyChanged("ReceiverUsername");
            }
        }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NotificationSettingsViewModel()
        {
            // Set File paths
            _emailSenderFilePath = DirectoryPath.NOTIFICATION_ENGINE_PATH + @"Config\EmailSenderInformation.xml";
            _emailReceiverFilePath = DirectoryPath.NOTIFICATION_ENGINE_PATH + @"Config\EmailReceiverInformation.xml";

            _hasSettingsChanged = false;

            //ReadParameters();
        }

        #region Command Trigger Methods

        /// <summary>
        /// Indicates if the 'Save' command can be executed or not
        /// </summary>
        /// <returns></returns>
        private bool SaveParametersCanExecute()
        {
            if (!string.IsNullOrEmpty(_senderUsername) && !string.IsNullOrWhiteSpace(_senderUsername)
                && !string.IsNullOrEmpty(_senderPassword) && !string.IsNullOrWhiteSpace(_senderPassword)
                && !string.IsNullOrEmpty(_receiverUsername) && !string.IsNullOrWhiteSpace(_receiverUsername) 
                && _hasSettingsChanged)
                return true;

            return false;
        }

        /// <summary>
        /// Called when 'Save' button is clicked
        /// </summary>
        private void SaveParametersExecute()
        {
            SaveParameters();
            _hasSettingsChanged = false;
        }

        #endregion

        /// <summary>
        /// Reads existing Historical parameter values
        /// </summary>
        private void ReadParameters()
        {
            var senderInformation = NotificationConfigurationReader.ReadEmailConfiguration("Sender", _emailSenderFilePath);
            var receiverInformation = NotificationConfigurationReader.ReadEmailConfiguration("Receiver", _emailReceiverFilePath);

            string senderUsername;
            if (!senderInformation.TryGetValue("username", out senderUsername))
                return;

            string senderPassword;
            if (!senderInformation.TryGetValue("password", out senderPassword))
                return;

            string receiverUsername;
            if (!receiverInformation.TryGetValue("username", out receiverUsername))
                return;

            SenderUsername = senderUsername;
            SenderPassword = senderPassword;
            ReceiverUsername = receiverUsername;
        }

        /// <summary>
        /// Saves updated parameter values
        /// </summary>
        private void SaveParameters()
        {
            // Save Sender Settings
            NotificationConfigurationWriter.WriteEmailConfiguration(_emailSenderFilePath,new Tuple<string, string>(_senderUsername, _senderPassword));

            // Save Receiver settings
            NotificationConfigurationWriter.WriteEmailConfiguration(_emailReceiverFilePath,new Tuple<string, string>(_receiverUsername, ""));
        }
    }
}
