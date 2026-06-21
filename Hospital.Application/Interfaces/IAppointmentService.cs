using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<bool> CancelAppointmentAsync(int id);
        Task<int> BookAppointmentAsync(AppointmentDto dto);

    }
}
