using Core.DataAccess.EntityFramework;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities.Concrete;
using Entities.DTOs.SizeDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFSizeDAL : EFRepositoryBase<Size, AppDbContext>, ISizeDAL
    {
        private readonly UserManager<User> _userManager;
public EFSizeDAL(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> AddSizeAsync(AddSizeDTO addSizeDTO)
        {
            try
            {
                var context = new AppDbContext();
                var currentUser = await _userManager.FindByIdAsync(addSizeDTO.CreatorUserId);
                Size newSize = new Size()
                {
                    NumberSize = addSizeDTO.Size,
                    CreatedDate = DateTime.Now,
                    UserId=currentUser.Id,
                   
                    
                };
                await context.AddAsync(newSize);
                context.SaveChanges();
                return new SuccessResult(message: "Ölçü Yaradıldı");
            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }

        }

        public async Task<IResult> DeleteSizeAsync(string id)
        {
            try
            {
                var context = new AppDbContext();
              var size= await context.Sizes
                    
                    .FirstOrDefaultAsync(x=>x.Id.ToString()==id);
                    
                    ;
                context.Sizes.Remove(size);
                context.ProductSizes.RemoveRange(context.ProductSizes.Where(x => x.SizeId.ToString() == id));
               await context.SaveChangesAsync();
                return new SuccessResult("Olcu silindi");
            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }
        }

        public IDataResult<List<SizeGetAdminDetailDTO>> GetSize(Expression<Func< Size,bool>>? expression = null)
        {
            try
            {
                var context = new AppDbContext();
                var query = expression is null ?
                                    context.Sizes.Include(x=>x.ProductSize)
                                    .ThenInclude(x=>x.Product)
                                    .ThenInclude(x=>x.productLanguages)
                                    .Include(x=>x.User).ToList()


                    :
                        context.Sizes.Where(expression).Include(x => x.ProductSize)
                                    .ThenInclude(x => x.Product)
                                    .ThenInclude(x => x.productLanguages)
                                    .Include(x => x.User).ToList();

                var result = query.Select(x => new SizeGetAdminDetailDTO
                {

                    SizeId = x.Id.ToString(),
                    ProductName =x.ProductSize.Select(x=>x.Product.ProductCode).ToList(),
                    Size=x.NumberSize,
                    StockSizeCount=x.ProductSize.Count,
                    CreatorUserName=x.User.UserName
                    

                }).ToList();
                return new SuccessDataResult<List<SizeGetAdminDetailDTO>>(result);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<SizeGetAdminDetailDTO>>(ex.Message);
            }
        }

        public async Task<IResult> UpdateSizeAsync(UpdateSizeDTO updateSizeDTO)
        {
            try
            {

                var context = new AppDbContext();
                var Size = await context.Sizes.FirstOrDefaultAsync(x => x.Id.ToString() == updateSizeDTO.SizeId);
                Size.NumberSize = updateSizeDTO.NewSizeContent;
                Size.UpdatedDate = DateTime.Now;
                Size.UpdatedUserId = updateSizeDTO.UpdatedUserId;
                context.Update(Size);
                await context.SaveChangesAsync();
                return new SuccessResult("Olcu Yenilendi");

            }
            catch (Exception ex)
            {

               return new ErrorResult(ex.Message);
            }
         
            
        }
    }
}
