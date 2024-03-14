using Core.DataAccess.EntityFramework;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities.Concrete;
using Entities.DTOs.PaymentMethodDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFPaymentMethodDAL : EFRepositoryBase<PaymentMethod, AppDbContext>, IPaymentMethodDAL
    {
        public IResult AddPaymentMethod(AddPaymentMethodsDTO addPaymentMethodsDTO)
        {
            try
            {

                using (var context = new AppDbContext())
                {


                    PaymentMethod paymentMethod = new PaymentMethod()
                    {
                        Api = addPaymentMethodsDTO.Api,
                    };
                    context.PaymentMethods.Add(paymentMethod);
                   
                    for (int i = 0; i < addPaymentMethodsDTO.LangCode.Count; i++)
                    {
                        PaymentMethodLaunge paymentMethodLaunge = new PaymentMethodLaunge()
                        {
                            LangCode = addPaymentMethodsDTO.LangCode[i],
                            Content = addPaymentMethodsDTO.Content[i],
                            PaymentMehtodId = paymentMethod.Id
                        };
                        context.PaymentMethodsLaunge.Add(paymentMethodLaunge);
                    }
                    context.SaveChanges();
                }


                
                return new SuccessResult();
            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }
           
        }

        public IResult DeletePaymentMethod(string Id)
        {
            using (var context = new AppDbContext())
            {
                var paymentMethod = context.PaymentMethods.FirstOrDefault(x => x.Id.ToString() == Id);
                var paymentMethodsLaunge=context.PaymentMethodsLaunge.Where(x=>x.PaymentMehtodId.ToString()==Id).ToList();
                context.PaymentMethodsLaunge.RemoveRange(paymentMethodsLaunge);
                context.PaymentMethods.Remove(paymentMethod);
                context.SaveChanges();

            }
            
            return new SuccessResult();
        }

        public IDataResult<List<GetPaymentMethodDTO>> GetAllPaymentMethod(string langCode)
        {
            List<GetPaymentMethodDTO> PaymentMethods;
            using (var context=new AppDbContext())
            {

               
                 PaymentMethods =context.PaymentMethods.Include(x=>x.PaymentMethodLaunguages).Select(x=>new GetPaymentMethodDTO
                {
                    Api=x.Api,
                    Content=x.PaymentMethodLaunguages.FirstOrDefault(y=>y.LangCode==langCode).Content,
                    Id=x.Id.ToString()
                   

                }).ToList();
            }
            return new SuccessDataResult<List<GetPaymentMethodDTO>>(PaymentMethods);
        }

        public IDataResult<GetPaymentMethodDTO> GetPaymentMethod(string Id, string langCode)
        {
            GetPaymentMethodDTO result;
            using (var context = new AppDbContext())
            {
                var paymentMethod = context.PaymentMethods.Include(x => x.PaymentMethodLaunguages).FirstOrDefault(x => x.Id.ToString() == Id).PaymentMethodLaunguages.FirstOrDefault(y => y.LangCode == langCode);
                result = new GetPaymentMethodDTO
                {
                    Id = paymentMethod.PaymentMehtodId.ToString(),
                    Content = paymentMethod.Content,
                    Api = paymentMethod.PaymentMethod.Api,

                };
                return new SuccessDataResult<GetPaymentMethodDTO>(result);
            }
        }
    }
}
