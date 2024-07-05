using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Areas.Customer.Controllers
{
	[Area(AppRoles.Customer)]
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
