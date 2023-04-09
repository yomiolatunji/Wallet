using AutoMapper;
using SBSC.Wallet.BusinessCore.DbModels;
using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.BusinessCore.Mappings
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