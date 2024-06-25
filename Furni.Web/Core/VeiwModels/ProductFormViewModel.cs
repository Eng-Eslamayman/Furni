using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni.Web.Core.VeiwModels
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }
        [Remote("AllowItem", null!, AdditionalFields = "Id", ErrorMessage = Errors.DuplicatedProduct)]
        [MaxLength(500, ErrorMessage = Errors.MaxLength)]
        public string Title { get; set; } = null!;
        [MaxLength(500, ErrorMessage = Errors.MaxLength)]
        public string Description { get; set; } = null!;
        [MaxLength(250, ErrorMessage = Errors.MaxLength)]
        public string Summary { get; set; } = null!;
        public int? Quantity { get; set; } = null;
        public float? Price { get; set; } = null;
        [Range(1, 1000, ErrorMessage = Errors.MaxRange)]
        public float? DiscountValue { get; set; } = null;
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; } = DateTime.Now;
        [RequiredIf("Id == 0", ErrorMessage = Errors.EmptyImage)]
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public string? ImagePublicId { get; set; }
        [Display(Name = "Categories")]
        public int CategoryId { get; set; } // IList => index
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}
