﻿using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories
{
	public class OrderRepository : BaseRepository<Order>, IOrderRepository
	{
		public OrderRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<OrderFormViewModel?> GetAsync(string userId)
		{
			return await _context.Users
								 .Where(u => u.Id == userId)
								 .Select(o => new OrderFormViewModel
								 {
									 FullName = o.FullName ?? string.Empty,
									 State = o.State ?? string.Empty,
									 Address = o.StreetNumber ?? string.Empty,
									 City = o.City ?? string.Empty,
									 PostalCode = o.PostalCode ?? string.Empty,
									 Email = o.Email ?? string.Empty,
									 PhoneNumber = o.PhoneNumber ?? string.Empty,
								 })
								 .FirstOrDefaultAsync();
		}



        public async Task UpdateUserDetailsAsync(string userId, OrderFormViewModel model)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.FullName = model.FullName ?? user.FullName;
                user.State = model.State ?? user.State;
                user.StreetNumber = model.Address ?? user.StreetNumber;
                user.City = model.City ?? user.City;
                user.PostalCode = model.PostalCode ?? user.PostalCode;
                user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CreateOrderAsync(string userId, OrderFormViewModel model)
        {
            var order = new Order
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now,
                ShippingDate = DateTime.Now.AddDays(5), // Example shipping date, adjust as necessary
                OrderStatus = "Pending",
                PaymentStatus = "Pending",
                PhoneNumber = model.PhoneNumber!,
                StreetNumber = model.Address!,
                City = model.City!,
                State = model.State!,
                PostalCode = model.PostalCode!,
                Name = model.FullName!,
                //CreatedById = userId,
                CreatedOn = DateTime.Now
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync(); // Save order to get its Id

            var orderDetails = _context.ShoppingCarts
                .Include(p => p.Product)
                .Where(pc => pc.ApplicationUserId == userId)
                .Select(cart => new OrderDetail
            {
                OrderId = order.Id, 
                ProductId = cart.Product!.Id,
                Count = cart.Count,
                Price = (float)Math.Round(cart.Product!.Price * (1f - (cart.Product.DiscountValue / 100f)), 2) // Round to 2 decimal places
            }).ToList();

            order.OrderTotal = orderDetails.Select(orderDetails => orderDetails.Count * orderDetails.Price).Sum();

            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();

            return order.Id;
        }

		public async Task UpdateStripePaymentIDAsync(int id, string sessionId, string paymentIntentId)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
			if (order == null)
			{
				return; // Handle the case where the order is not found
			}

			if (!string.IsNullOrEmpty(sessionId))
			{
				order.SessionId = sessionId;
			}

			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				order.PaymentIntentId = paymentIntentId;
				order.PaymentDate = DateTime.Now;
			}

			await _context.SaveChangesAsync();
		}

		public async Task UpdateStatusAsync(int id, string orderStatus, string? paymentStatus = null)
		{
			var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
			if (order == null)
			{
				return; // Handle the case where the order is not found
			}

			order.OrderStatus = orderStatus;
			if (!string.IsNullOrEmpty(paymentStatus))
			{
				order.PaymentStatus = paymentStatus;
			}

			await _context.SaveChangesAsync();
		}

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return;
            }

            // Remove order details
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Remove order
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
        }

    }
}
