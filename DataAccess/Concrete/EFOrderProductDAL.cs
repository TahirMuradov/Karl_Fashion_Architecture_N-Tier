using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities.DTOs.OrderProductDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFOrderProductDAL : IOrderProductDAL
    {
        public IDataResult<List<GetSoldProducts>> GetAllSoldProducts()
        {
			try
			{
				using (var context=new AppDbContext())
				{
					var result=context.OrderProducts.Include(x=>x.Order).Select(x=> new GetSoldProducts
					{
						Amount = x.Amount,
						Count = x.Count,
						Id=x.Id.ToString(),
						ProductName = x.ProductName,
						ProductCode = x.ProductCode,
						Size = x.Size,
						SoldDate=x.CreatedDate,
						OrderNumber=x.Order.OrderNumber
					}).ToList();
					return new SuccessDataResult<List<GetSoldProducts>>(data: result);
				}

			}
			catch (Exception ex)
			{

				return new ErrorDataResult<List<GetSoldProducts>>(message: ex.Message);
			}
        }
    }
}
