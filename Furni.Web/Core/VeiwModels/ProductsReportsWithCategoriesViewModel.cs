using Furni.Utility.Models;
using Furni.Utility.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni.Web.Core.VeiwModels
{
    public class ProductsReportsWithCategoriesViewModel
    {
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public string? Stock { get; set; } 
        [Display(Name = "Categories")]
        public List<int>? SelectedCategories { get; set; } = new List<int>();
        public PaginatedList<ProductReportViewModel> Products { get; set; }
    }
}
