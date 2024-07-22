using Furni.Utility.Dashboard;
using Furni.Utility.Reports;
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
        // Reports
        FinancialsReportViewModel GetFinancialReports(DateTime startDate, DateTime endDate, int pageSize, int? pageNumber = null);

	}
}
