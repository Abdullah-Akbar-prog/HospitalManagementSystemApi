using Hospital.Application.DTOs;
using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<int> CreateAsync(DoctorDto dt, string userId);
        Task<bool> UpdateAsync(DoctorDto dto, string callerUserId, bool isAdmin);
        Task<bool> DeleteAsync(int id);
        Task<Doctor?> GetByUserIdAsync(string userId);
    }
}
