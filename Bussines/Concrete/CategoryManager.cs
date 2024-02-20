using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Concrete
{
    public class CategoryManager : ICategoryService
    {
    private readonly ICategoryDAL _categoryDAL;

        public CategoryManager(ICategoryDAL categoryDAL)
        {
            _categoryDAL = categoryDAL;

                   
        }

        public IResult AddCategory(CategoryAddDTO categoryAddDTO)
        {
            return _categoryDAL.AddCategory(categoryAddDTO);
        }

        public async Task<IResult> AddCategoryAsync(CategoryAddDTO categoryAddDTO)
        {
            return await _categoryDAL.AddCategoryAsync(categoryAddDTO);
        }

        public IResult DeleteCategory(CategoryRemoveDTO categoryRemoveDTO)
        {
      return _categoryDAL.DeleteCategory(categoryRemoveDTO);
        }

        public async Task<IResult> DeleteCategoryAsync(CategoryRemoveDTO categoryRemoveDTO)
        {
            return await _categoryDAL.DeleteCategoryAsync(categoryRemoveDTO);
        }

        public async Task<IDataResult< List<CategoryGetAdminListDTO>>> GetCategoryAdminListAsync( Expression<Func<Category,bool>>? expression=null)
        {
            return expression is not null? await _categoryDAL.GetCategoriesAsync(expression: expression): await _categoryDAL.GetCategoriesAsync();
        }

        public IDataResult<List<CategoryGetDTO>> GetCategoryName(string culture)
        {
            return _categoryDAL.GetCategoryName(culture);
        }

        //public IResult UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
        //{
        // return _categoryDAL.UpdateCategoryAsync(categoryUpdateDTO);
        //}

        public async Task<IResult> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
          return await _categoryDAL.UpdateCategoryAsync(categoryUpdateDTO);
        }
    }
}
