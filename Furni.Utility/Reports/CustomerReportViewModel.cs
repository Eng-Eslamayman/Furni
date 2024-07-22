using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Reports
{
	public class CustomerReportViewModel
	{
		public string FullName { get; set; } = null!;
		public float TotalBuyingPrice { get; set; }
		public int NumberOfOrders { get; set; }
	}
}
