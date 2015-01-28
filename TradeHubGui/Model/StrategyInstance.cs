using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TradeHubGui.Model
{
	public class StrategyInstance
	{
		public string InstanceKey { get; set; }
		public string Symbol { get; set; }
		public string Description { get; set; }
		public SolidColorBrush StateBrush { get; set; }
	}
}
