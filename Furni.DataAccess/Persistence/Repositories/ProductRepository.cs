using Furni.DataAccess.Persistence.Repositories.IRepositories;

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

		public PaginatedList<CustomArrivalProductViewModel> GetShopProducts(int pageNumber, int pageSize,int? categoryId = null)
		{
			IQueryable<CustomArrivalProductViewModel> products;
			if (categoryId.HasValue)
			{
				 products = _context.Products
					.Include(p => p.Category)
					.Include(p => p.ProductImages)
					.Where(p => p.CategoryId == categoryId)
					 .Select(p => new CustomArrivalProductViewModel
					 {
						 Title = p.Title,
						 Id = p.Id,
						 Price = p.Price,
						 ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).Skip(0).Take(2).ToList(),
					 });
			}
			else
			{
				products = _context.Products
					 .Include(p => p.Category)
					 .Include(p => p.ProductImages)
					 .Select(p => new CustomArrivalProductViewModel
					 {
						 Title = p.Title,
						 Id = p.Id,
						 Price = p.Price,
						 ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).Skip(0).Take(2).ToList(),
					 });
			}
				

			return PaginatedList<CustomArrivalProductViewModel>.Create(products, pageNumber, pageSize);
		}


        public ProductDetailsViewModel? GetProduct(int id)
        {
            var product = _context?.Products
                .Where(p => p.Id == id)
                .Include(p => p.ProductImages)
                .Select(p => new
                {
                    p.Title,
                    p.Id,
                    p.Price,
                    p.Description,
                    p.Quantity,
                    p.DiscountValue,
                    ProductImages = p.ProductImages.Select(pi => pi.ImageUrl).ToList()
                })
                .FirstOrDefault(); // Find Does not work with eager loading Include. but it is more efficient. than First, and Single will give an exception if there more one product and less efficient. 

            if (product == null)
            {
                return null;
            }

            return new ProductDetailsViewModel
            {
                Title = product.Title,
                Id = product.Id,
                Price = product.Price,
                Description = product.Description,
                Quantity = product.Quantity,
                DiscountValue = product.DiscountValue,
                ImageUrls = product.ProductImages ?? new List<string>()
            };
        } 

    }

}
