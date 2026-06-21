using Microsoft.AspNetCore.Identity;

namespace Hospital.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
