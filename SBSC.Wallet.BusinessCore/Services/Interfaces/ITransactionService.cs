using SBSC.Wallet.CoreObject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSC.Wallet.BusinessCore.Services.Interfaces
{
    public interface ITransactionService
    {
        PagedList<WalletDto> GetTransactions(PagedRequest request);
    }
}
