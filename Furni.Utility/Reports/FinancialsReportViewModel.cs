using Furni.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Reports
{
	public class FinancialsReportViewModel
	{
		public string Duration { get; set; } = null!;
		public int TotalSales { get; set; }
		public float TotalRevenue { get; set; }
		public float TotalCost { get; set; }
		public float TotalProfit { get; set; }
		public PaginatedList<FinancialReportViewModel> Financials { get; set; }
	}
}
