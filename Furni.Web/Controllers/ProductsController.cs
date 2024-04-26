using AutoMapper;
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
            return View("Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
                View("Form", model);

            var product = _mapper.Map<Product>(model);

            if (model.Image is not null)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(model.Image, imageName, "/images/products", hasThumbnail: true);
                if (isUploaded)
                {
                    ModelState.AddModelError(nameof(Image), errorMessage!);
                    return View("Form");
                }

                product.ImageUrl = $"/images/products/{imageName}";
                product.ImageThumbnailUrl = $"/images/products/thumb/{imageName}";

            }

            //book.CreatedById = User.GetUserId();

            _unitOfWork.Products.Add(product);
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


    }
}
