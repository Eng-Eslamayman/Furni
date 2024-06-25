namespace Furni.DataAccess.Persistence.Seeds
{
    public static class CategoriesData
    {
        public static List<Category> Categories { get; set; } = null!;
        public static IEnumerable<Category> Seed()
        {
            Categories = new List<Category> { 
                new Category { Id = 1, Name = "Cheers", DisplayOrder = 1, ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 2, Name = "Couches", DisplayOrder = 2 , ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 3, Name = "Beds", DisplayOrder = 3 , ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 4, Name = "Tables", DisplayOrder = 4 , ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 5, Name = "Bed Rooms", DisplayOrder = 5 , ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 6, Name = "Children's Bedrooms", DisplayOrder = 6 , ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 7, Name = "Wordrobe", DisplayOrder = 7 , ImageUrl = "", ImageThumbnailUrl = "" },
            };
            return Categories;
        }
    }
}
