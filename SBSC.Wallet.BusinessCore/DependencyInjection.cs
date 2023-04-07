using Microsoft.Extensions.DependencyInjection;
using SBSC.Wallet.BusinessCore.DbModels;

namespace SBSC.Wallet.BusinessCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services)
        {
            services.AddDbContext<WalletContext>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
