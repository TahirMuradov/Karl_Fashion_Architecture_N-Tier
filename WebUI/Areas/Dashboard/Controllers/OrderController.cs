using Bussines.Abstract;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebUI.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly IShippingMethodsServices _shippingServices;
        private readonly IPaymentMethodServices _paymentMethods;

        public OrderController(IOrderServices orderServices, IShippingMethodsServices shippingServices, IPaymentMethodServices paymentMethods)
        {
            _orderServices = orderServices;
            _shippingServices = shippingServices;
            _paymentMethods = paymentMethods;
        }

        public IActionResult Index()
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            var orders = _orderServices.GetAllOrder();
            ViewBag.ShippingMethods = _shippingServices.GetShippingAll(langCode: currentCulture).Data;
            ViewBag.PaymentMethods = _paymentMethods.GetAllPaymentMethod(langCode:currentCulture).Data;
            string currentDomain = Request.Host.Value;
            ViewBag.CurrentDomain = currentDomain;
            return View(orders.Data);
        }
        [HttpPut]
        public IActionResult OrderStatusChange(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest("Id is null or empty");

            var result = _orderServices.ChangeOrderStatus(id);

            
            return result.IsSuccess? Ok(result):BadRequest(result);
        }
        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return BadRequest("Id is null or empty");
            var result=_orderServices.DeleteOrder(Id);
            return  result.IsSuccess?Ok(result):BadRequest(result);
        }
    }
}
