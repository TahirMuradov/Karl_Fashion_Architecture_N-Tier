using Bussines.Abstract;
using Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Karl_Fashion_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult >Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var result=await _userService.AddUserAsync(userRegisterDTO);

         return result.IsSuccess ? Ok(result) : BadRequest(result.Message);
        }


    }
}
