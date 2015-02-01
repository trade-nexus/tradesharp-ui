using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TradeHubGui.Views;


namespace TradeHubGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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
            this.ShowMessageAsync("About", "Something will be displayed here!", MessageDialogStyle.Affirmative);
        }
    }
}
