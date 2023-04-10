using AutoMapper;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Mappings
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<WalletDto, DbModels.Wallet>()
                .ReverseMap();
            CreateMap<AddWalletRequest, DbModels.Wallet>()
                .ReverseMap();
            CreateMap<Transaction, TransactionDto>()
                .ReverseMap();
        }
    }
}