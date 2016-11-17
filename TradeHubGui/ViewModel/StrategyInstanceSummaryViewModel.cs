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
using TradeHubGui.Common;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class StrategyInstanceSummaryViewModel : BaseViewModel
    {
        private StrategyInstance _strategyInstance;
        private RelayCommand _clearDataCommand;

        #region Properties

        /// <summary>
        /// Conatins instance details
        /// </summary>
        public StrategyInstance StrategyInstance
        {
            get { return _strategyInstance; }
            set
            {
                _strategyInstance = value;
                OnPropertyChanged("StrategyInstance");
            }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="strategyInstance">Contains instance details</param>
        public StrategyInstanceSummaryViewModel(StrategyInstance strategyInstance)
        {
            _strategyInstance = strategyInstance;
        }

        #region Commands

        /// <summary>
        /// Used for Clear Button
        /// </summary>
        public ICommand ClearDataCommand
        {
            get
            {
                return _clearDataCommand ?? (_clearDataCommand = new RelayCommand(param => ClearDataExecute()));
            }
        }

        #endregion

        #region Command Trigger Methods

        /// <summary>
        /// Called when clear data button is clicked
        /// </summary>
        private void ClearDataExecute()
        {
            StrategyInstance.InstanceSummary.Clear();
        }

        #endregion
    }
}
