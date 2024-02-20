using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities.DTOs.PaymentMethodDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Concrete
{
    public class PaymentMethodManager : IPaymentMethodServices
    {
        private readonly IPaymentMethodDAL _paymentMethodDAL;

        public PaymentMethodManager(IPaymentMethodDAL paymentMethodDAL)
        {
            _paymentMethodDAL = paymentMethodDAL;
        }

        public IResult Add(AddPaymentMethodsDTO addPaymentMethodsDTO)
        {
        return _paymentMethodDAL.AddPaymentMethod(addPaymentMethodsDTO);
        }

        public IResult DeletePaymentMethod(string Id)
        {
          return _paymentMethodDAL.DeletePaymentMethod(Id);
        }

        public IDataResult<List<GetPaymentMethodDTO>> GetAllPaymentMethod(string langCode)
        {
            return _paymentMethodDAL.GetAllPaymentMethod(langCode);
        }

        public IDataResult<GetPaymentMethodDTO> GetPaymentMethod(string Id, string langCode)
        {
           return _paymentMethodDAL.GetPaymentMethod(Id, langCode);
        }
    }
}
