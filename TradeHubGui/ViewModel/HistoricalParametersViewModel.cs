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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHubGui.Common;
using TradeHubGui.Common.Infrastructure;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;

namespace TradeHubGui.ViewModel
{
    public class HistoricalParametersViewModel : BaseViewModel
    {
        private Type _type = typeof (HistoricalParametersViewModel);

        #region Fields

        private readonly string _filePath;
        private MarketDataProvider _selectedProvider;

        private DateTime _selectedStartDate;
        private DateTime _selectedEndDate;

        private List<MarketDataProvider> _availableProviders;

        private RelayCommand _saveParameters;

        /// <summary>
        /// Indicates if the Historical parameter settings on UI have changed or not
        /// </summary>
        private bool _hasSettingsChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Holds the provider name selected by the user
        /// </summary>
        public MarketDataProvider SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                _selectedProvider = value;
                _hasSettingsChanged = true;
                OnPropertyChanged("SelectedProvider");
            }
        }

        /// <summary>
        /// Historical data start date
        /// </summary>
        public DateTime SelectedStartDate
        {
            get { return _selectedStartDate; }
            set
            {
                _selectedStartDate = value;
                _hasSettingsChanged = true;
                OnPropertyChanged("SelectedStartDate");
            }
        }

        /// <summary>
        /// Historical data end date
        /// </summary>
        public DateTime SelectedEndDate
        {
            get { return _selectedEndDate; }
            set
            {
                _selectedEndDate = value;
                _hasSettingsChanged = true;
                OnPropertyChanged("SelectedEndDate");
            }
        }

        /// <summary>
        /// List of current available providers in the system
        /// </summary>
        public List<MarketDataProvider> AvailableProviders
        {
            get { return _availableProviders; }
            set
            {
                _availableProviders = value;
                OnPropertyChanged("AvailableProviders");
            }
        }

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
                       (_saveParameters =new RelayCommand(param => SaveParametersExecute(), param => SaveParametersCanExecute()));
            }
        }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public HistoricalParametersViewModel()
        {
            // Initialize object
            _selectedProvider = new MarketDataProvider();
            _availableProviders = new List<MarketDataProvider>();
            _filePath = @"HistoricalDataConfiguration\HistoricalDataProvider.xml";

            PopulateProviders();
            ReadParameters();

            _hasSettingsChanged = false;
        }

        #region Command Trigger Methods

        /// <summary>
        /// Indicates if the 'Save' command can be executed or not
        /// </summary>
        /// <returns></returns>
        private bool SaveParametersCanExecute()
        {
            if (_selectedProvider != null && _hasSettingsChanged)
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
        /// Adds available Providers to the collection to be shown on UI
        /// </summary>
        private void PopulateProviders()
        {
            // Clear any existing values
            AvailableProviders.Clear();

            // Populate values as a separate list so that if it gets modified from UI the orignal values are not effected
            AvailableProviders = ProvidersController.MarketDataProviders.ToList();
        }

        /// <summary>
        /// Reads existing Historical parameter values
        /// </summary>
        private void ReadParameters()
        {
            var existingParameters = XmlFileManager.GetHistoricalParameters(_filePath);

            // Get Start Date
            SelectedStartDate = DateTime.ParseExact(existingParameters.Item1, "yyyy,MM,dd", CultureInfo.InvariantCulture);

            // Get End Date
            SelectedEndDate = DateTime.ParseExact(existingParameters.Item2, "yyyy,MM,dd", CultureInfo.InvariantCulture);

            // Get provider name
            SelectedProvider.ProviderName = existingParameters.Item3;
        }

        /// <summary>
        /// Saves updated parameter values
        /// </summary>
        private void SaveParameters()
        {
            // Save parameter values selected from UI
            XmlFileManager.SaveHistoricalParameters(
                SelectedStartDate.ToString("yyyy,MM,dd"),
                SelectedEndDate.ToString("yyyy,MM,dd"), 
                SelectedProvider.ProviderName, _filePath);
        }
    }
}
