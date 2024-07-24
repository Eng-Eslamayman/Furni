namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface IShoppingCartRepository : IBaseRepository<ShoppingCart>
    {
        Task AddProductToCartAsync(string userId, int productId, int count);
        Task<IList<ProductCardViewModel>> GetCartItemsAsync(string userId);
        Task RemoveCardAsync(int productId);
        Task ClearCartAsync(string userId);

        // Background Tasks
        Task<List<CartAdjustmentInfo>> AdjustCartItemCountsAsync();
    }
}
