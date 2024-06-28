using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Models
{
    public class CustomProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public float Price { get; set; }
        public string MainImageUrl { get; set; } = null!;
    }
}
