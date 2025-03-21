﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
    public class CustomerListingViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ImageThumbnailUrl { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public float Price { get; set;}
	}
}
