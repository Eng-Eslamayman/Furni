using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Utility.Dashboard;
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

    }
}
