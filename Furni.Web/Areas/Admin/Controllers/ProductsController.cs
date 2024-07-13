using AutoMapper;
using Furni.Web.Extensions;
using Furni.Web.Services;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;

namespace Furni.Web.Areas.Admin.Controllers
{
	[Area(AppRoles.Admin)]
	[Authorize(Roles = AppRoles.Admin)]
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
            ActionName();
            return View();
        }


        [HttpPost, IgnoreAntiforgeryToken]
        public IActionResult GetProducts()
        {
            var filterDto = Request.Form.GetFilters();

            var (products, recordsTotal) = _unitOfWork.Products.GetFiltered(filterDto);

            // Data that i want in the page
            var data = products.GetDataPage(filterDto);

            var mappedData = _mapper.ProjectTo<ProductViewModel>(data).ToList();

            var recordsFiltered = products.Count(); // Records After Filter

            var jsonData = new { recordsFiltered, recordsTotal, data = mappedData }; // recordsTotal => All records in database

            return Ok(jsonData);
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
            ActionName();
            return View("Form", PopulateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var product = _mapper.Map<Product>(model);

            // Handle Main Image
            if (model.Image is not null)
            {
                var mainImageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var (isMainUploaded, mainErrorMessage) = await _imageService.UploadeAsynce(model.Image, mainImageName, "/images/products", hasThumbnail: true);
                if (!isMainUploaded)
                {
                    ModelState.AddModelError(nameof(model.Image), mainErrorMessage!);
                    return View("Form", PopulateViewModel(model));
                }

                product.MainImageUrl = $"/images/products/{mainImageName}";
                product.MainImageThumbnailUrl = $"/images/products/thumb/{mainImageName}";
            }



            product.CreatedById = User.GetUserId();

            _unitOfWork.Products.Add(product);
            _unitOfWork.Complete();


            // Handle Additional Images
            if (images is not null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var imagePath = $"/images/products/{product.Id}/";
                    var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(image, imageName, imagePath, hasThumbnail: true);
                    if (!isUploaded)
                    {
                        ModelState.AddModelError(nameof(images), errorMessage!);
                        return View("Form", PopulateViewModel(model));
                    }

                    var productImage = new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = $"{imagePath}/{imageName}",
                        ImageThumbnailUrl = $"{imagePath}/thumb/{imageName}",
                    };

                    //if(product.ProductImages == null)
                    //    product.ProductImages = new List<ProductImage>();

                    _unitOfWork.ProductImages.Add(productImage);
                }
                _unitOfWork.Complete();
            }


            return RedirectToAction(nameof(Details), new { id = product.Id });
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _unitOfWork.Products.Find(p => p.Id == id);
            //var product = _unitOfWork.Products.Find(p => p.Id == id, include: p => (p.Include(pi => pi.ProductImages)));
            if (product is null)
                return NotFound();

            var model = _mapper.Map<ProductFormViewModel>(product);

            var (imageUrls, imageThumbnailUrls) = _unitOfWork.ProductImages.GetImagesUrl(id);
            model.ExistingImageUrls = imageUrls;
            model.ExistingImageThumbnailUrls = imageThumbnailUrls;

            var viewModel = PopulateViewModel(model);
            ActionName();

            return View("Form", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormViewModel model, List<IFormFile> images, string[] avatar_remove)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var product = _unitOfWork.Products.Find(b => b.Id == model.Id);

            if (product is null)
                return NotFound();

            // Handle Main Image
            if (model.Image is not null)
            {
                _imageService.Delete(product.MainImageUrl, product.MainImageThumbnailUrl);

                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var imagePath = "/images/products";

                var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(model.Image, imageName, "/images/products", hasThumbnail: true);

                if (!isUploaded)
                {
                    ModelState.AddModelError(nameof(model.Image), errorMessage!);
                    return View("Form", PopulateViewModel(model));
                }

                model.MainImageUrl = $"{imagePath}/{imageName}";
                model.MainImageThumbnailUrl = $"{imagePath}/thumb/{imageName}";
            }
            else
            {
                model.MainImageUrl = product.MainImageUrl;
                model.MainImageThumbnailUrl = product.MainImageThumbnailUrl;
            }


            // Handle Removal of Images
            if (avatar_remove[0] is not null)
            {
                var imageUrlsToRemove = avatar_remove[0].Split(',');
                foreach (var imageThumbnailUrl in imageUrlsToRemove)
                {
                    if (imageThumbnailUrl != null)
                    {
                        // Construct the corresponding ImageUrl from ImageThumbnailUrl
                        var imageUrl = imageThumbnailUrl.Replace("/thumb/", "/");
                        _imageService.Delete(imageUrl, imageThumbnailUrl); 
                        _unitOfWork.ProductImages.RemoveByThumbnailUrl(imageThumbnailUrl); 
                    }
                }
            }

            // Handle Additional Images
            if (images is not null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var imagePath = $"/images/products/{product.Id}/";
                    var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(image, imageName, imagePath, hasThumbnail: true);
                    if (!isUploaded)
                    {
                        ModelState.AddModelError(nameof(images), errorMessage!);
                        return View("Form", PopulateViewModel(model));
                    }

                    var productImage = new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = $"{imagePath}/{imageName}",
                        ImageThumbnailUrl = $"{imagePath}/thumb/{imageName}",
                    };

                    //if (product.ProductImages == null)
                    //    product.ProductImages = new List<ProductImage>();

                    _unitOfWork.ProductImages.Add(productImage);
                }
                _unitOfWork.Complete();
            }

            product.LastUpdatedById = User.GetUserId();
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
            product.CreatedById = User.GetUserId();
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

        private void ActionName()
        {
            ViewBag.ControllerName = RouteData.Values["controller"]!.ToString();
            ViewBag.ActionName = RouteData.Values["action"]!.ToString();
        }

    }
}