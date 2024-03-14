using Core.Utilities.Results.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Abstract
{
    public interface IPictureServices
    {

        public Task<IDataResult<List<string>>> CreatePictureAsync(string ProductId, List<IFormFile> photos);
        public IResult DeletePicture(string PictureId);
    }
}
