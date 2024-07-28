using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Models.Entities;
using Furni.Utility.Dashboard;
using Furni.Utility.Reports;

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


		public IEnumerable<ProductsAndCategoriesSearchViewModel> GetProductsSearch(string query, int count)
		{
			return _context.Products
				.Where(p => p.Title.Contains(query))
				.Select(p => new ProductsAndCategoriesSearchViewModel { Id = p.Id, Name = p.Title, MainImageThumbnailUrl = p.MainImageThumbnailUrl, Type = "product" })
                .Take(count)
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

        // Dashboard

        public async Task<IEnumerable<HighAndLowProductsRatedViewModel>> GetHighAndLowRatedProductsAsync()
        {
            // Fetch high-rated products
            var highRatedProducts = await _context.Products
                .Select(p => new
                {
                    Product = p,
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => (float)r.Rating) : 0
                })
                .OrderByDescending(x => x.AverageRating)
                .Take(4)
                .ToListAsync();

            // Fetch low-rated products with rating 1 or more
            var lowRatedProducts = await _context.Products
                .Select(p => new
                {
                    Product = p,
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => (float)r.Rating) : 0
                })
                .Where(x => x.AverageRating >= 1)
                .OrderBy(x => x.AverageRating)
                .Take(4)
                .ToListAsync();

            // Combine results
            var combinedProducts = highRatedProducts.Concat(lowRatedProducts).ToList();

            // Prepare the result list
            var highAndLowRatedProducts = new List<HighAndLowProductsRatedViewModel>();

            // Calculate ProfitOrLoss for each product asynchronously
            foreach (var product in combinedProducts)
            {
                var profitOrLoss = await CalculateProfitOrLossAsync(product.Product.Id);

                highAndLowRatedProducts.Add(new HighAndLowProductsRatedViewModel
                {
                    Id = product.Product.Id,
                    Title = product.Product.Title,
                    MainImageThumbnailUrl = product.Product.MainImageThumbnailUrl,
                    TotalRate = product.AverageRating,
                    ProfitOrLoss = profitOrLoss
                });
            }

            return highAndLowRatedProducts;
        }





        public async Task<IEnumerable<MostBuyingProductsViewModel>> GetMostBuyingProductsAsync(int count)
        {
            // Fetch products ordered by the total quantity sold
            var products = await _context.Products
                .Select(p => new
                {
                    Product = p,
                    TotalSoldQuantity = _context.OrderDetails
                        .Where(od => od.ProductId == p.Id)
                        .Sum(od => od.Count),
                    AverageRating = p.Reviews != null && p.Reviews.Any()
                        ? p.Reviews.Average(r => (float)r.Rating)
                        : 0 // Default to 0 if Reviews is null or empty
                })
                .OrderByDescending(p => p.TotalSoldQuantity)
                .Take(count)
                .ToListAsync();

            var viewModels = new List<MostBuyingProductsViewModel>();

            foreach (var product in products)
            {
                viewModels.Add(new MostBuyingProductsViewModel
                {
                    Id = product.Product.Id,
                    Title = product.Product.Title,
                    MainImageThumbnailUrl = product.Product.MainImageThumbnailUrl,
                    TotalRate = product.AverageRating,
                    ProfitOrLoss = await CalculateProfitOrLossAsync(product.Product.Id)
                });
            }

            return viewModels;
        }




        public async Task<IEnumerable<StockReportViewModel>> GetStockReportAsync(int count)
        {
            return await _context.Products
                .Select(p => new StockReportViewModel
                {
                    ProductTitle = p.Title,
                    ProductId = p.Id,
                    CreatedOn = p.CreatedOn,
                    Price = p.Price,
                    Status = p.Quantity > 3 ? "In Stock" : p.Quantity == 0 ? "Out of Stock" : "Low Stock",
                    Quantity = p.Quantity
                })
                //.OrderBy(p => p.Status)
                .Take(count)
                .ToListAsync();
        }

		public async Task<int> GetTotalItemsInStockAsync()
		{
			return await _context.Products.SumAsync(p => p.Quantity);
		}

		private async Task<float> CalculateProfitOrLossAsync(int productId)
        {
            // Fetch the product to get the cost price
            var product = await _context.Products
                .Where(p => p.Id == productId)
                .Select(p => new
                {
                    p.CostPrice  // Cost price of the product
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                // Handle the case where the product does not exist
                throw new InvalidOperationException("Product not found");
            }

            // Fetch all order details for the specified product
            var orderDetails = await _context.OrderDetails
                .Where(od => od.ProductId == productId)
                .ToListAsync();

            // Calculate total cost using CostPrice from the product table
            float totalCost = orderDetails.Sum(od => od.Count * product.CostPrice);

            // Calculate total selling price using Price from OrderDetails
            float totalSellingPrice = orderDetails.Sum(od => od.Count * od.Price);

            // Calculate profit or loss
            float profitOrLoss = totalSellingPrice - totalCost;

            return profitOrLoss;
        }


        // Reports
        public async Task<PaginatedList<ProductReportViewModel>> GetProductsReportAsync(IList<int>? selectedCategories, int pageSize, string? stock, int? pageNumber = null)
		{
			IQueryable<Product> query = _context.Products
				.Include(p => p.Category);

			if (selectedCategories != null && selectedCategories.Any())
			{
				query = query.Where(p => selectedCategories.Contains(p.CategoryId));
			}

			if (stock == "OutOfStock")
			{
				query = query.Where(p => p.Quantity == 0);
			}
			else if (stock == "LowStock")
			{
				query = query.Where(p => p.Quantity > 0 && p.Quantity <= 3);
			}
			else if (stock == "InStock")
			{
				query = query.Where(p => p.Quantity > 3);
			}

			var products = query
				.Select(p => new ProductReportViewModel
				{
					Id = p.Id,
					Title = p.Title,
					Quantity = p.Quantity,
					Price = p.Price,
					DiscountValue = p.DiscountValue,
					MainImageThumbnailUrl = p.MainImageThumbnailUrl,
					CategoryName = p.Category.Name,
					CostPrice = p.CostPrice
				});

			return PaginatedList<ProductReportViewModel>.Create(products, pageNumber ?? 1, pageSize);
		}



	}

}
