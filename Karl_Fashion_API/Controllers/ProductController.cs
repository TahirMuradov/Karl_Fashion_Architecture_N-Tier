using Bussines.Abstract;
using Core.Helper;
using Core.Helper.FileHelper;
using Entities.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;

namespace Karl_Fashion_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;


        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }


        [HttpGet("/[action]/{Id}/{langCode}")]

        public IActionResult GetProduct(string Id, string langCode)
        {
            var product = _productServices.GetProductDetailUI(Id, langCode);

            return Ok(product);

        }
        [HttpGet("/[action]/{langcode}")]
        public IActionResult GetProductAll(string langcode)
        {
            var product = _productServices.GetProductListUI(null, LangCode: langcode);
            if (product.IsSuccess)
                return Ok(product);
            return BadRequest(product.Message);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] IFormFileCollection fileData,  [FromForm]ProductAddDTO productAddDTO)
        {
            List<string> PhotoUrls = new List<string>();

            foreach (var photo in fileData)
            {

                PhotoUrls.Add(await FileHelper.SaveFileAsync(photo, wwwrootGetPath.GetwwwrootPath));
            }
            var result = await _productServices.AddProductAsync(productAddDTO);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(fileData);
        }

    }
}


