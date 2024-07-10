using Furni.Web.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Furni.Web.Areas.Customers.Controllers
{
    [Area(AppRoles.Customer)]
    //[Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Customer}")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
			if (User.Identity!.IsAuthenticated && User.IsInRole(AppRoles.Admin))
				return RedirectToAction(nameof(Index), controllerName: "Dashboard", new { area = AppRoles.Admin });

			var viewModel = new CustomerHomeViewModel
            {
                CustomProductViewModel = _unitOfWork
                                            .Products
                                            .GetCustomProducts(),
                CustomCategoryViewModel = _unitOfWork
                                            .Categories
                                            .GetCustomCategories(),
                CustomArrivalProductViewModel = _unitOfWork.Products
                                               .GetCustomArrivalProducts()
                                               .Skip(6).Take(7).ToList(),
			};

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Checkout()
        {
            return View();
        }
	}
}
