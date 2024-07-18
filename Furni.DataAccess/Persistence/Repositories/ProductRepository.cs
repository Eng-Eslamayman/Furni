﻿using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Models.Entities;

namespace Furni.DataAccess.Persistence.Repositories
{
    internal class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {

        }	

        public IQueryable<Product> GetDetails() => _context.Products
                .Include(b => b.Category);


        public (IQueryable<Product> products, int count) GetFiltered(GetFilteredDto dto)
        {
            IQueryable<Product> products = _context.Products.Include(b => b.Category);

            if (!string.IsNullOrEmpty(dto.SearchValue))
                products = products.Where(b => b.Title.Contains(dto.SearchValue!) || b.Category!.Name.Contains(dto.SearchValue!));

            products = products.OrderBy($"{dto.SortColumn} {dto.SortColumnDirection}");

            var recordsTotal = _context.Products.Count(); // All Records in Database

            return (products, recordsTotal);
        }


        public List<CustomProductViewModel> GetCustomProducts() => _context.Products
                .Select(p => new CustomProductViewModel
                {
                    Title = p.Title,
                    Id = p.Id,
                    MainImageUrl = p.MainImageUrl,
                    Price = p.Price
                }).Skip(0).Take(8).ToList();

		public IQueryable<CustomArrivalProductViewModel> GetCustomArrivalProducts(int? categoryId = null)
		{
			if (categoryId.HasValue)
				return _context.Products.Include(p => p.Category).Include(p => p.ProductImages)
					.Where(p => p.CategoryId == categoryId && !p.Category!.IsDeleted)
					 .Select(p => new CustomArrivalProductViewModel
					 {
						 Title = p.Title,
						 Id = p.Id,
						 Price = p.Price,
						 ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).Skip(0).Take(2).ToList()
					 });

			else
				return _context.Products.Include(p => p.ProductImages)
				  .Select(p => new CustomArrivalProductViewModel
				  {
					  Title = p.Title,
					  Id = p.Id,
					  Price = p.Price,
					  ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).Skip(0).Take(2).ToList()
				  });
		}


		public IEnumerable<ProductsAndCategoriesSearchViewModel> GetProductsSearch(string query, int skipCount)
		{
			return _context.Products
				.Where(p => p.Title.Contains(query))
				.Skip(skipCount)
				.Select(p => new ProductsAndCategoriesSearchViewModel { Id = p.Id, Name = p.Title, MainImageThumbnailUrl = p.MainImageThumbnailUrl, Type = "product" })
				.ToList();
		}



		public PaginatedList<CustomArrivalProductViewModel> GetShopProducts(int pageNumber, int pageSize, int? categoryId = null, string? searchTerm = null, string ? sortCriteria = null)
        {
            IQueryable<Product> productQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages);

            if (categoryId.HasValue && categoryId.Value != 0)
            {
                productQuery = productQuery.Where(p => p.CategoryId == categoryId.Value);
            }

            // Filter by search term if specified
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                productQuery = productQuery.Where(p =>
                    p.Title.ToLower().Contains(lowerSearchTerm) ||
                    (p.Description ?? string.Empty).ToLower().Contains(lowerSearchTerm) ||
                    p.Category.Name.ToLower().Contains(lowerSearchTerm)); 
            }

            // Apply sorting based on sortCriteria
            switch (sortCriteria?.ToLower())
            {
                case "featured":
                    productQuery = productQuery.OrderByDescending(p => p.Id); // Using Id as a placeholder for Featured
                    break;
                case "popular":
                    productQuery = productQuery.OrderByDescending(p => p.CreatedOn); // Using DateAdded as a proxy for Popular
                    break;
                case "priceasc":
                    productQuery = productQuery.OrderBy(p => p.Price);
                    break;
                case "pricedesc":
                    productQuery = productQuery.OrderByDescending(p => p.Price);
                    break;
                default:
                    productQuery = productQuery.OrderBy(p => p.Title); // Default sorting
                    break;
            }

            var products = productQuery.Select(p => new CustomArrivalProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).Take(2).ToList()
            });

            return PaginatedList<CustomArrivalProductViewModel>.Create(products, pageNumber, pageSize);
        }




        public ProductDetailsViewModel? GetProduct(int id)
		{
			try
			{
				var product = _context?.Products
					.Where(p => p.Id == id)
					.Include(p => p.ProductImages)
					.Include(p => p.Reviews) // Include reviews
						.ThenInclude(r => r.ApplicationUser) // Include ApplicationUser for each review
					.AsEnumerable() // Switch to client evaluation
					.Select(p => new ProductDetailsViewModel
					{
						Title = p.Title,
						Id = p.Id,
						Price = p.Price,
						Description = p.Description,
						Quantity = p.Quantity,
						DiscountValue = p.DiscountValue,
						ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).ToList(),
						Reviews = p.Reviews.Select(r => new ReviewViewModel
						{
							Rating = r.Rating,
							Comment = r.Comment,
							UserName = r.ApplicationUser?.FullName ?? "Unknown User", // Handle null case for ApplicationUser
							ReviewDate = r.ReviewDate,
							
						}).ToList(),
						ReviewData = p.Reviews.GroupBy(r => r.Rating)
									 .ToDictionary(g => g.Key, g => g.Count()),
						TotalReview = p.Reviews.Count(), // Calculate total number of reviews
						RatingAverage = p.Reviews.Any() ? (float)p.Reviews.Average(r => r.Rating) : 0.0f // Calculate average rating
					})
					.FirstOrDefault();

				return product;
			}
			catch (Exception ex)
			{
				return null;
			}
		}



	}

}
