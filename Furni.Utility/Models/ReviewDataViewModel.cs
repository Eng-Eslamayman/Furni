using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
	public class ReviewDataViewModel
	{
		public Dictionary<int, int> ReviewCounts { get; set; } = new Dictionary<int, int>();
		public int TotalReview { get; set; }
		public float RatingAverage { get; set; }
	}
}
