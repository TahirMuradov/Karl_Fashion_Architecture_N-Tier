using Azure;
using Bussines.Abstract;
using Entities.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

namespace WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductServices _ProductServices;
        private readonly ICategoryService _CategoryService;
        private readonly ISizeService _SizeService;

        public ShopController(IProductServices productServices, ICategoryService categoryService, ISizeService sizeService)
        {
            _ProductServices = productServices;
            _CategoryService = categoryService;
            _SizeService = sizeService;
        }

        public IActionResult Index()
        {
            var currentCulter = Thread.CurrentThread.CurrentCulture.Name;
            ViewBag.Categories = _CategoryService.GetCategoryName(currentCulter).Data;
            ViewBag.Size = _SizeService.GetSize(null).Data;
            ViewBag.Color = _ProductServices.GetProductsColor().Data.Distinct().ToList();
           var Prices = _ProductServices.GetProductsMaxAndMinPrice().Data;
            if (Prices is not null)
            {
                
            ViewBag.maxPrice = Prices["maxPrice"];
            }
            var products = _ProductServices.GetProductListUI(null, LangCode: currentCulter).Data;
            if (products is not null)
            {
                
            ViewBag.AllProductsCount = products.Count;
            }
            return View(products.Take(10).ToList());
        }
        [HttpGet]
        public IActionResult FilteredData(string? category, string? color, string? size, string? minPrice, string? maxPrice, string? page)
        {
                var currentCulter = Thread.CurrentThread.CurrentCulture.Name;
            if (!string.IsNullOrEmpty(color))
            {
                color = "#" + color;
            }

            var  FilteredProducts = _ProductServices.GetProductFilterUI(currentCulture: currentCulter, category: category, color: color, size: size, minPrice: minPrice, maxPrice:maxPrice);
            int AllDataCount = FilteredProducts.Data.Count;
            List<GetProductUIDTO> ResultProducts = new List<GetProductUIDTO>();

            bool CheckPageResult = int.TryParse(page, out int pageCount);
            if (page is not null)
            {
                if (!CheckPageResult)
                {
                    return BadRequest("Page value Converting Error");
                }
            }
            if (FilteredProducts.IsSuccess)
            {
                ResultProducts = page is not null && CheckPageResult ? FilteredProducts.Data.Skip((pageCount - 1) * 10).Take(10).ToList() : FilteredProducts.Data;
                return Json(new { AllDataCount, ResultProducts});

            }
            return BadRequest($"senol bura gelibse Nese Boyuk Prablem var: Errror Mesage: {FilteredProducts.Message}");




        }

    }
}
