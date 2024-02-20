using Core.Utilities.Results.Abstract;
using Entities;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Abstract
{
    public interface IProductServices
    {
       Task< IResult> AddProductAsync(ProductAddDTO productAddDTO);
        IDataResult<List<ProductGetAdminListDTO>> ProductGetAdminList();
        IDataResult<Product> GetProduct(Expression<Func<Product, bool>> expression);
        IDataResult<List<GetProductUIDTO>> GetProductListUI(Expression<Func<Product, bool>>? expressionl,string LangCode);
        IDataResult<GetProductUIDTO> GetProductDetailUI(string id, string langCode);
        IDataResult<List<GetProductUIDTO>> GetRelatedProduct(List<string> Categories, string CurrentCulture);

        IDataResult<List<GetProductUIDTO>> GetProductFilterUI(string currentCulture, string? category = null, string? color = null, string? size = null, string? minPrice = null, string? maxPrice = null);
        IDataResult<List<string>> GetProductsColor();
        /// <summary>
        /// To find the highest and lowest prices within the product prices
        /// </summary>

        /// <returns>Dictioanary key["maxPrice"]</returns>
        IDataResult<Dictionary<string,decimal>> GetProductsMaxAndMinPrice();
        IResult RemoveProduct(ProductRemoveDTO productRemoveDTO);
        IResult UpdateProduct(ProductUpdateDTO productUpdateDTO);


    }
}
