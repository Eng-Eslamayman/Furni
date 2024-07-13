using Furni.Utility.Consts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
	public class ShoppingCartViewModel
	{
		public IEnumerable<ProductCardViewModel> ProductCards { get; set; }
		public OrderFormViewModel Order { get; set; }
		public double TotalPrice { get; set; }
	}
}
