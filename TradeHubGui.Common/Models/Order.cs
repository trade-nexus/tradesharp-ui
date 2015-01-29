using System;

namespace TradeHubGui.Common.Models
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
