using System;
using System.Windows.Media;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Contains Individual Strategy Instance information
    /// </summary>
	public class StrategyInstance
	{
		public string InstanceKey { get; set; }
		public string Symbol { get; set; }
		public string Description { get; set; }
        public object[] Parameters { get; set; }
        public Type StrategyType { get; set; }

		public SolidColorBrush StateBrush { get; set; }
	}
}
