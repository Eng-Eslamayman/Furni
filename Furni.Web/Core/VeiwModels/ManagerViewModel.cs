using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Core.VeiwModels
{
    public class ManagerViewModel
    {

        [Required, MaxLength(100, ErrorMessage = Errors.MaxLength), Display(Name = "Full Name")]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string FullName { get; set; } = null!;

        [Phone]
        [Display(Name = "Phone number"), MaxLength(11, ErrorMessage = Errors.MaxLength)]
        [RegularExpression(RegexPatterns.MobileNumber, ErrorMessage = Errors.InvalidMobileNumber)]
        public string? PhoneNumber { get; set; } 
        public IFormFile? Avatar { get; set; }
        public bool ImageRemoved { get; set; }
    }
}
