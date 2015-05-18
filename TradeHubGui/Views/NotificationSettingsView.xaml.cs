using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using TradeHubGui.ViewModel;

namespace TradeHubGui.Views
{
    /// <summary>
    /// Interaction logic for NotificationSettingsView.xaml
    /// </summary>
    public partial class NotificationSettingsView : UserControl
    {
        public NotificationSettingsView()
        {
            InitializeComponent();
            DataContext = new NotificationSettingsViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            (passwordBox.DataContext as NotificationSettingsViewModel).SenderPassword = passwordBox.Password;
        }

        private void PasswordBox_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.Password = (passwordBox.DataContext as NotificationSettingsViewModel).SenderPassword;
        }
    }
}
