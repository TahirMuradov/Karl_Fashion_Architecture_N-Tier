using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.PaymentMethodDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IPaymentMethodDAL:IRepositoryBase<PaymentMethod>
    {
        public IResult AddPaymentMethod(AddPaymentMethodsDTO addPaymentMethodsDTO);
        public IResult DeletePaymentMethod(string Id);
        public IDataResult<List<GetPaymentMethodDTO>> GetAllPaymentMethod(string langCode);
        public IDataResult<GetPaymentMethodDTO> GetPaymentMethod(string Id,string langCode);
    }
}
