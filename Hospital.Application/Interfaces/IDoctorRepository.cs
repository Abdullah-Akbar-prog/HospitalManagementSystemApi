using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor> AddAsync(Doctor doctor);
        Task<List<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task UpdateAsync(Doctor doctor);
        Task<bool> DeleteAsync(int id);
    }
}
