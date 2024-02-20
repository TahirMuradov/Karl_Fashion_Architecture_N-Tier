using Bussines.Abstract;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.SizeDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Core;
using System.Security.Claims;

namespace WebUI.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    [Authorize(Roles = "Admin")]
    public class SizeController : Controller
    {

        private readonly ISizeService _sizeService;
        private readonly IHttpContextAccessor _contextAccessor;

        public SizeController(ISizeService sizeService, IHttpContextAccessor contextAccessor)
        {
            _sizeService = sizeService;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            var size = _sizeService.GetSize();
           
            return View(size.Data);
        }
        public IActionResult Create()
        {
            string errorMessage = TempData["ErrorMessage"] as string;
            if (errorMessage is not null)
            {
                ModelState.AddModelError("Error", errorMessage);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddSizeDTO addSizeDTO)
        {
            if (addSizeDTO.Size==0)
            {
                TempData["ErrorMessage"] =
                   "Ancaq Reqem Qebul edilir!Rum reqemi qebul olunmagi ucun Duzelisler lazimdir 0552784344 elaqe saxlayin!";
           return RedirectToAction("create");
                    }
            string currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
addSizeDTO.CreatorUserId = currentUserId;
            var result = await _sizeService.SizeAddAsync(addSizeDTO);
            if (result.IsSuccess)
            {
                return  RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Update(string SizeId)
        {
            string currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
         var size=   _sizeService.GetSize(x => x.Id.ToString() == SizeId).Data[0];
            UpdateSizeDTO updateSizeDTO = new UpdateSizeDTO()
            {SizeId=SizeId,
            UpdatedUserId=currentUserId,
                NewSizeContent = size.Size
             };

            return View(updateSizeDTO);
        }
        [HttpPost]
        public async Task<IActionResult >Update(UpdateSizeDTO updateSizeDTO)
        {
           await _sizeService.SizeUpdateAsync(updateSizeDTO);

            return View(updateSizeDTO);
        }
        [HttpGet]
        public  IActionResult Delete(string SizeId)
        {
            var size = _sizeService.GetSize(x => x.Id.ToString() == SizeId).Data[0];
            RemovSizeDTO categoryRemoveDTO = new RemovSizeDTO()
            {
                Id = size.SizeId,
                Size = size.Size
                

            };

      
            return View(categoryRemoveDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(RemovSizeDTO removSizeDTO)
        {
            var result= await  _sizeService.DeleteSizeAsync(removSizeDTO.Id);



            if (result.IsSuccess)

                return RedirectToAction("index");
            return View(result.Message);
            
        }
    }
}
