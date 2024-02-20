using AutoMapper;
using Bussines.Abstract;
using Core.Helper;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
namespace Bussines.Concrete
{
    public class UserManager : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly  IConfiguration _config;
        private readonly IMapper _mapper;
    
        
        public UserManager(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config = null, IMapper mapper = null)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _mapper = mapper;
            
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
                
          
                //bool result=  EmailHelper.SendEmail(UserName:$"{user.FirstName} {user.LastName}" ,userEmail:user.Email,confirmationLink:confirmLink,fromEmail: _config["EmailServices:FromEmail"],fromEmailPassword: _config["EmailServices:FromEmailPassword"],serviceName: _config["EmailServices:ServiceName"],servicePort:Convert.ToInt32(_config["EmailServices:ServicePort"]));
                //if (!result)
                //    return new ErrorResult("Email Gonderile Bilmedi");
              
            }
            if (user.PhoneNumber != userUpdateDTO.PhoneNumber)
                user.PhoneNumber = userUpdateDTO.PhoneNumber;
            await _userManager.UpdateAsync(user);
            return new SuccessResult("success");
        }

      public async Task<IDataResult<User>> AddUserAsync(UserRegisterDTO userRegisterDTO)
        {
            
            try
            {
                if (userRegisterDTO is null)
                {
                    return new ErrorDataResult<User>("Info Tam Deyil");
                }
                var checkEmail = await _userManager.FindByEmailAsync(userRegisterDTO.Email);
                if (checkEmail is not null)
                {
                    return new ErrorDataResult<User>("Bu Emailde Qeydiyyatda Istifadeci var");
                }
  
                var map = _mapper.Map<User>(userRegisterDTO);
    
                IdentityResult result = await _userManager.CreateAsync(map, userRegisterDTO.Password);
                if (!result.Succeeded)
                {
                    return new ErrorDataResult<User>("Qeydiyyatda Prablem Yarandi Yeniden Yoxlayin");
                }
                else
                {
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
                }
                return new SuccessDataResult<User>(map,"Qeydiyyat Ugurla Basa Catdi");

            }
            catch (Exception ex)
            {

                return new ErrorDataResult<User>(ex.Message);

            }
        }
    }
}
