namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        IEnumerable<Category> GetActiveCategories();
        List<CustomCategoryViewModel> GetCustomCategories();
        IEnumerable<ProductsAndCategoriesSearchViewModel> GetCategoryNames(string query, int count);

	}
}
