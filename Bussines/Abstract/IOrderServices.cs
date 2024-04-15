using Core.Utilities.Results.Abstract;
using Entities;
using Entities.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Abstract
{
    public interface IOrderServices
    {
        IResult AddOrder(OrderAddDTO orderAddDTO);
        IResult ChangeOrderStatus(string OrderId);
        IResult DeleteOrder(string OrderId);
        IDataResult<OrderGetDTO> GetOrder(string id);
        IDataResult<List<OrderGetDTO>> GetAllOrder();
    }
}
