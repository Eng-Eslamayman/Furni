using AutoMapper;
using Furni.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Areas.Admin.Controllers
{
    [Area(AppRoles.Admin)]
    [Authorize(Roles = AppRoles.Admin)]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ActionName();
            return View();
        }


        [HttpPost, IgnoreAntiforgeryToken]
        public IActionResult GetOrders()
        {
            var filterDto = Request.Form.GetFilters();

            var (orders, recordsTotal) = _unitOfWork.Orders.GetFiltered(filterDto);

            // Data that i want in the page
            var data = orders.GetDataPage(filterDto);

            var recordsFiltered = orders.Count(); // Records After Filter

            var jsonData = new { recordsFiltered, recordsTotal, data  }; // recordsTotal => All records in database

            return Ok(jsonData);
        }




        private void ActionName()
        {
            ViewBag.ControllerName = RouteData.Values["controller"]!.ToString();
            ViewBag.ActionName = RouteData.Values["action"]!.ToString();
        }
    }
}
