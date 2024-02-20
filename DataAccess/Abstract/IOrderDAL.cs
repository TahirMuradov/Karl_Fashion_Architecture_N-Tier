using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IOrderDAL:IRepositoryBase<Order>
    {

        IResult AddOrder(Order order);
        IResult DeleteOrder(Order order);
        IDataResult<Order> GetOrder(string id);
        IDataResult<List<Order>> GetAllOrder();
    }
}
