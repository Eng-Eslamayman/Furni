using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Utility.Dashboard;
using Furni.Utility.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetTotalCustomersThisMonthAsync()
        {
            return await _context.Users
                .CountAsync(u => u.CreatedOn.Month == DateTime.Now.Month && u.CreatedOn.Year == DateTime.Now.Year);
        }

        public async Task<IEnumerable<HighestSpendingCustomersViewModel>> GetHighestSpendingCustomersAsync()
        {
            return await _context.Users
                .Select(u => new HighestSpendingCustomersViewModel
                {
                    FullName = u.FullName,
                    ImageThumbnailUrl = u.ImageUrl,
                    TotalBuyingPrice = _context.Orders
                        .Where(o => o.ApplicationUserId == u.Id)
                        .Sum(o => o.OrderTotal), // Total money spent
                    NumberOfOrders = _context.Orders
                        .Where(o => o.ApplicationUserId == u.Id)
                        .Count() // Number of orders
                })
                .OrderByDescending(c => c.TotalBuyingPrice)
                .Take(5)
                .ToListAsync();
        }


        public async Task<IEnumerable<MostPurchasingCustomersViewModel>> GetMostPurchasingCustomersAsync()
        {
            return await _context.Users
                .Select(u => new MostPurchasingCustomersViewModel
                {
                    FullName = u.FullName,
                    ImageThumbnailUrl = u.ImageUrl,
                    TotalBuyingPrice = _context.Orders
                        .Where(o => o.ApplicationUserId == u.Id)
                        .Sum(o => o.OrderTotal), // Total money spent
                    NumberOfOrders = _context.Orders
                        .Count(o => o.ApplicationUserId == u.Id) 
                })
                .OrderByDescending(c => c.NumberOfOrders)
                .Take(5)
                .ToListAsync();
        }


        public PaginatedList<CustomerReportViewModel> GetCustomersReport(int pageSize, int? pageNumber = null)
        {
            var orderDetails = _context.Orders
                .SelectMany(o => o.OrderDetails, (o, od) => new { o.ApplicationUserId, od.Count, od.Price, od.Product.CostPrice, o.Id });

            var customerReports = _context.Users
                .Join(_context.UserRoles,
                      u => u.Id,
                      ur => ur.UserId,
                      (u, ur) => new { u, ur.RoleId })
                .Join(_context.Roles,
                      ur => ur.RoleId,
                      r => r.Id,
                      (ur, r) => new { ur.u, r.Name })
                .Where(ur => ur.Name == "Customer")
                .GroupBy(ur => ur.u.Id)
                .Select(g => new CustomerReportViewModel
                {
                    FullName = g.FirstOrDefault().u.FullName,
                    TotalBuyingPrice = orderDetails
                        .Where(od => od.ApplicationUserId == g.Key)
                        .Sum(od => od.Count * od.Price), // Total money spent
                    NumberOfOrders = orderDetails
                        .Where(od => od.ApplicationUserId == g.Key)
                        .Select(od => od.Id) // Use Order ID to count distinct orders
                        .Distinct() // Ensure unique order IDs
                        .Count() // Count distinct orders
                })
                .OrderByDescending(c => c.TotalBuyingPrice);

            return PaginatedList<CustomerReportViewModel>.Create(customerReports, pageNumber ?? 1, pageSize);
        }


        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

    }
}
