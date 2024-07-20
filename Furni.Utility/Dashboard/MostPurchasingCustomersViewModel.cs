using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Dashboard
{
    public class MostPurchasingCustomersViewModel
    {
        public string FullName { get; set; } = null!;
        public string? ImageThumbnailUrl { get; set; }
        public float TotalBuyingPrice { get; set; }
        public int NumberOfOrders { get; set; }
    }
}
