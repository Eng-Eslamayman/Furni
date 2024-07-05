using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni.Web.Areas.Customer.Components
{
	public class ShopCategoriesViewComponent : ViewComponent
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;
		public ShopCategoriesViewComponent(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}



		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = _unitOfWork.Categories.GetActiveCategories();
			var viewModel = _mapper.Map<IList<CategoryHeaderViewModel>>(categories);
			
			return View(viewModel);
		}
	}
}
