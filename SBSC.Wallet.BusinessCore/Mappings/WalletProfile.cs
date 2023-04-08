using AutoMapper;
using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.BusinessCore.Mappings
{
    public class WalletProfile:Profile
    {
        public WalletProfile()
        {
            CreateMap<WalletDto, DbModels.Wallet>()
                .ReverseMap();
            CreateMap<AddWalletRequest, DbModels.Wallet>()
                .ReverseMap();
            CreateMap<EditWalletRequest, DbModels.Wallet>()
                .ReverseMap();
        }
    }
}
