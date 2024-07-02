using AutoMapper;
using Furni.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

		public IActionResult Index(int? categoryId,
			int? pageNumber)
		{

			var categories = _unitOfWork.Categories.GetActiveCategories();

			var viewModel = new ShopProductViewModel
			{
				Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories)
			};

			if (pageNumber is not null)
				viewModel.Products = _unitOfWork.Products
											  .GetShopProducts(pageNumber ?? 0, (int)ReportsConfigurations.PageSize,categoryId)
											  .Select(p => new CustomArrivalProductViewModel
											  {
												  Title = p.Title,
												  Id = p.Id,
												  Price = p.Price,
												  ImageUrls = p.ImageUrls

											  }).ToList();
			return View(viewModel);
		}



		//public IActionResult GetAll()
		//{

		//}
	}
}
