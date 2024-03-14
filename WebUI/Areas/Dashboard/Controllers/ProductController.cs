using AutoMapper;
using Bussines.Abstract;
using Entities.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Core.Helper.FileHelper;
using Entities.DTOs.CategoryDTOs;

namespace WebUI.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public ProductController(IProductServices productServices, ICategoryService categoryService, IWebHostEnvironment env, IHttpContextAccessor contextAccessor, ISizeService sizeService, IMapper mapper)
        {
            _productServices = productServices;
            _categoryService = categoryService;
            _env = env;
            _contextAccessor = contextAccessor;
            _sizeService = sizeService;
            _mapper = mapper;
        }

        public async Task<IActionResult >Index()
        {
            string ErrorMessage = TempData["ErrorMessage"] as string;
            if (ErrorMessage is not null)
            {
                ModelState.AddModelError("Error", ErrorMessage);
                
            }
       
            var currentCultur=Thread.CurrentThread.CurrentCulture.Name;

            var products =_productServices.ProductGetAdminList().Data;
            ViewBag.Category = _categoryService.GetCategoryName(currentCultur).Data;
            ViewBag.Sizes = _sizeService.GetSize().Data.Select(x => x.Size).ToList();
          
            return View(products);
        }
        [HttpGet]
        public  IActionResult Create()
        {
            string ErrorMessage = TempData["ErrorMessage"] as string;
            if (ErrorMessage is not null)
            {
                ModelState.AddModelError("Error", ErrorMessage);
            }
            var currentCultur=Thread.CurrentThread.CurrentCulture.Name;
           
            ViewBag.Category = _categoryService.GetCategoryName(currentCultur).Data;
            ViewBag.Sizes = _sizeService.GetSize().Data.Select(x=>x.Size).ToList();
            if (_sizeService.GetSize().Data.Select(x => x.Size).ToList().Count==0|| _categoryService.GetCategoryName(currentCultur).Data.Count==0)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(ProductAddDTO productAddDTO,List<IFormFile> Photo)
        {
            string currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                //TempData["ErrorMessage"] = "Qeydiyyatdan Kecmemis Category Yarada bilmersen!";
                //return RedirectToAction("Create");
                ModelState.AddModelError("Error", "Qeydiyyatdan Kecmemis Category Yarada bilmersen!");
                return View();
            }
       
            if (productAddDTO.LangCode.Contains(null) || productAddDTO.ProductName.Contains(null) ||productAddDTO.ProductDescrption.Contains(null))
            {
                //TempData["ErrorMessage"] = "Data düzgün Gelmedi";
                //return RedirectToAction("create");
                ModelState.AddModelError("Error", "Data düzgün Gelmedi");
                return View();
            }

            if (productAddDTO.ProductName.Count != productAddDTO.LangCode.Count || productAddDTO.LangCode.Count <= 0||productAddDTO.LangCode.Count!=productAddDTO.ProductDescrption.Count)
            {
                //TempData["ErrorMessage"] = "Product adı ve ya Prodcut Description  dil kodu saylari uygun deyil veya sıfırdan kiçik.";
                //return RedirectToAction("create");
                ModelState.AddModelError("Error", "Product adı ve ya Prodcut Description  dil kodu saylari uygun deyil veya sıfırdan kiçik.");
                return View();
            }
            if ( Photo is null)
            {
                //TempData["ErrorMessage"] = "Mehsul Sekli Elave Edin";
                //return RedirectToAction("create");
                ModelState.AddModelError("Error", "Mehsul Sekli Elave Edin");
                return View();
            }
            List<string>PhotoUrls = new List<string>();

            foreach (var photo in Photo)
            {
                
                PhotoUrls.Add(await FileHelper.SaveFileAsync(photo,_env.WebRootPath) );
            }
            productAddDTO.PhotoUrl = PhotoUrls;
            productAddDTO.UserId=currentUserId;
    var result=     await _productServices.AddProductAsync(productAddDTO);
            if (!result.IsSuccess)
          return View(result.Message);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(string id,string name)
        {
            if (id is null)
            {
                TempData["ErrorMessage"] = "Id Duzgun Deyil";
                return RedirectToAction("Index");
            }
            ProductRemoveDTO productRemoveDTO = new ProductRemoveDTO()
            {
                ProductId=Guid.Parse(id),
                ProductName=name

            };

            return View(productRemoveDTO);
        }
        [HttpPost]
        public IActionResult Delete(ProductRemoveDTO productRemoveDTO)
        {
            if (productRemoveDTO.ProductId.ToString() is null)
            {
                TempData["ErrorMessage"] = "Id Duzgun Deyil";
                return RedirectToAction("index");
            }
            var result =_productServices.RemoveProduct(productRemoveDTO);
            if (result.IsSuccess)
            
                return RedirectToAction("Index");
            
            return View(result.Message);
        }
        [HttpGet]
        public IActionResult Update (string id)
        {
            string ErrorMessage = TempData["ErrorMessage"] as string;
            if (ErrorMessage is not null)
            {
                ModelState.AddModelError("Error", ErrorMessage);
               
            }
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Id Bos OlaBilmez!";
                return RedirectToAction("index");
            }
            string currentCultur=Thread.CurrentThread.CurrentCulture.Name;
          
            var product = _productServices.GetProduct(x => x.Id.ToString() == id).Data;
            ProductUpdateDTO productUpdateDTO = new ProductUpdateDTO()
            {
                Id = product.Id,
                Color = product.Color,
                ProductCode = product.ProductCode,
                DisCount = product.DisCount,
                Price = product.Price,
                PicturesUrls=product.Pictures.Select(x=>x.url).ToList(),
                ProductDescrption=product.productLanguages.Select(x=>x.Description).ToList(),
                ProductName=product.productLanguages.Select(x=>x.ProductName).ToList(),
                CategoryId=product.ProductCategories.Select(x=>x.CategoryId).ToList(),
                isFeatured=product.isFeatured,
                LangCode=product.productLanguages.Select(x=>x.LangCode).ToList(),
                ProductSizes=product.ProductSizes.Select(x=>x.Size.NumberSize).ToList(),
                SizesCount=product.ProductSizes.Select(x=>x.SizeStockCount).ToList(),
              


            };
              ViewBag.Category = _categoryService.GetCategoryName(currentCultur).Data;
            ViewBag.Sizes = _sizeService.GetSize().Data.Distinct().Select(x=>x.Size).ToList();
            return View(productUpdateDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Update (ProductUpdateDTO productUpdateDTO ,List<IFormFile> File)
        {


          

            if (productUpdateDTO.LangCode.Contains(null) || productUpdateDTO.ProductName.Contains(null) || productUpdateDTO.ProductDescrption.Contains(null))
            {
                TempData["ErrorMessage"] = "Data düzgün Gelmedi";
                return RedirectToAction("update");
            }

            if (productUpdateDTO.ProductName.Count != productUpdateDTO.LangCode.Count || productUpdateDTO.LangCode.Count <= 0 || productUpdateDTO.LangCode.Count != productUpdateDTO.ProductDescrption.Count)
            {
                TempData["ErrorMessage"] = "Product adı ve ya Prodcut Description  dil kodu saylari uygun deyil veya sıfırdan kiçik.";
                return RedirectToAction("Update");
            }
            if (File is null)
            {
                TempData["ErrorMessage"] = "Mehsul Sekli Elave Edin";
                return RedirectToAction("Update");
            }
            foreach (IFormFile file in File)
            {
               
                productUpdateDTO.PicturesUrls.Add(await FileHelper.SaveFileAsync(file, _env.WebRootPath));
                
            }
            var result=_productServices.UpdateProduct(productUpdateDTO);
            if (result.IsSuccess)
            return RedirectToAction("Index");
                
            return View(result.Message);
        }
    }
}
