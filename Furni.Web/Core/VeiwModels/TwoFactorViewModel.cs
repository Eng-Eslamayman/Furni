using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Core.VeiwModels
{
    public class TwoFactorViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
        public string SharedKey { get; set; } = null!;
        public byte[]? QRCodeBytes { get; set; }
        public string[] RecoveryCodes { get; set; } 
    }
}
