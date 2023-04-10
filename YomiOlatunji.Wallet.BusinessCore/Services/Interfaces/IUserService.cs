using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.BusinessCore.Services.Interfaces
{
    public interface IUserService
    {
        PagedList<UserDto> GetUsers(PagedRequest request);

        UserDto GetUserById(long id);

        Task<(bool status, string message)> Add(AddUserRequest request);
        Task<(bool status, string message)> AddAdmin(AddAdminRequest request);
        Task<(bool status, string message)> AddSuperAdmin(AddAdminRequest request);

        Task<(bool status, string message)> Edit(EditUserRequest request);
        Task<(bool status, string message)> ActivateUser(long userId);
        Task<(bool status, string message)> DeactivateUser(long userId);

        Task<(bool status, string message)> ChangePassword(ChangePasswordRequest request);

        Task<(bool status, string? token)> Login(LoginRequest request);

        Task<(bool status, string? token)> AdminLogin(LoginRequest request);
    }
}