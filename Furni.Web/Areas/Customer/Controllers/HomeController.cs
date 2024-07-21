using Furni.Web.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics;

namespace Furni.Web.Areas.Customers.Controllers
{
    [Area(AppRoles.Customer)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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
            throw new Exception("My Exception");
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

        public IActionResult Error(int id = 500)
        {
            return View(new ErrorViewModel { ErrorCode = id, ErrorDescription = ReasonPhrases.GetReasonPhrase(id) });
        }

    }
}
