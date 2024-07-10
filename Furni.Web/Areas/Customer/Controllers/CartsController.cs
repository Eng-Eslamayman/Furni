using Furni.Models.Entities;
using Furni.Utility.Models;
using Furni.Web.Extensions;
using Furni.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Furni.Web.Areas.Customer.Controllers
{
	[Area(AppRoles.Customer)]
	public class CartsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CartsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			if (User.Identity!.IsAuthenticated && User.IsInRole(AppRoles.Admin))
				return RedirectToAction(nameof(Index), controllerName: "Dashboard", new { area = AppRoles.Admin });
			return View();
		}



		//[HttpPost]
		//public IActionResult CheckOut(ShoppingCartViewModel viewModel)
		//{
		//	var claimsIdentity = (ClaimsIdentity)User.Identity;
		//	var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

		//	viewModel.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
		//	includeProperties: "Product");

		//	viewModel.OrderHeader.OrderDate = System.DateTime.Now;
		//	viewModel.OrderHeader.ApplicationUserId = userId;

		//	ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


		//	foreach (var cart in viewModel.ShoppingCartList)
		//	{
		//		cart.Price = GetPriceBasedOnQuantity(cart);
		//		viewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
		//	}

		//	if (applicationUser.CompanyId.GetValueOrDefault() == 0)
		//	{
		//		//it is a regular customer 
		//		viewModel.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
		//		viewModel.OrderHeader.OrderStatus = SD.StatusPending;
		//	}
		//	else
		//	{
		//		//it is a company user
		//		viewModel.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
		//		viewModel.OrderHeader.OrderStatus = SD.StatusApproved;
		//	}
		//	_unitOfWork.OrderHeader.Add(viewModel.OrderHeader);
		//	_unitOfWork.Save();
		//	foreach (var cart in viewModel.ShoppingCartList)
		//	{
		//		OrderDetail orderDetail = new()
		//		{
		//			ProductId = cart.ProductId,
		//			OrderHeaderId = viewModel.OrderHeader.Id,
		//			Price = cart.Price,
		//			Count = cart.Count
		//		};
		//		_unitOfWork.OrderDetail.Add(orderDetail);
		//		_unitOfWork.Save();
		//	}

		//	if (applicationUser.CompanyId.GetValueOrDefault() == 0)
		//	{
		//		//it is a regular customer account and we need to capture payment
		//		//stripe logic
		//		var domain = Request.Scheme + "://" + Request.Host.Value + "/";
		//		var options = new SessionCreateOptions
		//		{
		//			SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={viewModel.OrderHeader.Id}",
		//			CancelUrl = domain + "customer/cart/index",
		//			LineItems = new List<SessionLineItemOptions>(),
		//			Mode = "payment",
		//		};

		//		foreach (var item in viewModel.ShoppingCartList)
		//		{
		//			var sessionLineItem = new SessionLineItemOptions
		//			{
		//				PriceData = new SessionLineItemPriceDataOptions
		//				{
		//					UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
		//					Currency = "usd",
		//					ProductData = new SessionLineItemPriceDataProductDataOptions
		//					{
		//						Name = item.Product.Title
		//					}
		//				},
		//				Quantity = item.Count
		//			};
		//			options.LineItems.Add(sessionLineItem);
		//		}


		//		var service = new SessionService();
		//		Session session = service.Create(options);
		//		_unitOfWork.OrderHeader.UpdateStripePaymentID(viewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
		//		_unitOfWork.Save();
		//		Response.Headers.Add("Location", session.Url);
		//		return new StatusCodeResult(303);
		//	}

		//	return RedirectToAction(nameof(OrderConfirmation), new { id = viewModel.OrderHeader.Id });
		//}












		[Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int count)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            await _unitOfWork.ShoppingCarts.AddProductToCartAsync(userId, productId, count);

			return Json(new { success = true, message = "Product added to cart successfully." });
		}

		[Authorize]
		[HttpGet]
		[AjaxOnly]
		public async Task<IActionResult> GetCards()
		{
			var userId = User.GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}

			var cartItems = await _unitOfWork.ShoppingCarts.GetCartItemsAsync(userId);

			return PartialView("_Cards", cartItems);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> RemoveFromCart(int productId)
		{
			var userId = User.GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}
			await _unitOfWork.ShoppingCarts.RemoveCardAsync(productId);
			var cartItems = await _unitOfWork.ShoppingCarts.GetCartItemsAsync(userId);

			return PartialView("_Cards", cartItems);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Increase(int productId)
		{
			var userId = User.GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}
			await _unitOfWork.ShoppingCarts.AddProductToCartAsync(userId ,productId, 1);
			var cartItems = await _unitOfWork.ShoppingCarts.GetCartItemsAsync(userId);

			return PartialView("_Cards", cartItems);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Decrease(int productId)
		{
			var userId = User.GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}
			await _unitOfWork.ShoppingCarts.AddProductToCartAsync(userId, productId, -1);
			var cartItems = await _unitOfWork.ShoppingCarts.GetCartItemsAsync(userId);

			return PartialView("_Cards", cartItems);
		}

	}
}
