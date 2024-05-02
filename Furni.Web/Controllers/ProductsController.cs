﻿using AutoMapper;
using Furni.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

namespace Furni.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public IActionResult Index()
        {
			var productList = _unitOfWork.Products.GetAll();
            var viewModel = _mapper.Map<IEnumerable<ProductViewModel>>(productList);
			PopulateAction();
			return View(viewModel);
        }


        public IActionResult Details(int id)
        {
            var query = _unitOfWork.Products.GetDetails();

            var viewModel = _mapper.ProjectTo<ProductViewModel>(query).SingleOrDefault(b => b.Id == id);

            if (viewModel is null)
                return NotFound();

            return View(viewModel);
        }


        public IActionResult Create()
        {
			PopulateAction();
			return View("Form", PopulateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
                View("Form", PopulateViewModel(model));

            var product = _mapper.Map<Product>(model);

            if (model.Image is not null)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(model.Image, imageName, "/images/products", hasThumbnail: true);
                if (!isUploaded)
                {
                    ModelState.AddModelError(nameof(Image), errorMessage!);
                    return View("Form", PopulateViewModel(model));
                }

                product.ImageUrl = $"/images/products/{imageName}";
                product.ImageThumbnailUrl = $"/images/products/thumb/{imageName}";

            }

            //book.CreatedById = User.GetUserId();

            _unitOfWork.Products.Add(product);
            _unitOfWork.Complete();

            return RedirectToAction(nameof(Details), new { id = product.Id });
        }


        public IActionResult Edit(int id)
        {
            var product = _unitOfWork.Products.Find(p => p.Id == id);
            if (product is null)
                return NotFound();

            var model = _mapper.Map<ProductFormViewModel>(product);
            var viewModel = PopulateViewModel(model);
            PopulateAction();

			return View("Form", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
                View("Form", PopulateViewModel(model));

            var product = _mapper.Map<Product>(model);


            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    _imageService.Delete(product.ImageUrl, product.ImageThumbnailUrl);
                }

                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var imagePath = "/images/books";

                var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(model.Image, imageName, "/images/products", hasThumbnail: true);


                if (!isUploaded)
                {
                    ModelState.AddModelError(nameof(Image), errorMessage!);
                    return View("Form", PopulateViewModel(model));
                }

                model.ImageUrl = $"{imagePath}/{imageName}";
                model.ImageThumbnailUrl = $"{imagePath}/thumb/{imageName}";
            }
            else if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                model.ImageUrl = product.ImageUrl;
                model.ImageThumbnailUrl = product.ImageThumbnailUrl;
            }

            product = _mapper.Map(model, product);

            _unitOfWork.Complete();

            return RedirectToAction(nameof(Details), new { id = product.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var product = _unitOfWork.Products.GetById(id);

            if (product is null)
                return NotFound();

            product.IsDeleted = !product.IsDeleted;
            //product.CreatedById = User.GetUserId();
            product.LastUpdatedOn = DateTime.Now;
            _unitOfWork.Complete();
            return Ok(product.LastUpdatedOn.ToString());
        }


        public IActionResult AllowItem(ProductFormViewModel model)
        {
            var product = _unitOfWork.Products.Find(b => b.Title == model.Title);
            var isAllowed = product is null || product.Id.Equals(model.Id);

            return Json(isAllowed);
        }


        private ProductFormViewModel PopulateViewModel(ProductFormViewModel? model = null)
        {
            ProductFormViewModel viewModel = model is null ? new ProductFormViewModel() : model;

            var categories = _unitOfWork.Categories.GetActiveCategories();

            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);

            return viewModel;
        }

        private void PopulateAction()
        {
			ViewBag.ControllerName = RouteData.Values["controller"]!.ToString();
			ViewBag.ActionName = RouteData.Values["action"]!.ToString();
		}

    }
}
