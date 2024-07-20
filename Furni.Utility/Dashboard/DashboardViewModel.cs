using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Dashboard
{
	public class DashboardViewModel
	{
        public float AverageDailySales { get; set; }
        public int TotalOrdersThisMonth { get; set; }
        public int TotalCustomersThisMonth { get; set; }
        public int AverageOrdersPerDay { get; set; }
        public int TotalItemsInStock { get; set; }
        public IList<RecentOrdersViewModel>? RecentOrders { get; set; } // First 5
        public IList<ProductOrdersViewModel>? ProductOrders { get; set; } // First 8
        public IList<StockReportViewModel>? StockReport { get; set; } // First 8
        public IList<HighAndLowProductsRatedViewModel>? HighAndLowProductsRated { get; set; } // First 8, 4 for high and 4 for low
        public IList<MostBuyingProductsViewModel>? MostBuyingProducts { get; set; } // First 8
        public IList<MostPurchasingCustomersViewModel>? MostPurchasingCustomers { get; set; } // First 5
        public IList<HighestSpendingCustomersViewModel>? HighestSpendingCustomers { get; set; } // First 5

    }
}
