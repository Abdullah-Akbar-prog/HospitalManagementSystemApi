using Hospital.Application.DTOs;

namespace Hospital.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<bool> CancelAppointmentAsync(int id, int? callerPatientId, int? callerDoctorId, bool isAdmin);
        Task<int> BookAppointmentAsync(AppointmentDto dto, int id);
        Task<List<AppointmentDto?>> GetByDoctorIdAsync(int id);
        Task<List<AppointmentDto?>> GetByPatientIdAsync(int id);

    }
}
