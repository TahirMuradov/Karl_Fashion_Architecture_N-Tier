using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities.DTOs.OrderProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Concrete
{
    public class OrderProductManager : IOrderProductService
    {
        private readonly IOrderProductDAL _orderProductDAL;

        public OrderProductManager(IOrderProductDAL orderProductDAL)
        {
            _orderProductDAL = orderProductDAL;
        }

        public IDataResult<List<GetSoldProducts>> GetAllSoldProducts()
        {
            return _orderProductDAL.GetAllSoldProducts();
        }
    }
}
