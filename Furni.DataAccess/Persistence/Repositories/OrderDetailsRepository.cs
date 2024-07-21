using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Utility.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories
{
    public class OrderDetailsRepository : BaseRepository<OrderDetail>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductOrdersViewModel>> GetProductOrdersAsync()
        {
            return await _context.OrderDetails
                .Join(_context.Orders,
                    od => od.OrderId,
                    o => o.Id,
                    (od, o) => new { od, o })
                .Join(_context.Users,
                    combined => combined.o.ApplicationUserId,
                    u => u.Id,
                    (combined, u) => new { combined.od, combined.o, u })
                .Join(_context.Products,
                    combined => combined.od.ProductId,
                    p => p.Id,
                    (combined, p) => new { combined.od, combined.o, combined.u, p })
                .Take(7)
                .Select(data => new ProductOrdersViewModel
                {
                    OrderId = data.od.OrderId,
                    CreatedOn = data.o.CreatedOn,
                    CustomerName = data.u.FullName,
                    TotalPrice = data.od.Count * data.od.Price,
                    TotalProfit = (data.od.Count * data.od.Price) - (data.od.Count * data.p.CostPrice),
                    Status = data.o.OrderStatus
                })
                .ToListAsync();
        }




        public async Task<float> GetAverageDailySalesAsync()
        {
            var totalSales = await _context.Orders
                .Where(o => o.OrderDate >= DateTime.Now.AddDays(-30))
                .SumAsync(o => o.OrderTotal);

            var totalDays = (DateTime.Now - DateTime.Now.AddDays(-30)).TotalDays;
            return totalSales / (float)totalDays;
        }

        // Graphs 
        public async Task<IList<MonthlyFinancialReportViewModel>> GetMonthlyFinancialReportsAsync()
        {
            var startDate = DateTime.Now.AddYears(-1);
            var endDate = DateTime.Now;

            // Step 1: Generate a list of all months from startDate to endDate
            var months = new List<MonthlyFinancialReportViewModel>();
            for (var date = startDate; date <= endDate; date = date.AddMonths(1))
            {
                months.Add(new MonthlyFinancialReportViewModel
                {
                    Month = date.Month,
                    Year = date.Year,
                    TotalCost = 0f, // Initialize as float
                    TotalRevenue = 0f, // Initialize as float
                    TotalProfit = 0f // Initialize as float
                });
            }

            // Step 2: Fetch and aggregate the data
            var aggregatedData = await _context.OrderDetails
                .Where(od => od.Order.CreatedOn >= startDate && od.Order.CreatedOn <= endDate)
                .GroupBy(od => new { Month = od.Order.CreatedOn.Month, Year = od.Order.CreatedOn.Year })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    TotalCost = g.Sum(od => (od.Product != null ? od.Product.CostPrice : 0) * od.Count),
                    TotalRevenue = g.Sum(od => od.Count * od.Price)
                })
                .ToListAsync();

            // Step 3: Combine the two lists and round values to 2 decimal places
            foreach (var month in months)
            {
                var data = aggregatedData.FirstOrDefault(d => d.Month == month.Month && d.Year == month.Year);
                if (data != null)
                {
                    month.TotalCost = (float)Math.Round(data.TotalCost, 2);
                    month.TotalRevenue = (float)Math.Round(data.TotalRevenue, 2);
                    month.TotalProfit = (float)Math.Round(data.TotalRevenue - data.TotalCost, 2);
                }
                else
                {
                    month.TotalCost = 0f;
                    month.TotalRevenue = 0f;
                    month.TotalProfit = 0f;
                }
            }

            return months;
        }



        public async Task<IList<SalesThisMonthViewModel>> GetSalesThisMonthAsync()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // Start of the current month
            var endDate = DateTime.Now; // Current date

            // Step 1: Generate a list of all days from startDate to endDate
            var days = new List<SalesThisMonthViewModel>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                days.Add(new SalesThisMonthViewModel
                {
                    Daily = date,
                    SalesInDay = 0f // Initialize as float
                });
            }

            // Step 2: Fetch and aggregate the data
            var aggregatedData = await _context.OrderDetails
                .Where(od => od.Order.CreatedOn >= startDate && od.Order.CreatedOn <= endDate)
                .GroupBy(od => od.Order.CreatedOn.Date) // Group by date
                .Select(g => new
                {
                    Date = g.Key,
                    SalesInDay = g.Sum(od => od.Count * od.Price) // Calculate sales for the day
                })
                .ToListAsync();

            // Step 3: Combine the two lists and round values to 2 decimal places
            foreach (var day in days)
            {
                var data = aggregatedData.FirstOrDefault(d => d.Date.Date == day.Daily.Date);
                if (data != null)
                {
                    day.SalesInDay = (float)Math.Round(data.SalesInDay, 2);
                }
                else
                {
                    day.SalesInDay = 0f; // Set to 0 if no data
                }
            }

            return days;
        }








    }
}
