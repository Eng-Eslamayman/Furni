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
