using Core.DataAccess.EntityFramework;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFOrderDAL : EFRepositoryBase<Order, AppDbContext>, IOrderDAL
    {
        
        public IResult AddOrder(Order order)
        {
            try
            {
                var context = new AppDbContext();
                context.Orders.Add(order);
                context.SaveChanges();
                
                foreach (var product in order.Products)
                {
                    OrderProduct orderProduct = new OrderProduct()
                    {
                        
                        OrderId=order.Id,
                        ProductID=product.Id,
                      
                    };
                    context.OrderProducts.Add(orderProduct);


                }
             context.SaveChanges();

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
                        }

        }

        public IResult DeleteOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Order>> GetAllOrder()
        {
            throw new NotImplementedException();
        }

        public IDataResult<Order> GetOrder(string id)
        {
            throw new NotImplementedException();
        }
    }
}
