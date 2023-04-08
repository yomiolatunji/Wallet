using SBSC.Wallet.CoreObject.ViewModels;

namespace SBSC.Wallet.BusinessCore.Services.Interfaces
{
    public interface IUserService
    {
        public PagedList<UserDto> GetUsers(PagedRequest request);
        public UserDto GetUserById(long id);
        Task<(bool status,string message)> Add(AddUserRequest request);
        Task<(bool status,string message)> Edit(EditUserRequest request);
        Task<(bool status,string message)> ChangePassword(ChangePasswordRequest request);
        Task<(bool status, UserDto? user)> Login(LoginRequest request);
    }
}
