using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<int> CreateAsync(DoctorDto dto);
        Task<bool> Update(DoctorDto dto);
        Task<bool> Delete(int id);
    }
}
