using Bussines.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebUI.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductDetailController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        public IActionResult Index( string Id)
        {
            var currentCulture=Thread.CurrentThread.CurrentCulture.Name;
            if (string.IsNullOrEmpty(Id))
            {
                ModelState.AddModelError("Error", "Id Bos Gele Bilmez!");
                return Redirect(Request.Headers["Referer"].ToString());
            }
            var product = _productServices.GetProductDetailUI(Id, currentCulture);
            if (product.IsSuccess)
            {
                var relatedProducts=_productServices.GetRelatedProduct(product.Data.Product_Category, currentCulture);
                if (relatedProducts.IsSuccess)
                {
                    ViewBag.RealetedProducts = relatedProducts.Data;
                }
                return View(product.Data);
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
