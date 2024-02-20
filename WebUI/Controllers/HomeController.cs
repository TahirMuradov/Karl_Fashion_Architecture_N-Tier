using Bussines.Abstract;
using Core.Helper;
using Entities;
using Entities.DTOs.Cart;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;
using Entities.DTOs.SizeDTOs;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
    private readonly IProductServices _productServices;
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly LaunguageService _localization;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor contextAccessor, IProductServices productServices, LaunguageService localization, ICategoryService categoryService)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
            _productServices = productServices;
            _localization = localization;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
           
            var currentCulture=Thread.CurrentThread.CurrentCulture.Name;
            var products = _productServices.GetProductListUI(x=>x.isFeatured==true,LangCode:currentCulture).Data;
          
         ViewBag.Categories=_categoryService.GetCategoryName(currentCulture).Data.ToList();
            return View(products);
        }
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires=DateTimeOffset.UtcNow.AddYears(1)
                }
                );
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpGet]
        public IActionResult GetModalData (string Id,string size)

        {
            if (string.IsNullOrEmpty(Id)) return BadRequest("Id is Empty");
            if (string.IsNullOrEmpty(size)) return BadRequest("Almaq Istediyinz Olcunu Seçin Sayini Səbətə Keçid Edərək Artirin. Auto olaraq Say 1 dene Qebul edilir!");

            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

            var product = _productServices.GetProductDetailUI(Id,currentCulture).Data;
            for (int i = 0;i<size.Split(',').Length; i++)
            {
                if (i%2==0)
                {
                    if (!product.Product_Size.ContainsKey(int.Parse(size.Split(",")[i]))|| product.Product_Size[int.Parse(size.Split(",")[i])] !> int.Parse(size.Split(",")[i+1]))
                    {
                        return BadRequest($"{size.Split(",")[i]} bu olcu stokda yoxdur");
                    }
                    ;
                }
            }
            if (product is null) return BadRequest("Id Duzgun Gelmedi");

            ProductModalDTO productModalDTO = new ProductModalDTO()
            {
                ProductID = product.ProductID.ToString(),
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Color = product.Color,
                DisCount = product.DisCount,
                PicturesUrl= product.PicturesUrls[0],
                Price = product.Price,
                Product_Category=product.Product_Category,
                Product_Size = product.Product_Size



            };
            var productJson = JsonSerializer.Serialize<ProductModalDTO>(productModalDTO);
            return Ok("Elave Edildi");
        }
        
      
        public IActionResult Privacy()
        {
            return View();
        }



    }
}
