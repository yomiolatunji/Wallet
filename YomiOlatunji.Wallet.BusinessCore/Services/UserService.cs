using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.Enumerables;
using YomiOlatunji.Wallet.CoreObject.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace YomiOlatunji.Wallet.BusinessCore.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly WalletContext _context;
        private readonly IMapper _mapper;
        private readonly IWalletService _walletService;

        public UserService(WalletContext context, IMapper mapper, IWalletService walletService, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _mapper = mapper;
            _walletService = walletService;
        }

        public async Task<(bool status, string message)> Add(AddUserRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var userRequest = _mapper.Map<AddUserRequest, User>(request);
            userRequest.DateCreated = DateTime.Now;
            userRequest.CreatedBy = 0;
            userRequest.IsDeleted = false;
            userRequest.Password = PasswordService.HashPassword(request.Password);
            //TODO: Save profilepicture

            await _context.Users.AddAsync(userRequest);
            var inserted = (await _context.SaveChangesAsync()) > 0;
            if (inserted)
            {
                await CreateDefaultWallets(userRequest.Id);
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        private async Task CreateDefaultWallets(long userId)
        {
            var defaultCurrencies = GetAppSetting("DefaultWalletCurrency");
            foreach (var currency in defaultCurrencies.Split(","))
            {
                var walletRequest = new AddWalletRequest
                {
                    Currency = currency,
                    UserId = userId
                };
                await _walletService.CreateWallet(walletRequest);
            }
        }

        public async Task<(bool status, string message)> ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == request.Email);
            if (user == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            var validPassword = PasswordService.VerifyPassword(request.OldPassword, user.Password);
            if (!validPassword)
            {
                return (false, "Invalid Email/Password");
            }
            user.Password = PasswordService.HashPassword(request.NewPassword);
            user.DateUpdated = DateTime.Now;
            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public async Task<(bool status, string message)> Edit(EditUserRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == request.Id);
            if (user == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }
            if (user.FirstName != request.FirstName)
            {
                user.FirstName = request.FirstName;
            }
            if (user.LastName != request.LastName)
            {
                user.LastName = request.LastName;
            }
            user.DateUpdated = DateTime.Now;
            //TODO: Save profilepicture

            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public UserDto GetUserById(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var user = _context.Users.FirstOrDefault(a => a.Id == id);
            return _mapper.Map<UserDto>(user);
        }

        public PagedList<UserDto> GetUsers(PagedRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var collection = _context.Users as IQueryable<User>;
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = request.SearchQuery.Trim();
                collection = collection.Where(a => a.FirstName.Contains(searchQuery)
                    || a.LastName.Contains(searchQuery));
            }

            var users = PagedList<User>.ToPagedList(collection,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.SortDirection);

            return _mapper.Map<PagedList<UserDto>>(users);
        }

        public async Task<(bool status, string? token)> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == request.Email);
            if (user == null)
            {
                return (false, null);
            }
            var validPassword = PasswordService.VerifyPassword(request.Password, user.Password);
            if (!validPassword)
            {
                return (false, null);
            }
            var token = GenerateToken(_mapper.Map<UserDto>(user), UserRoles.User);
            return (true, token);
        }

        public async Task<(bool status, string? token)> AdminLogin(LoginRequest request)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == request.Email);
            if (admin == null)
            {
                return (false, null);
            }
            var validPassword = PasswordService.VerifyPassword(request.Password, admin.Password);
            if (!validPassword)
            {
                return (false, null);
            }
            var token = GenerateToken(_mapper.Map<UserDto>(admin), admin.Role);
            return (true, token);
        }

        private string GenerateToken(UserDto user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetAppSetting("Jwt:SecretKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("FullName", user.FullName ),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: GetAppSetting("Jwt:Issuer"),
                audience: GetAppSetting("Jwt:Audience"),
                claims: claims.ToArray(),
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(GetAppSetting("Jwt:TokenTimeout"))),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}