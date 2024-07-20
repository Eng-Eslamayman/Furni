using Furni.Utility.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
	public interface IOrderRepository : IBaseRepository<Order>
	{
		Task<OrderFormViewModel> GetAsync(string userId);
		Task UpdateUserDetailsAsync(string userId, OrderFormViewModel model);
		Task<int> CreateOrderAsync(string userId, OrderFormViewModel model);
		Task UpdateStatusAsync(int id, string orderStatus, string? paymentStatus = null);
		Task UpdateStripePaymentIDAsync(int id, string sessionId, string paymentIntentId);
		Task DeleteOrderAsync(int id);
		


		// Admin Side
		(IQueryable<CustomerListingViewModel> orders, int count) GetFiltered(GetFilteredDto dto);
		Task<OrderDetailsViewModel> GetDetails(int id);

		// Dashboard 
		Task<int> GetAverageOrdersPerDayAsync();
		Task<int> GetTotalOrdersThisMonthAsync();
		Task<IEnumerable<RecentOrdersViewModel>> GetRecentOrdersAsync();

    }
}
