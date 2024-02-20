using AutoMapper;
using Core.DataAccess.EntityFramework;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Concrete
{
    public class EFCategoryDAL : EFRepositoryBase<Category, AppDbContext>, ICategoryDAL
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapp;


        public EFCategoryDAL(UserManager<User> userManager, IMapper mapper, IMapper mapp)
        {
            _userManager = userManager;
            _mapp = mapp;
        }

        public IResult AddCategory(CategoryAddDTO categoryAddDTO)
        {
            try
            {
                using var context = new AppDbContext();
                var creatotUser = _userManager.Users.FirstOrDefault(x => x.Id == categoryAddDTO.CreatorUserId);

                Category category = new Category()
                {

                    User = creatotUser,
                    CreatedDate = DateTime.Now,
                };

                context.Categories.Add(category);
                context.SaveChanges();

                for (int i = 0; i < categoryAddDTO.LangCode.Count; i++)
                {
                    CategoryLanguage categoryLanguage = new CategoryLanguage()
                    {

                        CategoryName = categoryAddDTO.CategoryName[i],
                        LangCode = categoryAddDTO.LangCode[i],
                        CategoryId = category.Id,
                        Category = category


                    };
                    context.CategoryLanguages.Add(categoryLanguage);


                }
                context.SaveChanges();

                return new Result(true, "Category Elave Olundu");
            }
            catch (Exception)
            {

                return new Result(false, "Category Elave edilmedi");
            }

        }

        public async Task<IResult> AddCategoryAsync(CategoryAddDTO categoryAddDTO)
        {
            try
            {
                var creatotUser = await _userManager.FindByIdAsync(categoryAddDTO.CreatorUserId);
                using var context = new AppDbContext();
                Category category = new Category()
                {

                    UserId = creatotUser.Id,
                    CreatedDate = DateTime.Now,
                };

                await context.Categories.AddAsync(category);


                for (int i = 0; i < categoryAddDTO.LangCode.Count; i++)
                {
                    CategoryLanguage categoryLanguage = new CategoryLanguage()
                    {

                        CategoryName = categoryAddDTO.CategoryName[i],
                        LangCode = categoryAddDTO.LangCode[i],
                        CategoryId = category.Id,
                        Category = category,



                    };
                    await context.CategoryLanguages.AddAsync(categoryLanguage);


                }
                await context.SaveChangesAsync();

                return new Result(true, "Category Elave Olundu");
            }
            catch (Exception)
            {

                return new Result(false, "Category Elave edilmedi");
            }
        }

        public IResult DeleteCategory(CategoryRemoveDTO categgoryDeleteDTO)
        {

            try
            {
                using var context = new AppDbContext();
                //var category = context.Categories.FirstOrDefault(x => x.Id == categgoryDeleteDTO.CategoryId);
                var category = context.Categories.
                    Include(x => x.CategoryLanguages)
                    .Include(x => x.CategoryProducts)
                        .ThenInclude(x => x.Product)
                            .ThenInclude(x => x.productLanguages).ToList();

                context.RemoveRange(category);
                context.SaveChanges();

                return new Result(true, "Category  Silindi");

            }
            catch (Exception)
            {

                return new Result(false, "Category Silinmedi");
            }
        }

        public async Task<IResult> DeleteCategoryAsync(CategoryRemoveDTO categgoryDeleteDTO)
        {
            try
            {
                using var context = new AppDbContext();
                //var category = context.Categories.FirstOrDefault(x => x.Id == categgoryDeleteDTO.CategoryId);
                var category = context.Categories.
                    Include(x => x.CategoryLanguages)
                        .Include(x => x.User)
                    .Include(x => x.CategoryProducts).ThenInclude(x => x.Product).ThenInclude(a => a.productLanguages)
                    .ToList();
                context.RemoveRange(category);
                await context.SaveChangesAsync();


                return new Result(true, "Category  Silindi");


            }
            catch (Exception)
            {

                return new Result(false, "Category Silinmedi");
            }

        }

        public async Task<IDataResult<List<CategoryGetAdminListDTO>>> GetCategoriesAsync(Expression<Func<Category, bool>> expression = null)
        {

            using var context = new AppDbContext();

            var query = expression == null ? context.Categories
                .Include(x => x.CategoryLanguages)
               .Include(x => x.User)
                .Include(x => x.CategoryProducts).ThenInclude(a => a.Product).ThenInclude(a => a.productLanguages)


                .ToList() :
                    context.Categories.Include(x => x.CategoryProducts).ThenInclude(a => a.Product).ThenInclude(a => a.productLanguages)
                .Include(x => x.User)
                 .Include(x => x.CategoryLanguages)
                 .ToList();





            var resultMaped = query.Select(categoryLanguage => new CategoryGetAdminListDTO
            {
                id = categoryLanguage.Id.ToString(),
                CategoryName = categoryLanguage.CategoryLanguages.Select(x => x.CategoryName).ToList(),
                LaunguageCode = categoryLanguage.CategoryLanguages.Select(x => x.LangCode).ToList(),
                CreatedDate = categoryLanguage.CreatedDate,
                UpdatedDate = categoryLanguage.UpdatedDate,
                UpdatedUserId = categoryLanguage.UpdatedUserId,
                UserName = categoryLanguage.User.UserName,
                IsFeatured = categoryLanguage.IsFeatured,
            }).ToList();
            return new SuccessDataResult<List<CategoryGetAdminListDTO>>(resultMaped);



        }

        public IDataResult<List<CategoryGetDTO>> GetCategoryName(string langCode)
        {
            try
            {
                var context = new AppDbContext();
              
             List<CategoryGetDTO> categories=new List<CategoryGetDTO>();
            var category=    context.CategoryLanguages.Where(x => x.LangCode == langCode)
                    .Include(x=>x.Category)
                    
                    
                    .ToList();
                foreach (var categoryName in category)
                {
                    categories.Add(new CategoryGetDTO
                    {
                        CategoryId = categoryName.Category.Id.ToString(),
                        CategoryName = categoryName.CategoryName

                    });

                }

                return new SuccessDataResult<List<CategoryGetDTO>>(categories);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<CategoryGetDTO>>(ex.Message);
            }

        }

        //public IResult UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
        //{
        //    try
        //    {
        //        using var context = new AppDbContext();
        //        for (int i = 0; i < categoryUpdateDTO.LanguageCode.Count; i++)
        //        {
        //            var categoryLanguages = context.CategoryLanguages
        //                .FirstOrDefault(x => x.CategoryId == categoryUpdateDTO.CategoryId && x.LangCode == categoryUpdateDTO.LanguageCode[i]);
        //            if (categoryLanguages != null)
        //            {
        //                categoryLanguages.CategoryName = categoryUpdateDTO.LanguageCode[i];
        //                context.Update(categoryLanguages);
        //            }
        //        }

        //        context.SaveChanges();

        //        return new Result(true, "Category  Yenilendi");

        //    }
        //    catch (Exception)
        //    {

        //        return new Result(false, "Category Yenilenmedi");
        //    }
        //}

        public async Task<IResult> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {

                using var context = new AppDbContext();
                var category = context.Categories.Where(x => x.Id.ToString() == categoryUpdateDTO.CategoryId)
                            .Include(x => x.User)
                            .Include(x => x.CategoryProducts).ThenInclude(a => a.Product).ThenInclude(a => a.productLanguages)
                            .Include(x => x.CategoryLanguages)
                      ;
                foreach (var categoryUP in category)
                {
                    categoryUP.UpdatedDate = DateTime.Now;
                    categoryUP.UpdatedUserId = categoryUpdateDTO.UpdatedUserId;
                    for (int i = 0; i < categoryUpdateDTO.NewCategoryName.Count; i++)
                    {

                        categoryUP.CategoryLanguages[i].CategoryName = categoryUpdateDTO.NewCategoryName[i];

                    }
                }

                context.UpdateRange(category);
                await context.SaveChangesAsync();

                return new Result(true, "Category  Yenilendi");

            }
            catch (Exception)
            {

                return new Result(false, "Category Yenilenmedi");
            }
        }
    }
}
