using SBSC.Wallet.CoreObject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSC.Wallet.BusinessCore.Services.Interfaces
{
    public interface IWalletService
    {
        public Task<(bool status, string message)> CreateWallet(AddWalletRequest request);
        public Task<(bool status, string message)> DeleteWallet(long id);
        public Task<IEnumerable<WalletDto>> GetWalletByUser(long userId);
        public Task<WalletDto> GetWalletById(long id);
        public PagedList<WalletDto> GetWallets(PagedRequest request);
    }
}
