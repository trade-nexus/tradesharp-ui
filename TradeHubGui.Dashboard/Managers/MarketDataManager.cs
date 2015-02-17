using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.Common.Core.FactoryMethods;
using TradeHub.Common.Core.ValueObjects.AdminMessages;
using TradeHub.Common.Core.ValueObjects.MarketData;
using TradeHub.StrategyEngine.MarketData;

namespace TradeHubGui.Dashboard.Managers
{
    /// <summary>
    /// Provides functionality for Market Data related queries and response
    /// </summary>
    internal class MarketDataManager
    {
        /// <summary>
        /// Provides communication access with Market Data Server
        /// </summary>
        private readonly MarketDataService _marketDataService;

        #region Events

        // ReSharper disable InconsistentNaming
        private event Action _connectedEvent;
        private event Action _disconnectedEvent;
        private event Action<string> _logonArrivedEvent;
        private event Action<string> _logoutArrivedEvent;
        private event Action<Tick> _tickArrivedEvent;
        // ReSharper restore InconsistentNaming

        public event Action ConnectedEvent
        {
            add
            {
                if (_connectedEvent == null)
                {
                    _connectedEvent += value;
                }
            }
            remove { _connectedEvent -= value; }
        }

        public event Action DisconnectedEvent
        {
            add
            {
                if (_disconnectedEvent == null)
                {
                    _disconnectedEvent += value;
                }
            }
            remove { _disconnectedEvent -= value; }
        }

        public event Action<string> LogonArrivedEvent
        {
            add
            {
                if (_logonArrivedEvent == null)
                {
                    _logonArrivedEvent += value;
                }
            }
            remove { _logonArrivedEvent -= value; }
        }

        public event Action<string> LogoutArrivedEvent
        {
            add
            {
                if (_logoutArrivedEvent == null)
                {
                    _logoutArrivedEvent += value;
                }
            }
            remove { _logoutArrivedEvent -= value; }
        }

        public event Action<Tick> TickArrivedEvent
        {
            add
            {
                if (_tickArrivedEvent == null)
                {
                    _tickArrivedEvent += value;
                }
            }
            remove { _tickArrivedEvent -= value; }
        }

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="marketDataService">Provides communication access with Market Data Server</param>
        public MarketDataManager(MarketDataService marketDataService)
        {
            // Save Instance
            _marketDataService = marketDataService;

            SubscribeDataServiceEvents();

            _marketDataService.StartService();
        }

        /// <summary>
        /// Register Market Data Service Events
        /// </summary>
        private void SubscribeDataServiceEvents()
        {
            // Makes sure that events are only hooked once
            UnsubscribeDataServiceEvents();

            _marketDataService.Connected += OnConnected;
            _marketDataService.Disconnected += OnDisconnected;

            _marketDataService.LogonArrived += OnLogonArrived;
            _marketDataService.LogoutArrived += OnLogoutArrived;
            
            _marketDataService.TickArrived += OnTickArrived;
        }

        /// <summary>
        /// Unsubscribe Market Data Service Events
        /// </summary>
        private void UnsubscribeDataServiceEvents()
        {
            _marketDataService.Connected -= OnConnected;
            _marketDataService.Disconnected -= OnDisconnected;

            _marketDataService.LogonArrived -= OnLogonArrived;
            _marketDataService.LogoutArrived -= OnLogoutArrived;

            _marketDataService.TickArrived -= OnTickArrived;
        }

        /// <summary>
        /// Sends Connection request to Market Data Server
        /// </summary>
        /// <param name="providerName">Market Data Provider to connect</param>
        public void Connect(string providerName)
        {
            // Create a new login message
            Login login = new Login()
            {
                MarketDataProvider = providerName
            };

            _marketDataService.Login(login);
        }

        /// <summary>
        /// Sends request to Market Data Server for disconnecting given market data provider
        /// </summary>
        /// <param name="providerName">Market Data Provider to disconnect</param>
        public void Disconnect(string providerName)
        {
            // Create a new logout message
            Logout logout = new Logout()
            {
                MarketDataProvider = providerName
            };

            _marketDataService.Logout(logout);
        }

        /// <summary>
        /// Sends subscription request to Market Data Server
        /// </summary>
        /// <param name="security">Contains symbol information</param>
        /// <param name="providerName">Name of the provider on which to subscribe</param>
        public void Subscribe(Security security, string providerName)
        {
            // Create subscription message
            Subscribe subscribe = SubscriptionMessage.TickSubscription("", security, providerName);

            _marketDataService.Subscribe(subscribe);
        }

        /// <summary>
        /// Sends un-subscription request to Market Data Server
        /// </summary>
        /// <param name="security">Contains symbol information</param>
        /// <param name="providerName">Name of the provider on which to unsubscribe</param>
        public void Unsubscribe(Security security, string providerName)
        {
            // Create unsubscription message
            Unsubscribe unsubscribe = SubscriptionMessage.TickUnsubscription("", security, providerName);

            _marketDataService.Unsubscribe(unsubscribe);
        }

        #region Market Data Service Events

        /// <summary>
        /// Called when client is connected to Server
        /// </summary>
        private void OnConnected()
        {
            if (_connectedEvent!=null)
            {
                _connectedEvent();
            }
        }

        /// <summary>
        /// Called when client is disconnected from Server
        /// </summary>
        private void OnDisconnected()
        {
            if (_disconnectedEvent != null)
            {
                _disconnectedEvent();
            }
        }

        /// <summary>
        /// Called when requested market data provider is successfully 'Logged IN'
        /// </summary>
        /// <param name="providerName">Market Data Provider name</param>
        private void OnLogonArrived(string providerName)
        {
            if (_logonArrivedEvent != null)
            {
                _logonArrivedEvent(providerName);
            }
        }

        /// <summary>
        /// Called when requested market data provider is successfully 'Logged OUT'
        /// </summary>
        /// <param name="providerName">Narket Data Provider name</param>
        private void OnLogoutArrived(string providerName)
        {
            if (_logoutArrivedEvent != null)
            {
                _logoutArrivedEvent(providerName);
            }
        }

        /// <summary>
        /// Called when new Tick information is received from Market Data Sever
        /// </summary>
        /// <param name="tick">Contains market details</param>
        private void OnTickArrived(Tick tick)
        {
            if (_tickArrivedEvent != null)
            {
                _tickArrivedEvent(tick);
            }
        }

        #endregion

        /// <summary>
        /// Stops all market data activites and closes open connections
        /// </summary>
        public void Stop()
        {
            _marketDataService.StopService();
        }
    }
}
