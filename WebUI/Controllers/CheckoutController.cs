
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
using System.Net;
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
        private readonly IProductServices _productServices;
        private readonly UserManager<User> _userManager;
        private readonly IEmailHelper _emailHelper;
        private readonly IOrderServices _orderServices;

        public CheckoutController(UserManager<User> userManager, IHttpContextAccessor contextAccessor, IPaymentMethodServices paymentMethodServices, IShippingMethodsServices shippingMethodsServices, IEmailHelper emailHelper, IProductServices productServices, IOrderServices orderServices)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _paymentMethodServices = paymentMethodServices;
            _shippingMethodsServices = shippingMethodsServices;
            _emailHelper = emailHelper;
            _productServices = productServices;
            _orderServices = orderServices;
        }
        [HttpGet]
        public IActionResult Index()
        {
            string cookies = Request.Cookies["cart"];
            if (cookies is null)
                return Redirect("/shop/index");
            string currentUserId = string.Empty;
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
      
            List<CartAddItemCookieDTO> cartItems = new List<CartAddItemCookieDTO>();
            if (cookies != null)
            {

                cartItems = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cookies);
            }

            string cookiesShippingMethod = Request.Cookies["ShippingMethods"];
            ViewBag.ShippingMethod = JsonSerializer.Deserialize<GetShippingMethodDTO>(cookiesShippingMethod);
            ViewBag.CartItems = cartItems;
            ViewBag.PaymentMethods = _paymentMethodServices.GetAllPaymentMethod(currentCulture).Data;
         
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ShippingMethodAndUserInfoDTO OrderInfo)
        {

            string cookies = Request.Cookies["cart"];
            if (cookies is null)
                return Redirect("/shop/index");
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
          
       
            List<CartAddItemCookieDTO> cartItems = new List<CartAddItemCookieDTO>();
            if (cookies != null)
            {

                cartItems = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cookies);
            }

            HttpContext.Response.Cookies.Delete("cart");
            var shipping = JsonSerializer.Deserialize<GetShippingMethodDTO>(Request.Cookies["ShippingMethods"]);
            var ChecekShipping= _shippingMethodsServices.GetShipping(shipping.Id, currentCulture).Data;
            ViewBag.ShippingMethod = ChecekShipping;
            ViewBag.CartItems = cartItems;
            ViewBag.PaymentMethods = _paymentMethodServices.GetAllPaymentMethod(currentCulture).Data;
            var checkedPaymentMethod=_paymentMethodServices.GetPaymentMethod(OrderInfo.PaymentsMethodId, currentCulture).Data;
            if (!OrderInfo.ConfirmedDataInUser)
            {
                ModelState.AddModelError("Error", "Məlumatların duzgunluyunu tesdiq edin!");
                return View();
            }
            if (!ModelState.IsValid)
            {
               

                return View();
            }
            foreach (var product in cartItems)
            {
                var checkedProduct = _productServices.GetProductDetailUI(product.Id, currentCulture);
                if (checkedProduct is null)
                {
                  
                    ModelState.AddModelError("Error","Mehsullar Duzgun Gelmedi Yeniden Mehsul secin");
                    return RedirectToAction("Index");
                }
               List<int> size= new List<int>();
                List<int> count= new List<int>();
                size.Add(int.Parse( product.Size));
                count.Add(checkedProduct.Data.Product_Size.GetValueOrDefault(size[0])- product.Quantity);
                _productServices.UpdateProduct(new Entities.DTOs.ProductDTOs.ProductUpdateDTO
                {
                    Id = checkedProduct.Data.ProductID,
                    ProductSizes = size,
                    SizesCount = count,

                });

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
        

         List<string> pdfInfo=   FileHelper.SaveOrderPdf(
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
            _orderServices.AddOrder(new Entities.DTOs.OrderDTOs.OrderAddDTO
            {
                FirstName = OrderInfo.FirstName,
                LastName = OrderInfo.LastName,
                Address = OrderInfo.Address,
                Email = OrderInfo.Email,
                Message = OrderInfo.Message,
                PaymentMethodId = OrderInfo.PaymentsMethodId,
                ShippingMethodId = OrderInfo.ShippingMethodId,
                PhoneNumber = OrderInfo.PhoneNumber,
                OrderNumber = pdfInfo[1],
                OrderPDfUrl = pdfInfo[2],
                OrderProducts= cartItems.Select(x=>new OrderProduct
                {
                    Amount=x.Price,
                    Count=x.Quantity,
                    ProductCode=x.ProductCode,
                    ProductName=x.ProductName,
                    ProductId=Guid.Parse( x.Id),
                    Size=x.Size,
                    
                    
                }).ToList(),

            });
            if (pdfInfo is not null)
            {
                var currentUser=await _userManager.FindByIdAsync(currentUserId);
                if (currentUser != null)
                {

              var result=  await _emailHelper.SendEmailPdfAsync(currentUser.Email, currentUser.UserName, pdfInfo[0]);
                }
                else
                {
                 var result=  await _emailHelper.SendEmailPdfAsync(OrderInfo.Email, OrderInfo.FirstName+OrderInfo.LastName, pdfInfo[0]);

                }

            }
            ViewBag.Succes = true;


            //HttpClient httpClient = new HttpClient();  
            //   var response= await httpClient.GetAsync("https://localhost:7237/cart/ClearCart");

            //await Console.Out.WriteLineAsync(response.StatusCode.ToString());

            return View();
        }
    
    }
}
