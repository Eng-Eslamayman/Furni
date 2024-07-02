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
					.Where(p => p.CategoryId == categoryId)
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

		public IList<CustomArrivalProductViewModel> GetShopProducts(int pageNumber, int pageSize,int? categoryId = null)
		{
			IQueryable<CustomArrivalProductViewModel> products;
			if (categoryId.HasValue)
			{
				 products = _context.Products.Include(p => p.Category).Include(p => p.ProductImages)
					.Where(p => p.CategoryId == categoryId)
					 .Select(p => new CustomArrivalProductViewModel
					 {
						 Title = p.Title,
						 Id = p.Id,
						 Price = p.Price,
						 ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).Skip(0).Take(2).ToList()
					 });
			}
			else
			{
				products = _context.Products.Include(p => p.Category).Include(p => p.ProductImages)
				 .Select(p => new CustomArrivalProductViewModel
				 {
					 Title = p.Title,
					 Id = p.Id,
					 Price = p.Price,
					 ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).Skip(0).Take(2).ToList()
				 });
			}
				

			return GetPaginatedList(products, pageNumber, pageSize);
		}
		public PaginatedList<CustomArrivalProductViewModel> GetPaginatedList(IQueryable<CustomArrivalProductViewModel> query, int pageNumber, int pageSize)
		{
			return PaginatedList<CustomArrivalProductViewModel>.Create(query, pageNumber, pageSize);
		}


	}

}
