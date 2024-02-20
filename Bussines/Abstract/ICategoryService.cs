using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Abstract
{

    public interface ICategoryService
    {

        public IResult AddCategory(CategoryAddDTO categoryAddDTO);
        public Task< IResult> AddCategoryAsync(CategoryAddDTO categoryAddDTO);
        public IResult DeleteCategory(CategoryRemoveDTO categoryRemoveDTO);
        public Task< IResult> DeleteCategoryAsync(CategoryRemoveDTO categoryRemoveDTO);
        //public  IResult UpdateCategory (CategoryUpdateDTO categoryUpdateDTO);
        public Task< IResult> UpdateCategoryAsync (CategoryUpdateDTO categoryUpdateDTO);
        public Task<IDataResult< List<CategoryGetAdminListDTO>>> GetCategoryAdminListAsync(Expression<Func<Category, bool>>? expression = null);
        public IDataResult<List<CategoryGetDTO>> GetCategoryName(string culture);

    }
}
