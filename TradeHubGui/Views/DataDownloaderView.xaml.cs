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
    /// Interaction logic for DataDownloaderView.xaml
    /// </summary>
    public partial class DataDownloaderView : UserControl
    {
        public DataDownloaderView()
        {
            InitializeComponent();
            DataContext = new DataDownloaderViewModel();
        }

        /// <summary>
        /// On data downloader view loaded, initialize providers
        /// </summary>
        /// <param name="sender">DataDownloaderView</param>
        /// <param name="e"></param>
        void DataDownloaderView_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as DataDownloaderViewModel).InitializeMarketDataProviders();
        }
    }
}
