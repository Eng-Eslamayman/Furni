using Furni.Utility.Models;
using Furni.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Areas.Customer.Controllers
{
	[Area(AppRoles.Customer)]
	public class OrdersController : Controller
	{

		private readonly IUnitOfWork _unitOfWork;

		public OrdersController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			if (User.Identity!.IsAuthenticated && User.IsInRole(AppRoles.Admin))
				return RedirectToAction(nameof(Index), controllerName: "Dashboard", new { area = AppRoles.Admin });
			return View();
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> CheckOut()
		{
			var userId = User.GetUserId();

			var productCards = await _unitOfWork.ShoppingCarts.GetCartItemsAsync(userId);
			var order = await _unitOfWork.Orders.GetAsync(userId);

			if (productCards == null || order == null)
			{
				return NotFound();
			}

			ShoppingCartViewModel viewModel = new ShoppingCartViewModel
			{
				ProductCards = productCards,
				Order = order,
				TotalPrice = productCards.Sum(cart => ((cart.Price * (1.0f - (cart.DiscountValue / 100f))) * cart.Count))
			};

			return View(viewModel);
		}



	}
}
