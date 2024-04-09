using AutoMapper;
using Core.DataAccess.EntityFramework;
using Core.Helper;
using Core.Helper.FileHelper;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System.Drawing;
using System.Linq.Expressions;

namespace DataAccess.Concrete
{
    public class EFProductDAL : EFRepositoryBase<Product, AppDbContext>, IProductDAL
    {
        private readonly IMapper _map;

        public EFProductDAL(IMapper map)
        {
            _map = map;
        }

        public async Task<IResult> AddProductAsync(ProductAddDTO ProductAddDTO, List<IFormFile> Photos)
        {
            try
            {
                var context = new AppDbContext();


                Product product = new Product()
                {
                    ProductCode = ProductAddDTO.ProductCode,
                    Price = ProductAddDTO.Price,
                    DisCount = 0,
                    CreatedDate = DateTime.Now,
                    Color = ProductAddDTO.Color,
                   
                    UserId = ProductAddDTO.UserId,
                    isFeatured = ProductAddDTO.isFeatured




                };
                   List<string> url = await FileHelper.SaveFileRangeAsync(Photos, wwwrootGetPath.GetwwwrootPath);


                product.Pictures = url;
                await context.Products.AddAsync(product);


                await context.SaveChangesAsync();
                for (int i = 0; i < ProductAddDTO.ProductName.Count; i++)
                {

                    ProductLanguages productLanguages = new ProductLanguages()
                    {
                        Description = ProductAddDTO.ProductDescrption[i],
                        LangCode = ProductAddDTO.LangCode[i],
                        ProductName = ProductAddDTO.ProductName[i],
                        SeoUrl = ProductAddDTO.ProductName[i].ReplaceInvalidChars(),
                        ProductId = product.Id,
                        Product = product,

                    };
                    await context.ProductLanguages.AddAsync(productLanguages);
                    await context.SaveChangesAsync();
                }
                for (int i = 0; i < ProductAddDTO.CategoryId.Count; i++)
                {
                    var category = context.Categories.FirstOrDefault(x => x.Id == ProductAddDTO.CategoryId[i]);
                    CategoryProduct categoryProduct = new()
                    {
                        CategoryId = ProductAddDTO.CategoryId[i],


                        //           Category = category,
                        ProductId = product.Id,
                        //            Product = product,




                    };
                    await context.CategoryProducts.AddAsync(categoryProduct);
                    await context.SaveChangesAsync();

                };
                for (int i = 0; i < ProductAddDTO.Sizes.Count; i++)
                {
                    Entities.Concrete.Size productSize = context.Sizes.FirstOrDefault(x => x.NumberSize.ToString() == ProductAddDTO.Sizes[i]);

                    ProductSize newproductSize = new ProductSize()
                    {
                        ProductId = product.Id,
                        Product = product,
                        SizeId = productSize.Id,
                        Size = productSize,
                        SizeStockCount = ProductAddDTO.SizesCount[i]

                    };
                    await context.ProductSizes.AddAsync(newproductSize);
                    await context.SaveChangesAsync();


                }


                return new SuccessResult("Product Yaradildi");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }

        }

        public IDataResult<Product> GetProduct(Expression<Func<Product, bool>> expression)
        {
            try
            {
                var contex = new AppDbContext();

                Product product =

                    contex.Products
                      .Include(a => a.productLanguages)
                       .Include(a => a.ProductSizes)
                       .ThenInclude(a => a.Size)
                      
                       .Include(a => a.User)
                       .Include(a => a.ProductCategories)
                       .ThenInclude(a => a.Category)
                       .ThenInclude(a => a.CategoryLanguages)

                       .FirstOrDefault(expression);
                return new SuccessDataResult<Product>(product);

            }
            catch (Exception ex)
            {

                return new ErrorDataResult<Product>(ex.Message);
            }
        }

        public IDataResult<List<ProductGetAdminListDTO>> GetProductAdminList()
        {
            try
            {
                var context = new AppDbContext();
                var result = context.Products
                    .Include(a => a.productLanguages)
                    .Include(a => a.ProductSizes)
                    .ThenInclude(a => a.Size)
                    .Include(a => a.User)
                  
                    .Include(a => a.ProductCategories)
                    .ThenInclude(a => a.Category)
                    .ThenInclude(a => a.CategoryLanguages).ToList();


                var resultMap = result.Select(x => new ProductGetAdminListDTO
                {
                    DisCount = x.DisCount,
                    ProductPrice = x.Price,
                    ProductCode = x.ProductCode,
                    ImgUrls = x.Pictures,
                    ProductId = x.Id.ToString(),
                    categoryName = x.ProductCategories
         .Where(pc => pc.Category.CategoryLanguages.Any(cl => cl.LangCode == "az"))
         .Select(pc => pc.Category.CategoryLanguages.First(cl => cl.LangCode == "az").CategoryName)
         .ToList(),
                    ProductSizeAndCount = x.ProductSizes
         .ToDictionary(ps => ps.Size.NumberSize, ps => ps.SizeStockCount)
                }).ToList();

                return new SuccessDataResult<List<ProductGetAdminListDTO>>(resultMap);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<ProductGetAdminListDTO>>(ex.Message);
            }



        }

        public IDataResult<GetProductUIDTO> GetProductDetailUI(string id, string langCode)
        {
            try
            {
                var context = new AppDbContext();
                var ProductAdmin = context.ProductLanguages
                    .Where(x => x.ProductId.ToString() == id && x.LangCode == langCode)
                    .Include(x => x.Product)
                    .ThenInclude(x => x.ProductCategories)
                    .ThenInclude(x => x.Category)
                    .ThenInclude(x => x.CategoryLanguages)
                 
                    .Include(x => x.Product.ProductSizes)
                    .ThenInclude(x => x.Size)
                    .ToList();


                //Dictionary<Guid, List<int>> ProductSizesAndCount = new Dictionary<Guid, List<int>>();
                //for (int i = 0; i < ProductAdmin.Count; i++)
                //{
                //    if (!ProductSizesAndCount.ContainsKey(ProductAdmin[i].ProductId))
                //    {
                //        ProductSizesAndCount.Add(
                //            key: ProductAdmin[i].ProductId,

                //            value: new List<int>
                //            {


                //            }
                //            );
                //        for (int j = 0; j < ProductAdmin[i].Product.ProductSizes.Count; j++)
                //        {
                //            ProductSizesAndCount[ProductAdmin[i].ProductId].Add(ProductAdmin[i].Product.ProductSizes[j].Size.NumberSize);
                //            ProductSizesAndCount[ProductAdmin[i].ProductId].Add(ProductAdmin[i].Product.ProductSizes[j].SizeStockCount);
                //        }


                //    }



                //}


                //Dictionary<Guid, List<string>> ProductCategory = new Dictionary<Guid, List<string>>();
                //for (int i = 0; i < ProductAdmin.Count; i++)
                //{
                //    if (!ProductCategory.ContainsKey(ProductAdmin[i].ProductId))
                //    {
                //        ProductCategory.Add(
                //            key: ProductAdmin[i].ProductId,

                //            value: new List<string>
                //            {
                //            }
                //            );
                //        for (int j = 0; j < ProductAdmin[i].Product.ProductCategories.Count; j++)
                //        {

                //            ProductCategory[ProductAdmin[i].ProductId].Add(ProductAdmin[i].Product.ProductCategories[j].Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == langCode).CategoryName);

                //        }

                //    }



                //}



                //var product = ProductAdmin.Select(x => new GetProductUIDTO
                //{
                //    ProductID = x.ProductId,
                //    Color = x.Product.Color,
                //    DisCount = x.Product.DisCount,
                //    Price = x.Product.Price,
                //    ProductDescription = x.Description,
                //    PicturesUrls = x.Product.PicturesUrls,
                //    Product_Category = x.Product.ProductCategories.Where(x=>x.Category.CategoryLanguages.Where(x=>x.LangCode==langCode).Select(x=>x.CategoryName).ToList),
                //    ProductName = x.ProductName,
                //    Product_Size = ProductSizesAndCount,


                //}).ToList();
                return new SuccessDataResult<GetProductUIDTO>(
                    new GetProductUIDTO()
                    {
                        ProductID = ProductAdmin[0].ProductId,
                        Color = ProductAdmin[0].Product.Color,
                        DisCount = ProductAdmin[0].Product.DisCount,
                        Price = ProductAdmin[0].Product.Price,
                        PicturesUrls = ProductAdmin[0].Product.Pictures,
                        ProductCode = ProductAdmin[0].Product.productLanguages.FirstOrDefault().Product.ProductCode,
                        ProductDescription = ProductAdmin[0].Product.productLanguages.FirstOrDefault().Description,
                        ProductName = ProductAdmin[0].Product.productLanguages.FirstOrDefault().ProductName,
                        Product_Category = ProductAdmin[0].Product.ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == langCode).CategoryName).ToList(),
                        Product_Size = ProductAdmin[0].Product.ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                    });
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<GetProductUIDTO>(ex.Message);
            }

        }

        public IDataResult<List<GetProductUIDTO>> GetProductListUI(Expression<Func<Product, bool>>? expression, string LangCode)
        {
            try
            {
                var context = new AppDbContext();
                var ProductAdmin = expression is not null ? context.Products
                    .Include(a => a.productLanguages.Where(x => x.LangCode == LangCode))
                   .Include(a => a.ProductSizes)
                    .ThenInclude(a => a.Size)

                    .Include(a => a.User)
                    .Include(a => a.ProductCategories)
                    .ThenInclude(a => a.Category)
                    .ThenInclude(a => a.CategoryLanguages)

                    .Where(expression)
                    .ToList() :
                    context.Products
                    .Include(a => a.productLanguages.Where(x => x.LangCode == LangCode))
                   .Include(a => a.ProductSizes)
                    .ThenInclude(a => a.Size)
                
                    .Include(a => a.User)
                    .Include(a => a.ProductCategories)
                    .ThenInclude(a => a.Category)
                    .ThenInclude(a => a.CategoryLanguages)


                    .ToList()
                    ;

                List<GetProductUIDTO> products = new List<GetProductUIDTO>();
                for (int i = 0; i < ProductAdmin.Count; i++)
                {
                    products.Add(
                   new GetProductUIDTO
                   {
                       ProductID = ProductAdmin[i].Id,
                       Color = ProductAdmin[i].Color,
                       DisCount = ProductAdmin[i].DisCount,
                       Price = ProductAdmin[i].Price,
                       PicturesUrls = ProductAdmin[i].Pictures,
                       ProductCode = ProductAdmin[i].productLanguages.FirstOrDefault(x => x.LangCode == LangCode).Product.ProductCode,
                       ProductName = ProductAdmin[i].productLanguages.FirstOrDefault(x => x.LangCode == LangCode).ProductName,
                       Product_Category = ProductAdmin[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == LangCode)?.CategoryName).ToList(),
                       Product_Size = ProductAdmin[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                   }
                        );
                }


                return new SuccessDataResult<List<GetProductUIDTO>>(products);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<GetProductUIDTO>>(ex.Message);
            }
        }
        public IDataResult<List<GetProductUIDTO>> GetProductFilterUI(string currentCulture, string? category = null, string? color = null, string? size = null, string? minPrice = null, string? maxPrice = null)
        {
            try
            {
                var context = new AppDbContext();
                var products = context.Products
   .Include(a => a.productLanguages.Where(x => x.LangCode == currentCulture))
                   .Include(a => a.ProductSizes)
                    .ThenInclude(a => a.Size)
                    
                    .Include(a => a.User)
                    .Include(a => a.ProductCategories)
                    .ThenInclude(a => a.Category)
                    .ThenInclude(a => a.CategoryLanguages)


                    .ToList();

                List<GetProductUIDTO> result = new List<GetProductUIDTO>();
                //butun serti odeyen mehsulu elave edir asagdaki alqoritm
                object[] myParametrs = { category, color, size, minPrice, maxPrice };

                for (int i = 0; i < products.Count; i++)
                {
                    int isNotNullParametrCounter = 0;//nece dene parametrin null olmadigini bilmek ucun
                    int a = 0;//i-ci mehsulun necedene serti odediyini bilmek ucun
                              //sonda a==isNotNullParametrCounter ?sertini odeyen mehsulu elave edeceyik
                    for (int j = 0; j < myParametrs.Length; j++)
                    {
                        if (myParametrs[j] is not null)
                        {
                            isNotNullParametrCounter++;
                            if (j == 0 &&
                                products[i].ProductCategories
                                .Where(x => x.Category.CategoryLanguages.
                                Any(x => x.LangCode == currentCulture && x.CategoryName == category))
                                .ToList().Count != 0
                                 )
                            {
                                a++;
                            }
                            if (j == 1 && products[i].Color == color)
                            {
                                a++;
                            }
                            if (j == 2

                                )
                            {
                                if (products[i].ProductSizes.Any(x => x.Size.NumberSize == int.Parse(size)
                                 && x.SizeStockCount > 0))
                                {

                                    a++;
                                }

                            }
                            if (j == 3)

                            {
                                if ((products[i].DisCount == 0 ?
                                  products[i].Price >= decimal.Parse(minPrice) :
                                  products[i].DisCount >= decimal.Parse(minPrice))
                                )
                                {

                                    a++;
                                }

                            }


                            if (j == 4)
                            {
                                if (products[i].DisCount == 0 ?
                                  products[i].Price <= decimal.Parse(maxPrice) :
                                  products[i].DisCount <= decimal.Parse(maxPrice))
                                {

                                    a++;
                                }
                            }


                        }


                    }
                    if (a == isNotNullParametrCounter)
                    {

                        result.Add(
                               new GetProductUIDTO
                               {
                                   ProductID = products[i].Id,
                                   Color = products[i].Color,
                                   DisCount = products[i].DisCount,
                                   Price = products[i].Price,
                                   PicturesUrls = products[i].Pictures,
                                   ProductCode = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).Product.ProductCode,
                                   ProductName = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).ProductName,
                                   Product_Category = products[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == currentCulture)?.CategoryName).ToList(),
                                   ProductDescription = products[i].productLanguages.FirstOrDefault(x=>x.LangCode== currentCulture).Description,
                                   Product_Size = products[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                               }
                                    );
                    }

                }

                //1 serti odeyen mehsulu elave edir asagdaki alqoritm
                /*
                                for (int i = 0; i < products.Count; i++)
                                {
                                    ;
                                    if (category is not null &&
                                        products[i].ProductCategories.Where(x => x.Category.CategoryLanguages.Any(x => x.LangCode == currentCulture && x.CategoryName == category)).ToList().Count != 0
                                        &&
                                        !result.Any(x => x.ProductID == products[i].Id)
                                        )
                                    {

                                        result.Add(
                                            new GetProductUIDTO
                                            {
                                                ProductID = products[i].Id,
                                                Color = products[i].Color,
                                                DisCount = products[i].DisCount,
                                                Price = products[i].Price,
                                                PicturesUrls = products[i].PicturesUrls,
                                                ProductDescription = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).Description,
                                                ProductName = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).ProductName,
                                                Product_Category = products[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == currentCulture)?.CategoryName).ToList(),
                                                Product_Size = products[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                                            }
                                                 );
                                    }
                                    if (color is not null &&
                                        products[i].Color == color
                                        &&
                                       !result.Any(x => x.ProductID == products[i].Id))
                                    {
                                        result.Add(
                                    new GetProductUIDTO
                                    {
                                        ProductID = products[i].Id,
                                        Color = products[i].Color,
                                        DisCount = products[i].DisCount,
                                        Price = products[i].Price,
                                        PicturesUrls = products[i].PicturesUrls,
                                        ProductDescription = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).Description,
                                        ProductName = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).ProductName,
                                        Product_Category = products[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == currentCulture)?.CategoryName).ToList(),
                                        Product_Size = products[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                                    }
                                         );
                                    }
                                    if (size is not null &&
                                  products[i].ProductSizes.Any(x => x.Size.NumberSize == int.Parse(size) && x.SizeStockCount > 0)
                                  &&
                                 !result.Any(x => x.ProductID == products[i].Id))
                                    {
                                        result.Add(
                                    new GetProductUIDTO
                                    {
                                        ProductID = products[i].Id,
                                        Color = products[i].Color,
                                        DisCount = products[i].DisCount,
                                        Price = products[i].Price,
                                        PicturesUrls = products[i].PicturesUrls,
                                        ProductDescription = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).Description,
                                        ProductName = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).ProductName,
                                        Product_Category = products[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == currentCulture)?.CategoryName).ToList(),
                                        Product_Size = products[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                                    }
                                         );
                                    }
                                    if (minPrice is not null && maxPrice is null &&
                                  (products[i].DisCount == 0 ? products[i].Price >= decimal.Parse(minPrice) : products[i].DisCount >= decimal.Parse(minPrice))
                                  &&
                                 !result.Any(x => x.ProductID == products[i].Id))
                                    {
                                        result.Add(
                                    new GetProductUIDTO
                                    {
                                        ProductID = products[i].Id,
                                        Color = products[i].Color,
                                        DisCount = products[i].DisCount,
                                        Price = products[i].Price,
                                        PicturesUrls = products[i].PicturesUrls,
                                        ProductDescription = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).Description,
                                        ProductName = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).ProductName,
                                        Product_Category = products[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == currentCulture)?.CategoryName).ToList(),
                                        Product_Size = products[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                                    }
                                         );
                                    }

                                    if (minPrice is not null && maxPrice is not null &&
                                    (products[i].DisCount == 0 ? products[i].Price >= decimal.Parse(minPrice) && products[i].Price <= decimal.Parse(maxPrice) : products[i].DisCount >= decimal.Parse(minPrice) && products[i].DisCount <= decimal.Parse(maxPrice))
                                    &&
                                   !result.Any(x => x.ProductID == products[i].Id))
                                    {
                                        result.Add(
                                    new GetProductUIDTO
                                    {
                                        ProductID = products[i].Id,
                                        Color = products[i].Color,
                                        DisCount = products[i].DisCount,
                                        Price = products[i].Price,
                                        PicturesUrls = products[i].PicturesUrls,
                                        ProductDescription = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).Description,
                                        ProductName = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).ProductName,
                                        Product_Category = products[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == currentCulture)?.CategoryName).ToList(),
                                        Product_Size = products[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                                    }
                                         );
                                    }
                                    if (minPrice is null && maxPrice is not null &&
                                       (products[i].DisCount == 0 ? products[i].Price < decimal.Parse(maxPrice) : products[i].Price < decimal.Parse(maxPrice))
                                       &&
                                      !result.Any(x => x.ProductID == products[i].Id))
                                    {
                                        result.Add(
                                    new GetProductUIDTO
                                    {
                                        ProductID = products[i].Id,
                                        Color = products[i].Color,
                                        DisCount = products[i].DisCount,
                                        Price = products[i].Price,
                                        PicturesUrls = products[i].PicturesUrls,
                                        ProductDescription = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).Description,
                                        ProductName = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).ProductName,
                                        Product_Category = products[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == currentCulture)?.CategoryName).ToList(),
                                        Product_Size = products[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                                    }
                                         );
                                    }

                                    if (category is null && color is null && size is null && minPrice is null && maxPrice is null)
                                    {
                                        result.Add(
                                    new GetProductUIDTO
                                    {
                                        ProductID = products[i].Id,
                                        Color = products[i].Color,
                                        DisCount = products[i].DisCount,
                                        Price = products[i].Price,
                                        PicturesUrls = products[i].PicturesUrls,
                                        ProductDescription = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).Description,
                                        ProductName = products[i].productLanguages.FirstOrDefault(x => x.LangCode == currentCulture).ProductName,
                                        Product_Category = products[i].ProductCategories.Select(x => x.Category.CategoryLanguages.FirstOrDefault(x => x.LangCode == currentCulture)?.CategoryName).ToList(),
                                        Product_Size = products[i].ProductSizes.ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                                    }
                                         );
                                    }
                                }*/





                return new SuccessDataResult<List<GetProductUIDTO>>(result);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public IResult RemoveProduct(ProductRemoveDTO productRemoveDTO)
        {
            try
            {
                var context = new AppDbContext();
                var product = context.Products
                    .Include(x => x.productLanguages)
                    .Include(x => x.ProductCategories)
                    .ThenInclude(x => x.Category)
                 
                    .Include(x => x.ProductSizes)
                    .ThenInclude(x => x.Size)
                    .FirstOrDefault(x => x.Id == productRemoveDTO.ProductId);
                ;
                var PhotoRemove = product.Pictures;
                bool PhotoRemoveResult = FileHelper.RemoveFileRange(PhotoRemove);
                context.Products.Remove(product);
                context.SaveChanges();


                return new SuccessResult();
                    }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }

        }

        public IResult UpdateProduct(ProductUpdateDTO productUpdateDTO)
        {
            try
            {
                var context = new AppDbContext();
                var Product = context.Products.Include(a => a.productLanguages)
                    .Include(a => a.ProductSizes)
                    .ThenInclude(a => a.Size)
                    .Include(a => a.User)

                    .Include(a => a.ProductCategories)
                    .ThenInclude(a => a.Category)
                    .ThenInclude(a => a.CategoryLanguages)
                    .FirstOrDefault(x => x.Id == productUpdateDTO.Id);

                for (var i = 0; i < productUpdateDTO.ProductName.Count; i++)
                {
                    Product.productLanguages[i].ProductName = productUpdateDTO.ProductName[i];
                    Product.productLanguages[i].Description = productUpdateDTO.ProductDescrption[i];
                }
                //List<CategoryProduct> NewcategoryProducts = new List<CategoryProduct>();
                for (var i = 0; i < productUpdateDTO.CategoryId.Count; i++)
                {
                    if (!Product.ProductCategories.Select(x => x.Category).Select(x => x.Id).ToList().Contains(productUpdateDTO.CategoryId[i]))
                    {

                        CategoryProduct categoryProduct = new CategoryProduct()
                        {
                            CategoryId = productUpdateDTO.CategoryId[i],
                            ProductId = productUpdateDTO.Id
                        };

                        context.CategoryProducts.AddRange(categoryProduct);
                    }

                }
                context.SaveChanges();

                //List<ProductSize> NewSize = new List<ProductSize>();
                for (var i = 0; i < productUpdateDTO.SizesCount.Count; i++)
                {


                    var Size = context.Sizes.FirstOrDefault(x => x.NumberSize == productUpdateDTO.ProductSizes[i]);
                    if (!Product.ProductSizes.Select(x => x.SizeId).ToList().Contains(Size.Id))
                    {

                        ProductSize productSize = new ProductSize()
                        {
                            ProductId = productUpdateDTO.Id,
                            SizeId = Size.Id,
                            SizeStockCount = productUpdateDTO.SizesCount[i]

                        };
                        context.ProductSizes.Add(productSize);
                    }
                    else
                    {
                        var ProductSize = Size.ProductSize.FirstOrDefault(x => x.SizeId == Size.Id && x.ProductId == Product.Id);
                        ProductSize.SizeStockCount = productUpdateDTO.SizesCount[i];
                        context.ProductSizes.Update(ProductSize);
                    }



                }

                context.SaveChanges();
                List<string> RemovePhotos= new List<string>();
                foreach (string url in Product.Pictures)
                {
                    if (!productUpdateDTO.PicturesUrls.Contains(url))
                    {
                  RemovePhotos.Add(url);
                   
                    }
          
                }
                FileHelper.RemoveFileRange(RemovePhotos);
               
                Product.Price = productUpdateDTO.Price;
                Product.DisCount = productUpdateDTO.DisCount;
                Product.ProductCode = productUpdateDTO.ProductCode;
                Product.Pictures = productUpdateDTO.PicturesUrls;
                Product.Color = productUpdateDTO.Color;
                Product.UpdatedDate = DateTime.Now;
                Product.UpdatedUserId = productUpdateDTO.UpdatedUserId;
                //Product.ProductCategories = NewcategoryProducts;
                //Product.ProductSizes = NewSize;

                context.Products.Update(Product);
                context.SaveChanges();
                return new SuccessResult("Yenilendi");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IDataResult<List<string>> GetAllProductsColor()
        {
            try
            {
                var context = new AppDbContext();
                var color = context.Products.Select(x => x.Color).ToList();
                return new SuccessDataResult<List<string>>(color);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<List<string>>(ex.Message);
            }
            throw new NotImplementedException();
        }

        public IDataResult<Dictionary<string, decimal>> GetAllProductMaxAndMinPrice()
        {
            try
            {
                var context = new AppDbContext();
                var Prices = context.Products.Select(x => (x.DisCount == 0 ? x.Price : x.DisCount)).ToList();
                Dictionary<string, decimal> MaxAndMinPrice = new Dictionary<string, decimal>();
                MaxAndMinPrice.Add("maxPrice", Math.Ceiling(Prices.Max()));

                return new SuccessDataResult<Dictionary<string, decimal>>(MaxAndMinPrice);
            }
            catch (Exception ex)
            {

                return new ErrorDataResult<Dictionary<string, decimal>>(ex.Message);
            }


        }

        public IDataResult<List<GetProductUIDTO>> GetRelatedProduct(List<string> categories, string CurrentCulture)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var products = context.ProductLanguages
                        .Where(x => x.LangCode == CurrentCulture)
                        .Include(x => x.Product)
                        .ThenInclude(x => x.ProductCategories)
                        .ThenInclude(x => x.Category)
                        .ThenInclude(x => x.CategoryLanguages)
                        .Include(x => x.Product.ProductSizes)
                        .ThenInclude(x => x.Size)
                        .ToList();

                    if (products.Count == 0)
                    {
                        return new ErrorDataResult<List<GetProductUIDTO>>("Data Is EmptyCurrentCulture");
                    }

                    var FilteredeProducts = products
                        .Where(item => item.Product.ProductCategories
                            .Any(pc => categories.Contains(pc.Category.CategoryLanguages
                                .FirstOrDefault(lang => lang.LangCode == CurrentCulture)?.CategoryName)))
                        .Select(item => new GetProductUIDTO
                        {
                            ProductID = item.ProductId,
                            Color = item.Product.Color,
                            DisCount = item.Product.DisCount,
                            Price = item.Product.Price,
                            PicturesUrls = item.Product.Pictures,
                            ProductCode = item.Product.productLanguages.FirstOrDefault()?.Product.ProductCode,
                            ProductDescription = item.Product.productLanguages.FirstOrDefault()?.Description,
                            ProductName = item.Product.productLanguages.FirstOrDefault()?.ProductName,
                            Product_Category = item.Product.ProductCategories
                                .Select(x => x.Category.CategoryLanguages
                                    .FirstOrDefault(lang => lang.LangCode == CurrentCulture)?.CategoryName)
                                .ToList(),
                            Product_Size = item.Product.ProductSizes
                                .ToDictionary(x => x.Size.NumberSize, x => x.SizeStockCount)
                        }).ToList();

                    return new SuccessDataResult<List<GetProductUIDTO>>(FilteredeProducts);
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<GetProductUIDTO>>(ex.Message);
            }
        }
    }
}
