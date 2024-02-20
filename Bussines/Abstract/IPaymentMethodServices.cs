using Core.Utilities.Results.Abstract;
using Entities.DTOs.PaymentMethodDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Abstract
{
    public interface IPaymentMethodServices
    {
        IResult Add(AddPaymentMethodsDTO addPaymentMethodsDTO);
        public IResult DeletePaymentMethod(string Id);
        public IDataResult<List<GetPaymentMethodDTO>> GetAllPaymentMethod(string langCode);
        public IDataResult<GetPaymentMethodDTO> GetPaymentMethod(string Id, string langCode);
    }
}
