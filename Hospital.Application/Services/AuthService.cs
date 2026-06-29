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
                throw new UnauthorizedAppException("Invalid  credentials");
            }

            var check = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!check)
            {
                throw new UnauthorizedAppException("Invalid credentials");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return _jwtService.GenerateToken(user, roles);
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null) throw new BadRequestException("Email already register");

            var requestedRole = dto.Role?.Trim();
            if (string.IsNullOrEmpty(requestedRole) || !Roles.All.Contains(requestedRole, StringComparer.OrdinalIgnoreCase))
            {
                throw new BadRequestException($"Role must be one of : {string.Join(", ", Roles.All)}");
            }
            requestedRole = Roles.All.First(r => r.Equals(requestedRole, StringComparison.OrdinalIgnoreCase));

            if (requestedRole == Roles.Admin)
            {
                var existingAdmin = await _userManager.GetUsersInRoleAsync(Roles.Admin);
                if (existingAdmin.Count > 0)
                {
                    throw new BadRequestException("An admin account already exists. Register as Doctor or Patient instead.");
                }
            }
            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var error = string.Join(";", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"User registration failed : {error}");
            }
            await _userManager.AddToRoleAsync(user, requestedRole);
            var roles = await _userManager.GetRolesAsync(user);
            return _jwtService.GenerateToken(user, roles);
        }
    }
}
