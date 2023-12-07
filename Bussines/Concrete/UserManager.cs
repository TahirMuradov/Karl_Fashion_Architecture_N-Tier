using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Helper;
using Azure.Core;
using System.Security.Policy;
using System.Text.Encodings.Web;
using Core.Utilities.Results.Concrete.SuccessResults;
using Microsoft.EntityFrameworkCore;
using Core.Utilities.ConfirmMessageSend;
using Microsoft.Extensions.Configuration;
namespace Bussines.Concrete
{
    public class UserManager : IUserService
    {
        public readonly UserManager<User> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly  IConfiguration _config;

        public UserManager(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config = null)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }



        public IDataResult<User> GetUser(Expression<Func<User, bool>> expression)
        {
            User user = _userManager.Users.FirstOrDefault(expression);
            if (user != null) 
            return new SuccessDataResult<User>(user);
          
                return new ErrorDataResult<User>("User Tapilmadi");
        }

        public async Task<IDataResult<User>> GetUserAsync(Expression<Func<User, bool>> expression)
        {
            if (expression == null)
                return new ErrorDataResult<User>("Lambda Expression is Null!");
            User user = await _userManager.Users.FirstOrDefaultAsync(expression);
            if (user != null)
                return new SuccessDataResult<User>(user);

            return  new ErrorDataResult<User>("User Tapilmadi");
        }

        public IDataResult<List<User>> GetUsers()
        {
           List<User> Users=  _userManager.Users.ToList();
            if (Users.Count > 0)
                return  new SuccessDataResult<List<User>>("Success");

            return new ErrorDataResult<List<User>>("User Is Empty");

        }

      

       

        public async Task<IResult> UpdateUserAsync(UserUpdateDTO userUpdateDTO, string url)
        {
            if (userUpdateDTO.userId == null &&
                    userUpdateDTO.Email is null &&
                    userUpdateDTO.FirstName is null &&
                    userUpdateDTO.LastName is null &&
                    userUpdateDTO.PhoneNumber is null
                     )
                return new ErrorResult("Data Is Emty!");
            User user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userUpdateDTO.userId);
            if (user == null) return new ErrorResult("User Is Not Found");

         

       
         if(user.FirstName != userUpdateDTO.FirstName)
            user.FirstName = userUpdateDTO.FirstName;
         if(user.LastName != userUpdateDTO.LastName)
            user.LastName = userUpdateDTO.LastName;
         if(user.UserName != userUpdateDTO.UserName)
            user.UserName = userUpdateDTO.UserName;
            if (user.Email!=userUpdateDTO.Email)
            {
                
                var checkEmail= await _userManager.FindByEmailAsync(userUpdateDTO.Email);
                if (checkEmail is not null)
                {
                    return new ErrorResult("Bu Emailda qeydiyyatda Istifadeci Var");
                }
                string token = await _userManager.GenerateChangeEmailTokenAsync(user, userUpdateDTO.Email);
                var confirmLink = url + $"?token={token}&newEmail={userUpdateDTO.Email}&currentEmail={user.Email}";
                
          
                bool result=  EmailConfirme.SendEmail(UserName:$"{user.FirstName} {user.LastName}" ,userEmail:user.Email,confirmationLink:confirmLink,fromEmail: _config["EmailServices:FromEmail"],fromEmailPassword: _config["EmailServices:FromEmailPassword"],serviceName: _config["EmailServices:ServiceName"],servicePort:Convert.ToInt32(_config["EmailServices:ServicePort"]));
                if (!result)
                    return new ErrorResult("Email Gonderile Bilmedi");
              
            }
            if (user.PhoneNumber != userUpdateDTO.PhoneNumber)
                user.PhoneNumber = userUpdateDTO.PhoneNumber;
            await _userManager.UpdateAsync(user);
            return new SuccessResult("success");
        }
    }
}
