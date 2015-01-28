using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Model
{
	public class Execution
	{
		public string Key { get; set; }
		public int Executed { get; set; }
		public int BuyCount { get; set; }
		public int SellCount { get; set; }
	}
}
