using AutoMapper;
using Furni.Models.Enums;
using Furni.Utility.Models;
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
            var categories = _unitOfWork.Categories.GetActiveCategories();

            var viewModel = new ShopProductViewModel
            {
                Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories),
                //Products = _unitOfWork.Products.GetShopProducts(pageNumber ?? 1, (int)ReportsConfigurations.PageSize, categoryId)
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult GetProducts(int? categoryId, int? pageNumber)
        {
            var products = _unitOfWork.Products.GetShopProducts(pageNumber ?? 1, (int)ReportsConfigurations.PageSize, categoryId);
            //var viewModel = new PaginatedList<CustomArrivalProductViewModel>(pageNumber ?? 0, (int)ReportsConfigurations.PageSize, categoryId);

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
            => View(_unitOfWork.Products.GetProduct(id));
    }
}
