using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Services.Interfaces
{
    public interface ITransactionService
    {
        PagedList<TransactionDto> GetTransactions(TransactionRequest request);

        PagedList<TransactionDto> GetTransactionsByUser(UserTransactionRequest request);

        PagedList<TransactionDto> GetTransactionsByWallet(WalletTransactionRequest request);

        Task<(bool status, string message)> FundWallet(FundWalletRequest request);

        Task<(bool status, string message)> DebitWallet(FundWalletRequest request);
    }
}