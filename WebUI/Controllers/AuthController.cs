using AutoMapper;
using Bussines.Abstract;
using Bussines.Concrete;
using Core.Helper;
using Core.Utilities.ConfirmMessageSend;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using NuGet.Common;
using System.Security.Policy;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        public readonly IUserService _userService;
        public readonly UserManager<User> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly SignInManager<User> _signInManager;
        public readonly IConfiguration _config;
        public readonly IMapper _mapper;

        public AuthController(IUserService userService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IMapper mapper, IConfiguration config)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
        }

        public IActionResult Login()
        {




            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Password or Email is empty!");
                return View();
            }
            var checkEmail = await _userManager.FindByEmailAsync(user.Email);

            if (checkEmail == null)
            {

                ModelState.AddModelError("Error", "Email or Password is not valid!");
                return View();
            }
            if (checkEmail.ConfirmeTime < DateTime.Now)
            {
                ModelState.AddModelError("Error", "Elektron Poct Tesdiqlemenin Vaxti Kecib!Yeniden Qeydiyyatdan Kecin");
                return View();

            }
            if (!checkEmail.EmailConfirmed)
            {
                ModelState.AddModelError("Error", "Elektron Poct Tesdiqlenmeyib ! Elektron Poctunuzu Mesaj hissesinden size gonderilen linke kecid ederek tesdiqleyin!Eks Halda 1 Gun Erzinde Qeydiyatiniz Legv Olunacaq!");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(checkEmail, user.Password, user.RememberMe, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Error", "Email or Password is not valid!");
                return View();
            }

            return RedirectToAction(controllerName: "home", actionName: "index");

        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                if (!ModelState.IsValid) { ModelState.AddModelError("Error", "Data is Empty!"); return View(); }



                var checkEmail = await _userManager.FindByEmailAsync(userRegisterDTO.Email);

                if (checkEmail is not null)
                {
                    ModelState.AddModelError("Error", "Bu Emailda Istifadeci Artiq qeydiyyatdan kecib");
                    return View();
                }

                var map = _mapper.Map<User>(userRegisterDTO);
                map.UserName = UserNameHelper.CreatingUserName(map.FirstName, map.LastName);
                map.ConfirmeTime = DateTime.Now.AddDays(1);

                IdentityResult result = await _userManager.CreateAsync(map, userRegisterDTO.Password);

                var a = result.Errors;
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {

                        ModelState.AddModelError("Error", error.Description);
                    }
                    return View();
                }
                else
                {

                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(map).ConfigureAwait(false);

                    var confirmLink =
                        Url.ActionLink(controller: "Auth", action: "ConfirmEmail", host: Request.Host.Value, values: new { token, currentEmail = map.Email });



                    bool resultMessage = EmailConfirme.SendEmail(UserName: $"{map.FirstName} {map.LastName}", userEmail: map.Email, confirmationLink: confirmLink, fromEmail: _config["EmailServices:FromEmail"], fromEmailPassword: _config["EmailServices:FromEmailPassword"], serviceName: _config["EmailServices:ServiceName"], servicePort: Convert.ToInt32(_config["EmailServices:ServicePort"]));
                    if (!resultMessage)
                    {
                        ModelState.AddModelError("Error", "Email Tesdiqleme Mesaji gonderile Bilmedi");

                        return View();
                    }
                }
                if (_userManager.Users.ToList().Count() == 1)
                {
                    if (!_roleManager.Roles.Any(x => x.Name == "Admin"))
                    {
                        IdentityRole newRole = new IdentityRole()
                        {
                            Name = "Admin"
                        };
                        await _roleManager.CreateAsync(newRole);
                        await _userManager.AddToRoleAsync(map, "Admin");

                    }
                    else
                    {


                        await _userManager.AddToRoleAsync(map, "Admin");
                    }
                }
                else
                {
                    if (!_roleManager.Roles.Any(x => x.Name == "User"))
                    {
                        IdentityRole newRole = new IdentityRole()
                        {
                            Name = "User"
                        };
                        await _roleManager.CreateAsync(newRole);
                        await _userManager.AddToRoleAsync(map, "User");

                    }
                    else
                    {


                        await _userManager.AddToRoleAsync(map, "User");
                    }



                }
                return RedirectToAction("Login");







            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }

        }

        public async Task<IActionResult> ConfirmEmail(string token, string currentEmail, string newEmail = null)
        {
            if (!string.IsNullOrEmpty(currentEmail) && !string.IsNullOrEmpty(token))
            {

                var user = await _userManager.FindByEmailAsync(currentEmail);
                if (user == null)
                {
                  
                }
               //if(user.EmailConfirmed==false) {
                
                
                var tokenConfirm = await _userManager.VerifyUserTokenAsync(
                    user: user,
                    tokenProvider: _userManager.Options.Tokens.EmailConfirmationTokenProvider,
                    purpose: UserManager<User>.ConfirmEmailTokenPurpose,
                    token: token

                    );
               
                var a = await _userManager.ConfirmEmailAsync(user, token);
                    if (a .Succeeded)
                    {
                        var authenticationManager = HttpContext.Request.Cookies.FirstOrDefault(c => c.Value.ToString() ==token).Key;
                    
                       HttpContext.Response.Cookies.Delete(authenticationManager);
                    }
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Login");
                //}
                    

              
  
            }
            return RedirectToAction("Register");

        }



    }
}
