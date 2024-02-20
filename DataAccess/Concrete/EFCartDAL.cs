using Core.DataAccess.EntityFramework;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities.Concrete;
using Entities.DTOs.Cart;
using Entities.DTOs.CheckOutDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFCartDAL : EFRepositoryBase<Cart, AppDbContext>, ICartDAL
    {
        public IResult AddCart( List<CartAddDTO> cartAddDTO,ShippingMethodAndUserInfoDTO checkOutDTO)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    Cart cart = new Cart()
                    {

                        PhoneNumber = checkOutDTO.PhoneNumber,
                        ShippingMethodsId = Guid.Parse( checkOutDTO.ShippingMethodId),

                    };
                    if (!string.IsNullOrEmpty(checkOutDTO.FirstName) && !string.IsNullOrEmpty(checkOutDTO.LastName))
                    {
                        cart.FirstName = checkOutDTO.FirstName;
                        cart.LastName = checkOutDTO.LastName;
                    }
                    else if (checkOutDTO.UserId is not null)
                    {
                        cart.UserId = checkOutDTO.UserId;

                    }
                    context.Carts.Add(cart);
                    context.SaveChanges();


                    foreach (var item in cartAddDTO)
                    {
                        Item SoldProduct = new Item()
                        {
                           ProductId = item.ProductId,
                            ProductCode= item.ProductCode,
                            Price= item.Price,
                            ProductName= item.ProductName,
                            Size= item.Size,
                            count=item.Quantity,
                            DateOfSale=DateTime.Now,
                        };
                        var ProductDBDelete=context.Products.FirstOrDefault(x=>x.Id==SoldProduct.Id).ProductSizes.FirstOrDefault(x=>x.Size.NumberSize==int.Parse(SoldProduct.Size)).SizeStockCount-SoldProduct.count;
                        context.SoldProducts.Add(SoldProduct);


                        CartItem cartItem = new CartItem()
                        {
                            CartId = cart.Id,
                          ItemId=SoldProduct.Id,
                          
                        };
                        context.CartItems.Add(cartItem);
                    }
                    context.SaveChanges();

                    

                }
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            
            }
      
        }
    }
}
