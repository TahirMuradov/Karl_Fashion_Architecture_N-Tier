using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Dashboard.Controllers
{
    public class UserController : Controller
    {
        public readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            
            return View();
        }
        public async Task<IActionResult> ConfirmEmail (string token,string Email)
        {
            
          


            return Redirect(nameof(Index));
        }
        public async Task<IActionResult > ConfirmNewEmail()
        {


            return Redirect(nameof(Index));
        }
    }
}
