using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string? OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public float OrderTotal { get; set; }

        public IList<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();

    }
}
