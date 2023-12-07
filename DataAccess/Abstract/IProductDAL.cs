using Core.DataAccess;
using Entities;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;

namespace DataAccess.Abstract
{
    public interface IProductDAL:IRepositoryBase<Product>
    {
        Task<bool> AddProductAsync(ProductAddDTO ProductAddDTO);
        bool AddProduct(ProductAddDTO ProductAddDTO);
        Task<bool> DeleteCategoryAsync(ProductRemoveDTO ProductDeleteDTO);
        bool DeleteProduct(ProductRemoveDTO ProductDeleteDTO);
        Task<bool> UpdateProductAsync(Product ProductUpdate);
        bool UpdateProduct(Product ProductUpdate);

    }
}
