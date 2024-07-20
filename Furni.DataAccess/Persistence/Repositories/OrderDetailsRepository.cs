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
    }
}
