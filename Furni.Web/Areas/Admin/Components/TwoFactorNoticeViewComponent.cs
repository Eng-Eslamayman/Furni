using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Areas.Admin.Components
{
    [Area("Admin")]
    public class TwoFactorNoticeViewComponent : ViewComponent
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public TwoFactorNoticeViewComponent(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}
        [HttpGet]
        [Route("Admin/TwoFactorNotice")]
        public async Task<IViewComponentResult> InvokeAsync()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);
			var isTwoFactorEnabled = user != null && await _userManager.GetTwoFactorEnabledAsync(user);

			var viewModel = new TwoFactorNoticeViewModel
			{
				IsTwoFactorEnabled = isTwoFactorEnabled
			};

			return View(viewModel);
		}
	}

}
