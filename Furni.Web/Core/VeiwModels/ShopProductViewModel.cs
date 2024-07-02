using Furni.Utility.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni.Web.Core.VeiwModels
{
	public class ShopProductViewModel
	{
		[Display(Name = "Categories")]
		public List<int>? SelectedCategories { get; set; } = new List<int>();
		public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
		public PaginatedList<CustomArrivalProductViewModel> Products { get; set; }
	}
}
