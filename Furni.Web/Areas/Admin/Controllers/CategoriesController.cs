using AutoMapper;
using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Models.Entities;
using Furni.Web.Core.ViewModels;
using Furni.Web.Extensions;
using Furni.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Furni.Web.Areas.Admin.Controllers
{
    [Area(AppRoles.Admin)]
	[Authorize(Roles = AppRoles.Admin)]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;


        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
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
            var category = new CategoryFormViewModel();
            return PartialView("_Form", category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var category = _mapper.Map<Category>(model);


            // Handle Image
            if (model.Image is not null)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(model.Image, imageName, "/images/categories", hasThumbnail: true);
                if (!isUploaded)
                {
                    ModelState.AddModelError(nameof(Image), errorMessage!);
                    return View("Form", model);
                }

                category.ImageUrl = $"/images/categories/{imageName}";
                category.ImageThumbnailUrl = $"/images/categories/thumb/{imageName}";

            }



            category.CreatedById = User.GetUserId();

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
        public async Task<IActionResult> Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var category = _unitOfWork.Categories.GetById(model.Id);

            if (category is null)
                return NotFound();

            if (model.Image is not null)
            {
                _imageService.Delete(category.ImageUrl, category.ImageThumbnailUrl);

                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var imagePath = "/images/categories";

                var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(model.Image, imageName, "/images/categories", hasThumbnail: true);


                if (!isUploaded)
                {
                    ModelState.AddModelError(nameof(Image), errorMessage!);
                    return View("Form", model);
                }

                model.ImageUrl = $"{imagePath}/{imageName}";
                model.ImageThumbnailUrl = $"{imagePath}/thumb/{imageName}";
            }
            else
            {
                model.ImageUrl = category.ImageUrl;
                model.ImageThumbnailUrl = category.ImageThumbnailUrl;
            }

            category = _mapper.Map(model, category);
            category.LastUpdatedById = User.GetUserId();
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
            if (category is null)
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