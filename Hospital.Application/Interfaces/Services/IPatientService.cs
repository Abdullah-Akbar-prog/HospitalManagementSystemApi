using Hospital.Application.DTOs;
using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<int> CreateAsync(PatientDto dto, string userId);
        Task<bool> UpdateAsync(PatientDto dto);
        Task<bool> DeleteAsync(int id);
        Task<Patient?> GetByUserIdAsync(string userId);
    }
}
