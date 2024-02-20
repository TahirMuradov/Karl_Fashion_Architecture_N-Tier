using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using Entities;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bussines.Concrete
{
    public class ProductManager : IProductServices
    {
        private readonly IProductDAL _productDAL;

        public ProductManager(IProductDAL productDAL)
        {
            _productDAL = productDAL;
        }

        public async Task< IResult> AddProductAsync(ProductAddDTO productAddDTO)
        {
           return await _productDAL.AddProductAsync(productAddDTO);
        }

        public IDataResult<Product> GetProduct(Expression<Func<Product, bool>>? expression=null)
        {
            return _productDAL.GetProduct(expression);
        }

        public IDataResult<GetProductUIDTO> GetProductDetailUI(string id, string langCode)
        {
            return  _productDAL.GetProductDetailUI(id, langCode);
        }

        public IDataResult<List<GetProductUIDTO>> GetProductFilterUI(string currentCulture, string? category = null, string? color = null, string? size = null, string? minPrice = null, string? maxPrice = null)
        {
            return _productDAL.GetProductFilterUI(currentCulture, category, color, size, minPrice, maxPrice);
        }

        public IDataResult<List<GetProductUIDTO>> GetProductListUI(Expression<Func<Product, bool>>? expression=null ,string LangCode=null)
        {return _productDAL.GetProductListUI(expression,LangCode);
        }

        public IDataResult<List<string>> GetProductsColor()
        {
           return _productDAL.GetAllProductsColor();
        }

        public IDataResult<Dictionary<string, decimal>> GetProductsMaxAndMinPrice()
        {
            return _productDAL.GetAllProductMaxAndMinPrice();
        }

        public IDataResult<List<GetProductUIDTO>> GetRelatedProduct(List<string> Categories, string CurrentCulture)
        {
           return _productDAL.GetRelatedProduct(Categories, CurrentCulture);
        }

        public IDataResult<List<ProductGetAdminListDTO>> ProductGetAdminList()
        {
            return _productDAL.GetProductAdminList();
        }

        public IResult RemoveProduct(ProductRemoveDTO productRemoveDTO)
        {
            return _productDAL.RemoveProduct(productRemoveDTO);
        }

        public IResult UpdateProduct(ProductUpdateDTO productUpdateDTO)
        {
           return _productDAL.UpdateProduct(productUpdateDTO);
        }
    }
}
