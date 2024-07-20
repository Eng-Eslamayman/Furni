using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Dashboard
{
    public class StockReportViewModel
    {
        public string ProductTitle { get; set; } = null!;
        public int ProductId { get; set; }
        public DateTime CreatedOn { get; set; }
        public float Price { get; set; }
        public string Status { get; set; } = null!; // In Stock(More than 3) or Out of Stock(0) or Low Stock(3 or less)
        public int Quantity { get; set; }

        public string StatusBadgeClass
        {
            get
            {
                return Status switch
                {
                    "In Stock" => "badge-light-success",
                    "Out of Stock" => "badge-light-danger",
                    "Low Stock" => "badge-light-warning",
                    _ => "badge-light-secondary"
                };
            }
        }
    }
}
