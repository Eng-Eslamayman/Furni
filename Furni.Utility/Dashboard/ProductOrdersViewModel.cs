using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Dashboard
{
    public class ProductOrdersViewModel
    {
        public int OrderId { get; set; }
        public DateTime CreatedOn { get; set; } // 5min ago or 5Days ago or 3hours ago
        public string CustomerName { get; set; } = null!;
        public float TotalPrice { get; set; }
        public float TotalProfit { get; set; }
        public string Status { get; set; } = null!; // Rejected or Shipped or Confirmed or Pending

        // Computed property to display time ago
        public string TimeAgo
        {
            get
            {
                var timeSpan = DateTime.Now - CreatedOn;
                if (timeSpan.TotalMinutes < 1)
                    return "just now";
                if (timeSpan.TotalMinutes < 60)
                    return $"{timeSpan.Minutes} min ago";
                if (timeSpan.TotalHours < 24)
                    return $"{timeSpan.Hours} hours ago";
                if (timeSpan.TotalDays < 30)
                    return $"{timeSpan.Days} days ago";
                return CreatedOn.ToString("MMM dd, yyyy");
            }
        }
    }
}
