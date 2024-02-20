using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.SizeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Concrete
{
    public class SizeManager : ISizeService
    {
        private readonly ISizeDAL _sizeDAL;

        public SizeManager(ISizeDAL sizeDAL)
        {
            _sizeDAL = sizeDAL;
        }

        public async Task<IResult> DeleteSizeAsync(string id)
        {
            return await _sizeDAL.DeleteSizeAsync(id);
        }

        public IDataResult<List<SizeGetAdminDetailDTO>> GetSize(Expression<Func<Size, bool>>? expression = null)
        {
            return _sizeDAL.GetSize(expression);
        }

        public async Task<IResult> SizeAddAsync(AddSizeDTO addSizeDTO)
        {
            return await _sizeDAL.AddSizeAsync(addSizeDTO);
        }

        public async Task<IResult> SizeUpdateAsync(UpdateSizeDTO updateSizeDTO)
        {
           return await _sizeDAL.UpdateSizeAsync(updateSizeDTO);
        }
    }
}
