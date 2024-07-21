using Furni.Utility.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface IOrderDetailsRepository : IBaseRepository<OrderDetail>
    {
        Task<IEnumerable<ProductOrdersViewModel>> GetProductOrdersAsync();
        Task<float> GetAverageDailySalesAsync();
        Task<IList<MonthlyFinancialReportViewModel>> GetMonthlyFinancialReportsAsync();
        Task<IList<SalesThisMonthViewModel>> GetSalesThisMonthAsync();
    }
}
