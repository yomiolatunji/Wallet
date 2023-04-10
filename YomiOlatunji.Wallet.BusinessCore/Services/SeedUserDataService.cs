using Microsoft.Extensions.DependencyInjection;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.CoreObject.Enumerables;

namespace YomiOlatunji.Wallet.BusinessCore.Services
{
    public class SeedUserDataService
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<WalletContext>();

                if (context == null)
                    return;

                var admin = new Admin()
                {
                    Name = "Super Admin",
                    Email = "superadmin@wallet.com",
                    Role = UserRoles.SuperAdmin,
                    CreatedBy = 0,
                    DateCreated = DateTime.Now,
                    Password = PasswordService.HashPassword("P@ssword1"),
                    IsActive = true,
                    IsDeleted = false
                };

                if (!context.Admins.Any(u => u.Email == admin.Email))
                {
                    context.Admins.Add(admin);
                    context.SaveChanges();
                }
            }
        }
    }
}