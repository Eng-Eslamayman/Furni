﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

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
        [Range(1, 1000, ErrorMessage = Errors.MaxRange)]
        public float DiscountValue { get; set; }
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; } = DateTime.Now;
        public IFormFile Image { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; } 
        public string? ImagePublicId { get; set; }
        [Display(Name = "Categories")]
        public int CategoryId { get; set; } // IList => index
        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}