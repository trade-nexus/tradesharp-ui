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
using TradeHub.Common.Core.Utility;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.Views;

namespace TradeHubGui.ViewModel
{
    public class PriceAlertSettingsViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// Indicates if BID alert condition is to specified
        /// </summary>
        private bool _isBidConditionEnabled;

        /// <summary>
        /// Indicates if ASK alert condition is to specified
        /// </summary>
        private bool _isAskConditionEnabled;

        /// <summary>
        /// Indicates if TRADE alert condition is to specified
        /// </summary>
        private bool _isTradeConditionEnabled;

        /// <summary>
        /// Holds the alert condition selected for BID
        /// </summary>
        private string _selectedBidCondition;

        /// <summary>
        /// Holds the alert condition selected for ASK
        /// </summary>
        private string _selectedAskCondition;

        /// <summary>
        /// Holds the alert condition selected for TRADE
        /// </summary>
        private string _selectedTradeCondition;

        /// <summary>
        /// Holds the price to be used in BID alert condition
        /// </summary>
        private decimal _bidConditionPrice;

        /// <summary>
        /// Holds the price to be used in ASK alert condition
        /// </summary>
        private decimal _askConditionPrice;

        /// <summary>
        /// Holds the price to be used in TRADE alert condition
        /// </summary>
        private decimal _tradeConditionPrice;

        /// <summary>
        /// Contains all possible conditions
        /// </summary>
        private List<string> _availableConditions;

        /// <summary>
        /// Holds the complete BID condition specified by the user
        /// </summary>
        private PriceAlertCondition _bidAlertCondition;

        /// <summary>
        /// Holds the complete ASK condition specified by the user
        /// </summary>
        private PriceAlertCondition _askAlertCondition;

        /// <summary>
        /// Holds the complete TRADE condition specified by the user
        /// </summary>
        private PriceAlertCondition _tradeAlertCondition;

        /// <summary>
        /// Command binding used for 'Save' button
        /// </summary>
        private RelayCommand _saveSettingsCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if BID alert condition is to specified
        /// </summary>
        public bool IsBidConditionEnabled
        {
            get { return _isBidConditionEnabled; }
            set
            {
                _isBidConditionEnabled = value;
                OnPropertyChanged("IsBidConditionEnabled");
            }
        }

        /// <summary>
        /// Indicates if ASK alert condition is to specified
        /// </summary>
        public bool IsAskConditionEnabled
        {
            get { return _isAskConditionEnabled; }
            set
            {
                _isAskConditionEnabled = value;
                OnPropertyChanged("IsAskConditionEnabled");
            }
        }

        /// <summary>
        /// Indicates if TRADE alert condition is to specified
        /// </summary>
        public bool IsTradeConditionEnabled
        {
            get { return _isTradeConditionEnabled; }
            set
            {
                _isTradeConditionEnabled = value;
                OnPropertyChanged("IsTradeConditionEnabled");
            }
        }

        /// <summary>
        /// Contains all possible conditions
        /// </summary>
        public List<string> AvailableConditions
        {
            get { return _availableConditions; }
            set { _availableConditions = value; }
        }

        /// <summary>
        /// Holds the alert condition selected for BID
        /// </summary>
        public string SelectedBidCondition
        {
            get { return _selectedBidCondition; }
            set { _selectedBidCondition = value; }
        }

        /// <summary>
        /// Holds the alert condition selected for ASK
        /// </summary>
        public string SelectedAskCondition
        {
            get { return _selectedAskCondition; }
            set { _selectedAskCondition = value; }
        }

        /// <summary>
        /// Holds the alert condition selected for TRADE
        /// </summary>
        public string SelectedTradeCondition
        {
            get { return _selectedTradeCondition; }
            set { _selectedTradeCondition = value; }
        }

        /// <summary>
        /// Holds the price to be used in BID alert condition
        /// </summary>
        public decimal BidConditionPrice
        {
            get { return _bidConditionPrice; }
            set { _bidConditionPrice = value; }
        }

        /// <summary>
        /// Holds the price to be used in ASK alert condition
        /// </summary>
        public decimal AskConditionPrice
        {
            get { return _askConditionPrice; }
            set { _askConditionPrice = value; }
        }

        /// <summary>
        /// Holds the price to be used in TRADE alert condition
        /// </summary>
        public decimal TradeConditionPrice
        {
            get { return _tradeConditionPrice; }
            set { _tradeConditionPrice = value; }
        }

        /// <summary>
        /// Holds the complete BID condition specified by the user
        /// </summary>
        public PriceAlertCondition BidAlertCondition
        {
            get { return _bidAlertCondition; }
        }

        /// <summary>
        /// Holds the complete ASK condition specified by the user
        /// </summary>
        public PriceAlertCondition AskAlertCondition
        {
            get { return _askAlertCondition; }
        }

        /// <summary>
        /// Holds the complete TRADE condition specified by the user
        /// </summary>
        public PriceAlertCondition TradeAlertCondition
        {
            get { return _tradeAlertCondition; }
        }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PriceAlertSettingsViewModel()
        {
            // Initialize
            _availableConditions = new List<string>();

            PopulateConditionOptions();
        }

        #region Commands

        /// <summary>
        /// Command used 'Save' button is clicked
        /// </summary>
        public ICommand SaveSettingsCommand
        {
            get
            {
                return _saveSettingsCommand ?? (_saveSettingsCommand = new RelayCommand(param => SaveSettingsExecute(), param => SaveSettingsCanExecute()));
            }
        }

        #endregion

        #region Command Trigger Methods

        /// <summary>
        /// Triggerd when 'Save' button is clicked for saving provided alert conditions
        /// </summary>
        private void SaveSettingsExecute()
        {
            SaveAlertConditions();
        }

        /// <summary>
        /// Indicated if the 'Save' settings button is enabled or disabled
        /// </summary>
        private bool SaveSettingsCanExecute()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Populates list of available options to be used for setting conditions
        /// </summary>
        private void PopulateConditionOptions()
        {
            var options = EnumUtility.GetValues<ConditionOperator>();

            foreach (var conditionOperator in options)
            {
                _availableConditions.Add(EnumUtility.GetEnumDescription(conditionOperator));
            }
        }

        /// <summary>
        /// Save the conditions specified in the current iteration
        /// </summary>
        private void SaveAlertConditions()
        {
            if (_isBidConditionEnabled)
            {
                CreateNewCondition(_selectedBidCondition, _bidConditionPrice, ref _bidAlertCondition);
            }

            if (_isAskConditionEnabled)
            {
                CreateNewCondition(_selectedAskCondition, _askConditionPrice, ref _askAlertCondition);
            }

            if (_isTradeConditionEnabled)
            {
                CreateNewCondition(_selectedTradeCondition, _tradeConditionPrice, ref _tradeAlertCondition);
            }
        }

        /// <summary>
        /// Creates a new condition to be used for alerts
        /// </summary>
        private void CreateNewCondition(string conditionName, decimal conditionPrice, ref PriceAlertCondition alertCondition)
        {
            // Convert string to respective Enum value
            var conditionOperator = StringNameToEnum(conditionName);

            alertCondition = new PriceAlertCondition(conditionOperator, conditionPrice);
        }

        /// <summary>
        /// Uses existing conditions to populate fields for BID alert condition
        /// </summary>
        /// <param name="bidCondition"></param>
        public void PopulateExistingBidCondition(PriceAlertCondition bidCondition)
        {
            if (bidCondition != null)
            {
                // Use existing data to populate UI elements
                IsBidConditionEnabled = true;
                BidConditionPrice = bidCondition.ConditionPrice;
                SelectedBidCondition = EnumToStringName(bidCondition.ConditionOperator);
            }
            else
            {
                // Populate UI elements with default values
                IsBidConditionEnabled = false;
                SelectedBidCondition = EnumToStringName(ConditionOperator.Equals);
            }
        }

        /// <summary>
        /// Uses existing conditions to populate fields for ASK alert condition
        /// </summary>
        /// <param name="askCondition"></param>
        public void PopulateExistingAskCondition(PriceAlertCondition askCondition)
        {
            if (askCondition != null)
            {
                // Use existing data to populate UI elements
                IsAskConditionEnabled = true;
                AskConditionPrice = askCondition.ConditionPrice;
                SelectedAskCondition = EnumToStringName(askCondition.ConditionOperator);
            }
            else
            {
                // Populate UI elements with default values
                IsAskConditionEnabled = false;
                SelectedAskCondition = EnumToStringName(ConditionOperator.Equals);
            }
        }

        /// <summary>
        /// Uses existing conditions to populate fields for TRADE alert condition
        /// </summary>
        /// <param name="tradeCondition"></param>
        public void PopulateExistingTradeCondition(PriceAlertCondition tradeCondition)
        {
            if (tradeCondition != null)
            {
                // Use existing data to populate UI elements
                IsTradeConditionEnabled = true;
                TradeConditionPrice = tradeCondition.ConditionPrice;
                SelectedTradeCondition = EnumToStringName(tradeCondition.ConditionOperator);
            }
            else
            {
                // Populate UI elements with default values
                IsTradeConditionEnabled = false;
                SelectedTradeCondition = EnumToStringName(ConditionOperator.Equals);
            }
        }

        /// <summary>
        /// Converts the given String name to corresponding Enum value
        /// </summary>
        private ConditionOperator StringNameToEnum(string conditionName)
        {
            if (conditionName.Equals(EnumToStringName(ConditionOperator.Equals)))
            {
                return ConditionOperator.Equals;
            }
            else if (conditionName.Equals(EnumToStringName(ConditionOperator.Greater)))
            {
                return ConditionOperator.Greater;
            }
            else if (conditionName.Equals(EnumToStringName(ConditionOperator.Less)))
            {
                return ConditionOperator.Less;
            }
            return ConditionOperator.Equals;
        }

        /// <summary>
        /// Converts the given Enum value to corresponding String name
        /// </summary>
        private string EnumToStringName(ConditionOperator conditionOperator)
        {
            return EnumUtility.GetEnumDescription(conditionOperator);
        }
    }
}
