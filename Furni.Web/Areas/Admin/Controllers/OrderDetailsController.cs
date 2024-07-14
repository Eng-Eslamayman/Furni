using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Furni.Web.Areas.Admin.Controllers
{
    [Area(AppRoles.Admin)]
    [Authorize(Roles = AppRoles.Admin)]
    public class OrderDetailsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {

            var orderDetail = _unitOfWork.OrderDetails.GetById(id);
            if (orderDetail is null)
                return NotFound();

            orderDetail.IsDeleted = !orderDetail.IsDeleted;
            orderDetail.LastUpdatedOn = DateTime.Now;
            _unitOfWork.Complete();

            return Ok(orderDetail.LastUpdatedOn.ToString());
        }
    }
}
