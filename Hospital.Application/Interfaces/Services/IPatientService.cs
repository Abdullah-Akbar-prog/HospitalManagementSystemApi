using Hospital.Application.DTOs;
using Hospital.Domain.Common;
using Hospital.Domain.Entities;

namespace Hospital.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<PagedResult<PatientDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<PatientDto> GetByIdAsync(int id);
        Task<int> CreateAsync(PatientDto dto, string userId);
        Task<bool> UpdateAsync(PatientDto dto, string callerUserId, bool isAdmin);
        Task<bool> DeleteAsync(int id);
        Task<Patient?> GetByUserIdAsync(string userId);
    }
}
