using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Models.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; } = null!;
		public bool IsDeleted { get; set; }
		public string? CreatedById { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public string? LastUpdatedById { get; set; }
		public DateTime? LastUpdatedOn { get; set; }
	}
}
