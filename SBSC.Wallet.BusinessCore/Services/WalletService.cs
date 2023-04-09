using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SBSC.Wallet.BusinessCore.DbModels;
using SBSC.Wallet.BusinessCore.Services.Interfaces;
using SBSC.Wallet.CoreObject.Enumerables;
using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.BusinessCore.Services
{
    public class WalletService : BaseService, IWalletService
    {
        private readonly WalletContext _context;
        private readonly IMapper _mapper;

        public WalletService(WalletContext context, IMapper mapper, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool status, string message)> CreateWallet(AddWalletRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var walletRequest = _mapper.Map<AddWalletRequest, DbModels.Wallet>(request);
            walletRequest.DateCreated = DateTime.Now;
            walletRequest.CreatedBy = 0;
            walletRequest.IsDeleted = false;
            walletRequest.WalletNumber = GetNextWalletNumber();

            await _context.Wallets.AddAsync(walletRequest);
            var inserted = (await _context.SaveChangesAsync()) > 0;
            if (inserted)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public async Task<(bool status, string message)> DeleteWallet(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.Id == id);
            if (wallet == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            wallet.IsDeleted = true;
            wallet.DateDeleted = DateTime.Now;
            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public PagedList<WalletDto> GetWallets(PagedRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var collection = _context.Wallets.Include(a => a.User) as IQueryable<DbModels.Wallet>;
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = request.SearchQuery.Trim();
                collection = collection.Where(a => a.WalletNumber.Contains(searchQuery)
                    || a.User.FirstName.Contains(searchQuery)
                    || a.User.LastName.Contains(searchQuery));
            }

            var users = PagedList<DbModels.Wallet>.ToPagedList(collection,
            request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.SortDirection);

            return _mapper.Map<PagedList<WalletDto>>(users);
        }

        public async Task<WalletDto> GetWalletById(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var wallets = await _context.Wallets.FirstOrDefaultAsync(a => a.Id == id);

            return _mapper.Map<WalletDto>(wallets);
        }

        public async Task<IEnumerable<WalletDto>> GetWalletByUser(long userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var wallets = await _context.Wallets.Where(a => a.UserId == userId).ToListAsync();

            return _mapper.Map<IEnumerable<WalletDto>>(wallets);
        }

        public string GetNextWalletNumber()
        {
            string seqQuence;
            var connection = _context.Database.GetDbConnection();
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "Select NEXT VALUE FOR dbo.[SeqWalletNumber]";
                var obj = cmd.ExecuteScalar();
                seqQuence = obj.ToString()?.PadLeft(10, '0');
            }
            connection.Close();
            return seqQuence;
        }

        public async Task<(bool status, string message)> DisableWallet(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.Id == id);
            if (wallet == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            wallet.IsActive = false;
            wallet.DateUpdated = DateTime.Now;
            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public async Task<(bool status, string message)> RestrictWalletCredit(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.Id == id);
            if (wallet == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            wallet.CreditRestricted = true;
            wallet.DateUpdated = DateTime.Now;
            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public async Task<(bool status, string message)> RestrictWalletDebit(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.Id == id);
            if (wallet == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            wallet.DebitRestricted = true;
            wallet.DateUpdated = DateTime.Now;
            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public async Task<(bool status, string message)> EnableWallet(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(a => a.Id == id);
            if (wallet == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            wallet.IsActive = true;
            wallet.DateUpdated = DateTime.Now;
            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
            throw new NotImplementedException();
        }
    }
}