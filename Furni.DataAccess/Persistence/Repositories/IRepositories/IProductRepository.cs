namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        IQueryable<Product> GetDetails();
        (IQueryable<Product> products, int count) GetFiltered(GetFilteredDto dto);
        List<CustomProductViewModel> GetCustomProducts();
        List<CustomArrivalProductViewModel> GetCustomArrivalProducts();

	}
}
