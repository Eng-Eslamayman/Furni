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

		public List<CustomArrivalProductViewModel> GetCustomArrivalProducts() =>
	            _context.Products.Include(p => p.ProductImages)
		            .Select(p => new CustomArrivalProductViewModel
		            {
			            Title = p.Title,
			            Id = p.Id,
			            Price = p.Price,
			            ImageUrls = p.ProductImages.Select(pi => pi.ImageUrl).ToList()
		            })
		            .Skip(6)
		            .Take(7)
		            .ToList();

	}
}
