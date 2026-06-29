using Hospital.Application.DTOs;
using Hospital.Application.Interfaces.Services;
using Hospital.Domain.Common;
using Hospital.Domain.Exceptions;
using Hospital.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IJwtService jwtService, UserManager<ApplicationUser> userManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
        }

        public async Task<List<string>> GetAvailableRolesAsync()
        {
            var admin = await _userManager.GetUsersInRoleAsync(Roles.Admin);
            return admin.Count == 0 ? Roles.All.ToList() : Roles.RegistrableNonAdmin.ToList();
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new Exception("Invalid  credentials");
            }

            var check = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!check)
            {
                throw new Exception("Invalid credentials");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return _jwtService.GenerateToken(user, roles);
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null) throw new BadRequestException("Email already register");

            var requestRole = dto.Roles?.Trim();

            var result = _userManager.CreateAsync(user, dto.Password);

            if (!result.Result.Succeeded)
            {
                throw new Exception("User registration failed");
            }

            await _userManager.AddToRoleAsync(user, "Patient");
            var roles = await _userManager.GetRolesAsync(user);
            return _jwtService.GenerateToken(user, roles);
        }
    }
}
