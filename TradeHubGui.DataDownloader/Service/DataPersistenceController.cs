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
using TraceSourceLogger;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Infrastructure.FileWriter;
using TradeHubGui.Common;
using Disruptor;
using Disruptor.Dsl;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.ValueObjects;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;

namespace TradeHubGui.DataDownloader.Service
{
    /// <summary>
    /// Provides access for Market Data persistence related functionality
    /// </summary>
    public class DataPersistenceController : IEventHandler<Tick>, IEventHandler<BarDetail>, IEventHandler<HistoricBarData>
    {
        private Type _type = typeof (DataPersistenceController);

        private bool _persistBinary = false;
        private bool _persistCsv = false;

        /// <summary>
        /// Persists data in CSV format
        /// </summary>
        private CsvWriter _writerCsv;

        /// <summary>
        /// Persists data in Binary format
        /// </summary>
        private BinaryWriter _writerBinany;
        
        #region Disruptor fields

        // Bar/Historic Disruptor Ring Buffer Size 
        private readonly int _ringSize = 16384; // Must be multiple of 2

        // Tick Disruptor Ring Buffer Size 
        private readonly int _tickDisruptorRingSize = 65536; // Must be multiple of 2

        // Handles Tick data
        private Disruptor<Tick> _tickDisruptor;
        // Handles Bar data
        private Disruptor<BarDetail> _barDisruptor;
        // Handles Historic Bar data
        private Disruptor<HistoricBarData> _historicDisruptor;

        // Ring buffer to be used with Tick disruptor
        private RingBuffer<Tick> _tickRingBuffer;
        // Ring buffer to be used with Bar disruptor
        private RingBuffer<BarDetail> _barRingBuffer;
        // Ring buffer to be used with Historic Bar disruptor
        private RingBuffer<HistoricBarData> _historicRingBuffer;

        // Publishes messages to Tick Disruptor
        private EventPublisher<Tick> _tickPublisher;
        // Publishes messages to Bar Disruptor
        private EventPublisher<BarDetail> _barPublisher;
        // Publishes messages to Historic Bar data Disruptor
        private EventPublisher<HistoricBarData> _historicPublisher;

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DataPersistenceController()
        {
            // Initialize
            _writerCsv = new CsvWriter();
            _writerBinany = new BinaryWriter();

            InitializeDisurptor();

            // Subscribe Events
            EventSystem.Subscribe<Tick>(HandleTickData);
            EventSystem.Subscribe<BarDetail>(HandleBarData);
            EventSystem.Subscribe<HistoricBarData>(HandleHistoricBarData);
        }

        /// <summary>
        /// Initialize all local disruptors
        /// </summary>
        private void InitializeDisurptor()
        {
            // Initialize Disruptor
            _tickDisruptor = new Disruptor<Tick>(() => new Tick(), _tickDisruptorRingSize, TaskScheduler.Default);
            _barDisruptor = new Disruptor<BarDetail>(() => new BarDetail(new Bar(""), new BarParameters()), _ringSize, TaskScheduler.Default);
            _historicDisruptor = new Disruptor<HistoricBarData>(() => new HistoricBarData(new Security(), "", DateTime.UtcNow), _ringSize, TaskScheduler.Default);

            // Add Disruptor Consumer
            _tickDisruptor.HandleEventsWith(this);
            _barDisruptor.HandleEventsWith(this);
            _historicDisruptor.HandleEventsWith(this);

            // Start Disruptor
            _tickRingBuffer = _tickDisruptor.Start();
            _barRingBuffer = _barDisruptor.Start();
            _historicRingBuffer = _historicDisruptor.Start();

            // Start Event Publisher
            _tickPublisher = new EventPublisher<Tick>(_tickRingBuffer);
            _barPublisher = new EventPublisher<BarDetail>(_barRingBuffer);
            _historicPublisher = new EventPublisher<HistoricBarData>(_historicRingBuffer);
        }

        /// <summary>
        /// Specifies the data is to be persisted in which format
        /// </summary>
        /// <param name="persistBinary">Indicates if the data is to be persisted in binary format</param>
        /// <param name="persistCsv">Indicates if the data is to be persisted in csv format</param>
        public void SavePersistInformation(bool persistBinary, bool persistCsv)
        {
            _persistBinary = persistBinary;
            _persistCsv = persistCsv;
        }

        /// <summary>
        /// Sends Bar request with the given properties
        /// </summary>
        /// <param name="security"></param>
        /// <param name="barLength"></param>
        /// <param name="pipSize"></param>
        /// <param name="barFormat"></param>
        /// <param name="priceType"></param>
        /// <param name="dataProvider"></param>
        public void SubmitBarRequest(Security security, decimal barLength, decimal pipSize, string barFormat, string priceType, MarketDataProvider dataProvider)
        {
            // Create a new Subscription request
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest(security,
                dataProvider, MarketDataType.Bar, SubscriptionType.Subscribe);

            // Set Bar details
            subscriptionRequest.SetLiveBarDetails(barLength, pipSize, barFormat, priceType);

            EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);
        }

        /// <summary>
        /// Sends Historical bar data request with given properties
        /// </summary>
        /// <param name="security"></param>
        /// <param name="barType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interval"></param>
        /// <param name="dataProvider"></param>
        public void SubmitHistoricDataRequest(Security security, string barType, DateTime startDate, DateTime endDate, uint interval, MarketDataProvider dataProvider)
        {
            // Create a new Subscription request
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest(security,
                dataProvider, MarketDataType.Historical, SubscriptionType.Subscribe);

            // Set Bar details
            subscriptionRequest.SetHistoricalBarDetails(barType, interval, startDate, endDate);

            EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);
        }

        #region Incoming Data

        /// <summary>
        /// Handles incoming Tick data for persistence
        /// </summary>
        /// <param name="tick"></param>
        private void HandleTickData(Tick tick)
        {
            // Send incoming Tick to Disruptor
            _tickPublisher.PublishEvent((tickObject, sequenceNo) =>
            {
                // Set parameters
                tickObject.AskPrice = tick.AskPrice;
                tickObject.AskSize = tick.AskSize;
                tickObject.BidPrice = tick.BidPrice;
                tickObject.BidSize = tick.BidSize;
                tickObject.LastPrice = tick.LastPrice;
                tickObject.LastSize = tick.LastSize;

                tickObject.Security = tick.Security;
                tickObject.DateTime = tick.DateTime;
                tickObject.MarketDataProvider = tick.MarketDataProvider;

                // Return updated object
                return tickObject;
            });
        }

        /// <summary>
        /// Handles incoming Bar data for persistence
        /// </summary>
        /// <param name="barDetail"></param>
        private void HandleBarData(BarDetail barDetail)
        {
            // Send incoming Bar to Disruptor
            _barPublisher.PublishEvent((barObject, sequenceNo) =>
            {
                // Set bar details
                barObject.Bar.RequestId = barDetail.Bar.RequestId;

                barObject.Bar.Open = barDetail.Bar.Open;
                barObject.Bar.High = barDetail.Bar.High;
                barObject.Bar.Low = barDetail.Bar.Low;
                barObject.Bar.Close = barDetail.Bar.Close;

                barObject.Bar.Security = barDetail.Bar.Security;
                barObject.Bar.DateTime = barDetail.Bar.DateTime;
                barObject.Bar.MarketDataProvider = barDetail.Bar.MarketDataProvider;

                // Set bar parameters
                barObject.BarParameters.BarLength = barDetail.BarParameters.BarLength;
                barObject.BarParameters.Format = barDetail.BarParameters.Format;
                barObject.BarParameters.PriceType = barDetail.BarParameters.PriceType;
                barObject.BarParameters.PipSize = barDetail.BarParameters.PipSize;

                // Return updated object
                return barObject;
            });
        }

        /// <summary>
        /// Handles incoming Historical Bar data for persistence
        /// </summary>
        /// <param name="historicBarData"></param>
        private void HandleHistoricBarData(HistoricBarData historicBarData)
        {
            // Send Historical Bar data to Disruptor
            _historicPublisher.PublishEvent((historicBarObject, sequenceNo) =>
            {
                // Set parameters
                historicBarObject.ReqId = historicBarData.ReqId;
                historicBarObject.Bars = (Bar[]) historicBarData.Bars.Clone();
                historicBarObject.Security = historicBarData.Security;
                historicBarObject.DateTime = historicBarData.DateTime;
                historicBarObject.MarketDataProvider = historicBarData.MarketDataProvider;
                historicBarObject.BarsInformation = historicBarData.BarsInformation;

                // Return updated object
                return historicBarObject;
            });
        }

        #endregion

        #region Persist Methods

        /// <summary>
        /// Saves Market data in CSV format
        /// </summary>
        /// <param name="tick"></param>
        private void PersistCsvData(Tick tick)
        {
            try
            {
                // Write new tick in csv format
                _writerCsv.Write(tick);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "PersistCsvData - Tick");
            }
        }

        /// <summary>
        /// Saves Bar data in CSV format
        /// </summary>
        /// <param name="barDetail"></param>
        private void PersistCsvData(BarDetail barDetail)
        {
            try
            {
                // Write new bar in csv format
                _writerCsv.Write(barDetail.Bar, barDetail.BarParameters.Format, barDetail.BarParameters.PriceType,
                    barDetail.BarParameters.BarLength.ToString());
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "PersistCsvData - Bar");
            }
        }

        /// <summary>
        /// Saves Historical Bar data in CSV format
        /// </summary>
        /// <param name="historicBarData"></param>
        private void PersistCsvData(HistoricBarData historicBarData)
        {
            try
            {
                // Write new Historical bar data in csv format
                _writerCsv.Write(historicBarData);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "PersistCsvData - HistoricBar");
            }
        }

        /// <summary>
        /// Saves Market data in Binary format
        /// </summary>
        /// <param name="tick"></param>
        private void PersistBinaryData(Tick tick)
        {
            try
            {
                // Write new tick in binary format
                _writerBinany.Write(tick);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "PersistBinaryData - Tick");
            }
        }

        /// <summary>
        /// Saves Bar data in Binary format
        /// </summary>
        /// <param name="barDetail"></param>
        private void PersistBinaryData(BarDetail barDetail)
        {
            try
            {
                // Write new Bar in binary format
                _writerBinany.Write(barDetail.Bar, barDetail.BarParameters.Format, barDetail.BarParameters.PriceType,
                    barDetail.BarParameters.BarLength.ToString());
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "PersistBinaryData - Bar");
            }
        }

        /// <summary>
        /// Saves Historic bar data in Binary format
        /// </summary>
        /// <param name="historicBarData"></param>
        private void PersistBinaryData(HistoricBarData historicBarData)
        {
            try
            {
                // Write new Historical bar data in binary format
                _writerBinany.Write(historicBarData);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, _type.FullName, "PersistBinaryData - HistoricBar");
            }
        }

        #endregion

        #region Disruptor's 'OnNext' methods

        /// <summary>
        /// Called when a publisher has committed an event to the <see cref="T:Disruptor.RingBuffer`1"/>
        /// </summary>
        /// <param name="data">Data committed to the <see cref="T:Disruptor.RingBuffer`1"/></param><param name="sequence">Sequence number committed to the <see cref="T:Disruptor.RingBuffer`1"/></param><param name="endOfBatch">flag to indicate if this is the last event in a batch from the <see cref="T:Disruptor.RingBuffer`1"/></param>
        public void OnNext(Tick data, long sequence, bool endOfBatch)
        {
            // Save in CSV format
            if (_persistCsv)
            {
                PersistCsvData(data);
            }
            // Save in Binary format
            if (_persistBinary)
            {
                PersistBinaryData(data);
            }
        }

        /// <summary>
        /// Called when a publisher has committed an event to the <see cref="T:Disruptor.RingBuffer`1"/>
        /// </summary>
        /// <param name="data">Data committed to the <see cref="T:Disruptor.RingBuffer`1"/></param><param name="sequence">Sequence number committed to the <see cref="T:Disruptor.RingBuffer`1"/></param><param name="endOfBatch">flag to indicate if this is the last event in a batch from the <see cref="T:Disruptor.RingBuffer`1"/></param>
        public void OnNext(BarDetail data, long sequence, bool endOfBatch)
        {
            // Save in CSV format
            if (_persistCsv)
            {
                PersistCsvData(data);
            }
            // Save in Binary format
            if (_persistBinary)
            {
                PersistBinaryData(data);
            }
        }

        /// <summary>
        /// Called when a publisher has committed an event to the <see cref="T:Disruptor.RingBuffer`1"/>
        /// </summary>
        /// <param name="data">Data committed to the <see cref="T:Disruptor.RingBuffer`1"/></param><param name="sequence">Sequence number committed to the <see cref="T:Disruptor.RingBuffer`1"/></param><param name="endOfBatch">flag to indicate if this is the last event in a batch from the <see cref="T:Disruptor.RingBuffer`1"/></param>
        public void OnNext(HistoricBarData data, long sequence, bool endOfBatch)
        {
            // Save in CSV format
            if (_persistCsv)
            {
                PersistCsvData(data);
            }
            // Save in Binary format
            if (_persistBinary)
            {
                PersistBinaryData(data);
            }
        }

        #endregion
    }
}
