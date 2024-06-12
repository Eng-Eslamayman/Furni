namespace Furni.Models.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; } 
        [Range(1, 100, ErrorMessage = Errors.MaxRange)]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser? ApplicationUser { get; set; } 
    }
}
