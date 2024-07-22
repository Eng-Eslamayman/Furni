using Furni.Utility.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Reports
{
	public class OrdersReportViewModel
	{
		public string? Status { get; set; } // Get All if null, Status => approved, pending, denied
		public string Duration { get; set; } = null!; 
		public PaginatedList<OrderReportViewModel> Orders { get; set; }
	}
}
