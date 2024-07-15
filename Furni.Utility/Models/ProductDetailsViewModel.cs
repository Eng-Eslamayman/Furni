using Furni.Utility.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
	public class ProductDetailsViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public int Quantity { get; set; }
		public float Price { get; set; }
		[Range(1, 100, ErrorMessage = Errors.MaxRange)]
		public float DiscountValue { get; set; }
		//public float ProductAfterDiscount { get; set; }
		public IList<string>? ImageUrls { get; set; }
		public string PriceAfterDiscount => (Price * (1.0f - (DiscountValue / 100f))).ToString("0.00");

		public Dictionary<int, int> ReviewData { get; set; }

		public List<ReviewViewModel> Reviews { get; set; }
		public int TotalReview { get; set; }
		public float RatingAverage { get; set; }

		public ProductDetailsViewModel()
		{
			ImageUrls = new List<string>();
			Reviews = new List<ReviewViewModel>();
			ReviewData = new Dictionary<int, int>();
		}
	}
}
