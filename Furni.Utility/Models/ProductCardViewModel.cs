using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
    public class ProductCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MainImageUrl { get; set; }
        public float Price { get; set; }
        public int Count { get; set; }
		public float DiscountValue { get; set; }
        public float PriceAfterDiscount => (float)Math.Round(Price * (1.0f - (DiscountValue / 100f)), 2);

	}
}
