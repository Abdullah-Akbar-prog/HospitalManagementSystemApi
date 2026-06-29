using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<List<string>> GetAvailableRolesAsync();
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
