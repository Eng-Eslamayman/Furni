using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
	public class ReviewViewModel
	{
		public int Rating { get; set; }
		public string Comment { get; set; }
		public string UserName { get; set; } = null!;
		public DateTime ReviewDate { get; set; }
	}
}
