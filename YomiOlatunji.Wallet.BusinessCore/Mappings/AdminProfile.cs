using AutoMapper;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Mappings
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, UserDto>()
                .ForMember(src => src.FirstName, dest => dest.MapFrom(a => a.Name.Split(' ', System.StringSplitOptions.None).FirstOrDefault()))
                .ForMember(src => src.FullName, dest => dest.MapFrom(a => a.Name))
                .ReverseMap();

            CreateMap<AddAdminRequest, Admin>()
                .ReverseMap();
            
            CreateMap<Currency, CurrencyDto>()
                .ReverseMap();
            CreateMap<AddCurrencyRequest, Currency>()
                .ReverseMap();

        }
    }
}