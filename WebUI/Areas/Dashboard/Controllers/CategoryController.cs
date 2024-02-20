using Bussines.Abstract;
using Entities.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace WebUI.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CategoryController(ICategoryService categoryService, IHttpContextAccessor contextAccessor)
        {
            _categoryService = categoryService;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> index()
        {
            string errorMessage = TempData["ErrorMessage"] as string;
            if (errorMessage is not null)
            {
                ModelState.AddModelError("Error", errorMessage);
            }
            var categories = await _categoryService.GetCategoryAdminListAsync();
            return View(categories.Data);
        }
        public IActionResult Create()
        {
            string ErrorMessage = TempData["ErrorMessage"] as string;
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError("Error", ErrorMessage);
            }


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryAddDTO categoryAddDTO)
        {
            string currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
           
            if (string.IsNullOrEmpty(currentUserId))
            {
                TempData["ErrorMessage"] = "Qeydiyyatdan Kecmemis Category Yarada bilmersen!";
                return RedirectToAction("Create");
            }
            categoryAddDTO.CreatorUserId = currentUserId;
            if (categoryAddDTO.LangCode.Contains(null) || categoryAddDTO.CategoryName.Contains(null))
            {
                TempData["ErrorMessage"] = "Data düzgün Gelmedi";
                return RedirectToAction("create");
            }

            if (categoryAddDTO.CategoryName.Count != categoryAddDTO.LangCode.Count || categoryAddDTO.LangCode.Count <= 0)
            {
                TempData["ErrorMessage"] = "Category adı ve dil kodu saylari uygun deyil veya sıfırdan kiçik.";
                return RedirectToAction("Update");
            }

            var result=  await _categoryService.AddCategoryAsync(categoryAddDTO);
            if (result.IsSuccess)
            {

            return RedirectToAction("index");
            }
            return View(result.Message);
        }
        public async Task<IActionResult> Update (string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
            {
                return RedirectToAction("index");
            }
            string errorMessage = TempData["ErrorMessage"] as string;
            if (errorMessage is not null)
            {
                ModelState.AddModelError("Error", errorMessage);
                
            }
            var category = await _categoryService.GetCategoryAdminListAsync(x => x.Id.ToString() == categoryId);
            CategoryUpdateDTO categoryUpdateDTO = new CategoryUpdateDTO();
            for (int i = 0; i < category.Data.Count; i++)
            {
               
             categoryUpdateDTO.CategoryId=categoryId;
                categoryUpdateDTO.NewCategoryName = category.Data[i].CategoryName;
                categoryUpdateDTO.LangCode = category.Data[i].LaunguageCode;
                

            }


            return View(categoryUpdateDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDTO categoryUpdateDTO)
        {
            try                                           
            {
                string currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                categoryUpdateDTO.UpdatedUserId = currentUserId;

               
                if (categoryUpdateDTO.LangCode.Contains(null) || categoryUpdateDTO.NewCategoryName.Contains(null))
                {
                    TempData["ErrorMessage"]= "Data düzgün Gelmedi";
                    return RedirectToAction("Update", new { categoryId = categoryUpdateDTO.CategoryId });
                }

                if (categoryUpdateDTO.NewCategoryName.Count != categoryUpdateDTO.LangCode.Count || categoryUpdateDTO.LangCode.Count <= 0)
                {
                    TempData["ErrorMessage"]= "Category adı ve dil kodu saylari uygun deyil veya sıfırdan kiçik.";
                    return RedirectToAction("Update", new { categoryId = categoryUpdateDTO.CategoryId });
                }

                categoryUpdateDTO.UpdatedDateTime = DateTime.Now;

                var result = await _categoryService.UpdateCategoryAsync(categoryUpdateDTO);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"]= result.Message;
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"]= $"Error message:{ex}";
            }

            return View(categoryUpdateDTO);
        }


        [HttpGet]
        public async Task<IActionResult>Delete(string categoryRemoveId)
        {
            if (string.IsNullOrEmpty(categoryRemoveId))
            {
                TempData["ErrorMessage"] ="Silme zamani Id Bos Geldi";
                return RedirectToAction("index");
            }
            
            var category=_categoryService.GetCategoryAdminListAsync(x=>x.Id.ToString() == categoryRemoveId);
            if (category.Result.Data.Count==0)
            {
                TempData["ErrorMessage"] = "Bu Id de Category Yoxdur!Silmek Mumkun Olmadi";
                return RedirectToAction("index");
            }
            CategoryRemoveDTO categoryRemoveDTO = new CategoryRemoveDTO()
            {
                CategoryId = categoryRemoveId,
                CategoryName = category.Result.Data[0].CategoryName[0]
                
               
        };
      
           return View(categoryRemoveDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryRemoveDTO categoryRemove)
        {
            var result = _categoryService.DeleteCategory(categoryRemove);
            if (result.IsSuccess)
       
                return RedirectToAction("Index");
          
            return View(result.Message);
        }
    }
}
