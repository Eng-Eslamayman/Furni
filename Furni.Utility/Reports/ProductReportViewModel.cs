using Furni.Utility.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Furni.Utility.Reports
{
	public class ProductReportViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public int Quantity { get; set; }
		public float Price { get; set; }
		[Range(1, 1000, ErrorMessage = Errors.MaxRange)]
		public float DiscountValue { get; set; }
		public string MainImageThumbnailUrl { get; set; } = null!;
		public string CategoryName { get; set; }
		public float CostPrice { get; set; }
	}
}
