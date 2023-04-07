using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.BusinessCore.Services.Interfaces
{
    public interface IUserService
    {
        public PagedList<UserDto> GetUsers(PagedRequest request);
        Task<(bool status,string message)> Add(AddUserRequest request);
        Task<(bool status,string message)> Edit(EditUserRequest request);
    }
}
