using Microsoft.Extensions.DependencyInjection;
using SBSC.Wallet.BusinessCore.DbModels;
using SBSC.Wallet.BusinessCore.Services;
using SBSC.Wallet.BusinessCore.Services.Interfaces;

namespace SBSC.Wallet.BusinessCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services)
        {
            services.AddDbContext<WalletContext>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IWalletService, WalletService>();

            return services;
        }
    }
}
