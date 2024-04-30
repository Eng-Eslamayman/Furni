namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        IEnumerable<Category> GetActiveCategories();
    }
}
