using AutoMapper;
using SBSC.Wallet.BusinessCore.DbModels;
using SBSC.Wallet.CoreObject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSC.Wallet.BusinessCore.Mappings
{
    public class AdminProfile:Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin,UserDto>()
                .ForMember(src=>src.FirstName, dest=>dest.MapFrom(a=>a.Name.Split(' ', System.StringSplitOptions.None).FirstOrDefault()))
                .ForMember(src=>src.FullName, dest=>dest.MapFrom(a=>a.Name))
                .ReverseMap();
        }
    }
}
