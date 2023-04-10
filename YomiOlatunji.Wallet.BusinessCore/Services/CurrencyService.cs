using AutoMapper;
using Microsoft.Extensions.Configuration;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.BusinessCore.Integrations.Interfaces;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.Enumerables;
using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Services
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        private readonly WalletContext _context;
        private readonly ICloudinaryIntegration _cloudinaryIntegration;
        private readonly IMapper _mapper;
        public CurrencyService(WalletContext context, ICloudinaryIntegration cloudinaryIntegration, IMapper mapper, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _cloudinaryIntegration = cloudinaryIntegration;
            _mapper = mapper;
        }

        public async Task<(bool status, string message)> Add(AddCurrencyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var ccyRequest = _mapper.Map<AddCurrencyRequest, Currency>(request);
            ccyRequest.DateCreated = DateTime.Now;
            ccyRequest.CreatedBy = 0;
            ccyRequest.IsDeleted = false;
            ccyRequest.CurrencyLogoUrl = _cloudinaryIntegration.UploadImage(request.CurrencyLogoBase64);

            await _context.Currencies.AddAsync(ccyRequest);
            var inserted = (await _context.SaveChangesAsync()) > 0;
            if (inserted)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public IEnumerable<CurrencyDto> GetCurrencies()
        {
            var currencies = _context.Currencies.ToList();
            return _mapper.Map<IEnumerable<CurrencyDto>>(currencies);
        }
    }
}