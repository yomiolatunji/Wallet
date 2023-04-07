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
    public class UserProfile:Profile
    {
        public UserProfile() {
            CreateMap<AddUserRequest, User>()
                .ReverseMap();
            CreateMap<UserDto, User>() 
                .ReverseMap();
            CreateMap<EditUserRequest, User>() 
                .ReverseMap();
        }
    }
}
