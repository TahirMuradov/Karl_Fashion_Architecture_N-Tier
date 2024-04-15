using Core.DataAccess.EntityFramework;
using Core.Helper.FileHelper;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities;
using Entities.Concrete;
using Entities.Concrete.EnumClass;
using Entities.DTOs.OrderDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Concrete
{
    public class EFOrderDAL : EFRepositoryBase<Order, AppDbContext>, IOrderDAL
    {

        public IResult AddOrder(OrderAddDTO orderAddDTO)
        {
            try
            {
                var context = new AppDbContext();
                Order order = new Order()
                {
                    Address = orderAddDTO.Address,
                    CreatedDate=DateTime.Now,
                    Email = orderAddDTO.Email,
                    FirstName = orderAddDTO.FirstName,
                    LastName = orderAddDTO.LastName,
                    Message = orderAddDTO.Message,
                    OrderStatus= 0,
                    PhoneNumber = orderAddDTO.PhoneNumber,
                    PaymentMethodId= orderAddDTO.PaymentMethodId,
                    ShippingMethodId= orderAddDTO.ShippingMethodId,

                    OrderNumber= orderAddDTO.OrderNumber,
                    OrderPDfUrl= orderAddDTO.OrderPDfUrl,

                    
                };
                context.Orders.Add(order);
                context.SaveChanges();
                decimal TotlaAmountOrder = 0;
                foreach (var item in orderAddDTO.OrderProducts)
                {
                    OrderProduct orderProduct = new OrderProduct()
                    {
                        Amount = item.Amount,
                        Count = item.Count,
                        CreatedDate = DateTime.Now,
                        OrderId = order.Id,
                        ProductName = item.ProductName,
                        ProductCode = item.ProductCode,
                        ProductId = item.ProductId,
                        Size = item.Size
                        

                    };
                    TotlaAmountOrder += orderProduct.Amount * orderProduct.Count;
                    context.OrderProducts.Add(orderProduct);
                }
                order.TotalAmount = TotlaAmountOrder;
                context.Orders.Update(order);
                context.SaveChanges();
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }

        }

        public IResult ChangeOrderStatus(string OrderId)
        {
            try
            {
                using (var context=new AppDbContext())
                {
                    var order = context.Orders.FirstOrDefault(x => x.Id.ToString() == OrderId);
                    switch (order.OrderStatus)
                    {
                        case OrderStatus.Gözlənilir:
                            order.OrderStatus = OrderStatus.Çatdirildı;
                            break;
                        case OrderStatus.Çatdirildı:
                            order.OrderStatus = OrderStatus.İmtina;
                            break;
                        case OrderStatus.İmtina:
                            order.OrderStatus = OrderStatus.Gözlənilir;
                            break;
                       
                    }
                            context.Orders.Update(order);
                            context.SaveChanges();

                }
                return new SuccessResult();

            }
            catch (Exception ex)
            {

                return new ErrorResult(message: ex.Message);
            }
        }

        public IResult DeleteOrder(string OrderId)
        {
            try
            {
                using (var context=new AppDbContext())
                {
                    var Order = context.Orders.FirstOrDefault(x => x.Id.ToString() == OrderId);
                    if (Order == null) return new ErrorResult(message:"Order is not Found!");
                  bool result=  FileHelper.RemoveFile(Order.OrderPDfUrl);
                    if (result)
                    {
                    var OrderProduct = context.OrderProducts.Where(x => x.OrderId.ToString() == OrderId);
                    context.OrderProducts.RemoveRange(OrderProduct);
                    context.Orders.Remove(Order);
                    context.SaveChanges();
                return new SuccessResult();

                    }
                }
                return new ErrorResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(message: ex.Message);
            }
        }

        public IDataResult<List<OrderGetDTO>> GetAllOrder()
        {
            try
            {
                using var context = new AppDbContext();
                return new SuccessDataResult<List<OrderGetDTO>>(data:


                    context.Orders.Include(x => x.Products).Select(x => new OrderGetDTO
                    {
                        Address = x.Address,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Message = x.Message,
                        OrderId = x.Id.ToString(),
                        OrderNubmer = x.OrderNumber,
                        OrderPdfUrl = x.OrderPDfUrl,
                        PaymentMethodId = x.PaymentMethodId,
                        PhoneNumber = x.PhoneNumber,
                        ShippingMethodId = x.ShippingMethodId,
                        Status=x.OrderStatus


                    }).ToList()

                    );
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<OrderGetDTO>>(message:ex.Message);
            }
         
        }

        public IDataResult<OrderGetDTO> GetOrder(string id)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var data = context.Orders.Include(x => x.Products).FirstOrDefault(x => x.Id.ToString() == id);

                    if (data == null)
                        return new ErrorDataResult<OrderGetDTO>(message: "Order is not found!");

                   
                    var orderGetDTO = new OrderGetDTO
                    {
                        OrderId = data.Id.ToString(),
                        OrderNubmer = data.OrderNumber,
                        OrderPdfUrl = data.OrderPDfUrl,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        PhoneNumber = data.PhoneNumber,
                        Email = data.Email,
                        Address = data.Address,
                        Message = data.Message,
                        PaymentMethodId = data.PaymentMethodId,
                        ShippingMethodId = data.ShippingMethodId,
                        Status=data.OrderStatus
                    };

                    return new SuccessDataResult<OrderGetDTO>(data:orderGetDTO);
                }
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<OrderGetDTO>(message: ex.Message);
            }

        }
    }
}
