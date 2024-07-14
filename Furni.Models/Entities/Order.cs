using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Models.Entities
{
    public class Order 
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser? ApplicationUser { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedOn { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; } 
        public float OrderTotal { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }

        public DateTime? PaymentDate { get; set; }  // Nullable for cases where payment hasn't occurred yet
        public DateTime? PaymentDueDate { get; set; }  // Nullable if not applicable

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

		public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
