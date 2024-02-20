using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.SizeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Abstract
{
    public interface ISizeService
    {
        Task<IResult> SizeAddAsync(AddSizeDTO addSizeDTO);
        IDataResult<List<SizeGetAdminDetailDTO>> GetSize(Expression<Func<Size, bool>>? expression = null);
        Task<IResult> SizeUpdateAsync(UpdateSizeDTO updateSizeDTO);
        Task<IResult> DeleteSizeAsync(string id);
    }
}
