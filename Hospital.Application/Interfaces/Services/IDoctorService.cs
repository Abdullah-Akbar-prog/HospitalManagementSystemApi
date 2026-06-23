using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<int> CreateAsync(DoctorDto dto);
        Task<bool> UpdateAsync(DoctorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
