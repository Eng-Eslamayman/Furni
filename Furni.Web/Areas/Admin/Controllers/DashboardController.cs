using Furni.Utility.Dashboard;
using Furni.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Areas.Admin.Controllers
{
    [Area(AppRoles.Admin)]
    [Authorize(Policy = "InitialAccessPolicy")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch data from repositories
            var averageDailySales = await _unitOfWork.OrderDetails.GetAverageDailySalesAsync(); // 
            var totalOrdersThisMonth = await _unitOfWork.Orders.GetTotalOrdersThisMonthAsync(); // 
            var totalCustomersThisMonth = await _unitOfWork.ApplicationUsers.GetTotalCustomersThisMonthAsync(); // 
			var totalItemsInStock = await _unitOfWork.Products.GetTotalItemsInStockAsync(); // 
			var averageOrdersPerDay = await _unitOfWork.Orders.GetAverageOrdersPerDayAsync(); // 
            var recentOrders = await _unitOfWork.Orders.GetRecentOrdersAsync(); // 
            var productOrders = await _unitOfWork.OrderDetails.GetProductOrdersAsync(); //
            var stockReport = await _unitOfWork.Products.GetStockReportAsync(8); // 
            var highAndLowRatedProducts = await _unitOfWork.Products.GetHighAndLowRatedProductsAsync(); // 
            var mostBuyingProducts = await _unitOfWork.Products.GetMostBuyingProductsAsync(8); // 
            var mostPurchasingCustomers = await _unitOfWork.ApplicationUsers.GetMostPurchasingCustomersAsync(); // 
            var highestSpendingCustomers = await _unitOfWork.ApplicationUsers.GetHighestSpendingCustomersAsync(); // 

            // Create the dashboard view model
            var dashboardViewModel = new DashboardViewModel
            {
                AverageDailySales = averageDailySales,
                TotalOrdersThisMonth = totalOrdersThisMonth,
                TotalCustomersThisMonth = totalCustomersThisMonth,
                TotalItemsInStock = totalItemsInStock,
                AverageOrdersPerDay = averageOrdersPerDay,
                RecentOrders = recentOrders.ToList(),
                ProductOrders = productOrders.ToList(),
                StockReport = stockReport.ToList(),
                HighAndLowProductsRated = highAndLowRatedProducts.ToList(),
                MostBuyingProducts = mostBuyingProducts.ToList(),
                MostPurchasingCustomers = mostPurchasingCustomers.ToList(),
                HighestSpendingCustomers = highestSpendingCustomers.ToList()
            };

            // Return the view with the view model
            return View(dashboardViewModel);
        }

        [AjaxOnly]
        public async Task<IActionResult> GetMonthlyFinancialReportsAsync()
        {
            var financial = await _unitOfWork.OrderDetails.GetMonthlyFinancialReportsAsync();
            return Ok(financial);
        }

        [AjaxOnly]
        public async Task<IActionResult> GetSalesThisMonthAsync()
        {
            var data = await _unitOfWork.OrderDetails.GetSalesThisMonthAsync();
            return Ok(data);
        }
    } 
}
