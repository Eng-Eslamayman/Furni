
namespace Furni.Web.Core.VeiwModels
{
    public class ProductViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public int Quantity { get; set; }
        public float CostPrice { get; set; }
        public float Price { get; set; }
        [Range(1, 1000, ErrorMessage = Errors.MaxRange)]
        public float DiscountValue { get; set; }
        public string MainImageUrl { get; set; } = null!;
        public string MainImageThumbnailUrl { get; set; } = null!;
        public string? ImagePublicID { get; set; }
        public string Category { get; set; } = null!;
    }
}
