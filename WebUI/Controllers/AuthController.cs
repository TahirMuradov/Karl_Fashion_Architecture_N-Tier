using Bussines.Abstract;
using Core.Helper.EmailHelper;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;
using System.Text.RegularExpressions;

namespace WebUI.Controllers
{

    public class AuthController :BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailHelper _emailHelper;


        public AuthController(IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, IEmailHelper emailHelper)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailHelper = emailHelper;

        }

        public IActionResult Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(controllerName: "home", actionName: "index");
            }


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
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(checkEmail, user.Password, user.RememberMe, true);
            if (!signInResult.Succeeded)
            {
               
                return View();
            }

            return RedirectToAction(controllerName: "home", actionName: "index");

        }
       
        public async Task< IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(controllerName: "home", actionName: "index");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            if (!Regex.IsMatch(userRegisterDTO.PhoneNumber, "^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$"))
            {
                ModelState.AddModelError("Error", "Please enter valid phone no.");
                return View();
            }
            userRegisterDTO.UserName= userRegisterDTO.UserName.Trim(' ');
            var result = await _userService.AddUserAsync(userRegisterDTO);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("Error", result.Message);
                return View();
            }

            User user = _userManager.Users.FirstOrDefault(x => x.Id == result.Data.Id);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            user.ConfirmeTime = DateTime.Now.AddDays(1);
            await _userManager.UpdateAsync(user);
            var confirmLink = Url.ActionLink(controller: "Auth",
                action: "ConfirmEmail",
                host: Request.Host.Value,
                values: new { token, currentEmail = user.Email });
            await _emailHelper.SendEmailAsync(user.Email, confirmLink, user.UserName);

            return RedirectToAction("Login");


        }

        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(controllerName: "home", actionName: "index");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }
            var user = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user == null)
            {
                ModelState.AddModelError("Error", "İstifadeci Tapilmadi");
                return View();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (user.ConfirmeTime is null)
            {

                user.ConfirmeTime = DateTime.Now.AddDays(1);
            }
            else
            {
                ModelState.AddModelError("Error", "Size Tesdiqleme Linki Gonderilib!");
                return View();
            }
         
            var confirmLink = Url.ActionLink(controller: "Auth",
                action: "ForgotPasswordChange",
                host: Request.Host.Value,
                values: new { token, email = user.Email });
            var result = await _emailHelper.SendEmailAsync(user.Email, confirmLink, user.UserName);
            if (result.IsSuccess)
            {
                ModelState.AddModelError("Error", "Elektron Poctunuza Tesdiqleme Linki Gonderildi!");
                await _userManager.UpdateAsync(user);
                return View();
            }
            else
            {
                ModelState.AddModelError("Error", "Prablem Yarandi Tekrar Yoxlayin");

                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ForgotPasswordChange(string token, string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.ConfirmeTime is null)
            {
               return RedirectToAction("ErrorPage");
            }
            bool tokenResult = await _userManager.VerifyUserTokenAsync(
               user: user,
               tokenProvider: _userManager.Options.Tokens.PasswordResetTokenProvider,
               purpose: UserManager<User>.ResetPasswordTokenPurpose,
               token: token
                              );
            ForgotPasswordChangeDTO forgotPasswordChangeDTO = new ForgotPasswordChangeDTO();

            if (!tokenResult)
            {
                RedirectToAction("ErrorPage");
            }
            else
            {
                forgotPasswordChangeDTO.Token = token;
                forgotPasswordChangeDTO.UserId = user.Id;

            }

            user.ConfirmeTime = null;
            await _userManager.UpdateAsync(user);

            return View(forgotPasswordChangeDTO);
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPasswordChange(ForgotPasswordChangeDTO forgotPasswordChangeDTO)
        {
            User user = await _userManager.FindByIdAsync(forgotPasswordChangeDTO.UserId);
            if (user == null)
            {
                ModelState.AddModelError("Error", "User Tapilmadi");
                return View();
            };
            IdentityResult result = await _userManager.ResetPasswordAsync(user, forgotPasswordChangeDTO.Token, forgotPasswordChangeDTO.ForgotPasswordChange);
            if (result.Succeeded)
            {

                return Redirect("Login");

            }
            else
            {
                return RedirectToAction("Errorpage");
            }
        }
        public async Task<IActionResult> ConfirmEmail(string token, string currentEmail, string newEmail = null)
        {

            if (newEmail is not null)
            {
                if (!string.IsNullOrEmpty(currentEmail) && !string.IsNullOrEmpty(token))
                {
                   


                    var user = await _userManager.FindByEmailAsync(currentEmail);
                    if (user == null)
                    {
                        ModelState.AddModelError("Error", "Istifaddeci Tapilmadi");
                    }

                    if ((user.EmailConfirmed == true && user.ConfirmeTime < DateTime.Now) || user.ConfirmeTime == null)
                    {
                        return Redirect("auth/erropage");
                    }
                    var EmailChange = await _userManager.ChangeEmailAsync(user, newEmail, token);
                    user.ConfirmeTime = null;
                    if (EmailChange.Succeeded)
                    {
                        await _userManager.UpdateAsync(user);
                    }
                    else
                    {
                        return Redirect("auth/erropage");
                    }
                    return Redirect("dashboard/index");





                }

            }

            if (!string.IsNullOrEmpty(currentEmail) && !string.IsNullOrEmpty(token))
            {


                var user = await _userManager.FindByEmailAsync(currentEmail);

                if ((user.EmailConfirmed == true && user.ConfirmeTime < DateTime.Now) || user.ConfirmeTime == null)
                {
                    return RedirectToAction("errorpage");
                }


                var EmailConfirm = await _userManager.ConfirmEmailAsync(user, token);
                user.ConfirmeTime = null;
                if (EmailConfirm.Succeeded)
                {
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    return RedirectToAction("errorpage");
                }
                return RedirectToAction("Login");





            }
            else
            {
                return Redirect("auth/erropage");
            }


        }

        public IActionResult ErrorPage()
        {

            return View();
        }


    }
}
