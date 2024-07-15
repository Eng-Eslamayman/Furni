using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Models.Entities
{
	public class Review : BaseEntity
	{
		public int ReviewId { get; set; }
		public int ProductId { get; set; }
		public Product? Product { get; set; }
		public string ApplicationUserId { get; set; } = null!;
		public ApplicationUser? ApplicationUser { get; set; }
		[Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
		public int Rating { get; set; }
		public string? Comment { get; set; }
		public DateTime ReviewDate { get; set; }
	}
}
