using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<Doctor?> GetByUserIdAsync(string UserId);
    }
}
