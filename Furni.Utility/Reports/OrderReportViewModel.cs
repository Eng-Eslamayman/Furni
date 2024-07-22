using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Reports
{
	public class OrderReportViewModel
	{
		public int OrderId { get; set; }
		public DateTime CreatedOn { get; set; } 
		public string CustomerName { get; set; } = null!;
		public float TotalPrice { get; set; }
		public float TotalProfit { get; set; }
		public string Status { get; set; } = null!; // Rejected or Shipped or Confirmed or Pending
	}
}
