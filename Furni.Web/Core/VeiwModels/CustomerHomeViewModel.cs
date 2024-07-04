namespace Furni.Web.Core.VeiwModels
{
    public class CustomerHomeViewModel
    {
        public IList<Utility.Models.CustomProductViewModel> CustomProductViewModel { get; set; }
        public IList<Utility.Models.CustomCategoryViewModel> CustomCategoryViewModel { get; set; }
        public IList<Utility.Models.CustomArrivalProductViewModel> CustomArrivalProductViewModel { get; set; }
    }
}
