using Bussines.Abstract;
using Entities.Concrete;
using Entities.DTOs.Cart;
using Entities.DTOs.ShippingMethodsDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Packaging;
using Org.BouncyCastle.Security.Certificates;
using System.Diagnostics;
using System.Text.Json;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IShippingMethodsServices _shippingMethodsServices;

        public CartController(IProductServices productServices, IShippingMethodsServices shippingMethodsServices)
        {
            _productServices = productServices;
            _shippingMethodsServices = shippingMethodsServices;
        }

        public IActionResult Index()
        {
            var Error = TempData["Error"] as string;
            if (Error is not  null)
            {
                ModelState.AddModelError("Error", Error);
            }

            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            var cartCokie = Request.Cookies["cart"];
            var shippingMethodCokie = Request.Cookies["ShippingMethods"];
            if (shippingMethodCokie is not null)
            {
                
            ViewBag.CurrentShippingMethods=JsonSerializer.Deserialize<GetShippingMethodDTO>(shippingMethodCokie);
            }
            ViewBag.ShippingMethods = _shippingMethodsServices.GetShippingAll(currentCulture).Data;
            List<CartAddItemCookieDTO> CurrentItemCart = new List<CartAddItemCookieDTO>();
            List<CartDetailDTO> CartItems = new List<CartDetailDTO>();
            if (cartCokie is not null)
            {

                CurrentItemCart = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cartCokie);

                foreach (var item in CurrentItemCart)
                {
                    var CurrentItemDb = _productServices.GetProductDetailUI(item.Id, currentCulture).Data;

                    var SizeResult = CurrentItemDb.Product_Size.TryGetValue(Convert.ToInt32(item.Size), out int CountSizeDb);
                    if (CurrentItemDb is not null && SizeResult)
                    {

                        CartItems.Add(

                            new CartDetailDTO
                            {
                                Id = item.Id,
                                Color = CurrentItemDb.Color,
                                MaxQuantity = CountSizeDb,
                                NeededQuantity = item.Quantity >= CountSizeDb ? CountSizeDb : item.Quantity,
                                PictureUrl = item.PhotoUrl,
                                Price = CurrentItemDb.DisCount == 0 ? CurrentItemDb.Price : CurrentItemDb.DisCount,
                                ProductName = item.ProductName,
                                
                                Size = item.Size,
                                
                            }
                            );
                    }
                }

            }



            return View(CartItems);
        }

        public IActionResult CartDeleteItem(string id, string size)
        {

            var cartItems = Request.Cookies["cart"];
            if (cartItems == null)
            {

                return RedirectToAction("index");
            }
            var CurrentItemCart = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cartItems);
            for (int i = 0; i < CurrentItemCart.Count; i++)
            {
                if (CurrentItemCart[i].Id == id && CurrentItemCart[i].Size == size)
                {
                    CurrentItemCart.Remove(CurrentItemCart[i]);
                }
            }
            Response.Cookies.Append("cart", JsonSerializer.Serialize(CurrentItemCart));
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task< IActionResult> ClearCart()
        {

            var cartItems = Request.Cookies["cart"];
            if (cartItems == null)
            {

                return BadRequest();
            }
           
            Response.Cookies.Delete("cart");
            return Ok();
        }


        [HttpPut]
        public IActionResult UpdateCart([FromBody] List<UpdateCartDTO> updates)
        {

            var cartItems = Request.Cookies["cart"];
            if (cartItems == null)
            {

                return RedirectToAction("index");
            }
            var CurrentItemCart = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cartItems);
            for (int i = 0; i < updates.Count; i++)
            {
                CurrentItemCart.FirstOrDefault(x => x.Id == updates[i].Id && x.Size == updates[i].Size).Quantity = int.Parse(updates[i].Quantity);
            }
            Response.Cookies.Append("cart", JsonSerializer.Serialize(CurrentItemCart));
            return RedirectToAction("Index");

        }
        public IActionResult ShippingMethodChecked(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // id değeri null veya boş ise BadRequest dönebilirsiniz.
                return BadRequest("id parameter cannot be null or empty.");
            }

            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            var result = _shippingMethodsServices.GetShipping(id, currentCulture);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
           
            Response.Cookies.Append("ShippingMethods", JsonSerializer.Serialize(result.Data));
        
            

            return Ok(result.Data);
        }
    }

    }
