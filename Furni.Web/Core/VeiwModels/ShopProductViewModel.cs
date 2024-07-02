using Furni.Utility.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni.Web.Core.VeiwModels
{
	public class ShopProductViewModel
	{
		[Display(Name = "Category")]
		public int CategoryId { get; set; }
		public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
		public IList<CustomArrivalProductViewModel> Products { get; set; }
	}
}
