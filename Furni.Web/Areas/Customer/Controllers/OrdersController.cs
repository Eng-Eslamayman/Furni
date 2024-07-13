using Furni.Models.Entities;
using Furni.Utility.Models;
using Furni.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Text.Encodings.Web;

namespace Furni.Web.Areas.Customer.Controllers
{
	[Area(AppRoles.Customer)]
	public class OrdersController : Controller
	{

		private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailBodyBuilder _emailBodyBuilder;
        private readonly IEmailSender _emailSender;

        public OrdersController(IUnitOfWork unitOfWork, IEmailBodyBuilder emailBodyBuilder, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailBodyBuilder = emailBodyBuilder;
            _emailSender = emailSender;
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


		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CheckOut(ShoppingCartViewModel model)
		{
			var userId = User.GetUserId();

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var orderId = await _unitOfWork.Orders.CreateOrderAsync(userId, model.Order);
			var productCards = await _unitOfWork.ShoppingCarts.GetCartItemsAsync(userId);

			if (orderId != 0)
			{
				var domain = $"{Request.Scheme}://{Request.Host.Value}/";
				var options = new SessionCreateOptions
				{
					SuccessUrl = domain + $"Customer/Orders/OrderConfirmation?id={orderId}",
					CancelUrl = domain + $"Customer/Orders/CancelOrder?id={orderId}",
					LineItems = productCards.Select(item => new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(item.PriceAfterDiscount  * 100), 
							Currency = "usd",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Title
							}
						},
						Quantity = item.Count
					}).ToList(),
					Mode = "payment"
				};

				var service = new SessionService();
				Session session = await service.CreateAsync(options);

				await _unitOfWork.Orders.UpdateStripePaymentIDAsync(orderId, session.Id, session.PaymentIntentId);
				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);
			}

			//await ClearShoppingCartAsync(userId);
			return RedirectToAction(nameof(OrderConfirmation), new { id = orderId });
		}

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int id)
        {
            await _unitOfWork.Orders.DeleteOrderAsync(id);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public async Task<IActionResult> OrderConfirmation(int id)
		{
			// Retrieve order including ApplicationUser
			var order = _unitOfWork.Orders.GetById(id);

			if (order == null)
			{
				return NotFound(); // Handle case where order with given id is not found
			}

			var service = new SessionService();
			Session session = service.Get(order.SessionId);

			if (session.PaymentStatus.ToLower() == "paid")
			{
                // Update order with payment details
                await _unitOfWork.Orders.UpdateStripePaymentIDAsync(id, session.Id, session.PaymentIntentId);
                await _unitOfWork.Orders.UpdateStatusAsync(id, "approved", "approved");
			}

			// Clear session data
			//HttpContext.Session.Clear();

			var userId = User.GetUserId();
			if (userId == null)
			{
				return Unauthorized();
			}
			var user = _unitOfWork.ApplicationUsers.Find(u => u.Id == userId);

			// Send email notification
			var callbackUrl = Url.Page(
					"/Customer/Home",
					pageHandler: null,
					values: new { area = "Customer", controller = "Home", action = "Index" },
					protocol: Request.Scheme);

			// Set place holders to send it to email Body Builder to replace holders in html file
			var placeHolders = new Dictionary<string, string>()
				{
					{ "imageUrl", "https://res.cloudinary.com/dzqc5nfai/image/upload/v1717798788/qkp9tphwwlqkrmkdukpx.svg" },
					{ "header", $"Hey {user.FullName}, thanks for making order from furnihuture!" },
					{ "body", "Your Order will will arrive on {Date}" },
					{ "url", $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
					{ "linkTitle", "Buy more" }
				};

			// Email Body
			var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Email, placeHolders);

			// Send the email
			//await _emailSender.SendEmailAsync(user.Email, "Go to Furnihuture", body);

			// Remove items from Shopping Cart 
			await ClearShoppingCartAsync(order.ApplicationUserId);

			return View(order.Id);
		}


		[HttpPost]
		[Authorize]
		public async Task<IActionResult> UpdateShipingAddress(OrderFormViewModel model)
		{
            var userId = User.GetUserId();

            if (!ModelState.IsValid) 
                return View(model);


            await _unitOfWork.Orders.UpdateUserDetailsAsync(userId, model);
            return PartialView("_ShippingAddress", model);
        }


        private async Task ClearShoppingCartAsync(string userId)
        {
            await _unitOfWork.ShoppingCarts.ClearCartAsync(userId);
        }
    }
}
