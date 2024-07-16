using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Core.VeiwModels
{
	public class ManagerUserViewModel
	{
        public string? Id { get; set; }
        [Required, MaxLength(100, ErrorMessage = Errors.MaxLength), Display(Name = "Full Name")]
		[RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
		public string FullName { get; set; } = null!;

		[Phone]
		[Display(Name = "Phone number"), MaxLength(11, ErrorMessage = Errors.MaxLength)]
		[RegularExpression(RegexPatterns.MobileNumber, ErrorMessage = Errors.InvalidMobileNumber)]
		public string? PhoneNumber { get; set; }
		public IFormFile? Avatar { get; set; }
		public bool ImageRemoved { get; set; }

		public string? Street { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? PostalCode { get; set; }
        public string? About { get; set; }

        [MaxLength(20, ErrorMessage = Errors.MaxLength), Display(Name = "Username")]
        [Remote("AllowUserName", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        [RegularExpression(RegexPatterns.Username, ErrorMessage = Errors.InvalidUsername)]
        public string UserName { get; set; } = null!;

        [EmailAddress]
        [MaxLength(200, ErrorMessage = Errors.MaxLength)]
        [Remote("AllowEmail", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        public string Email { get; set; } = null!;
    }
}
