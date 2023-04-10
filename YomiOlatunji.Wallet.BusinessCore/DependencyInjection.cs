using Microsoft.Extensions.DependencyInjection;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.BusinessCore.Services;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;

namespace YomiOlatunji.Wallet.BusinessCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services)
        {
            services.AddDbContext<WalletContext>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}