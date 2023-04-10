using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Services.Interfaces
{
    public interface IWalletService
    {
        Task<(bool status, string message)> CreateWallet(AddWalletRequest request);

        Task<(bool status, string message)> DeleteWallet(long id);

        Task<(bool status, string message)> EnableWallet(long id);

        Task<(bool status, string message)> DisableWallet(long id);

        Task<(bool status, string message)> RestrictWalletCredit(long id);

        Task<(bool status, string message)> RestrictWalletDebit(long id);

        Task<IEnumerable<WalletDto>> GetWalletByUser(long userId);

        Task<WalletDto> GetWalletById(long id);

        PagedList<WalletDto> GetWallets(PagedRequest request);
    }
}