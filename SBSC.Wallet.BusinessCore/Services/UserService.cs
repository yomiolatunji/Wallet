using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using SBSC.Wallet.BusinessCore.DbModels;
using SBSC.Wallet.BusinessCore.Services.Interfaces;
using SBSC.Wallet.CoreObject.Enumerables;
using SBSC.Wallet.CoreObject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSC.Wallet.BusinessCore.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly WalletContext _context;
        private readonly IMapper _mapper;

        public UserService(WalletContext context, IMapper mapper, IConfiguration configuration) : base(configuration)
        {
            _context = context;
            _mapper = mapper;
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
            //TODO: Save profilepicture

            await _context.Users.AddAsync(userRequest);
            var inserted= (await _context.SaveChangesAsync()) > 0;
            if(inserted)
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
            var user= await _context.Users.FirstOrDefaultAsync(a=>a.Id==request.Id);
            if(user == null)
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
            //TODO: Save profilepicture

            var updated = (await _context.SaveChangesAsync()) > 0;
            if (updated)
            {
                return (true, ResponseCodes.Success.message);
            }
            return (false, ResponseCodes.Failed.message);
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
    }
}
