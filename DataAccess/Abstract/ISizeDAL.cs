using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.SizeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ISizeDAL:IRepositoryBase<Size>
    {
        Task<IResult> AddSizeAsync(AddSizeDTO addSizeDTO);
       IDataResult<List<SizeGetAdminDetailDTO>> GetSize(Expression<Func<Size,bool>>? expression = null);

        Task<IResult> UpdateSizeAsync(UpdateSizeDTO updateSizeDTO);
        Task<IResult> DeleteSizeAsync(string id);
        //Task<IDataResult<Size>> GetSize();

    }
}
