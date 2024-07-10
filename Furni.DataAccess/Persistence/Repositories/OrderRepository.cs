using Furni.DataAccess.Persistence.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories
{
	public class OrderRepository : BaseRepository<Order>, IOrderRepository
	{
		public OrderRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<OrderViewModel?> GetAsync(string userId)
		{
			return await _context.Users
								 .Where(u => u.Id == userId)
								 .Select(o => new OrderViewModel
								 {
									 Name = o.FullName ?? string.Empty,
									 State = o.State ?? string.Empty,
									 StreetNumber = o.StreetNumber ?? string.Empty,
									 City = o.City ?? string.Empty,
									 PostalCode = o.PostalCode ?? string.Empty,
									 Email = o.Email ?? string.Empty,
									 PhoneNumber = o.PhoneNumber ?? string.Empty,
								 })
								 .FirstOrDefaultAsync();
		}
	}
}
