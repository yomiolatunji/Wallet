using Microsoft.Extensions.Configuration;

namespace SBSC.Wallet.BusinessCore.Services
{
    public class BaseService
    {
        private readonly IConfiguration configuration;

        public BaseService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetAppSetting(string key)
        {
            try
            {
                return configuration[key];
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);
                throw;
            }
        }
    }
}