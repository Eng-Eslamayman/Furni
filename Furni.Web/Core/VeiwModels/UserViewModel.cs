namespace Furni.Web.Core.VeiwModels
{
    public class UserViewModel : BaseViewModel
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
