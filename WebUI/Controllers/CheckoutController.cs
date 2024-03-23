
using Bussines.Abstract;
using Core.Helper.EmailHelper;
using Core.Helper.EmailHelper.Concrete;
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
        private readonly IEmailHelper _emailHelper;

        public CheckoutController(UserManager<User> userManager, IHttpContextAccessor contextAccessor, IPaymentMethodServices paymentMethodServices, IShippingMethodsServices shippingMethodsServices, IEmailHelper emailHelper)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _paymentMethodServices = paymentMethodServices;
            _shippingMethodsServices = shippingMethodsServices;
            _emailHelper = emailHelper;
        }

        public IActionResult Index()
        {
            string currentUserId=string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                
             currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var currentCulture =Thread.CurrentThread.CurrentCulture.Name;
            if (Request.Cookies["ShippingMethods"] is null)
            {
                TempData["Error"] = "Catdirilma Novunu Secin!";
                
                return RedirectToAction(controllerName:"cart",actionName:"index");
            }
          
            if (User.Identity.IsAuthenticated)
            {
                

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
        public async Task<IActionResult> Index(ShippingMethodAndUserInfoDTO OrderInfo)
        {
            string currentUserId=string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                
             currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }


            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            if (Request.Cookies["ShippingMethods"] is null)
            {
                TempData["Error"] = "Catdirilma Novunu Secin!";

                return RedirectToAction(controllerName: "cart", actionName: "index");
            }

            if (User.Identity.IsAuthenticated)
            {


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
            var ChecekShipping= _shippingMethodsServices.GetShipping(shipping.Id, currentCulture).Data;
            ViewBag.ShippingMethod = ChecekShipping;
            ViewBag.CartItems = cartItems;
            ViewBag.PaymentMethods = _paymentMethodServices.GetAllPaymentMethod(currentCulture).Data;
            var checkedPaymentMethod=_paymentMethodServices.GetPaymentMethod(OrderInfo.PaymentsMethodId, currentCulture).Data;
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

         string pdfPath=   FileHelper.SaveOrderPdf(
                productInPDF,
                new ShippingMethodInOrderPdfDTO
                {
                    Id = ChecekShipping.Id,
                    Price = ChecekShipping.Price,
                    ShippingContent = ChecekShipping.Content


                },
                new PaymentMethodInOrderPdfDTO
                {
                    Id = checkedPaymentMethod.Id,
                    Content = checkedPaymentMethod.Content,
                }
                );
            if (pdfPath is not null)
            {
                var currentUser=await _userManager.FindByIdAsync(currentUserId);
                if (currentUser != null)
                {

                await _emailHelper.SendEmailPdfAsync(currentUser.Email, currentUser.UserName, pdfPath);
                }
                else
                {
                   await _emailHelper.SendEmailPdfAsync(OrderInfo.Email, OrderInfo.FirstName+OrderInfo.LastName, pdfPath);

                }

            }



            Response.Cookies.Delete("cart");

            return Redirect("/home/index");
        }
    }
}
