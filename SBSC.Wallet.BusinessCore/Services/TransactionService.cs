using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SBSC.Wallet.BusinessCore.DbModels;
using SBSC.Wallet.BusinessCore.Services.Interfaces;
using SBSC.Wallet.CoreObject.Enumerables;
using SBSC.Wallet.CoreObject.Requests;
using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.BusinessCore.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly WalletContext _context;
        private readonly IMapper _mapper;

        public TransactionService(WalletContext context, IMapper mapper, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool status, string message)> FundWallet(FundWalletRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.Id == request.WalletId);
            if (wallet == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            if (!wallet.IsActive.GetValueOrDefault())
            {
                return (false, ResponseCodes.InactiveWallet.message);
            }
            if (wallet.CreditRestricted.GetValueOrDefault())
            {
                return (false, ResponseCodes.CreditRestrictionWallet.message);
            }
            if (wallet.Currency.Trim().ToLower() == request.Currency.Trim().ToLower())
            {
                return (false, ResponseCodes.InvalidCurrency.message);
            }
            var transaction = new Transaction
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Narration = request.Narration,
                TransactionDate = DateTime.Now,
                WalletId = request.WalletId,
                UserId = request.UserId,
                TransactionType = "C",
                TransactionCode = "FTR"
            };
            //TODO: create internal account and debit it to balance the accounting entries
            await _context.Transactions.AddAsync(transaction);
            var inserted = (await _context.SaveChangesAsync()) > 0;
            if (inserted)
            {
                wallet.AcyBalance += request.Amount;
                var updated = (await _context.SaveChangesAsync()) > 0;
                if (updated)
                {
                    return (true, ResponseCodes.Success.message);
                }
            }
            return (false, ResponseCodes.Failed.message);
        }

        public async Task<(bool status, string message)> DebitWallet(FundWalletRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.Id == request.WalletId);
            if (wallet == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            if (!wallet.IsActive.GetValueOrDefault())
            {
                return (false, ResponseCodes.InactiveWallet.message);
            }
            if (wallet.CreditRestricted.GetValueOrDefault())
            {
                return (false, ResponseCodes.CreditRestrictionWallet.message);
            }
            if (wallet.Currency.Trim().ToLower() == request.Currency.Trim().ToLower())
            {
                return (false, ResponseCodes.InvalidCurrency.message);
            }
            var transaction = new Transaction
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Narration = request.Narration,
                TransactionDate = DateTime.Now,
                WalletId = request.WalletId,
                UserId = request.UserId,
                TransactionType = "D",
                TransactionCode = "FTR"
            };
            //TODO: create internal account and credit it to balance the accounting entries
            await _context.Transactions.AddAsync(transaction);
            var inserted = (await _context.SaveChangesAsync()) > 0;
            if (inserted)
            {
                wallet.AcyBalance -= request.Amount;
                var updated = (await _context.SaveChangesAsync()) > 0;
                if (updated)
                {
                    return (true, ResponseCodes.Success.message);
                }
            }
            return (false, ResponseCodes.Failed.message);
        }

        public PagedList<TransactionDto> GetTransactions(TransactionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var collection = _context.Transactions.Include(a => a.Wallet.User) as IQueryable<Transaction>;
            if (request.StartDate.HasValue && request.EndDate.HasValue && (request.EndDate.GetValueOrDefault() >= request.StartDate.GetValueOrDefault()))
            {
                collection.Where(a => a.TransactionDate >= request.StartDate.GetValueOrDefault() && a.TransactionDate <= request.EndDate.GetValueOrDefault());
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = request.SearchQuery.Trim();
                collection = collection.Where(a => !string.IsNullOrWhiteSpace(a.Narration) && a.Narration.Contains(searchQuery));
            }

            var transactions = PagedList<Transaction>.ToPagedList(collection,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.SortDirection);

            return _mapper.Map<PagedList<TransactionDto>>(transactions);
        }

        public PagedList<TransactionDto> GetTransactionsByUser(UserTransactionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var collection = _context.Transactions.Include(a => a.Wallet.User).Where(a => a.Wallet.UserId == request.UserId);
            if (request.StartDate.HasValue && request.EndDate.HasValue && (request.EndDate.GetValueOrDefault() >= request.StartDate.GetValueOrDefault()))
            {
                collection.Where(a => a.TransactionDate >= request.StartDate.GetValueOrDefault() && a.TransactionDate <= request.EndDate.GetValueOrDefault());
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = request.SearchQuery.Trim();
                collection = collection.Where(a => !string.IsNullOrWhiteSpace(a.Narration) && a.Narration.Contains(searchQuery));
            }

            var transactions = PagedList<Transaction>.ToPagedList(collection,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.SortDirection);

            return _mapper.Map<PagedList<TransactionDto>>(transactions);
        }

        public PagedList<TransactionDto> GetTransactionsByWallet(WalletTransactionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var collection = _context.Transactions.Include(a => a.Wallet.User).Where(a => a.WalletId == request.WalletId);
            if (request.StartDate.HasValue && request.EndDate.HasValue && (request.EndDate.GetValueOrDefault() >= request.StartDate.GetValueOrDefault()))
            {
                collection.Where(a => a.TransactionDate >= request.StartDate.GetValueOrDefault() && a.TransactionDate <= request.EndDate.GetValueOrDefault());
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = request.SearchQuery.Trim();
                collection = collection.Where(a => !string.IsNullOrWhiteSpace(a.Narration) && a.Narration.Contains(searchQuery));
            }

            var transactions = PagedList<Transaction>.ToPagedList(collection,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.SortDirection);

            return _mapper.Map<PagedList<TransactionDto>>(transactions);
        }
    }
}