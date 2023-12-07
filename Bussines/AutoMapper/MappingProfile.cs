using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
namespace Bussines.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDTO, User>();
        }
    }
}
