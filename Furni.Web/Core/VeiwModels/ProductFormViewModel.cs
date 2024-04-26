using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni.Web.Core.VeiwModels
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }
        [Remote("AllowItem", null!, AdditionalFields = "Id", ErrorMessage = Errors.DuplicatedProduct)]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public int Quantity { get; set; }
        public float Price { get; set; }
        [Range(1, 1000, ErrorMessage = Errors.MaxRange)]
        public float DiscountValue { get; set; }
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; } = DateTime.Now;
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public string? ImagePublicID { get; set; }
        [Display(Name = "Categories")]
        public int CategoryID { get; set; }
        public Category? Category { get; set; }
    }
}
