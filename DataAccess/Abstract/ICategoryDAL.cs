using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Entities;
using Entities.DTOs.CategoryDTOs;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICategoryDAL:IRepositoryBase<Category>
    {
        Task<IResult> AddCategoryAsync(CategoryAddDTO categoryAddDTO);
        IResult AddCategory(CategoryAddDTO categoryAddDTO);
        Task<IResult> DeleteCategoryAsync(CategoryRemoveDTO categgoryDeleteDTO);
        IResult DeleteCategory(CategoryRemoveDTO categgoryDeleteDTO);
        Task<IResult> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDTO);
        //IResult UpdateCategory(CategoryUpdateDTO categoryUpdateDTO);
       Task< IDataResult<List<CategoryGetAdminListDTO>>> GetCategoriesAsync(Expression<Func<Category, bool>>? expression = null);


        IDataResult<List<CategoryGetDTO>>GetCategoryName(string langCode);



    }
}
