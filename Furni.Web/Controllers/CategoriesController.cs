using AutoMapper;
using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Web.Core.ViewModels;
using Furni.Web.Extensions;
using Furni.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var categories = _unitOfWork.Categories.GetAll();
			ActionName();
            return View(_mapper.Map<IEnumerable<CategoryViewModel>>(categories));
        }
        [AjaxOnly]
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var category = _mapper.Map<Category>(model);
            //category.CreatedById = User.GetUserId();

            _unitOfWork.Categories.Add(category);
            _unitOfWork.Complete();

            var viewModel = _mapper.Map<CategoryViewModel>(category);

            return PartialView("_CategoryRow", viewModel);
        }
        [AjaxOnly]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _unitOfWork.Categories.GetById(id);

            if (category is null)
                return NotFound();

            var viewModel = _mapper.Map<CategoryFormViewModel>(category);

            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var category = _unitOfWork.Categories.GetById(model.Id);

            if (category is null)
                return NotFound();

            category = _mapper.Map(model, category);
            //category.LastUpdatedById = User.GetUserId();
            category.LastUpdatedOn = DateTime.Now;

            _unitOfWork.Complete();

            var viewModel = _mapper.Map<CategoryViewModel>(category);

            return PartialView("_CategoryRow", viewModel);
        }

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
        public IActionResult AllowItem(CategoryFormViewModel model)
        {
            var category = _unitOfWork.Categories.Find(c => c.Name == model.Name);
            var isAllowed = category is null || category.Id.Equals(model.Id);

            return Json(isAllowed);
        }


        private void ActionName()
        {
            ViewBag.ControllerName = RouteData.Values["controller"]!.ToString();
            ViewBag.ActionName = RouteData.Values["action"]!.ToString();
        }

    }
}
