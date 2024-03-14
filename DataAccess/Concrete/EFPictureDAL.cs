using Core.DataAccess.EntityFramework;
using Core.Helper;
using Core.Helper.FileHelper;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFPictureDAL : EFRepositoryBase<Picture, AppDbContext>, IPictureDAL
    {
        public async Task< IDataResult<List<string>>> CreatePictureAsync(string ProductId, List<IFormFile> photos)
        {
            try
            {
                if (photos is null || photos.Count==0)
              
                    return new ErrorDataResult<List<string>>("Photo is empty");
                if (ProductId is null)
                    return new ErrorDataResult<List<string>>("Id is empty");
                using var context = new AppDbContext();

                var product = context.Products.FirstOrDefault(x => x.Id.ToString() == ProductId);
                if (product is null)
                    return new ErrorDataResult<List<string>>("No product found");
         
                    List<string> urls= new List<string>();
                urls = await FileHelper.SaveFileRangeAsync(photos, wwwrootGetPath.GetwwwrootPath);

                
                foreach (string url in urls)
                {
                    Picture picture = new Picture()
                    {
                        ProductId=product.Id,
                        url=url
                    };
                    context.Pictures.Add(picture);
                }
                context.SaveChanges();

                return new SuccessDataResult<List<string>>(data: urls);


            }
            catch (Exception ex)
            {

                return new SuccessDataResult<List<string>>(ex.Message);
            }
        }

        public IResult DeletePicture(string PictureId)
        {
            try
            {
                if (string.IsNullOrEmpty(PictureId))
                    return new ErrorResult();
                using var context = new AppDbContext(); 
                var picture = context.Pictures.FirstOrDefault(x=>x.Id.ToString() == PictureId);
                context.Pictures.Remove(picture);
                context.SaveChanges();
                return new SuccessResult();

            }
            catch (Exception ex )
            {

               return new ErrorResult(ex.Message);
            }
        }
    }
}
