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

namespace TradeHubGui.Views
{
	/// <summary>
	/// Interaction logic for DashboardView.xaml
	/// </summary>
	public partial class DashboardView : UserControl
	{
		public DashboardView()
		{
			InitializeComponent();
			this.DataContext = this;

			//var theme = ThemeManager.DetectAppStyle(Application.Current);
			//var accent = ThemeManager.GetAccent("Taupe");
			//ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);

			var random = new Random();
			var instruments = new List<Instrument>();

			//for (int i = 0; i < 10000; i++)
			//{
			instruments.Add(new Instrument("AAPL", 23, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("GOOG", 43, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("MSFT", 33, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("HP", 42, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("AOI", 34, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("WAS", 15, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("AAPL", 23, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("GOOG", 23, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("MSFT", 45, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("HP", 33, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("AOI", 24, 450.34f, 20, 456.00f, 445.34f, 23));
			instruments.Add(new Instrument("WAS", 23, 450.34f, 20, 456.00f, 445.34f, 23));
			//}

			MarketDataGrid.ItemsSource = instruments;
		}
	}
}
