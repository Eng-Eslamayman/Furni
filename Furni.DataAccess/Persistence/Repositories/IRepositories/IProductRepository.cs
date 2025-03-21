﻿using Furni.Utility.Dashboard;
using Furni.Utility.Reports;

namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        IQueryable<Product> GetDetails();
        (IQueryable<Product> products, int count) GetFiltered(GetFilteredDto dto);
        IQueryable<CustomArrivalProductViewModel> GetCustomArrivalProducts(int? categoryId = null);
        IEnumerable<ProductsAndCategoriesSearchViewModel> GetProductsSearch(string query, int skipCount);
		PaginatedList<CustomArrivalProductViewModel> GetShopProducts(int pageNumber, int pageSize, int? categoryId = null, string? searchTerm = null, string ? sortCriteria = null);
        List<CustomProductViewModel> GetCustomProducts();
        ProductDetailsViewModel GetProduct(int id);

        // Dashboard
        Task<IEnumerable<HighAndLowProductsRatedViewModel>> GetHighAndLowRatedProductsAsync();
        Task<IEnumerable<MostBuyingProductsViewModel>> GetMostBuyingProductsAsync(int count);
        Task<IEnumerable<StockReportViewModel>> GetStockReportAsync(int count);
		Task<int> GetTotalItemsInStockAsync();

        // Reports
        Task<PaginatedList<ProductReportViewModel>> GetProductsReportAsync(IList<int>? selectedCategories, int pageSize, string? stock, int? pageNumber = null);
	}

}
