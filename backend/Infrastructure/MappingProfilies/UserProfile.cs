using AutoMapper;
using BussinesLogic.EntityDtos;
using Domain.Entities;

namespace Infrastructure.MappingProfilies
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>()
                .ReverseMap();
        }
    }
}
