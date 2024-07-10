using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Models.Entities
{
	public class OrderDetail
	{
		public int Id { get; set; }
		public int OrderHeaderId { get; set; }
		public Order? OrderHeader { get; set; }
		public int ProductId { get; set; }
		public Product? Product { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }

    }
}
