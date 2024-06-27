namespace Furni.DataAccess.Persistence.Seeds
{
    public static class CategoriesData
    {
        public static List<Category> Categories { get; set; } = null!;
        public static IEnumerable<Category> Seed()
        {
            Categories = new List<Category> {
                new Category { Id = 1, Name = "CHAIRS", DisplayOrder = 3, ImageUrl = "/images/categories/3b61ac7e-4157-4e65-8409-4fd0c5b0e8ae.jpg", ImageThumbnailUrl = "/images/categories/thumb/3b61ac7e-4157-4e65-8409-4fd0c5b0e8ae.jpg" },
                new Category { Id = 2, Name = "COUCHES", DisplayOrder = 7 , ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 3, Name = "BEDS", DisplayOrder = 6 , ImageUrl = "/images/categories/3adc43a5-d623-4d97-8693-8241bdc80e1d.jpg", ImageThumbnailUrl = "/images/categories/thumb/3adc43a5-d623-4d97-8693-8241bdc80e1d.jpg" },
                new Category { Id = 4, Name = "TABLES", DisplayOrder = 2 , ImageUrl = "/images/categories/f5bc5d01-6d9e-4b86-958a-4b5ec2bde6e9.jpg", ImageThumbnailUrl = "/images/categories/thumb/f5bc5d01-6d9e-4b86-958a-4b5ec2bde6e9.jpg" },
                new Category { Id = 5, Name = "Bed Rooms", DisplayOrder = 8 , ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 6, Name = "Children's Bedrooms", DisplayOrder = 9 , ImageUrl = "", ImageThumbnailUrl = "" },
                new Category { Id = 7, Name = "WARDROBES", DisplayOrder = 5 , ImageUrl = "/images/categories/999eae65-2149-4fee-8687-b7356b67b06a.jpg", ImageThumbnailUrl = "/images/categories/thumb/999eae65-2149-4fee-8687-b7356b67b06a.jpg" },
                new Category { Id = 8, Name = "SOFA", DisplayOrder = 1 , ImageUrl = "/images/categories/4bb63bbc-4b87-42ea-b0c6-5752153b8491.jpg", ImageThumbnailUrl = "/images/categories/thumb/4bb63bbc-4b87-42ea-b0c6-5752153b8491.jpg" },
                new Category { Id = 9, Name = "LIGHTS", DisplayOrder = 4 , ImageUrl = "/images/categories/104e9c53-b0ca-44e8-bcf3-540a3ceab9eb.jpg", ImageThumbnailUrl = "/images/categories/thumb/104e9c53-b0ca-44e8-bcf3-540a3ceab9eb.jpg" },
            };
            return Categories;
        }
    }
}
