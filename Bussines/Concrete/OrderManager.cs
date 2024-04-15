using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities.DTOs.OrderDTOs;

namespace Bussines.Concrete
{
    public class OrderManager : IOrderServices
    {
        private readonly IOrderDAL _orderDAL;

        public OrderManager(IOrderDAL orderDAL)
        {
            _orderDAL = orderDAL;
        }

        public IResult AddOrder(OrderAddDTO orderAddDTO)
        {
            return _orderDAL.AddOrder(orderAddDTO);
        }

        public IResult ChangeOrderStatus(string OrderId)
        {
          return _orderDAL.ChangeOrderStatus(OrderId);
        }

        public IResult DeleteOrder(string OrderId)
        {
            return _orderDAL.DeleteOrder(OrderId);
        }

        public IDataResult<List<OrderGetDTO>> GetAllOrder()
        {
          return _orderDAL.GetAllOrder();
        }

        public IDataResult<OrderGetDTO> GetOrder(string id)
        {
            throw new NotImplementedException();
        }
    }
}
