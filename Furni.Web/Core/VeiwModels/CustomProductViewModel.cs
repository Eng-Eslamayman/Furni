namespace Furni.Web.Core.VeiwModels
{
    public class CustomProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public float Price { get; set; }
        public string MainImageThumbnailUrl { get; set; } = null!;
    }
}
