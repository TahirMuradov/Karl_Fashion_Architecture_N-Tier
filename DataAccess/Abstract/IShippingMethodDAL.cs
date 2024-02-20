using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.ShippingMethods;
using Entities.DTOs.ShippingMethodsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IShippingMethodDAL:IRepositoryBase<ShippingMethods>
    {
        IResult AddShipping(AddShippingMethodDTO addShipping);
        IResult DeleteShipping(string id);
        IDataResult<GetShippingMethodDTO> GetShipping(string id,string langCode);
        IDataResult<List<GetShippingMethodDTO>> GetShippingAll(string langCode);




    }
}
