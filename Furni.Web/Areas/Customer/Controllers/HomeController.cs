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
            var viewModel = new CustomerHomeViewModel
            {
                CustomProductViewModel = _unitOfWork
                                            .Products
                                            .GetCustomProducts()
                                            .Select(p => new CustomProductViewModel {
                                                Title = p.Title,
                                                Id = p.Id,
                                                Price = p.Price,
                                                MainImageUrl = p.MainImageUrl })
                                            .ToList(),
                CustomCategoryViewModel = _unitOfWork
                                            .Categories
                                            .GetCustomProducts()
                                            .Select(c => new CustomCategoryViewModel
                                            {
                                                Id = c.Id,
                                                Name = c.Name,
                                                ImageUrl = c.ImageUrl 
                                            })
                                            .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
