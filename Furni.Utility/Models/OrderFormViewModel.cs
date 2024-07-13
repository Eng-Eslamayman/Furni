using Furni.Utility.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
	public class OrderFormViewModel
	{
		public int Id { get; set; }
        public string Address { get; set; } = null!;
		public string City { get; set; } = null!;
		public string? State { get; set; } 
		public string PostalCode { get; set; } = null!;
        [MaxLength(100)]
        [Display(Name = "First Name")]
        [RegularExpression(RegexPatterns.DenySpecialCharacters, ErrorMessage = Errors.DenySpecialCharacters)]
        public string? FullName { get; set; }
        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }
        [MaxLength(11)]
        [Display(Name = "Mobile Number")]
        [RegularExpression(RegexPatterns.MobileNumber, ErrorMessage = Errors.InvalidMobileNumber)]
        public string PhoneNumber { get; set; } = null!;
	}
}
