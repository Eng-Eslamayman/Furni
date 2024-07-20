using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.Utility.Dashboard
{
    public class HighAndLowProductsRatedViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string MainImageThumbnailUrl { get; set; } = null!;
        public float TotalRate { get; set; }
        public float ProfitOrLoss { get; set; }
    }
}
