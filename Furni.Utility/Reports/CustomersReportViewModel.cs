using Furni.Utility.Models;

namespace Furni.Utility.Reports
{
	public class CustomersReportViewModel
	{
        public PaginatedList<CustomerReportViewModel> Customers { get; set; } // Most Purchasing Customers
	}
}
