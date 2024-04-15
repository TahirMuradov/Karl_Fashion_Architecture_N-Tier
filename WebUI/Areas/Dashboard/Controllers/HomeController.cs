using Bussines.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Dashboard.Controllers
{
        [Area(nameof(Dashboard))]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IOrderProductService _orderProductService;

        public HomeController(IOrderProductService orderProductService)
        {
            _orderProductService = orderProductService;
        }

        public IActionResult Index()
        {
            var data=_orderProductService.GetAllSoldProducts();


            return View(data.Data);
        }
    }
}
