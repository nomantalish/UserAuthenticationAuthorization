using UserCreationAPI.DTOs;
using UserCreationAPI.Models;

namespace UserCreationAPI.Services.Contracts
{
    public interface IUserService
    {
        Task<string?> Login(LoginDTO input);
        Task<string> Refresh();
        Task<bool> ChangePassword(ChangePasswordDTO input);
        Task<User?> Create(CreateUserDTO input);
    }
}
