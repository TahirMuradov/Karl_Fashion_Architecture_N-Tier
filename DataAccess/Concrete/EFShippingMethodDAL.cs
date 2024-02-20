using Core.DataAccess.EntityFramework;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities.Concrete;
using Entities.DTOs.ShippingMethods;
using Entities.DTOs.ShippingMethodsDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFShippingMethodDAL : EFRepositoryBase<ShippingMethods, AppDbContext>, IShippingMethodDAL
    {
        public IResult AddShipping(AddShippingMethodDTO addShipping)
        {
            try
            {
                var context = new AppDbContext();

                ShippingMethods shippingMethods = new ShippingMethods()
                {
                    DeliveryPrice = addShipping.DeliveryPrice,

                };
                context.ShippingMethods.Add(shippingMethods);
                context.SaveChanges();
                for (int i = 0; i < addShipping.LangCode.Count; i++)
                {

                    ShippingLaunguage shippingLaunguage = new ShippingLaunguage()
                    {
                        ShippingMethodId = shippingMethods.Id,
                        Content = addShipping.ShippingContent[i],
                        LangCode = addShipping.LangCode[i]


                    };
                    context.ShippingLaunguages.Add(shippingLaunguage);

                }
                context.SaveChanges();

                return new SuccessResult();
            }
            catch (Exception ex)
            {

              return new ErrorResult(ex.Message);
            }

          
        }

        public IResult DeleteShipping(string id)
        {
            try
            {
                var context=new AppDbContext();
                var shippingMethod = context.ShippingMethods.FirstOrDefault(x => x.Id.ToString() == id);
                var shippingMethodLanguages=context.ShippingLaunguages.Where(x=>x.ShippingMethodId.ToString() == id).ToList();
                context.ShippingLaunguages.RemoveRange(shippingMethodLanguages);
                context.ShippingMethods.Remove(shippingMethod);
                context.SaveChanges();
    
                return new SuccessResult();
            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }
        }

        public IDataResult<GetShippingMethodDTO> GetShipping(string id, string langCode)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var result = context.ShippingMethods.Include(x=>x.ShippingLaunguages)
                        .FirstOrDefault(x => x.Id.ToString() == id )
                        
                      
                        ;

                    if (result == null)
                    {
                        return new ErrorDataResult<GetShippingMethodDTO>("Shipping method not found for the given id and langCode");
                    }

                    return new SuccessDataResult<GetShippingMethodDTO>(new GetShippingMethodDTO
                    {
                        Id = result.Id.ToString(),
                        Content = result.ShippingLaunguages.FirstOrDefault(x=>x.LangCode==langCode).Content,
                        Price = result.DeliveryPrice,
                    });
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<GetShippingMethodDTO>(ex.Message);
            }
        }

        public IDataResult<List<GetShippingMethodDTO>> GetShippingAll(string langCode)
        {
            try
            {
                var context = new AppDbContext();
                var result = context.ShippingMethods.Select(x => new GetShippingMethodDTO
                {
                    Id = x.Id.ToString(),
                    Content = x.ShippingLaunguages.FirstOrDefault(y => y.LangCode == langCode).Content,
                    Price = x.DeliveryPrice


                }).ToList();
                return new SuccessDataResult<List<GetShippingMethodDTO>>(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
