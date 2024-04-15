using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities;
using Entities.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IOrderDAL:IRepositoryBase<Order>
    {

        IResult AddOrder(OrderAddDTO orderAddDTO);
        IResult DeleteOrder(string OrderId);
        IResult ChangeOrderStatus(string OrderId);
        IDataResult<OrderGetDTO> GetOrder(string id);
        IDataResult<List<OrderGetDTO>> GetAllOrder();
    }
}
