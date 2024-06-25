using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Areas.Customer.Controllers
{
	[Area(AppRoles.Customer)]
	public class ProductsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var products = _unitOfWork.Products.GetAll();
			return View();
		}

		//public IActionResult GetAll()
		//{

		//}
	}
}
