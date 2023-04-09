using Microsoft.Extensions.DependencyInjection;
using SBSC.Wallet.BusinessCore.DbModels;
using SBSC.Wallet.CoreObject.Enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSC.Wallet.BusinessCore.Services
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
                    Email = "superadmin@sbscwallet.com",
                    Role = UserRoles.SuperAdmin,
                    CreatedBy = 0,
                    DateCreated = DateTime.Now,
                    Password = PasswordService.HashPassword("P@ssword1"),
                    IsActive = true,
                    IsDeleted = false
                };

                if (!context.Users.Any(u => u.Email == admin.Email))
                {
                    context.Admins.Add(admin);
                    context.SaveChanges();
                }
            }
        }
    }
}
