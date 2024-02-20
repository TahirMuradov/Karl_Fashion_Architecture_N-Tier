
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;

        public RoleController(Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();

            
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (role.Name == null)
            {
                ViewData["Error"] = "Role Name Is empty!";
                return View();
            }
            await _roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }
       
            public async Task<IActionResult> Edit(string roleId)
            {
                if (roleId == null)
                {
                    ModelState.AddModelError("Error", "Role Id or Name is Not found!");
                    return View();
                }
                IdentityRole Role = await _roleManager.FindByIdAsync(roleId);

                if (Role == null)
                {
                    ModelState.AddModelError("Error", "Role Id or Name is Not found!");
                    return View();
                }

                return View(Role);
            }
        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            if (role == null)
            {
                ModelState.AddModelError("Error", "Role Id or Name is Not found!");
                return View();
            }
            var Role = await _roleManager.UpdateAsync(role);

            if (!Role.Succeeded)
            {
                foreach (var errors in Role.Errors)
                {

                ModelState.AddModelError("Error",errors.Description);
                }
                return View();
            }

            return View(Role);
        }
        public async Task<IActionResult> Delete(string roleId)
        {
            IdentityRole role = _roleManager.Roles.FirstOrDefault(r => r.Id == roleId);
            if (roleId == null)
            {
                ModelState.AddModelError("Error", "Role Id or Name is Not found!");
                return View();
            }
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", "Something went wrong!");
                return View();
            }

            return RedirectToAction(actionName: "index");
        }
    }
}
