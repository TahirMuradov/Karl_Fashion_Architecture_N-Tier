using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities.DTOs.ShippingMethods;
using Entities.DTOs.ShippingMethodsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Concrete
{
    public class ShippingMethodManager : IShippingMethodsServices
    {
        private readonly IShippingMethodDAL _shippingMethodDAL;

        public ShippingMethodManager(IShippingMethodDAL shippingMethodDAL)
        {
            _shippingMethodDAL = shippingMethodDAL;
        }

        public IResult AddShipping(AddShippingMethodDTO addShippingMethodDTO)
        {
            return _shippingMethodDAL.AddShipping(addShippingMethodDTO);
        }

        public IResult DeleteShipping(string id)
        {
          return _shippingMethodDAL.DeleteShipping(id);
        }

        public IDataResult<GetShippingMethodDTO> GetShipping(string id, string langCode)
        {
           return _shippingMethodDAL.GetShipping(id, langCode);
        }

        public IDataResult<List<GetShippingMethodDTO>> GetShippingAll(string langCode)
        {
            return _shippingMethodDAL.GetShippingAll(langCode);
        }
    }
}
