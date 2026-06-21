using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<int> CreateAsync(PatientDto dto);
        Task<bool> UpdateAsync(PatientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
