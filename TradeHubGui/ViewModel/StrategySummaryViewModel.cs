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
using TradeHubGui.Common.Infrastructure;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class StrategySummaryViewModel : BaseViewModel
    {
        private Strategy _strategy;
        private RelayCommand _clearDataCommand;
        private RelayCommand _exportStatisticsCommand;

        #region Properties

        /// <summary>
        /// Conatins instance details
        /// </summary>
        public Strategy Strategy
        {
            get { return _strategy; }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="strategy">Contains instance details</param>
        public StrategySummaryViewModel(Strategy strategy)
        {
            _strategy = strategy;
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

        public ICommand ExportStatisticsCommand
        {
            get
            {
                return _exportStatisticsCommand ?? (_exportStatisticsCommand = new RelayCommand(param => ExportStatisticsExecute(), param => ExportStatisticsCanExecute()));
            }
        }

        #endregion

        #region Command Trigger Methods

        /// <summary>
        /// Called when clear data button is clicked
        /// </summary>
        private void ClearDataExecute()
        {
            Strategy.ClearStatistics();
        }

        /// <summary>
        /// Called when 'Export' button is clicked
        /// </summary>
        private void ExportStatisticsExecute()
        {
            string folderPath = string.Empty;

            // Get Directory in which to save stats
            using (System.Windows.Forms.FolderBrowserDialog form = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = form.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.Yes || dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    folderPath = form.SelectedPath;
                }
                else
                {
                    return;
                }
            }

            // Save data
            Task.Run(() => ExportStatistics(folderPath));

        }

        private bool ExportStatisticsCanExecute()
        {
            return _strategy.Statistics.Count > 0;
        }

        #endregion

        /// <summary>
        /// Exports currents strategy statistics to a CSV file
        /// </summary>
        /// <param name="folderPath"></param>
        private void ExportStatistics(string folderPath)
        {
            IList<string> informationList = new List<string>();

            foreach (var strategyStatistics in _strategy.Statistics)
            {
                informationList.Add(strategyStatistics.Information);
            }

            PersistCsv.SaveData(folderPath, informationList as IReadOnlyList<string>, _strategy.Name);
        }
    }
}
