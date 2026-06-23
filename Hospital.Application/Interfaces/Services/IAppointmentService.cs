using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<bool> CancelAppointmentAsync(int id);
        Task<int> BookAppointmentAsync(AppointmentDto dto, int id);

    }
}
