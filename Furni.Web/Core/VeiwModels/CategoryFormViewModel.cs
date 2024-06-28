using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Core.ViewModels
{
	public class CategoryFormViewModel
	{
		public int Id { get; set; }
        [Remote("AllowName", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        public string Name { get; set; } = null!;
		[Display(Name = "Display Order")]
        [Remote("AllowDisplayOrder", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        public int DisplayOrder { get; set; }
        [RequiredIf("Id == 0", ErrorMessage = Errors.EmptyImage)]
		public IFormFile? Image { get; set; }
		public string? ImageUrl { get; set; }
		public string? ImageThumbnailUrl { get; set; }

	}
}