using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> AddAsync(Patient patient);
        Task<List<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task UpdateAsync(Patient patient);
        Task<bool> DeleteAsync(int id);
    }
}
