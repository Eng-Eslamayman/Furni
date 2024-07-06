namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        IQueryable<Product> GetDetails();
        (IQueryable<Product> products, int count) GetFiltered(GetFilteredDto dto);
        IQueryable<CustomArrivalProductViewModel> GetCustomArrivalProducts(int? categoryId = null);
		PaginatedList<CustomArrivalProductViewModel> GetShopProducts(int pageNumber, int pageSize,int? categoryId = null);
        List<CustomProductViewModel> GetCustomProducts();
        ProductDetailsViewModel GetProduct(int id);

	}

}
