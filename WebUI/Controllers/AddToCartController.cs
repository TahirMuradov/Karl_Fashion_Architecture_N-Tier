using Bussines.Abstract;
using Entities.DTOs.Cart;
using Entities.DTOs.SizeDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace WebUI.Controllers
{
    public class AddToCartController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IHttpClientFactory _httpClientFactory;
        public AddToCartController(IProductServices productServices, IHttpClientFactory httpClientFactory)
        {
            _productServices = productServices;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> AddToCart(string Id, string size)
        {
            var cartCokie = Request.Cookies["cart"];
            List<CartAddItemCookieDTO> CurrentItemCart = new List<CartAddItemCookieDTO>();
            if (cartCokie is not null)
            {

                CurrentItemCart = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cartCokie);
            }
            var cookieOptions = new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddMonths(1),
                Secure = true,

                Path = "/"
            };


            if (string.IsNullOrEmpty(Id)) return BadRequest("Id bos ola bilmez!");
            if (string.IsNullOrEmpty(size)) return BadRequest("Almaq istediyinz olcunu ve sayini seçin");

            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

            var product = _productServices.GetProductDetailUI(Id, currentCulture);

            if (!product.IsSuccess)
            {

                return BadRequest(product.Message);
            }

            string[] SizeAndCount = size.Split(',');
            for (int i = 0; i < SizeAndCount.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (!product.Data.Product_Size.ContainsKey(int.Parse(SizeAndCount[i])))
                    {
                        return BadRequest($"{SizeAndCount[i]} bu olcu stokda  yoxdur");

                    }
                    if (product.Data.Product_Size[int.Parse(SizeAndCount[i])] < int.Parse(SizeAndCount[i + 1]))
                    {

                        return BadRequest($"{SizeAndCount[i]} bu olcu ${SizeAndCount[i + 1]} sayda stokda  yoxdur");
                    }
                    ;
                }
            }

            for (int i = 0; i < SizeAndCount.Length; i++)
            {
                if (i % 2 == 0)
                {
                    if (CurrentItemCart.Count != 0 && CurrentItemCart.Any(x => x.Id == Id))
                    {

                        if (CurrentItemCart.Where(x => x.Id == Id).Any(x => x.Size == SizeAndCount[i]))
                        {



                            if ((CurrentItemCart.FirstOrDefault(x => x.Id == Id && x.Size == SizeAndCount[i]).Quantity + int.Parse(SizeAndCount[i + 1])) > product.Data.Product_Size[int.Parse(SizeAndCount[i])])
                            {
                                CurrentItemCart.FirstOrDefault(x => x.Id == Id && x.Size == SizeAndCount[i]).Quantity = product.Data.Product_Size[int.Parse(SizeAndCount[i])];
                            }
                            else
                            
                            {
                                CurrentItemCart.FirstOrDefault(x => x.Id == Id && x.Size == SizeAndCount[i]).Quantity += int.Parse(SizeAndCount[i + 1]);

                            }

                        }
                        else
                        {
                            CartAddItemCookieDTO newCookieItem = new CartAddItemCookieDTO()
                            {
                                Id = product.Data.ProductID.ToString(),
                                Size = SizeAndCount[i],
                                PhotoUrl = product.Data.PicturesUrls[0],
                                ProductName = product.Data.ProductName,
                                ProductDescription = product.Data.ProductDescription,
                                ProductCode= product.Data.ProductCode,
                                Quantity = int.Parse(SizeAndCount[i + 1]),
                                Price = product.Data.DisCount == 0 ? product.Data.Price : product.Data.DisCount,




                            };
                            CurrentItemCart.Add(newCookieItem);
                        }





                    }
                    else
                    {

                        CartAddItemCookieDTO newCookieItem = new CartAddItemCookieDTO()
                        {
                            Id = product.Data.ProductID.ToString(),
                            Size = SizeAndCount[i],
                            PhotoUrl = product.Data.PicturesUrls[0],
                            ProductName = product.Data.ProductName,
                            ProductCode=product.Data.ProductCode,
                            ProductDescription=product.Data.ProductDescription,
                            Quantity = int.Parse(SizeAndCount[i + 1]),
                            Price = product.Data.DisCount == 0 ? product.Data.Price : product.Data.DisCount,




                        };
                        CurrentItemCart.Add(newCookieItem);
                    }
                }
            }



            var cookieJson = JsonSerializer.Serialize(CurrentItemCart);
            Response.Cookies.Append("cart", cookieJson);
           

            return Ok("Mehsul Elave Edildi");

        }

        public IActionResult GetCartData()
        {
            var cartCookie = Request.Cookies["cart"];
            decimal totalPrice = 0;
            int totalQuantity = 0;
            if (cartCookie is null)
            {
                return Ok(new object[] { JsonSerializer.Serialize(new List<CartAddItemCookieDTO> { null }), totalPrice, totalQuantity });


            }
            var datas = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cartCookie);
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;


            foreach (var data in datas)
            {
                var product = _productServices.GetProductDetailUI(data.Id, currentCulture).Data;

                data.Price = product.DisCount == 0 ? product.Price : product.DisCount;
                data.ProductName = product.ProductName;
            }
            foreach (var item in datas)
            {
                totalPrice += item.Price*item.Quantity;
                totalQuantity += item.Quantity;
            }

            return Ok(new object[] { JsonSerializer.Serialize(datas), totalPrice, totalQuantity });
        }

        public IActionResult CartItemDelete (string id)
        {
            var cartCokie = Request.Cookies["cart"];
         
            List<CartAddItemCookieDTO>  CurrentItemCart = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(cartCokie);

            CurrentItemCart = CurrentItemCart.FindAll(x => x.Id != id);

            var result =JsonSerializer.Serialize(CurrentItemCart);
            Response.Cookies.Delete("cart");
            var cookieOptions = new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddMonths(1),
                Secure = true,

                Path = "/"
            };
            Response.Cookies.Append("cart", result);

            return Ok();
        }
   
    public IActionResult CheckedCartItemInfo()
        {
            var items = JsonSerializer.Deserialize<List<CartAddItemCookieDTO>>(Request.Cookies["cart"]);
            var currentCulture=Thread.CurrentThread.CurrentCulture.Name;
            decimal totalCartPrice = 0;
            foreach (var item in items)
            {
                var product = _productServices.GetProductDetailUI(item.Id, currentCulture).Data;

                if (product == null) return BadRequest("id is valid or failed");
                if (item.Price!=(product.DisCount==0? product.Price:product.DisCount))
                {
                    item.Price = (product.DisCount==0?product.Price:product.DisCount);
                }

                bool TrySizeCountConverting = int.TryParse(item.Size, out int Size);
                if (!TrySizeCountConverting)
                    return BadRequest("Size Is NoutFound");


                if (!product.Product_Size.ContainsKey(Size) ||
                !product.Product_Size.TryGetValue(Size, out int SizeCount))
                
                {


                    return BadRequest("Size or Size Count Is NoutFound");
                }
                else if( SizeCount < item.Quantity)
                {
                    item.Quantity = SizeCount;
                }

                totalCartPrice += (item.Price * item.Quantity);
                


            }


            Response.Cookies.Append("cart", JsonSerializer.Serialize( items ));
            return Ok(new {totalCartPrice});

        }
    
    }
    
 
}
