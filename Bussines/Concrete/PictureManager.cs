using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Concrete
{
    public class PictureManager : IPictureServices
    {
        private readonly IPictureDAL _pictureDAL;

        public PictureManager(IPictureDAL pictureDAL)
        {
            _pictureDAL = pictureDAL;
        }

        public Task<IDataResult<List<string>>> CreatePictureAsync(string ProductId, List<IFormFile> photos)
        {
      return _pictureDAL.CreatePictureAsync(ProductId, photos);
        }

        public IResult DeletePicture(string PictureId)
        {
            return _pictureDAL.DeletePicture(PictureId);
        }
    }
}
