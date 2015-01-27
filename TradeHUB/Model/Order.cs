using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHUB.Model
{
	public class Order
	{
		public string ID { get; set; }
		public string Side { get; set; }
		public string Type { get; set; }
		public float Price { get; set; }
		public int Quantity { get; set; }
		public DateTime Time { get; set; }
		public string Status { get; set; }
	}
}
