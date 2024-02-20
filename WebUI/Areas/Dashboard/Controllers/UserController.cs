using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        public readonly UserManager<User> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
           var user= _userManager.Users.ToList();
            return View(user);
        }
      
        public async Task< IActionResult> DeleteUser(string userId)
        {
            if (userId == null) {
                
                return Redirect("auth/errorpage");
            }
            var user= await _userManager.FindByIdAsync(userId);
            if (user == null)
            {

                return Redirect("auth/errorpage");
            }
         var result=   await _userManager.DeleteAsync(user);

            return Json(result);
        }
   public async Task<IActionResult> AddRole (string userId)
        {
            if (userId == null)
                return Redirect("auth/errorpage");
                var User= await  _userManager.FindByIdAsync(userId);

            var userRole = await _userManager.GetRolesAsync(User);
            
            var roles=_roleManager.Roles.Select(x=>x.Name).ToList();
            if (User == null) 
                return Redirect("auth/errorpage");
            UserAddRoleDTO userAddRoleDTO = new UserAddRoleDTO()
            {
                UserId = userId,
                Roles=roles.Except(userRole)
            };
            
            return View(userAddRoleDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(UserAddRoleDTO userAddRoleDTO)
        {
            if (userAddRoleDTO.UserId == null)
                return Redirect("auth/errorpage");
            var User = await _userManager.FindByIdAsync(userAddRoleDTO.UserId);
            if (User == null)
                return Redirect("auth/errorpage");
            foreach (var role in userAddRoleDTO.Roles)
            {
                await _userManager.AddToRoleAsync(user: User, role);
            }
            return View();
        }

    }
}
