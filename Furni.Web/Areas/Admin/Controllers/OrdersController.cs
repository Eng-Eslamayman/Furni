using AutoMapper;
using Furni.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Areas.Admin.Controllers
{
    [Area(AppRoles.Admin)]
    [Authorize(Policy = "InitialAccessPolicy")]
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _unitOfWork.Orders.GetDetails(id);

            if(viewModel is null) return NotFound();    

            return View(viewModel);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id, string status)
        {
            if(status is null)
                return NotFound();


            var order = _unitOfWork.Orders.GetById(id);
            if (order is null)
            {
                return NotFound();
            }

            // Convert status to uppercase
            var statusUpper = status.ToUpper();

            // Define a list of valid statuses in uppercase
            var validStatuses = new HashSet<string>
                        {
                            "PENDING",
                            "APPROVED",
                            "DELIVERING",
                            "COMPLETED",
                            "DENIED"
                        };

            // Check if the provided status is valid
            if (!validStatuses.Contains(statusUpper))
            {
                return NotFound();
            }

            // Update order status
            order.OrderStatus = status;
            order.LastUpdatedOn = DateTime.Now;

            _unitOfWork.Complete();

            return Ok(status);
        }


        [HttpGet]
        public IActionResult LoadStatusOptionsPartial()
        {
            return PartialView("_StatusOptions");
        }


        private void ActionName()
        {
            ViewBag.ControllerName = RouteData.Values["controller"]!.ToString();
            ViewBag.ActionName = RouteData.Values["action"]!.ToString();
        }
    }
}
