

namespace Furni.Models.Entities
{
    public class Category : BaseEntity
    { 
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [Range(1, 100, ErrorMessage = Errors.MaxRange)]
        public int DisplayOrder { get; set; } // If we have multiple categories => which category should be displayed first on the page
    }
}
