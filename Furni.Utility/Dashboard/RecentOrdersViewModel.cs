using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Dashboard
{
    public class RecentOrdersViewModel
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = null!;
		public string MainImageThumbnailUrl { get; set; } = null!;
		public int Quantity { get; set; }
        public float Price { get; set; }
        public float TotalPrice { get; set; }

    }
}
