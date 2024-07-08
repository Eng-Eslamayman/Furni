﻿using Furni.DataAccess.Persistence.Repositories.IRepositories;
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

			var product = await _context.Products
				.FirstOrDefaultAsync(p => p.Id == productId);

			if (product == null)
			{
				// Handle the case where the product is not found
				return;
			}

			int availableQuantity = product.Quantity;
			if (cartItem == null)
			{
				int adjustedCount = Math.Min(availableQuantity, count);
				cartItem = new ShoppingCart
				{
					ApplicationUserId = userId,
					ProductId = productId,
					Count = adjustedCount
				};
				_context.ShoppingCarts.Add(cartItem);
			}
			else
			{
				int newCount = cartItem.Count + count;
				cartItem.Count = Math.Min(availableQuantity, newCount);
			}

			// Ensure the count does not fall below zero
			if (cartItem.Count < 0)
			{
				cartItem.Count = 0;
			}

			if(cartItem.Count == 0)
			{
				_context.ShoppingCarts.Remove(cartItem);
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

		public async Task RemoveCardAsync(int productId)
		{
			var card = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.ProductId == productId);

            if (card is not null)
			{
				_context.ShoppingCarts.Remove(card);
				await _context.SaveChangesAsync();
			}

		}
	}
}
