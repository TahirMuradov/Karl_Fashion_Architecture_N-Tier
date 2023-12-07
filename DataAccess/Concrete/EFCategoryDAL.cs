using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DataAccess.Concrete
{
    public class EFCategoryDAL : EFRepositoryBase<Category, AppDbContext>, ICategoryDAL
    {
        private readonly UserManager<User> _userManager;
        
        public EFCategoryDAL(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public bool AddCategory(CategoryAddDTO categoryAddDTO)
        {
            try
            {
                using var context = new AppDbContext();
           var currentUser=_userManager.Users.FirstOrDefault(x=>x.Id==categoryAddDTO.CreatorUserId);
                Category category = new Category()
                {
                    CreatorUserId=categoryAddDTO.CreatorUserId,
                    User=currentUser,
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

                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public async Task<bool> AddCategoryAsync(CategoryAddDTO categoryAddDTO)
        {
            try
            {
                using var context = new AppDbContext();
                Category category = new Category()
                {
                    CreatedDate = DateTime.Now,
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                for (int i = 0; i < categoryAddDTO.LangCode.Count; i++)
                {
                    CategoryLanguage categoryLanguage = new CategoryLanguage()
                    {

                        CategoryName = categoryAddDTO.CategoryName[i],
                        LangCode = categoryAddDTO.LangCode[i],
                        CategoryId = category.Id,
                        Category = category


                    };
                    await context.CategoryLanguages.AddAsync(categoryLanguage);


                }
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteCategory(CategoryRemoveDTO categgoryDeleteDTO)
        {

            try
            {
                using var context = new AppDbContext();
                //var category = context.Categories.FirstOrDefault(x => x.Id == categgoryDeleteDTO.CategoryId);
                var categoryLanguages = context.CategoryLanguages
                    .Where(x => x.CategoryId == categgoryDeleteDTO.CategoryId)
                    .Include(x => x.Category).ToList();
                context.RemoveRange(categoryLanguages);
                context.SaveChanges();

                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(CategoryRemoveDTO categgoryDeleteDTO)
        {
            try
            {
                using var context = new AppDbContext();
                //var category = context.Categories.FirstOrDefault(x => x.Id == categgoryDeleteDTO.CategoryId);
                var categoryLanguages = context.CategoryLanguages
                    .Where(x => x.CategoryId == categgoryDeleteDTO.CategoryId)
                    .Include(x => x.Category).ToList();
                context.RemoveRange(categoryLanguages);
             await   context.SaveChangesAsync();

                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }

        public List<Category> GetAllCategories(CategoryGetDTO categoryGetDTO)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                using var context = new AppDbContext();
             for (int i = 0;i<categoryUpdateDTO.LanguageCode.Count;i++)
                {
                    var categoryLanguages = context.CategoryLanguages
                        .FirstOrDefault(x => x.CategoryId == categoryUpdateDTO.CategoryId && x.LangCode == categoryUpdateDTO.LanguageCode[i]);
                    if (categoryLanguages!=null)
                    {
                        categoryLanguages.CategoryName= categoryUpdateDTO.LanguageCode[i];
                        context.Update(categoryLanguages);
                    }
                }

             context.SaveChanges();

                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                using var context = new AppDbContext();
                for (int i = 0; i < categoryUpdateDTO.LanguageCode.Count; i++)
                {
                    var categoryLanguages = await context.CategoryLanguages

                        .FirstOrDefaultAsync(x => x.CategoryId == categoryUpdateDTO.CategoryId && x.LangCode == categoryUpdateDTO.LanguageCode[i]);
                    if (categoryLanguages != null)
                    {
                        categoryLanguages.CategoryName = categoryUpdateDTO.LanguageCode[i];
                        context.Update(categoryLanguages);
                    }
                }

               await context.SaveChangesAsync();

                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
