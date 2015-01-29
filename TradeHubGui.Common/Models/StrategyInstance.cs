using System.Windows.Media;

namespace TradeHubGui.Common.Models
{
	public class StrategyInstance
	{
		public string InstanceKey { get; set; }
		public string Symbol { get; set; }
		public string Description { get; set; }
		public SolidColorBrush StateBrush { get; set; }
	}
}
