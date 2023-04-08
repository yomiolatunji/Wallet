using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SBSC.Wallet.BusinessCore.DbModels;
using SBSC.Wallet.BusinessCore.Services.Interfaces;
using SBSC.Wallet.CoreObject.Enumerables;
using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.BusinessCore.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly WalletContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public UserService(WalletContext context, IMapper mapper, IPasswordService passwordService, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<(bool status, string message)> Add(AddUserRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var userRequest = _mapper.Map<AddUserRequest, User>(request);
            userRequest.DateCreated = DateTime.Now;
            //userRequest.CreatedBy = username;
            userRequest.IsDeleted = false;
            userRequest.Password = _passwordService.HashPassword(request.Password);
            //TODO: Save profilepicture

            await _context.Users.AddAsync(userRequest);
            var inserted = (await _context.SaveChangesAsync()) > 0;
            if (inserted)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
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
            var validPassword = _passwordService.VerifyPassword(request.OldPassword, user.Password);
            if (!validPassword)
            {
                return (false, "Invalid Email/Password");
            }
            user.Password = _passwordService.HashPassword(request.NewPassword);
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

        public async Task<(bool status, UserDto? user)> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == request.Email);
            if (user == null)
            {
                return (false, null);
            }
            var validPassword = _passwordService.VerifyPassword(request.Password, user.Password);
            if (!validPassword)
            {
                return (false, null);
            }
            return (true, _mapper.Map<UserDto>(user));
        }
    }
}
