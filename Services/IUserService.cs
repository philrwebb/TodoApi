using TodoApi.Models;

namespace TodoApi.Services;

    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModelDto userDto);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModelDto model);
        // Task LogoutUserAsync();
    }
