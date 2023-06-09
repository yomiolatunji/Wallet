﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YomiOlatunji.Wallet.BusinessCore.DbModels;
using YomiOlatunji.Wallet.BusinessCore.Helpers;
using YomiOlatunji.Wallet.BusinessCore.Integrations.Interfaces;
using YomiOlatunji.Wallet.BusinessCore.Services.Interfaces;
using YomiOlatunji.Wallet.CoreObject.Enumerables;
using YomiOlatunji.Wallet.CoreObject.Requests;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly WalletContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryIntegration _cloudinaryIntegration;
        private readonly IWalletService _walletService;

        public UserService(WalletContext context, IMapper mapper, ICloudinaryIntegration cloudinaryIntegration, IWalletService walletService, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryIntegration = cloudinaryIntegration;
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
            userRequest.Password = PasswordHelper.HashPassword(request.Password);
            userRequest.ProfilePictureUrl = _cloudinaryIntegration.UploadImage(request.ProfilePictureBase64);
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
            var validPassword = PasswordHelper.VerifyPassword(request.OldPassword, user.Password);
            if (!validPassword)
            {
                return (false, "Invalid Email/Password");
            }
            user.Password = PasswordHelper.HashPassword(request.NewPassword);
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
            if (!string.IsNullOrWhiteSpace(request.ProfilePictureBase64))
                user.ProfilePictureUrl = _cloudinaryIntegration.UploadImage(request.ProfilePictureBase64);

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
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Email.Trim().ToLower() == request.Email.Trim().ToLower());
            if (user == null)
            {
                return (false, "Invalid Email/Password");
            }
            var validPassword = PasswordHelper.VerifyPassword(request.Password, user.Password);
            if (!validPassword)
            {
                return (false, "Invalid Email/Password");
            }
            if (!user.IsActive.GetValueOrDefault())
            {
                return (false, "Inactive User");
            }
            var token = GenerateToken(_mapper.Map<UserDto>(user), UserRoles.User);
            return (true, token);
        }

        public async Task<(bool status, string? token)> AdminLogin(LoginRequest request)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email.Trim().ToLower() == request.Email.Trim().ToLower());
            if (admin == null)
            {
                return (false, "Invalid Email/Password");
            }
            var validPassword = PasswordHelper.VerifyPassword(request.Password, admin.Password);
            if (!validPassword)
            {
                return (false, "Invalid Email/Password");
            }
            if (!admin.IsActive.GetValueOrDefault())
            {
                return (false, "Inactive User");
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

        public async Task<(bool status, string message)> AddAdmin(AddAdminRequest request)
        {
            return await CreateAdmin(request, UserRoles.Admin);
        }

        public async Task<(bool status, string message)> AddSuperAdmin(AddAdminRequest request)
        {
            return await CreateAdmin(request, UserRoles.SuperAdmin);
        }

        private async Task<(bool status, string message)> CreateAdmin(AddAdminRequest request, string role)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var adminRequest = _mapper.Map<AddAdminRequest, Admin>(request);
            adminRequest.Role = role;
            adminRequest.DateCreated = DateTime.Now;
            adminRequest.CreatedBy = 0;
            adminRequest.IsDeleted = false;
            adminRequest.Password = PasswordHelper.HashPassword(request.Password);

            await _context.Admins.AddAsync(adminRequest);
            var inserted = (await _context.SaveChangesAsync()) > 0;
            if (inserted)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public async Task<(bool status, string message)> ActivateUser(long userId)
        {
            if (userId > 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);
            if (user == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }

            user.IsActive = true;
            user.DateUpdated = DateTime.Now;

            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }

        public async Task<(bool status, string message)> DeactivateUser(long userId)
        {
            if (userId > 0)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId);
            if (user == null)
            {
                return (false, ResponseCodes.NotFound.message);
            }

            user.IsActive = false;
            user.DateUpdated = DateTime.Now;

            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
        }
    }
}