using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories
{
    public class ShoppingCartRepository : BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddProductToCartAsync(string userId, int productId, int count)
        {
            var cartItem = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new ShoppingCart
                {
                    ApplicationUserId = userId,
                    ProductId = productId,
                    Count = count // Use the provided count
                };
                _context.ShoppingCarts.Add(cartItem);
            }
            else
            {
                cartItem.Count += count; // Increment the existing count by the provided count
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IList<ProductCardViewModel>> GetCartItemsAsync(string userId)
        {
            var cartItems = await _context.ShoppingCarts
                .Where(c => c.ApplicationUserId == userId)
                .Include(c => c.Product)
                .ToListAsync();

            return cartItems.Select(c => new ProductCardViewModel
            {
                Id = c.Product.Id,
                Title = c.Product.Title,
                Count = c.Count,
                MainImageUrl = c.Product.MainImageUrl,
                Price = c.Product.Price,
                DiscountValue = c.Product.DiscountValue,
            }).ToList();
        }

    }
}
