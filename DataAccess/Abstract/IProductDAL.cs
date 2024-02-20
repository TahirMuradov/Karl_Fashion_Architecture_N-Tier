using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface IProductDAL:IRepositoryBase<Product>
    {
        Task<IResult> AddProductAsync(ProductAddDTO ProductAddDTO);
        IDataResult<List<ProductGetAdminListDTO>> GetProductAdminList();
        IDataResult<Product>GetProduct(Expression<Func<Product, bool>> expression);
      IDataResult<GetProductUIDTO>GetProductDetailUI(string id,string langCode);
        IDataResult<List<GetProductUIDTO>>GetProductListUI(Expression<Func<Product, bool>>? expression ,string LangCode);
        IDataResult<List<GetProductUIDTO>> GetRelatedProduct(List<string>Categories,string CurrentCulture);
        IDataResult<List<GetProductUIDTO>> GetProductFilterUI(string currentCulture, string? category = null, string? color = null, string? size = null, string? minPrice = null, string? maxPrice = null);
        IDataResult<List<string>> GetAllProductsColor();
        IDataResult<Dictionary<string,decimal>> GetAllProductMaxAndMinPrice();
        IResult RemoveProduct(ProductRemoveDTO productRemoveDTO);
        IResult UpdateProduct(ProductUpdateDTO productUpdateDTO); 

    }
}
