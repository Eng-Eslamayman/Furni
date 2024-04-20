using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Controllers
{
	public class CategoriesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
