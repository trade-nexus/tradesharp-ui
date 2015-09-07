using System.Windows.Threading;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBoxUtils;
using TraceSourceLogger;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common;
using TradeHubGui.Common.ApplicationSecurity;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.ViewModel;
using TradeHubGui.Views;


namespace TradeHubGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Dispatcher _currentDispatcher;

        public MainWindow()
        {
            Logger.SetLoggingLevel();
            Logger.LogDirectory(DirectoryStructure.CLIENT_LOGS_LOCATION);

            TradeSharpLicenseManager.GetLicense();

            InitializeComponent();

            // Save UI thread reference
            _currentDispatcher = Dispatcher.CurrentDispatcher;

            DataContext = new BaseViewModel();
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            if (!TradeSharpLicenseManager.GetLicense().IsActive)
            {
                WPFMessageBox.Show("The application license has expired.\n\nPlease contact TradeSharp support at: tradesharp@aurorasolutions.io", "TRADESHARP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Subscribe Events
            EventSystem.Subscribe<UiElement>(UpdateRelayCommands);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MetroWindow mw = new SettingsWindow();
            mw.ShowTitleBar = true;
            mw.ShowIconOnTitleBar = true;
            mw.ShowInTaskbar = false;
            mw.Owner = this;
            mw.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            mw.ShowDialog();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder aboutMessage = new StringBuilder();
            aboutMessage.AppendLine("TradeSharp is a C# based data feed & broker neutral Algorithmic Trading Platform that lets trading firms or individuals automate any rules based trading strategies in stocks, forex and ETFs. TradeSharp has been developed and is being maintained by Aurora Solutions.");
            aboutMessage.AppendLine("\n\n");
            aboutMessage.AppendLine("This product is licensed to: " + TradeSharpLicenseManager.GetLicense().ClientDetails);
            aboutMessage.AppendLine("The licensed will expire on: " + TradeSharpLicenseManager.GetLicense().ExpirationDate.Date.ToString("D"));
            aboutMessage.AppendLine("\n");
            aboutMessage.Append("For queries contact: tradesharp@aurorasolutions.io");

            var metroDialog = new MetroDialogSettings();
            metroDialog.ColorScheme=MetroDialogColorScheme.Theme;

            this.ShowMessageAsync("TRADESHARP", aboutMessage.ToString(), MessageDialogStyle.Affirmative, metroDialog);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            (passwordBox.DataContext as ProviderCredential).CredentialValue = passwordBox.Password;
        }

        private void PasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.Password = (passwordBox.DataContext as ProviderCredential).CredentialValue;
        }

        private void OnApplicationClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventSystem.Publish<string>("Close");

            // Close all open windows
            foreach (Window window in Application.Current.Windows)
            {
                // Because Main Window is already in closing state
                if (window != Application.Current.MainWindow)
                {
                    window.Close();
                }
            }

            Thread.Sleep(1000);

            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(ex.Message, "Uncaught Thread Exception",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Force updates the Relay Commands
        /// </summary>
        /// <param name="uiElement"></param>
        private void UpdateRelayCommands(UiElement uiElement)
        {
            _currentDispatcher.Invoke(DispatcherPriority.Normal, (Action)(CommandManager.InvalidateRequerySuggested));
        }
    }
}
