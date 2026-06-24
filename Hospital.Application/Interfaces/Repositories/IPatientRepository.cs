using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetByUserIdAsync(string id);
    }
}
