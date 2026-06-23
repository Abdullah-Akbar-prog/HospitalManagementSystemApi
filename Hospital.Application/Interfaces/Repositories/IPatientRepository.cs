using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);
        Task<List<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient?> GetByUserIdAsync(string id);
        Task UpdateAsync(Patient patient);
        Task<bool> DeleteAsync(int id);
    }
}
