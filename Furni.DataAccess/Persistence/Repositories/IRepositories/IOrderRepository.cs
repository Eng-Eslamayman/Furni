using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
	public interface IOrderRepository : IBaseRepository<Order>
	{
		Task<OrderViewModel> GetAsync(string userId);
	}
}
