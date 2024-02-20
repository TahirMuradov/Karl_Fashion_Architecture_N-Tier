using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.Cart;
using Entities.DTOs.CheckOutDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICartDAL:IRepositoryBase<Cart>
    {
        public IResult AddCart(List<CartAddDTO> cartAddDTO, ShippingMethodAndUserInfoDTO checkOutDTO);

    }
}
