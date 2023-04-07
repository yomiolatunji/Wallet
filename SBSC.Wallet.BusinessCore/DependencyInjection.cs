using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SBSC.Wallet.BusinessCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services)
        {
            return services;
        }
    }
}
