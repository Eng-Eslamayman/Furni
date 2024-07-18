using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
	public class ProductsAndCategoriesSearchViewModel
	{
		public int? Id { get; set; }
		public string? Name { get; set; } 
		public string? MainImageThumbnailUrl { get; set; }
		public string Type { get; set; } = null!;
	}
}
