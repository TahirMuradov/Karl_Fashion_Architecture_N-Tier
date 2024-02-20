using Bussines.Abstract;
using Entities.DTOs.ShippingMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    [Authorize(Roles = "Admin")]
    public class ShippingMethodController : Controller
    {
        private readonly IShippingMethodsServices _shippingServices;

        public ShippingMethodController(IShippingMethodsServices shippingServices)
        {
            _shippingServices = shippingServices;
        }

        public IActionResult Index()
        {
            string currentCulture=Thread.CurrentThread.CurrentCulture.Name;
            var result = _shippingServices.GetShippingAll(currentCulture).Data;


            return View(result);
        }


        public IActionResult Create()
        {
           
            return View();
        }


        [HttpPost]
        public IActionResult Create(AddShippingMethodDTO addShippingMethodDTO)
        {
            var result = _shippingServices.AddShipping(addShippingMethodDTO);

            if (result.IsSuccess)
            {
                
            return RedirectToAction("Index");
            }
            return View(result.Message);
        }
        public IActionResult Delete(string id)
        {
            var result=_shippingServices.DeleteShipping(id);

            return RedirectToAction("Index");
        }
    }
}
