using Microsoft.Extensions.Configuration;
using SBSC.Wallet.BusinessCore.Services.Interfaces;

namespace SBSC.Wallet.BusinessCore.Services
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        public CurrencyService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}