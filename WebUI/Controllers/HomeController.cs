using Bussines.Abstract;
using Entities.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService _categoryServices;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryServices, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _categoryServices = categoryServices;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            //var currentUserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //List<string> categoryName = new List<string>
            //{
            //    "Texnologiya",
            //    "Технологии",
            //    "Technology"


            //};
            //List<string> langueageCode = new List<string>
            //{
            //    "az-AZ",
            //    "ru-RU",
            //    "us-EN"


            //};
            //CategoryAddDTO categoryAddDTO = new CategoryAddDTO()
            //{
            //    //CreatorUserId=currentUserId,
            //   CategoryName= categoryName,
            //   LangCode= langueageCode
            //};
          

            //_categoryServices.AddCategory(categoryAddDTO);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



    }
}
