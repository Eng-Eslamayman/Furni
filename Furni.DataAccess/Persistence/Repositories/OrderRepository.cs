using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Utility.Dashboard;
using Microsoft.AspNetCore.Http.HttpResults;
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
                Address = model.Address!,
                City = model.City!,
                State = model.State!,
                PostalCode = model.PostalCode!,
                Name = model.FullName!,
                Email = model.Email!,
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


        // Admin Side
        public (IQueryable<CustomerListingViewModel> orders, int count) GetFiltered(GetFilteredDto dto)
        {
            IQueryable<CustomerListingViewModel> orders = 
                _context.Orders.Include(o => o.ApplicationUser)
                .Select(order => new CustomerListingViewModel
                {
                    Id = order.Id,
                    FullName = order.ApplicationUser!.FullName!,
                    Email = order.Email!,
                    ImageThumbnailUrl = order.ApplicationUser!.ImageThumbnailUrl,
                    IsDeleted = order.ApplicationUser!.IsDeleted,
                    CreatedOn = order.CreatedOn,
                    Price = order.OrderTotal,
                });

            if (!string.IsNullOrEmpty(dto.SearchValue))
                orders = orders.Where(b => b.FullName!.Contains(dto.SearchValue!) || b.Email!.Contains(dto.SearchValue!));

            orders = orders.OrderBy($"{dto.SortColumn} {dto.SortColumnDirection}");

            var recordsTotal = _context.Products.Count(); // All Records in Database

            return (orders, recordsTotal);
        }

        public async Task<OrderDetailsViewModel> GetDetails(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailsViewModel
                {
                    Id = o.Id,
                    PhoneNumber = o.PhoneNumber,
                    Address = o.Address,
                    City = o.City,
                    State = o.State,
                    PostalCode = o.PostalCode,
                    Name = o.Name,
                    Email = o.Email,
                    OrderStatus = o.OrderStatus,
                    OrderDate = o.OrderDate,
                    ShippingDate = o.ShippingDate,
                    OrderTotal = o.OrderTotal,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailViewModel
                    {
                        Id = od.Id,
                        ProductTitle = od.Product!.Title,
                        ProductMainImageThumbnailUrl = od.Product.MainImageThumbnailUrl,
                        OrderId = od.OrderId,
                        ProductId = od.ProductId,
                        Count = od.Count,
                        Price = od.Price,
                        IsDeleted = od.IsDeleted,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return order ?? new OrderDetailsViewModel(); // Handle the case when order is not found
        }



        // Dashboard
        public async Task<int> GetAverageOrdersPerDayAsync()
        {
            // Define the time frame for the last month
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-30);

            // Get the total number of orders in the last month
            var totalOrders = await _context.Orders
                .Where(o => o.CreatedOn >= startDate && o.CreatedOn <= endDate)
                .CountAsync();

            // Calculate the number of days in the time frame
            var totalDays = (endDate - startDate).TotalDays;

            // Ensure totalDays is not zero to avoid division by zero
            if (totalDays <= 0)
            {
                return 0; // or handle this case appropriately
            }

            // Calculate average orders per day
            var averageOrdersPerDay = (float)totalOrders / (float)totalDays;

            // Use Math.Ceiling to round up to the next whole number if there's any order
            return totalOrders > 0 ? (int)Math.Ceiling(averageOrdersPerDay) : 0;
        }




        public async Task<int> GetTotalOrdersThisMonthAsync()
        {
            return await _context.Orders
                .CountAsync(o => o.CreatedOn.Month == DateTime.Now.Month && o.CreatedOn.Year == DateTime.Now.Year);
        }

        public async Task<IEnumerable<RecentOrdersViewModel>> GetRecentOrdersAsync()
        {
            return await _context.Orders
                .OrderByDescending(o => o.CreatedOn)
                .Take(3)
                .Select(o => new RecentOrdersViewModel
                {
                    ProductId = o.OrderDetails.FirstOrDefault().ProductId,
                    Title = _context.Products.Where(p => p.Id == o.OrderDetails.FirstOrDefault().ProductId).Select(p => p.Title).FirstOrDefault(),
                    MainImageThumbnailUrl = _context.Products.Where(p => p.Id == o.OrderDetails.FirstOrDefault().ProductId).Select(p => p.MainImageThumbnailUrl).FirstOrDefault(),
                    Quantity = o.OrderDetails.FirstOrDefault().Count,
                    Price = o.OrderDetails.FirstOrDefault().Price,
                    TotalPrice = o.OrderDetails.FirstOrDefault().Count * o.OrderDetails.FirstOrDefault().Price
                })
                .ToListAsync();
        }
    }
}
