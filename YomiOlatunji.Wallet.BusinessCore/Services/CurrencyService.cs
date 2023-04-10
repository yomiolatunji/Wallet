using Microsoft.Extensions.Configuration;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;

namespace YomiOlatunji.Wallet.BusinessCore.Services
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        public CurrencyService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}