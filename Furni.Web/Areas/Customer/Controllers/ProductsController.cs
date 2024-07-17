using AutoMapper;
using Furni.Models.Enums;
using Furni.Utility.Models;
using Furni.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Furni.Web.Areas.Customer.Controllers
{
	[Area(AppRoles.Customer)]
	public class ProductsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public IActionResult Index(int? categoryId, int? pageNumber)
		{
			if (User.Identity!.IsAuthenticated && User.IsInRole(AppRoles.Admin))
				return RedirectToAction(nameof(Index), controllerName: "Dashboard", new { area = AppRoles.Admin });
			var categories = _unitOfWork.Categories.GetActiveCategories();

			var viewModel = new ShopProductViewModel
			{
				Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories),
				//Products = _unitOfWork.Products.GetShopProducts(pageNumber ?? 1, (int)ReportsConfigurations.PageSize, categoryId)
			};

			return View(viewModel);
		}

        [AjaxOnly]
        [HttpGet]
        public IActionResult GetProducts(int? categoryId, int? pageNumber, string? searchTerm)
        {
            var products = _unitOfWork.Products.GetShopProducts(pageNumber ?? 1, (int)ReportsConfigurations.PageSize, categoryId, searchTerm);
            return PartialView("_ShopProducts", products);
        }




        //[HttpGet]
        //public IActionResult GetProducts(int? categoryId, int? pageNumber)
        //{
        //    var viewModel = new ShopProductViewModel
        //    {
        //    };

        //    if (pageNumber is not null)
        //        viewModel.Products = _unitOfWork.Products
        //                    .GetShopProducts(pageNumber ?? 0, (int)ReportsConfigurations.PageSize, categoryId);
        //    else
        //        viewModel.Products = _unitOfWork.Products
        //                    .GetShopProducts(1, (int)ReportsConfigurations.PageSize, categoryId);

        //    return PartialView("_ShopProducts", viewModel.Products);
        //}

        public IActionResult Details(int id)
		{
			if (User.Identity!.IsAuthenticated && User.IsInRole(AppRoles.Admin))
				return RedirectToAction(nameof(Index), controllerName: "Dashboard", new { area = AppRoles.Admin });

			var viewModel = _unitOfWork.Products.GetProduct(id);

			//TempData["ReviewData"] = new[]
			//{
			//	new { label = "5", inner_label = viewModel.ReviewData.ContainsKey(5) ? viewModel.ReviewData[5].ToString() : "0", value = viewModel.ReviewData.ContainsKey(5) ? viewModel.ReviewData[5] : 0, color = "#88b131" },
			//	new { label = "4", inner_label = viewModel.ReviewData.ContainsKey(4) ? viewModel.ReviewData[4].ToString() : "0", value = viewModel.ReviewData.ContainsKey(4) ? viewModel.ReviewData[4] : 0, color = "#99cc00" },
			//	new { label = "3", inner_label = viewModel.ReviewData.ContainsKey(3) ? viewModel.ReviewData[3].ToString() : "0", value = viewModel.ReviewData.ContainsKey(3) ? viewModel.ReviewData[3] : 0, color = "#ffcf02" },
			//	new { label = "2", inner_label = viewModel.ReviewData.ContainsKey(2) ? viewModel.ReviewData[2].ToString() : "0", value = viewModel.ReviewData.ContainsKey(2) ? viewModel.ReviewData[2] : 0, color = "#ff9f02" },
			//	new { label = "1", inner_label = viewModel.ReviewData.ContainsKey(1) ? viewModel.ReviewData[1].ToString() : "0", value = viewModel.ReviewData.ContainsKey(1) ? viewModel.ReviewData[1] : 0, color = "#e17a69" }
			//};

			return View(viewModel);
		}

	}
}
