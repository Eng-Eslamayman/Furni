using Furni.Utility.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface IApplicationUserRepository : IBaseRepository<ApplicationUser>
    {
        Task<int> GetTotalCustomersThisMonthAsync();
        Task<IEnumerable<HighestSpendingCustomersViewModel>> GetHighestSpendingCustomersAsync();
        Task<IEnumerable<MostPurchasingCustomersViewModel>> GetMostPurchasingCustomersAsync();
    }
}
