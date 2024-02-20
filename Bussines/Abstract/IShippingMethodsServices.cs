using Core.Utilities.Results.Abstract;
using Entities.DTOs.ShippingMethods;
using Entities.DTOs.ShippingMethodsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Abstract
{
    public interface IShippingMethodsServices
    {
        IResult AddShipping(AddShippingMethodDTO addShippingMethodDTO);
        IResult DeleteShipping(string id);
        IDataResult<GetShippingMethodDTO> GetShipping(string id, string langCode);
        IDataResult<List<GetShippingMethodDTO>> GetShippingAll(string langCode);
    }
}
