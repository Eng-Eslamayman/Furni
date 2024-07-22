using Furni.Utility.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Furni.Utility.Reports
{
	public class ProductsReportViewModel
	{
		public PaginatedList<ProductReportViewModel> Products { get; set; }
	}
}
