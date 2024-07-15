using Furni.Utility.Models;
using Furni.Web.Extensions;
using Furni.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Furni.Web.Areas.Customer.Controllers
{
	[Area(AppRoles.Customer)]
	public class ReviewsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ReviewsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}

		[AjaxOnly]
		public async Task<IActionResult> GetReviewData(int id)
		{
			// Fetch updated reviewData from your repository or service
			var viewModel = await _unitOfWork.Reviews.GetReviewData(id);

			if (viewModel == null)
			{
				return StatusCode(500, "Error: Review data could not be retrieved.");
			}

			var reviewData = new[]
			{
				new { label = "5", inner_label = viewModel.ReviewCounts.ContainsKey(5) ? viewModel.ReviewCounts[5].ToString() : "0", value = viewModel.ReviewCounts.ContainsKey(5) ? viewModel.ReviewCounts[5] : 0, color = "#88b131" },
				new { label = "4", inner_label = viewModel.ReviewCounts.ContainsKey(4) ? viewModel.ReviewCounts[4].ToString() : "0", value = viewModel.ReviewCounts.ContainsKey(4) ? viewModel.ReviewCounts[4] : 0, color = "#99cc00" },
				new { label = "3", inner_label = viewModel.ReviewCounts.ContainsKey(3) ? viewModel.ReviewCounts[3].ToString() : "0", value = viewModel.ReviewCounts.ContainsKey(3) ? viewModel.ReviewCounts[3] : 0, color = "#ffcf02" },
				new { label = "2", inner_label = viewModel.ReviewCounts.ContainsKey(2) ? viewModel.ReviewCounts[2].ToString() : "0", value = viewModel.ReviewCounts.ContainsKey(2) ? viewModel.ReviewCounts[2] : 0, color = "#ff9f02" },
				new { label = "1", inner_label = viewModel.ReviewCounts.ContainsKey(1) ? viewModel.ReviewCounts[1].ToString() : "0", value = viewModel.ReviewCounts.ContainsKey(1) ? viewModel.ReviewCounts[1] : 0, color = "#e17a69" }
			};

			// Return the updated reviewData as JSON
			return Json(new { reviewData });
		}


		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SubmitReview(ReviewFormViewModel model)
		{
			if (User.Identity!.IsAuthenticated && User.IsInRole(AppRoles.Admin))
				return RedirectToAction(nameof(Index), controllerName: "Dashboard", new { area = AppRoles.Admin });

			if (ModelState.IsValid)
			{
				var userId = User.GetUserId();
				var newReview = await _unitOfWork.Reviews.AddReviewAsync(model, userId);

				return PartialView("_ReviewRow", newReview); 
			}

			// Return error messages if the model is invalid
			var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
			return Json(new { success = false, errors });
		}



	}
}
