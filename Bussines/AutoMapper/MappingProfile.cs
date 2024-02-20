using AutoMapper;
using Entities;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Entities.DTOs.ProductDTOs;
using Entities.DTOs.UserDTOs;
namespace Bussines.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDTO, User>();
            CreateMap<CategoryUpdateDTO,Category>();
            CreateMap<ProductUpdateDTO, Product>().ReverseMap();
            CreateMap<Product, GetProductUIDTO>();
            CreateMap<Product, GetProductUIDTO>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.productLanguages.Select(x => x.ProductName).FirstOrDefault()))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.productLanguages.Select(x => x.Description).FirstOrDefault()))
            .ForMember(dest => dest.PicturesUrls, opt => opt.MapFrom(src => src.PicturesUrls))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.DisCount, opt => opt.MapFrom(src => src.DisCount))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            //.ForMember(dest => dest.Product_Size.Keys, opt => opt.MapFrom(src => src.ProductSizes.Select(x=>x.Size)))
            //.ForMember(dest => dest.Product_Size.Values, opt => opt.MapFrom(src => src.ProductSizes.Select(x=>x.SizeStockCount)))
            .ForMember(dest=>dest.Product_Category,opt=>opt.MapFrom(src=>src.ProductCategories.Select(x=>x.Category).Select(x=>x.CategoryLanguages.Select(x=>x.CategoryName)).ToList()))
            
            //.ForMember(dest => dest.Product_Category, opt => opt.MapFrom(src => MapProductCategories(src.ProductCategories)))

            ;






        }
    }
}
