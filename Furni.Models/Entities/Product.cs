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
        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public string? ImagePublicID { get; set; }
        public int CategoryID { get; set; }
        public Category? Category { get; set; }
    }
}
