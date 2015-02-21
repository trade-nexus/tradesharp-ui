using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.ValueObjects.AdminMessages;
using TradeHub.StrategyEngine.OrderExecution;

namespace TradeHubGui.Dashboard.Managers
{
    /// <summary>
    /// Provides Communication access with Order Execution Server
    /// </summary>
    internal class OrderExecutionManager : IDisposable
    {
        /// <summary>
        /// Provides communication access with Order Execution Server
        /// </summary>
        private readonly OrderExecutionService _orderExecutionService;

        private bool _disposed = false;

        #region Events

        // ReSharper disable InconsistentNaming
        private event Action _connectedEvent;
        private event Action _disconnectedEvent;
        private event Action<string> _logonArrivedEvent;
        private event Action<string> _logoutArrivedEvent;
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

        #endregion

        /// <summary>
        /// Argument Constructor
        /// </summary>
        /// <param name="orderExecutionService">Provides communication access with Order Execution Server</param>
        public OrderExecutionManager(OrderExecutionService orderExecutionService)
        {
            _orderExecutionService = orderExecutionService;

            SubscribeExecutionServiceEvents();

            // Start Service
            _orderExecutionService.StartService();
        }

        /// <summary>
        /// Register Order Execution Service Events
        /// </summary>
        private void SubscribeExecutionServiceEvents()
        {
            // Makes sure that events are only hooked once
            UnsubscribeExecutionServiceEvents();

            _orderExecutionService.Connected += OnConnected;
            _orderExecutionService.Disconnected += OnDisconnected;

            _orderExecutionService.LogonArrived += OnLogonArrived;
            _orderExecutionService.LogoutArrived += OnLogoutArrived;
        }

        /// <summary>
        /// Unsubscribe Order Execution Service Events
        /// </summary>
        private void UnsubscribeExecutionServiceEvents()
        {
            _orderExecutionService.Connected -= OnConnected;
            _orderExecutionService.Disconnected -= OnDisconnected;

            _orderExecutionService.LogonArrived -= OnLogonArrived;
            _orderExecutionService.LogoutArrived -= OnLogoutArrived;
        }


        /// <summary>
        /// Sends Connection request to Order Execution Server
        /// </summary>
        /// <param name="providerName">Order Execution Provider to connect</param>
        public void Connect(string providerName)
        {
            // Create a new login message
            Login login = new Login()
            {
                OrderExecutionProvider = providerName
            };

            _orderExecutionService.Login(login);
        }

        /// <summary>
        /// Sends request to Order Execution Server for disconnecting given order execution provider
        /// </summary>
        /// <param name="providerName">Order Execution Provider to disconnect</param>
        public void Disconnect(string providerName)
        {
            // Create a new logout message
            Logout logout = new Logout()
            {
                OrderExecutionProvider = providerName
            };

            _orderExecutionService.Logout(logout);
        }

        #region Order Execution Service Events

        /// <summary>
        /// Called when client is connected to Server
        /// </summary>
        private void OnConnected()
        {
            if (_connectedEvent != null)
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
        /// Called when requested order execution provider is successfully 'Logged IN'
        /// </summary>
        /// <param name="providerName">Order Execution Provider name</param>
        private void OnLogonArrived(string providerName)
        {
            if (_logonArrivedEvent != null)
            {
                _logonArrivedEvent(providerName);
            }
        }

        /// <summary>
        /// Called when requested order execution provider is successfully 'Logged OUT'
        /// </summary>
        /// <param name="providerName">Order Execution Provider name</param>
        private void OnLogoutArrived(string providerName)
        {
            if (_logoutArrivedEvent != null)
            {
                _logoutArrivedEvent(providerName);
            }
        }

        #endregion

        /// <summary>
        /// Stops all Order Execution activites and closes open connections
        /// </summary>
        public void Stop()
        {
            _orderExecutionService.StopService();
            _orderExecutionService.Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _orderExecutionService.StopService();
                }
                // Release unmanaged resources.
                _disposed = true;
            }
        }
    }
}
