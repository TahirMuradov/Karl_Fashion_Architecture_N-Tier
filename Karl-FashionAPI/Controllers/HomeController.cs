using Bussines.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Karl_FashionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly IProductServices _productServices;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductServices productServices, ICategoryService categoryService)
        {
            _productServices = productServices;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Home(string culture)
        {
        var products = _productServices.GetProductListUI(x => x.isFeatured == true, LangCode: culture).Data;

        var categories=_categoryService.GetCategoryName(culture).Data.ToList();
            return Ok(
                new
                {
                    products = products,
                    categories = categories

                }
                );
        }
    }
}
