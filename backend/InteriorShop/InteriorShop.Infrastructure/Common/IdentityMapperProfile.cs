using AutoMapper;
using InteriorShop.Application.DTOs.Users;
using InteriorShop.Application.Requests.Auth;
using InteriorShop.Application.Requests.Users;
using InteriorShop.Infrastructure.Identity;

namespace InteriorShop.Infrastructure.Common
{
    public class IdentityMapperProfile : Profile
    {
        public IdentityMapperProfile()
        {
            CreateMap<ApplicationUser, UserDto>();

            CreateMap<RegisterRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserUpdateRequest, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}