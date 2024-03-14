using Bussines.Abstract;
using Core.Helper;
using Core.Helper.FileHelper;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Karl_Fashion_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IPictureServices _pictureServices;

        public PictureController(IPictureServices pictureServices)
        {
            _pictureServices = pictureServices;
        }

        [HttpPost]
        public async Task< IActionResult> CreatePciture([FromForm] List<IFormFile> photos,[FromForm]string productId)
        {
            if (photos is null|| photos.Count==0)
                return BadRequest();



            var result = await _pictureServices.CreatePictureAsync(productId, photos);

           return result.IsSuccess ? Ok(result) : BadRequest(result);
         
                
       
       

        
        
        }
    }
}
