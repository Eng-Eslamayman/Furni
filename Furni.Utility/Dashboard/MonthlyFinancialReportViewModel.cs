using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Dashboard
{
    public class MonthlyFinancialReportViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public float TotalCost { get; set; }
        public float TotalRevenue { get; set; }
        public float TotalProfit { get; set; }
    }

}
