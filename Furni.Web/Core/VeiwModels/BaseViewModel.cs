namespace Furni.Web.Core.VeiwModels
{
    public class BaseViewModel
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
