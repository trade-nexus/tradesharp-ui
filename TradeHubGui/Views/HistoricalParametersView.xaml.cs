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
    /// Interaction logic for HistoricalParametersView.xaml
    /// </summary>
    public partial class HistoricalParametersView : UserControl
    {
        public HistoricalParametersView()
        {
            InitializeComponent();
            DataContext = new HistoricalParametersViewModel();
        }
    }
}
