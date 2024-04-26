using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Controllers
{
	public class CategoriesController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
		[HttpGet]
        public IActionResult Index()
		{
			var categories = _unitOfWork.Categories.GetAll();
			return View(categories);
		}

		[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult Create()
		//{
		//	return View();
		//}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ToggleStatus(int id)
		{
			var category = _unitOfWork.Categories.GetById(id);
			if(category is null)
				return NotFound();

			category.IsDeleted = !category.IsDeleted;
			category.LastUpdatedOn = DateTime.Now;
			_unitOfWork.Complete();

			return Ok(category.LastUpdatedOn.ToString());
		}
	}
}
