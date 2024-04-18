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

		public int Complete()
		{
			return _context.SaveChanges();
		}
	}
}
