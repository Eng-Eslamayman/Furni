using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

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
        public int Quantity { get; set; }
        public float Price { get; set; } 
        public float CostPrice { get; set; } 
        [Range(1, 1000, ErrorMessage = Errors.MaxRange)]
        public float? DiscountValue { get; set; } = null;
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; } = DateTime.Now;
        [RequiredIf("Id == 0", ErrorMessage = Errors.EmptyImage)]
        public IFormFile? Image { get; set; }
        //[RequiredIf("Id == 0", ErrorMessage = Errors.EmptyImage)]
        //public List<IFormFile>? Images { get; set; }
        public string? MainImageUrl { get; set; }
        public string? MainImageThumbnailUrl { get; set; }
        public string? ImagePublicId { get; set; }
        [Display(Name = "Categories")]
        public int CategoryId { get; set; } // IList => index
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IList<string>? ExistingImageUrls { get; set; } = new List<string>();
        public IList<string>? ExistingImageThumbnailUrls { get; set; } = new List<string>();
    }
}
