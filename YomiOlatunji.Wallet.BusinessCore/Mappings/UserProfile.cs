using AutoMapper;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AddUserRequest, User>()
                .ReverseMap();
            CreateMap<User, UserDto>()
                .ForMember(src => src.FullName, dest => dest.MapFrom(a => $"{a.FirstName} {a.LastName}"))
                .ReverseMap();
            CreateMap<EditUserRequest, User>()
                .ReverseMap();
        }
    }
}