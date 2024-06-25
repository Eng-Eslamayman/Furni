namespace Furni.Web.Core.VeiwModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
