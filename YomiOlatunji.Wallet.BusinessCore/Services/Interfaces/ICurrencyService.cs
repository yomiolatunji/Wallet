using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<(bool status, string message)> Add(AddCurrencyRequest request);

        IEnumerable<CurrencyDto> GetCurrencies();
    }
}