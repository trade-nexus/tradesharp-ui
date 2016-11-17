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


﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeHubGui.Common;
using TradeHubGui.Common.ApplicationSecurity;
using TradeHubGui.Common.ValueObjects;

namespace TradeHubGui.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private RelayCommand _showLogsCommand;

        public bool IsWorking
        {
            get { return TradeSharpLicenseManager.GetLicense().IsActive; }
        }

        public MetroWindow MainWindow
        {
            get { return (MetroWindow)Application.Current.MainWindow; }
        }

        /// <summary>
        /// Show strategy Runner Log
        /// </summary>
        public ICommand ShowLogsCommand
        {
            get
            {
                return _showLogsCommand ??
                       (_showLogsCommand = new RelayCommand(param => ShowLogsExecute()));
            }
        }

        /// <summary>
        /// Show logs fayout window
        /// </summary>
        private void ShowLogsExecute()
        {
            ToggleFlyout(3);
        }

        /// <summary>
        /// Shows or Hide flayout window
        /// </summary>
        /// <param name="index">flayout index</param>
        /// <returns>returns true if flayout is found, otherwise returns false</returns>
        public bool ToggleFlyout(int index)
        {
            var flyout = MainWindow.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return false;
            }

            flyout.IsOpen = !flyout.IsOpen;
            return true;
        }

        /// <summary>
        /// Traverse through all windows of application and trying to find window by title
        /// </summary>
        /// <param name="title">window title</param>
        /// <returns>window</returns>
        public MetroWindow FindWindowByTitle(string title)
        {
            MetroWindow result = null;
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MetroWindow && window.Title == title)
                {
                    result = (MetroWindow)window;
                    break;
                }
            }

            return result;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
