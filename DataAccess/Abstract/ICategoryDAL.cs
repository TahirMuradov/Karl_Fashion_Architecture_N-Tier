using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities;
using Entities.DTOs.CategoryDTOs;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICategoryDAL:IRepositoryBase<Category>
    {
        Task<bool> AddCategoryAsync(CategoryAddDTO categoryAddDTO);
        bool AddCategory(CategoryAddDTO categoryAddDTO);
        Task<bool> DeleteCategoryAsync(CategoryRemoveDTO categgoryDeleteDTO);
        bool DeleteCategory(CategoryRemoveDTO categgoryDeleteDTO);
        Task<bool> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDTO);
        bool UpdateCategory(CategoryUpdateDTO categoryUpdateDTO);
    
    

    }
}
