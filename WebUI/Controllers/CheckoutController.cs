
using Bussines.Abstract;
using Core.Helper.FileHelper;
using Entities.Concrete;
using Entities.DTOs.Cart;
using Entities.DTOs.CheckOutDTOs;
using Entities.DTOs.ShippingMethodsDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace WebUI.Controllers
{
    public class CheckoutController : Controller
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPaymentMethodServices _paymentMethodServices;
        private readonly IShippingMethodsServices _shippingMethodsServices;
        private readonly UserManager<User> _userManager;

        public CheckoutController(UserManager<User> userManager, IHttpContextAccessor contextAccessor, IPaymentMethodServices paymentMethodServices, IShippingMethodsServices shippingMethodsServices)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _paymentMethodServices = paymentMethodServices;
            _shippingMethodsServices = shippingMethodsServices;
        }

        public IActionResult Index()
        {
            var currentCulture=Thread.CurrentThread.CurrentCulture.Name;
            if (Request.Cookies["ShippingMethods"] is null)
            {
                TempData["Error"] = "Catdirilma Novunu Secin!";
                
                return RedirectToAction(controllerName:"cart",actionName:"index");
            }
          
            if (User.Identity.IsAuthenticated)
            {
                
            string currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentUser =_userManager.Users.FirstOrDefault(x=>x.Id==currentUserId);
            
            ViewBag.CurrentUserInfo=currentUser;
            }
            string cookies = Request.Cookies["cart"];
            List<CartAddItemCookieDTO> cartItems= new List<CartAddItemCookieDTO>();
            if (cookies != null)
            {

                cartItems = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cookies);
            }

            string cookiesShippingMethod = Request.Cookies["ShippingMethods"];
            ViewBag.ShippingMethod = JsonSerializer.Deserialize<GetShippingMethodDTO>(cookiesShippingMethod);
            ViewBag.CartItems= cartItems;
            ViewBag.PaymentMethods = _paymentMethodServices.GetAllPaymentMethod(currentCulture).Data;

            return View();
        }
        [HttpPost]
        public IActionResult Index(ShippingMethodAndUserInfoDTO OrderInfo)
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            if (Request.Cookies["ShippingMethods"] is null)
            {
                TempData["Error"] = "Catdirilma Novunu Secin!";

                return RedirectToAction(controllerName: "cart", actionName: "index");
            }

            if (User.Identity.IsAuthenticated)
            {

                string currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var currentUser = _userManager.Users.FirstOrDefault(x => x.Id == currentUserId);

                ViewBag.CurrentUserInfo = currentUser;
            }
            string cookies = Request.Cookies["cart"];
            List<CartAddItemCookieDTO> cartItems = new List<CartAddItemCookieDTO>();
            if (cookies != null)
            {

                cartItems = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cookies);
            }

            
            var shipping = JsonSerializer.Deserialize<GetShippingMethodDTO>(Request.Cookies["ShippingMethods"]);
            var ChecekShipping= _shippingMethodsServices.GetShipping(shipping.Id, currentCulture);
            ViewBag.ShippingMethod = ChecekShipping;
            ViewBag.CartItems = cartItems;
            ViewBag.PaymentMethods = _paymentMethodServices.GetAllPaymentMethod(currentCulture).Data;
            var checkedPaymentMethod=_paymentMethodServices.GetPaymentMethod(OrderInfo.PaymentsMethodId, currentCulture);
            if (!ModelState.IsValid)
            {
               

                return View();
            }
           
            List<GeneratePdfOrderProductDTO> productInPDF=  new List<GeneratePdfOrderProductDTO>();
          productInPDF.AddRange(cartItems.Select(x => new GeneratePdfOrderProductDTO
            {
              Price = x.Price,
              ProductCode = x.ProductCode,
              ProductName = x.ProductName,
              Quantity = x.Quantity,
              size=int.Parse( x.Size)
            }));

            //FileHelper.SaveOrderPdf(
            //    productInPDF,
            //    new ShippingMethodInOrderPdfDTO
            //    {
            //        Id = ChecekShipping.Data.Id,
            //        Price = ChecekShipping.Data.Price,
            //        ShippingContent = ChecekShipping.Data.Content


            //    },
            //    new PaymentMethodInOrderPdfDTO
            //    {
            //        Id = checkedPaymentMethod.Data.Id,
            //        Content = checkedPaymentMethod.Data.Content,
            //    }
            //    );
          


            return Redirect("/home/index");
        }
    }
}
