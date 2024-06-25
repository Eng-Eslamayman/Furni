using Furni.DataAccess.Persistence.Repositories.IRepositories;

namespace Furni.DataAccess.Persistence.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

        public ICategoryRepository Categories => new CategoryRepository(_context);
        public IProductRepository Products => new ProductRepository(_context);
        public IShoppingCartRepository ShoppingCarts => new ShoppingCartRepository(_context);

        public int Complete()
		{
			return _context.SaveChanges();
		}
	}
}
