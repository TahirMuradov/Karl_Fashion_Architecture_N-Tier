using Core.Utilities.Results.Abstract;
using Entities.DTOs.OrderProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Abstract
{
    public interface IOrderProductService
    {
        public IDataResult<List<GetSoldProducts>> GetAllSoldProducts();
    }
}
