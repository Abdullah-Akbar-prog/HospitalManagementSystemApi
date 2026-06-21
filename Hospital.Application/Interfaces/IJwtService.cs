using Hospital.Infrastructure.Identity;

namespace Hospital.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> role);
    }
}
