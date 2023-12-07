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
            return _categoryDAL.AddCategory(categoryAddDTO) ? new SuccessResult("Category created successfully") : new ErrorResult();
        }

        public async Task< IResult> AddCategoryAsync(CategoryAddDTO categoryAddDTO)
        {
            return await _categoryDAL.AddCategoryAsync(categoryAddDTO) ? new SuccessResult("Category created successfully") : new ErrorResult();

        }

        public IResult DeleteCategory(CategoryRemoveDTO categoryRemoveDTO)
        {
            return _categoryDAL.DeleteCategory(categoryRemoveDTO) ? new SuccessResult("Category deleted successfully") : new ErrorResult();
        }

        public async Task< IResult> DeleteCategoryAsync(CategoryRemoveDTO categoryRemoveDTO)
        {
            return await _categoryDAL.DeleteCategoryAsync(categoryRemoveDTO) ? new SuccessResult("Category deleted successfully") : new ErrorResult();
        }

        public  IResult UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
        {
            return  _categoryDAL.UpdateCategory(categoryUpdateDTO) ? new SuccessResult("Category updated successfully") : new ErrorResult();
        }

        public async Task<IResult> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
          return await _categoryDAL.UpdateCategoryAsync(categoryUpdateDTO) ? new SuccessResult("Category updated successfully") : new ErrorResult();
        }
    }
}
