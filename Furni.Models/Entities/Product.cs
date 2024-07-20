using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Models.Entities
{
	public class Product : BaseEntity
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Summary { get; set; } = null!;
		public int Quantity { get; set; }
		public float Price { get; set; }
		[Range(1, 1000, ErrorMessage = Errors.MaxRange)]
		public float DiscountValue { get; set; }
		public string MainImageUrl { get; set; } = null!;
		public string MainImageThumbnailUrl { get; set; } = null!;
		public string? ImagePublicId { get; set; }
		public int CategoryId { get; set; }
		public Category? Category { get; set; }
		public ICollection<ProductImage>? ProductImages { get; set; }
		public ICollection<Review>? Reviews { get; set; }

        // Store the cost price of the product
        public float CostPrice { get; set; }

    }
}
