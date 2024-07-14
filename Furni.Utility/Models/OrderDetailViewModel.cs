using Furni.Utility.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public string ProductTitle { get; set; }
        public string ProductMainImageThumbnailUrl { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
